namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a user refresh token model.
    /// </summary>
    public class UserRefreshTokenModel
    {
        /// <summary>
        /// Unique identifier for the token.
        /// </summary>
        public Guid TokenId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Hashed value of the token.
        /// </summary>
        public byte[] TokenHash { get; set; } = [];

        /// <summary>
        /// Indicates whether the token has been revoked.
        /// </summary>
        public bool Revoked { get; set; } = false;

        /// <summary>
        /// The vendor associated with the token, if any.
        /// </summary>
        public Vendor? Vendor { get; set; }

        /// <summary>
        /// The employee associated with the token, if any.
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Date and time when the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the token was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the token was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
