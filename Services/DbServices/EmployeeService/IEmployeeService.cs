using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.EmployeeService
{
    /// <summary>
    /// Interface for employee service
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Creates a new employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Task<Employee?> CreateEmployee(EmployeesResponse employee);

        /// <summary>
        /// Get an employee by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<Employee?> GetEmployee(string email);

        /// <summary>
        /// Reset an employee password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public Task<bool> ResetEmployeePassword(string email, string oldpassword, string newpassword);

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Task<bool> UpdateEmployee(EmployeesResponse employee);

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<bool> DeleteEmployee(string email);
    }
}
