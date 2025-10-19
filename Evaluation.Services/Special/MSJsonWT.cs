using Evaluation.Services.Models.JWT;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class MSJsonWT : BaseJsonWT
    {
        private readonly HttpClientServices _httpClientServices;
        private readonly AzureADConfig _azureADConfig;
        private readonly IMemoryCache _memoryCache;
        private static readonly string JwksCacheKey = "AzureAD_JWKS_Keys";
        private static readonly TimeSpan JwksCacheDuration = TimeSpan.FromHours(24);

        public MSJsonWT(
            HttpClientServices httpClientServices,
            AzureADConfig azureADConfig,
            FormJwtConfig formAuthConfig,
            IMemoryCache memoryCache)
            : base(formAuthConfig)
        {
            _httpClientServices = httpClientServices;
            _azureADConfig = azureADConfig;
            _memoryCache = memoryCache;

            if (!string.IsNullOrEmpty(_azureADConfig.Instance))
                _azureADConfig.Instance = _azureADConfig.Instance.Replace("{tenantId}", _azureADConfig.TenantId);
            if (!string.IsNullOrEmpty(_azureADConfig.AuthorizationEndpoint))
                _azureADConfig.AuthorizationEndpoint = _azureADConfig.AuthorizationEndpoint.Replace("{tenantId}", _azureADConfig.TenantId);
            if (!string.IsNullOrEmpty(_azureADConfig.TokenEndpoint))
                _azureADConfig.TokenEndpoint = _azureADConfig.TokenEndpoint.Replace("{tenantId}", _azureADConfig.TenantId);
            if (!string.IsNullOrEmpty(_azureADConfig.KeysEndpoint))
                _azureADConfig.KeysEndpoint = _azureADConfig.KeysEndpoint.Replace("{tenantId}", _azureADConfig.TenantId);
        }

        #region 🔐 Microsoft ID Token Validation

        public async Task<ClaimsPrincipal> ValidateMSIdToken(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
                throw new ArgumentException("Empty ID token");

            var clientId = _azureADConfig.ClientId;

            var jwks = await GetCachedJWKSAsync();
            if (jwks?.Keys == null || jwks.Keys.Count == 0)
                throw new SecurityTokenException("Unable to retrieve JWKS keys from Azure AD.");

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _azureADConfig.Instance,
                ValidateAudience = true,
                ValidAudience = clientId,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(2), // small tolerance
                IssuerSigningKeys = jwks.Keys,
                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(idToken, validationParams, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.Ordinal))
                throw new SecurityTokenException("Invalid token algorithm.");

            return claimsPrincipal;
        }

        private async Task<JwKeysResponse> GetCachedJWKSAsync()
        {
            if (_memoryCache.TryGetValue(JwksCacheKey, out JwKeysResponse cachedKeys))
                return cachedKeys!;

            var (response, httpResponse) = await _httpClientServices.GetResponse<HttpResponseMessage>(_azureADConfig.KeysEndpoint);
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var jwks = JsonConvert.DeserializeObject<JwKeysResponse>(content);

            _memoryCache.Set(JwksCacheKey, jwks, JwksCacheDuration);
            return jwks!;
        }

        #endregion

        #region 🌐 Authorization URL

        public string GenerateMSAuthorizationURL(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email required to generate authorization URL.");

            string authorizationEndpoint = _azureADConfig.AuthorizationEndpoint;
            string clientId = _azureADConfig.ClientId;
            string redirectUri = _azureADConfig.RedirectUri;
            string scope = _azureADConfig.Scope;

            // Add state and nonce for security
            string state = Guid.NewGuid().ToString("N");
            string nonce = Guid.NewGuid().ToString("N");

            var queryParams = new Dictionary<string, string>
            {
                { "response_type", "code" },
                { "client_id", clientId },
                { "redirect_uri", redirectUri },
                { "scope", scope },
                { "login_hint", email },
                { "prompt", "login" },
                { "state", state },
                { "nonce", nonce }
            };

            var query = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            return $"{authorizationEndpoint}?{query}";
        }

        #endregion

        #region 🔁 Token Exchange and Refresh

        public async Task<SSOAccessToken> GenerateMSAccessToken(string authorizationCode)
        {
            if (string.IsNullOrEmpty(authorizationCode))
                throw new ArgumentException("Empty authorization code.");

            var body = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", _azureADConfig.ClientId },
                { "client_secret", _azureADConfig.ClientSecret },
                { "redirect_uri", _azureADConfig.RedirectUri },
                { "code", authorizationCode },
                { "scope", _azureADConfig.Scope }
            };

            var headers = new List<KeyValuePair<string, string>>
            {
                new("Content-Type", "application/x-www-form-urlencoded")
            };

            var response = await _httpClientServices.PostResponse<SSOAccessToken>(
                _azureADConfig.TokenEndpoint,
                new FormUrlEncodedContent(body),
                null,
                headers
            );

            if (response.response == null)
                throw new Exception("Failed to exchange authorization code for token.");

            return response.response;
        }

        public async Task<SSOAccessToken> RefreshMSIdToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("Invalid refresh token.");

            var body = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", _azureADConfig.ClientId },
                { "client_secret", _azureADConfig.ClientSecret },
                { "refresh_token", refreshToken }
            };

            var headers = new List<KeyValuePair<string, string>>
            {
                new("Content-Type", "application/x-www-form-urlencoded")
            };

            var response = await _httpClientServices.PostResponse<SSOAccessToken>(
                _azureADConfig.TokenEndpoint,
                new FormUrlEncodedContent(body),
                null,
                headers
            );

            if (response.response == null)
                throw new Exception("Failed to refresh Microsoft token.");

            return response.response;
        }

        #endregion

        #region 🧾 Utilities

        public async Task<List<Claim>> ExtractClaimsFromIdToken(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
                throw new ArgumentException("Empty token.");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(idToken);

            return token?.Claims?.ToList() ?? new List<Claim>();
        }

        #endregion
    }

}
