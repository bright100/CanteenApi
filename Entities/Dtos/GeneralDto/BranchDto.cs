using System.ComponentModel.DataAnnotations;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for a branch, containing basic information about the branch.
    /// </summary>
    public class BranchDto
    {
        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        /// <remarks>
        /// The branch name must be at least 5 characters long.
        /// </remarks>
        [MinLength(5), Required]
        public string BranchName { get; set; } = "Leadway - Head Office";

        /// <summary>
        /// Gets or sets the address of the branch.
        /// </summary>
        /// <remarks>
        /// The branch address must be at least 10 characters long.
        /// </remarks>
        [Required, MinLength(10)]
        public string BranchAddress { get; set; } = "121/123 Funsho Williams Avenue, Iponri Surulere.";

        /// <summary>
        /// Gets or sets the state where the branch is located.
        /// </summary>
        /// <remarks>
        /// The state must be at least 5 characters long.
        /// </remarks>
        [Required, MinLength(5)]
        public string State { get; set; } = "Lagos";

        /// <summary>
        /// Gets or sets the local government area (LGA) where the branch is located.
        /// </summary>
        /// <remarks>
        /// The LGA must be at least 10 characters long.
        /// </remarks>
        [Required, MinLength(10)]
        public string LGA { get; set; } = "Surulere - Iponri";

        /// <summary>
        /// Gets or sets a note about the branch.
        /// </summary>
        public string Note { get; set; } = "This is the head office of Leadway Canteen";
    }
}