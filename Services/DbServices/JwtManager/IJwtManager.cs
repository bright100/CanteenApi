using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.JwtManager
{
    /// <summary>
    /// Interface for managing JWT refresh tokens.
    /// Provides methods for storing, retrieving, deleting, and revoking refresh tokens.
    /// </summary>
    public interface IJwtManager
    {
        /// <summary>
        /// Deletes a stored refresh token by its unique identifier.
        /// </summary>
        /// <param name="TokenId">The unique identifier of the token to delete.</param>
        void DeleteStoredRefreshToken(Guid TokenId);

        /// <summary>
        /// Stores a refresh token for the user asynchronously.
        /// </summary>
        /// <param name="userRefreshToken">The refresh token model containing the user's refresh token information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task StoreRefreshTokenAsync(UserRefreshTokenModel userRefreshToken);

        /// <summary>
        /// Retrieves a stored refresh token for a specific user based on their email and role.
        /// </summary>
        /// <param name="Email">The email address of the user whose refresh token is being retrieved.</param>
        /// <param name="role">The role of the user whose refresh token is being retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a <see cref="UserRefreshTokenModel"/> containing the stored refresh token, or null if no token is found.</returns>
        public Task<UserRefreshTokenModel?> GetStoredRefreshToken(string Email, string role);

        /// <summary>
        /// Revokes a stored refresh token asynchronously.
        /// </summary>
        /// <param name="srt">The refresh token model to be revoked.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RevokeRefreshTokenAsync(UserRefreshTokenModel srt);
    }

}
