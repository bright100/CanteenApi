namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for password reset.
    /// </summary>
    public class PasswordResetDto
    {
        /// <summary>
        /// Gets or sets the date and time when the password reset token was created.
        /// </summary>
        public DateTime TokenCreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the password reset token expires.
        /// </summary>
        public DateTime TokenExpiresAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the password reset token was used.
        /// </summary>
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a flag indicating whether the password reset has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; } = false;

        /// <summary>
        /// Gets or sets the password reset token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a nested password reset DTO (not recommended, consider removing).
        /// </summary>
        public PasswordResetDto resetDto { get; set; } = new PasswordResetDto();

        /// <summary>
        /// Gets or sets the date and time when the password reset was cancelled.
        /// </summary>
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    }
}