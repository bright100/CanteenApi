namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a vendor request, including the request details, status, and timestamps.
    /// </summary>
    public class VendorRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vendor request.
        /// </summary>
        public Guid RequestId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the description or details of the vendor request.
        /// </summary>
        public string Request { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current status of the vendor request.
        /// </summary>
        public VendorRequestStatus Status { get; set; } = VendorRequestStatus.PENDING;

        /// <summary>
        /// Gets or sets the date and time when the vendor request was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the vendor request was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Represents the possible statuses of a vendor request.
    /// </summary>
    public enum VendorRequestStatus
    {
        /// <summary>
        /// The vendor request has been approved.
        /// </summary>
        APPROVED,

        /// <summary>
        /// The vendor request has been rejected.
        /// </summary>
        REJECTED,

        /// <summary>
        /// The vendor request is still pending and has not been processed.
        /// </summary>
        PENDING
    }

}
