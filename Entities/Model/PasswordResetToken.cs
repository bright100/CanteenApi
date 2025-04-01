namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a password reset token entity.
    /// </summary>
    public class PasswordResetToken
    {
        /// <summary>
        /// Unique identifier for the token.
        /// </summary>
        public Guid TokenId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and time when the token was created.
        /// </summary>
        public DateTime TokenCreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the token expires.
        /// </summary>
        public DateTime TokenExpiresAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the token was used.
        /// </summary>
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the token has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// The actual token value.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the token was cancelled.
        /// </summary>
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    }
}
