using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Represents a data transfer object for an employee's response.
    /// </summary>
    public class EmployeesResponse
    {
        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        /// <remarks>
        /// The full name is required and must not exceed 225 characters.
        /// </remarks>
        [NotNull, Required, MaxLength(225)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        /// <remarks>
        /// The email address is required and must be in a valid email format.
        /// </remarks>
        [EmailAddress, NotNull, Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the employee's user image.
        /// </summary>
        [NotNull]
        public string UserImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief bio of the employee.
        /// </summary>
        public string Bio { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the employee.
        /// </summary>
        /// <remarks>
        /// The password is required and must meet the following complexity requirements:
        /// - At least 12 characters long
        /// - Contain at least one uppercase letter
        /// - Contain at least one lowercase letter
        /// - Contain at least one special character
        /// - Contain at least one number
        /// </remarks>
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%%*?&]{8,}$",
        ErrorMessage = "Password must be at least 12 and contain at least on uppercase letter one lowercase letter one special character and one number"), NotNull]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a data transfer object for an employee's response.
    /// </summary>
    public class EmployeeResponse
    {
        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        /// <remarks>
        /// The full name is required.
        /// </remarks>
        [NotNull, Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        /// <remarks>
        /// The email address is required and must be in a valid email format.
        /// </remarks>
        [Required, NotNull, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the employee's user image.
        /// </summary>
        public string UserImageUrl { get; set; } = string.Empty;
    }
}
