using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;
using System.Text.Json;

namespace LeadwaycanteenApi.Services.DbServices.JwtManager
{
    /// <summary>
    /// Implements the IJwtManager interface for managing JWT refresh tokens.
    /// Provides methods to store, retrieve, revoke, and delete refresh tokens for users.
    /// </summary>
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the JwtManager class.
        /// </summary>
        /// <param name="config">The configuration object to access application settings.</param>
        public JwtManager(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Revokes a refresh token by marking it as revoked in the database.
        /// </summary>
        /// <param name="srt">The refresh token model to revoke.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RevokeRefreshTokenAsync(UserRefreshTokenModel srt)
        {
            DbWrapper<UserRefreshTokenModel> dbWrappers = new(_config);
            var sql = "UPDATE UserRefreshTokens SET Revoked = 'true' WHERE TokenId = @TokenId";
            await dbWrappers.ExecuteAsync(sql, new { srt.TokenId });
        }

        /// <summary>
        /// Deletes a stored refresh token from the database by its unique identifier.
        /// </summary>
        /// <param name="TokenId">The unique identifier of the token to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async void DeleteStoredRefreshToken(Guid TokenId)
        {
            DbWrapper<UserRefreshTokenModel> dbWrappers = new(_config);
            var sql = "DELETE FROM UserRefreshTokens WHERE TokenId = @TokenId";
            await dbWrappers.ExecuteAsync(sql, new { TokenId });
        }

        /// <summary>
        /// Retrieves a stored refresh token for a user based on their email and role.
        /// </summary>
        /// <param name="Email">The email address of the user whose refresh token is being retrieved.</param>
        /// <param name="role">The role of the user whose refresh token is being retrieved.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a <see cref="UserRefreshTokenModel"/> containing the stored refresh token, or null if no token is found.</returns>
        public async Task<UserRefreshTokenModel?> GetStoredRefreshToken(string Email, string role)
        {
            DbWrapper<UserRefreshTokenModel> dbWrappers = new(_config);

            if (role == "Admin" || role == "Employee")
            {
                DbWrapper<Employee> dbWrapperUser = new(_config);
                var sql =
                    "SELECT u.CreatedAt, u.ExpiresAt, u.Revoked, u.TokenHash, u.TokenId, u.UpdatedAt, u.EmployeeId FROM UserRefreshTokens u LEFT JOIN Employee a ON u.EmployeeId = a.EmployeeId WHERE a.Email = @Email";
                var user =
                    await dbWrapperUser.QueryFirstOrDefaultAsync("SELECT * FROM Employee WHERE Email = @Email", new { Email });
                var return_value = await dbWrappers.QueryFirstOrDefaultAsync(sql, new { Email });

                if (return_value == null) return null;
                return_value.Employee = user;

                return return_value;
            }
            else if (role == "Vendor")
            {
                DbWrapper<Vendor> dbWrapperVendor = new(_config);
                var sql =
                    "SELECT u.CreatedAt, u.ExpiresAt, u.Revoked, u.TokenHash, u.TokenId, u.UpdatedAt, u.VendorId FROM UserRefreshTokens u LEFT JOIN Vendor ON u.VendorId = Vendor.VendorId WHERE Vendor.Email = @Email";
                var user =
                    await dbWrapperVendor.QueryFirstOrDefaultAsync("SELECT * FROM Vendor WHERE Email = @Email", new { Email });
                var return_value = await dbWrappers.QueryFirstOrDefaultAsync(sql, new { Email });
                if (return_value == null) return null;
                return_value.Vendor = user;
                return return_value;
            }

            return null;
        }

        /// <summary>
        /// Stores a refresh token for a user (either vendor or employee) in the database.
        /// </summary>
        /// <param name="userRefreshToken">The refresh token model to store, containing the token's data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StoreRefreshTokenAsync(UserRefreshTokenModel userRefreshToken)
        {
            if (userRefreshToken == null || (userRefreshToken.Vendor == null && userRefreshToken.Employee == null)) return;

            DbWrapper<UserRefreshTokenModel> dbWrappers = new(_config);

            var sql = userRefreshToken.Vendor is null ?
                "INSERT INTO UserRefreshTokens (TokenHash, ExpiresAt, Revoked, EmployeeId) VALUES(@TokenHash, @ExpiresAt, @Revoked, @EmployeeId)"
                :"INSERT INTO UserRefreshTokens (TokenHash, ExpiresAt, Revoked, VendorId) VALUES(@TokenHash, @ExpiresAt, @Revoked, @VendorId)";

            await (userRefreshToken.Vendor is null ?
                dbWrappers.ExecuteAsync(sql, new { userRefreshToken.TokenHash, userRefreshToken.ExpiresAt, userRefreshToken.Revoked, userRefreshToken.Employee.EmployeeId })
                :dbWrappers.ExecuteAsync(sql, new { userRefreshToken.TokenHash, userRefreshToken.ExpiresAt, userRefreshToken.Revoked, userRefreshToken.Vendor.VendorId }));
        }
    }

}
