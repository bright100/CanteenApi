namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a branch entity.
    /// </summary>
    public class Branch
    {
        /// <summary>
        /// Unique identifier for the branch.
        /// </summary>
        public Guid BranchId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the branch.
        /// </summary>
        public string BranchName { get; set; } = "Leadway - Head Office";

        /// <summary>
        /// Physical address of the branch.
        /// </summary>
        public string BranchAddress { get; set; } = "121/123 Funsho Williams Avenue, Iponri Surulere.";

        /// <summary>
        /// State where the branch is located.
        /// </summary>
        public string State { get; set; } = "Lagos";

        /// <summary>
        /// Local Government Area (LGA) where the branch is located.
        /// </summary>
        public string LGA { get; set; } = "Surulere - Iponri";

        /// <summary>
        /// Additional notes or comments about the branch.
        /// </summary>
        public string Note { get; set; } = "This is the head office of Leadway Canteen";

        /// <summary>
        /// Date and time when the branch was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the branch was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
