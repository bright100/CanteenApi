using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.AuthService
{
    /// <summary>
    /// Defines the contract for authentication-related services, including vendor login, token management,
    /// password reset validation, and vendor account creation.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Logs in a vendor with the provided email and password, returning a <see cref="VendorResponseDto"/> on success.
        /// </summary>
        /// <param name="Email">The email address of the vendor.</param>
        /// <param name="password">The password of the vendor.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="VendorResponseDto"/> if login is successful, or null if unsuccessful.</returns>
        public Task<VendorResponseDto?> LoginVendor(string Email, string password);

        /// <summary>
        /// Creates a new vendor account using the provided vendor information.
        /// </summary>
        /// <param name="User">The <see cref="VendorDto"/> containing the vendor's details.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the vendor creation was successful.</returns>
        public Task<bool?> CreateVendor(Vendor User);

        /// <summary>
        /// Logs in a user with the provided email and password, returning a <see cref="UserLoginResponseDto"/> on success.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="UserLoginResponseDto"/> if login is successful, or null if unsuccessful.</returns>
        public Task<UserLoginResponseDto?> Login(string Email, string password);

        /// <summary>
        /// Refreshes the user's login token using the provided email, token, and role.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="token">The refresh token used for token renewal.</param>
        /// <param name="role">The role of the user (e.g., "admin", "user").</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="UserLoginResponseDto"/> containing the new token information.</returns>
        public Task<UserLoginResponseDto?> RefreshToken(string Email, string token, string role);

        /// <summary>
        /// Revokes the user's token to prevent further access.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="token">The token to be revoked.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the token revocation was successful.</returns>
        public Task<bool> RevokeToken(string Email, string token, string role);

        /// <summary>
        /// Sets a new password for the vendor using the provided email and password.
        /// </summary>
        /// <param name="Email">The email address of the vendor.</param>
        /// <param name="password">The new password for the vendor.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the password update was successful.</returns>
        public Task<bool> SetVendorPassword(string Email, string password);

        /// <summary>
        /// Validates the password reset token for the specified email address.
        /// </summary>
        /// <param name="email">The email address for which the password reset token is being validated.</param>
        /// <param name="passwordResetToken">The password reset token to be validated.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the token is valid.</returns>
        public Task<bool> ValidatePasswordResetToken(string email, string passwordResetToken);
    }
}
