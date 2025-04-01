using LeadwaycanteenApi.Entities.Model;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for Vendor information.
    /// </summary>
    public class VendorDto
    {
        /// <summary>
        /// Unique identifier for the Vendor.
        /// </summary>
        public Guid VendorId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Email address of the Vendor.
        /// </summary>
        [EmailAddress, NotNull, Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Name of the Vendor.
        /// </summary>
        [NotNull, MaxLength(100), Required]
        public string VendorName { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the Vendor.
        /// </summary>
        [Phone, Required]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Address of the Vendor.
        /// </summary>
        [NotNull, MaxLength(225), Required]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// First name of the Vendor contact person.
        /// </summary>
        [NotNull, MaxLength(100), Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the Vendor contact person.
        /// </summary>
        [NotNull, MaxLength(100), Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Account number of the Vendor.
        /// </summary>
        [NotNull, Required]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Name of the bank associated with the Vendor's account.
        /// </summary>
        [NotNull, MaxLength(100), Required]
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the account holder.
        /// </summary>
        [NotNull, MaxLength(225), Required]
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Status of the Vendor.
        /// </summary>
        public VendorStatus Status { get; set; } = VendorStatus.ACTIVE;

        /// <summary>
        /// Contract form for the Vendor.
        /// </summary>
        [Required, NotNull]
        public string VendorsContractForm { get; set; } = string.Empty;

        /// <summary>
        /// Letter of agreement for the Vendor.
        /// </summary>
        [Required, NotNull]
        public string LetterOfAgreement { get; set; } = string.Empty;

        /// <summary>
        /// Company registration document for the Vendor.
        /// </summary>
        [Required, NotNull]
        public string CompanyRegistrationDocument { get; set; } = string.Empty;

        /// <summary>
        /// Additional information about the Vendor.
        /// </summary>
        [AllowNull]
        public string Others { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response object for Vendor information.
    /// </summary>
    public class VendorResponse
    {
        /// <summary>
        /// Email address of the Vendor.
        /// </summary>
        [Required, EmailAddress, NotNull]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Name of the Vendor.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string VendorName { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the Vendor.
        /// </summary>
        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Address of the Vendor.
        /// </summary>
        [Required, NotNull, MinLength(10)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// First name of the Vendor contact person.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the Vendor contact person.
        /// </summary>
        [Required, NotNull, MinLength(5)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Account number of the Vendor.
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Name of the bank associated with the Vendor's account.
        /// </summary>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the account holder.
        /// </summary>
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Status of the Vendor.
        /// </summary>
        public VendorStatus Status { get; set; } = VendorStatus.ACTIVE;
    }
}
