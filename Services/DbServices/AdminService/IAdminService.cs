using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.AdminService
{
    /// <summary>
    /// Defines the contract for administrative services, including adding, deleting, retrieving, and updating admin users.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Adds an admin role to an employee identified by their email address.
        /// </summary>
        /// <param name="email">The email address of the employee to be updated to admin.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> AddAdmin(string email);

        /// <summary>
        /// Retrieves an admin user based on their email address.
        /// </summary>
        /// <param name="Email">The email address of the admin user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="EmployeesResponse"/> containing the admin user details if found, or null if no matching admin user is found.</returns>
        public Task<EmployeesResponse?> GetAdminUser(string Email);

        /// <summary>
        /// Deletes an admin user identified by their email address.
        /// </summary>
        /// <param name="Email">The email address of the admin user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> DeleteAdminUser(string Email);

        /// <summary>
        /// Updates the details of an existing admin user.
        /// </summary>
        /// <param name="user">An <see cref="Employee"/> object containing updated information for the admin user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> UpdateAdminUser(Employee user);
    }

}
