using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models.Api.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class AuthenticationBL : ApiBase
{
    private readonly MSJsonWT _msJsonWT;
    private readonly RequestInfo _requestInfo;

    public AuthenticationBL(
        IServiceScopeFactory serviceScopeFactory,
        CacheDataProvider cacheDataProvider,
        UnitOfWork uow,
        LoggingServices loggingServices,
        UserInfo userInfo,
        MSJsonWT msJsonWT,
        RequestInfo requestInfo,
        IServiceProvider serviceProvider)
        : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, userInfo, serviceProvider, requestInfo)
    {
        _msJsonWT = msJsonWT;
        _requestInfo = requestInfo;
    }

    #region ?? Microsoft SSO Login

    /// <summary>
    /// Generate Microsoft OAuth 2.0 authorization URL for login redirection.
    /// </summary>
    public string GetMSAuthorizationURL(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidEmailRequest);

        return _msJsonWT.GenerateMSAuthorizationURL(email.Trim());
    }

    /// <summary>
    /// Handles the login callback after successful Microsoft authorization.
    /// </summary>
    public async Task<(Guid userId, string token)> LoginMinistryAsync(AuthorizationCodeRequest model)
    {
        if (string.IsNullOrEmpty(model.code))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequestEmptyCode);

        // 1. Exchange the authorization code for an access token
        var tokenResponse = await _msJsonWT.GenerateMSAccessToken(model.code);

        // 2. Validate the ID token and extract claims
        var claimsPrincipal = await _msJsonWT.ValidateMSIdToken(tokenResponse.id_token);
        var email = claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value
                    ?? throw new BusinessException(ConstantKeys.ExceptionMessage.NoEmailFound);

        // 3. Verify user exists in the MinistryUser table
        using var uow = serviceScopeFactory.CreateScopedUow();
        var ministryUser = await uow.GetRepository<User>()
            .GetAllActiveNonDeleted(x => x.Email == email)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException(ConstantKeys.ExceptionMessage.NoMinistryUserFound);

        // 4. Log user login activity
        await RegisterLoginLog(ministryUser.Id);

        // 5. Generate claims and create JWT
        var claims = GenerateClaimsForUserProfile(ministryUser, UserType.Ministry);
        var token = _msJsonWT.GenerateToken(claims);

        // 6. Store token info for audit
        await RegisterUserToken(ministryUser.Id, token);

        return (ministryUser.Id, token);
    }

    #endregion

    #region ?? Token Management

    /// <summary>
    /// Refresh the access token for the Ministry user.
    /// </summary>
    public async Task<(Guid userId, string token)> RefreshTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoTokenFound);

        // 1. Validate old token and extract claims
        var claimsPrincipal = _msJsonWT.ValidateToken(token);
        var email = claimsPrincipal.FindFirst(UserProfileClaim.Email.ToString())?.Value
                    ?? throw new BusinessException(ConstantKeys.ExceptionMessage.NoEmailFound);

        using var uow = serviceScopeFactory.CreateScopedUow();

        // 2. Find the user
        var ministryUser = await uow.GetRepository<User>()
            .GetAllActiveNonDeleted(x => x.Email == email)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException(ConstantKeys.ExceptionMessage.NoMinistryUserFound);

        // 3. Generate new JWT
        var claims = GenerateClaimsForUserProfile(ministryUser, UserType.Ministry);
        var newToken = _msJsonWT.GenerateToken(claims);

        // 4. Update token table (mark old as deprecated, store new)
        await DeprecateUserToken(token);
        await RegisterUserToken(ministryUser.Id, newToken);

        return (ministryUser.Id, newToken);
    }

    /// <summary>
    /// Store a newly generated user token for audit purposes.
    /// </summary>
    public async Task RegisterUserToken(Guid userId, string token)
    {
        using var uow = serviceScopeFactory.CreateScopedUow();

        var entity = new UserToken
        {
            UserId = userId,
            Token = token,
            UserAgent = _requestInfo.UserAgent,
            IP = _requestInfo.UserIp,
            Deprecated = false,
            TokenExpiryDate = GetTokenExpirationTime(token),
            CreateById = userId
        };

        await uow.GetRepository<UserToken>().InsertAsync(entity);
        await uow.CommitAsync();
    }

    /// <summary>
    /// Mark an existing token as deprecated (invalidated).
    /// </summary>
    public async Task DeprecateUserToken(string token)
    {
        using var uow = serviceScopeFactory.CreateScopedUow();

        var entity = await uow.GetRepository<UserToken>()
            .GetAllActiveNonDeleted()
            .Where(x => x.Token == token && x.UserId == userInfo.UserId)
            .FirstOrDefaultAsync();

        if (entity != null)
        {
            entity.Deprecated = true;
            entity.DeprecatedDate = DateTime.Now;
            entity.UpdateById = userInfo.UserId;

            uow.GetRepository<UserToken>().Update(entity);
            await uow.CommitAsync();
        }
    }

    /// <summary>
    /// Extract expiration time from JWT.
    /// </summary>
    private DateTime GetTokenExpirationTime(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadToken(token) as JwtSecurityToken;

        var expClaim = jwt?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        if (expClaim == null)
            throw new BusinessException("Token missing expiration claim.");

        return DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).LocalDateTime;
    }

    #endregion

    #region ?? Audit Logging

    /// <summary>
    /// Record user login details in the UserProfileLoginLog table.
    /// </summary>
    public async Task RegisterLoginLog(Guid userId)
    {
        using var uow = serviceScopeFactory.CreateScopedUow();

        var log = new UserLoginLog
        {
            UserId = userId,
            IP = _requestInfo.UserIp,
            UserAgent = _requestInfo.UserAgent,
            CreateById = userId
        };

        await uow.GetRepository<UserLoginLog>().InsertAsync(log);

        var profile = await uow.GetRepository<User>()
            .GetAllActiveNonDeleted(x => x.Id == userId)
            .FirstOrDefaultAsync();

        if (profile != null)
        {
            profile.LastLoginDate = DateTime.Now;
            uow.GetRepository<User>().Update(profile);
        }

        await uow.CommitAsync();
    }

    #endregion

    #region ?? Helpers

    private Dictionary<string, string> GenerateClaimsForUserProfile(User user, UserType userType)
    {
        return new()
        {
            [UserProfileClaim.UserType.ToString()] = userType.ToString(),
            [UserProfileClaim.UserId.ToString()] = user.Id.ToString(),
            [UserProfileClaim.Email.ToString()] = user.Email ?? "",
            [UserProfileClaim.FullNameAr.ToString()] = user.NameAr ?? "",
            [UserProfileClaim.FullNameEn.ToString()] = user.NameEn ?? "",
            [UserProfileClaim.Mobile.ToString()] = user.Mobile ?? "",
            [UserProfileClaim.PreferredLang.ToString()] = user.PreferredLanguage ?? "",
            [UserProfileClaim.LastLogin.ToString()] = user.LastLoginDate?.ToString() ?? "",
            [UserProfileClaim.TokenExpirationTime.ToString()] = _msJsonWT.GetTokenExpirationTime().ToString()
        };
    }

    #endregion
}
