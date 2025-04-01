namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a payment entity.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Unique identifier for the payment.
        /// </summary>
        public Guid PaymentId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and time when the payment was made.
        /// </summary>
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the payment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the payment was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
