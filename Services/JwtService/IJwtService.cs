using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;
using System.Security.Claims;

namespace LeadwaycanteenApi.Services.JwtService
{
    /// <summary>
    /// Defines the contract for JWT-related services, including token generation, refresh token creation, and handling expired tokens.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token for the given email and role.
        /// </summary>
        /// <param name="email">The email address associated with the token.</param>
        /// <param name="role">The role of the user for which the token will be generated.</param>
        /// <returns>A <see cref="TokenModel"/> representing the generated JWT token, or null if the token could not be generated.</returns>
        public TokenModel? GenerateJwtToken(string email, string role);

        /// <summary>
        /// Generates a refresh token for the given email and role.
        /// </summary>
        /// <param name="email">The email address associated with the refresh token.</param>
        /// <param name="role">The role of the user for which the refresh token will be generated.</param>
        /// <returns>A <see cref="TokenModel"/> representing the generated refresh token, or null if the refresh token could not be generated.</returns>
        public TokenModel? GenerateRefreshToken(string email, string role);

        /// <summary>
        /// Retrieves the claims principal from an expired JWT token.
        /// </summary>
        /// <param name="token">The JWT token that has expired.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> object containing the claims from the expired token.</returns>
        public ClaimsPrincipal GetExpiredClaimsPrincipal(string token);

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <returns>A string representing the generated refresh token.</returns>
        public string CreateRefreshToken();
    }

}
