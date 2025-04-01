using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LeadwaycanteenApi.Services.JwtService
{
    /// <summary>
    /// Service that provides methods for generating JWT tokens, creating refresh tokens, and handling expired JWT tokens.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </remarks>
    /// <param name="configuration">The application configuration containing the JWT settings.</param>
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Generates a JWT token containing the specified email and role, with an expiration time of 30 minutes.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="role">The role associated with the user.</param>
        /// <returns>A <see cref="TokenModel"/> containing the generated access token and refresh token, or null if email is null.</returns>
        public TokenModel? GenerateJwtToken(string email, string role)
        {
            if (email != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["jwtSettings:SecretKey"];

                var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Aud, _configuration["jwtSettings:Audience"]),
                new(JwtRegisteredClaimNames.Iss, _configuration["jwtSettings:Issuer"])
            };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "")),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = CreateRefreshToken();

                TokenModel result = new()
                {
                    RefreshToken = refreshToken,
                    IsValid = true,
                    AccessToken = tokenHandler.WriteToken(token),
                };

                return result;
            }

            return null;
        }

        /// <summary>
        /// Generates a refresh token using the same method as generating a JWT token.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A <see cref="TokenModel"/> containing the generated refresh token and access token.</returns>
        public TokenModel? GenerateRefreshToken(string email, string role)
        {
            return GenerateJwtToken(email, role);
        }

        /// <summary>
        /// Retrieves the claims principal from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> object containing the claims from the expired token.</returns>
        /// <exception cref="SecurityTokenException">Thrown when the token is invalid or cannot be validated.</exception>
        public ClaimsPrincipal GetExpiredClaimsPrincipal(string token)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <returns>A string representing the generated refresh token.</returns>
        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

}
