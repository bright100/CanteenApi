using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for reviews.
    /// </summary>
    public class ReviewsDto
    {
        /// <summary>
        /// Gets or sets the review text.
        /// </summary>
        [Required, NotNull, MinLength(10)]
        public string Review { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating given.
        /// </summary>
        [Required, NotNull]
        public decimal Rating { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee who made the review.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the vendor being reviewed.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string VendorName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data transfer object for review responses.
    /// </summary>
    public class ReviewsResponseDto
    {
        /// <summary>
        /// Gets or sets the employee who made the review.
        /// </summary>
        public EmployeeResponse Employee { get; set; } = new EmployeeResponse();

        /// <summary>
        /// Gets or sets the vendor being reviewed.
        /// </summary>
        public VendorResponse Vendor { get; set; } = new VendorResponse();

        /// <summary>
        /// Gets or sets the review text.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string Review { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating given.
        /// </summary>
        [Required, NotNull]
        public decimal Rating { get; set; }
    }
}
