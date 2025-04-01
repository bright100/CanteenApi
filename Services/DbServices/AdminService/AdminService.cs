using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;

namespace LeadwaycanteenApi.Services.DbServices.AdminService
{
    /// <summary>
    /// Provides administrative services such as adding, deleting, retrieving, and updating admin users.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AdminService"/> class.
    /// </remarks>
    /// <param name="config">The configuration object used for database connection settings.</param>
    public class AdminService(IConfiguration config) : IAdminService
    {
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Adds an admin role to an employee identified by their email address.
        /// </summary>
        /// <param name="email">The email address of the employee to be updated to admin.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public async Task<bool> AddAdmin(string email)
        {
            try
            {
                DbWrapper<EmployeesResponse> dbWrapper = new(_config);
                var sql = "UPDATE Employee SET Roles = 'Admin' WHERE Email = @Email";
                return await dbWrapper.ExecuteAsync(sql, new { Email = email });
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an admin user identified by their email address.
        /// </summary>
        /// <param name="Email">The email address of the admin user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAdminUser(string Email)
        {
            DbWrapper<EmployeesResponse> dbWrapper = new(_config);
            var sql = "DELETE FROM Employee WHERE Email = @Email AND Roles = 'Admin'";
            return await dbWrapper.ExecuteAsync(sql, Email);
        }

        /// <summary>
        /// Retrieves an admin user based on their email address.
        /// </summary>
        /// <param name="Email">The email address of the admin user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="EmployeesResponse"/> containing the admin user details if found, or null if no matching admin user is found.</returns>
        public async Task<EmployeesResponse?> GetAdminUser(string Email)
        {
            DbWrapper<EmployeesResponse> dbWrapper = new(_config);
            var sql = "SELECT * FROM Employee WHERE Email = @Email AND Roles = 'Admin'";
            return await dbWrapper.QuerySingleOrDefaultAsync(sql, Email);
        }

        /// <summary>
        /// Updates the details of an existing admin user.
        /// </summary>
        /// <param name="user">The <see cref="Employee"/> object containing updated information for the admin user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public async Task<bool> UpdateAdminUser(Employee user)
        {
            DbWrapper<EmployeesResponse> dbWrapper = new(_config);
            var sql = "UPDATE Employee SET FullName = @FullName, UserImageUrl = @UserImageUrl WHERE Email = @Email AND Roles = 'Admin'";
            return await dbWrapper.ExecuteAsync(sql, user);
        }
    }

}
