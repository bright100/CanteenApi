using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for user login.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress, Required, NotNull]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <remarks>
        /// The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one special character, and one number.
        /// </remarks>
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 and contain at least an uppercase letter one lowercase letter one special character and a number"), Required]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data transfer object for employee login.
    /// </summary>
    public class EmployeeLoginDto
    {
        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        [EmailAddress, Required, NotNull]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the employee.
        /// </summary>
        [Required, NotNull]
        public string Password { get; set; } = string.Empty;
    }
}
