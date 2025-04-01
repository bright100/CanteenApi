using System.ComponentModel.DataAnnotations;
using Dapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;
using Microsoft.Data.SqlClient;

namespace LeadwaycanteenApi.Services.DbServices.EmployeeService
{
    /// <summary>
    /// Employee service
    /// </summary>
    public class EmployeeService(IConfiguration config) : IEmployeeService
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(config), "Connection string 'DefaultConnection' not found.");
        private readonly IConfiguration _config = config;
        /// <summary>
        /// Creates a new employee
        /// </summary>
        /// <param name="employee">employee details</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Employee?> CreateEmployee(EmployeesResponse employee)
        {
            DbWrapper<Employee> dbWrapper = new(_config);
            var sql = "INSERT INTO Employee (Email, FullName, Bio, PasswordHash, Roles, UserImageUrl) " +
                      "VALUES (@Email, @FullName, @Bio, @PasswordHash, @Roles, @UserImageUrl)";

            var created = await dbWrapper.ExecuteAsync(sql, new { employee.Email,employee.FullName, employee.Bio, PasswordHash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(employee.Password)), Roles = "Employee", employee.UserImageUrl });

            if (!created) return null;

            return await GetEmployee(employee.Email);
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="email">employee email address</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteEmployee([EmailAddress]string email)
        {
            DbWrapper<Employee> dbWrapper = new(_config);
            var sql = "DELETE FROM Employee WHERE Email = @Email";

            return await dbWrapper.ExecuteAsync(sql, new { Email = email });
        }

        /// <summary>
        /// Get an employee by email
        /// </summary>
        /// <param name="email">employee email address</param>
        /// <returns>the employees detail</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Employee?> GetEmployee(string email)
        {
            DbWrapper<Employee> dbWrapper = new(_config);
            var sql = "SELECT * FROM Employee WHERE Email = @Email";

            return await dbWrapper.QuerySingleOrDefaultAsync(sql, new { Email = email });
        }

        /// <summary>
        /// Reset an employee password
        /// </summary>
        /// <param name="email">employee email address</param>
        /// <param name="oldpassword">password to be changed</param>
        /// <param name="newpassword">new password</param>
        /// <returns>bool</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ResetEmployeePassword([EmailAddress]string email, string oldpassword, string newpassword)
        {
            var newpasswordHash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(newpassword));
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var transaction = await connection.BeginTransactionAsync();

            var getoldpassword = "SELECT PasswordHash FROM Employee WHERE Email = @Email";
            var sql = "UPDATE Employee SET PasswordHash = @PasswordHash WHERE Email = @Email";

            try
            {
                var oldpasswordHash = await connection.QuerySingleAsync(getoldpassword, new { Email = email }, transaction);
                if (oldpasswordHash == null || oldpasswordHash != System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(oldpassword)))
                    return false;

                await connection.ExecuteAsync(sql, new { Email = email, PasswordHash = newpasswordHash }, transaction);
                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="employee">the employee details</param>
        /// <returns>bool</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> UpdateEmployee(EmployeesResponse employee)
        {
            DbWrapper<Employee> dbWrapper = new(_config);
            var sql = "UPDATE Employee SET FullName = @FullName, Bio = @Bio, Roles = @Roles, UserImageUrl = @UserImageUrl WHERE Email = @Email";

            return dbWrapper.ExecuteAsync(sql, employee);
        }
    }
}
