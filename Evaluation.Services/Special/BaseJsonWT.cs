using Evaluation.Services.Models.JWT;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class BaseJsonWT
    {
        private readonly FormJwtConfig formAuthConfig;

        public BaseJsonWT(FormJwtConfig formAuthConfig)
        {
            this.formAuthConfig = formAuthConfig ?? throw new ArgumentNullException(nameof(formAuthConfig));
        }

        #region === TOKEN GENERATION ===

        public string GenerateToken(List<Claim> authClaims, int? expirationOverrideMinutes = null)
        {
            return GenerateJwt(authClaims, expirationOverrideMinutes);
        }

        public string GenerateToken(Dictionary<string, string> pairs, int? expirationOverrideMinutes = null)
        {
            var authClaims = pairs
                .Select(p => new Claim(p.Key, p.Value ?? string.Empty))
                .ToList();

            return GenerateJwt(authClaims, expirationOverrideMinutes);
        }

        private string GenerateJwt(IEnumerable<Claim> claims, int? expirationOverrideMinutes = null)
        {
            var keyBytes = Encoding.UTF8.GetBytes(formAuthConfig.Key);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: formAuthConfig.Issuer,
                audience: formAuthConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationOverrideMinutes ?? formAuthConfig.ExpirationTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        #endregion

        #region === TOKEN VALIDATION ===

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(formAuthConfig.Key)),

                ValidateIssuer = !string.IsNullOrEmpty(formAuthConfig.Issuer),
                ValidIssuer = formAuthConfig.Issuer,

                ValidateAudience = !string.IsNullOrEmpty(formAuthConfig.Audience),
                ValidAudience = formAuthConfig.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1) // small grace window
            };

            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                // Optional: Validate algorithm
                if (validatedToken is JwtSecurityToken jwtToken &&
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenExpiredException("Token has expired.");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new SecurityTokenInvalidSignatureException("Invalid token signature.");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException($"Token validation failed: {ex.Message}", ex);
            }
        }

        #endregion

        #region === CLAIMS UTILITIES ===

        public async Task<T?> GetClaimValueAsync<T>(JwtSecurityToken jwtSecurityToken, string key)
        {
            if (jwtSecurityToken?.Claims == null)
                return default;

            var claim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (claim == null)
                return default;

            try
            {
                return (T)Convert.ChangeType(claim.Value, typeof(T));
            }
            catch
            {
                throw new InvalidOperationException($"Cannot convert claim '{key}' value to type '{typeof(T).Name}'.");
            }
        }

        public Dictionary<string, string> GetClaimValues(JwtSecurityToken jwtSecurityToken)
        {
            return jwtSecurityToken?.Claims?
                .ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetClaimValues(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims?
                .ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();
        }

        public int GetTokenExpirationTime() => formAuthConfig.ExpirationTime;

        #endregion
    }

}
