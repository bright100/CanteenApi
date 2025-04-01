using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;

namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents a vendor with various details such as name, contact information, 
    /// payment details, and associated documents.
    /// </summary>
    public class Vendor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vendor.
        /// </summary>
        public Guid VendorId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the email address of the vendor.
        /// </summary>
        [NotNull, Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the vendor.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string VendorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number of the vendor.
        /// </summary>
        [NotNull, Required, Phone]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the physical address of the vendor.
        /// </summary>
        [NotNull, Required, MinLength(10)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password hash for the vendor's account.
        /// </summary>
        [NotNull, Required]
        public byte[] PasswordHash { get; set; } = [];

        /// <summary>
        /// Gets or sets the first name of the vendor.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the canteen status of the vendor.
        /// </summary>
        public string CanteenStatus { get; set; } = "Closed";

        /// <summary>
        /// Gets or sets the last name of the vendor.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account number associated with the vendor.
        /// </summary>
        [NotNull, Required, MinLength(10)]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the vendor's bank.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name on the vendor's bank account.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vendor's contract form document path or identifier.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string VendorsContractForm { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vendor's letter of agreement document path or identifier.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string LetterOfAgreement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vendor's company registration document path or identifier.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string CompanyRegistrationDocument { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any additional documents related to the vendor.
        /// </summary>
        [NotNull, Required, MinLength(5)]
        public string Others { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password reset token associated with the vendor's account.
        /// </summary>
        public PasswordResetToken ResetToken { get; set; } = new PasswordResetToken();

        /// <summary>
        /// Gets or sets the date and time when the vendor was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the vendor was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the current status of the vendor.
        /// </summary>
        public VendorStatus Status { get; set; } = VendorStatus.ACTIVE;

        /// <summary>
        /// Gets or sets the refresh token model associated with the vendor's user account.
        /// </summary>
        public UserRefreshTokenModel? UserRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the list of food items offered by the vendor.
        /// </summary>
        public List<InventoryItem> FoodItems { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of orders placed by customers for the vendor.
        /// </summary>
        public List<Orders> Orders { get; set; } = [];

        /// <summary>
        /// Gets or sets the branch that the vendor belongs to.
        /// </summary>
        public Branch Branch { get; set; } = new Branch();

        /// <summary>
        /// Gets or sets the list of payments made to the vendor.
        /// </summary>
        public List<Payment> Payments { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of reviews given to the vendor.
        /// </summary>
        public List<ReviewsDto> Reviews { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of vendor requests associated with the vendor.
        /// </summary>
        public List<VendorRequest> VendorRequests { get; set; } = [];

        /// <summary>
        /// Gets or sets the inventory associated with the vendor.
        /// </summary>
        public Inventory? Inventory { get; set; }
    }

    /// <summary>
    /// Represents the possible statuses of a vendor.
    /// </summary>
    public enum VendorStatus
    {
        /// <summary>
        /// The vendor is active and fully operational.
        /// </summary>
        ACTIVE,

        /// <summary>
        /// The vendor is inactive and may not be providing services.
        /// </summary>
        INACTIVE,

        /// <summary>
        /// The vendor has been suspended and is temporarily restricted.
        /// </summary>
        SUSPENDED
    }

}
