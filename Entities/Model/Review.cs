namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a review entity.
    /// </summary>
    public class Reviews
    {
        /// <summary>
        /// Unique identifier for the review.
        /// </summary>
        public Guid ReviewId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The actual review text.
        /// </summary>
        public string Review { get; set; } = string.Empty;

        /// <summary>
        /// Rating given by the reviewer.
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// The vendor being reviewed.
        /// </summary>
        public Vendor Vendor { get; set; } = new Vendor();

        /// <summary>
        /// The employee who made the review.
        /// </summary>
        public Employee Employee { get; set; } = new Employee();

        /// <summary>
        /// Date and time when the review was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the review was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
