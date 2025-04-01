namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents an employee entity.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Unique identifier for the employee.
        /// </summary>
        public Guid EmployeeId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Full name of the employee.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the employee.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for the employee.
        /// </summary>
        public byte[] PasswordHash { get; set; } = [];

        /// <summary>
        /// Brief biography of the employee.
        /// </summary>
        public string Bio { get; set; } = string.Empty;

        /// <summary>
        /// URL of the employee's profile image.
        /// </summary>
        public string UserImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Roles assigned to the employee.
        /// </summary>
        public string Roles { get; set; } = "Employee";

        /// <summary>
        /// Date and time when the employee was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the employee was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
