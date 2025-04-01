using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.EmailService
{
    /// <summary>
    /// Defines the contract for email-related services, including sending emails, verification, password resets, and data encryption.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email verification message to the employee.
        /// </summary>
        /// <param name="employee">The employee who will receive the verification email.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the verification email was successfully sent.</returns>
        public Task<bool> SendVerificationMail(Employee employee);

        /// <summary>
        /// Generates a password reset token for the given email address.
        /// </summary>
        /// <param name="email">The email address for which the password reset token will be generated.</param>
        /// <returns>A string representing the generated password reset token.</returns>
        public string GeneratePasswordResetToken(string email);

        /// <summary>
        /// Smtp implementation of the mail service
        /// </summary>
        /// <param name="toEmail">To who?</param>
        /// <returns>True or False</returns>
        public  Task<bool> SendMessageViaSmtp(string toEmail);
    }

}
