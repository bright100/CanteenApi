using LeadwaycanteenApi.Entities.Dtos.GeneralDto;

namespace LeadwaycanteenApi.Entities.Dtos.ResponseDto
{
    /// <summary>
    /// Data transfer object for user login response information.
    /// </summary>
    public class UserLoginResponseDto
    {
        /// <summary>
        /// Access token for the user.
        /// </summary>
        public string Accesstoken { get; set; } = string.Empty;

        /// <summary>
        /// Vendor information for the user.
        /// </summary>
        public VendorDto? Vendor { get; set; }

        /// <summary>
        /// Employee information for the user.
        /// </summary>
        public EmployeesResponse? Employee { get; set; }
    }

    /// <summary>
    /// Model representing a token response.
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// The access token for authentication.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// The refresh token for obtaining a new access token.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the token is valid.
        /// </summary>
        public bool IsValid { get; set; } = true;
    }

    /// <summary>
    /// Data transfer object for employee response information.
    /// </summary>
    public class EmployeeResponseDto
    {
        /// <summary>
        /// Access token for the employee.
        /// </summary>
        public string Accesstoken { get; set; } = string.Empty;

        /// <summary>
        /// Employee response information.
        /// </summary>
        public EmployeeResponse? Employee { get; set; }
    }

    /// <summary>
    /// Data transfer object for vendor response information.
    /// </summary>
    public class VendorResponseDto
    {
        /// <summary>
        /// Access token for the vendor.
        /// </summary>
        public string Accesstoken { get; set; } = string.Empty;

        /// <summary>
        /// Vendor information.
        /// </summary>
        public VendorDto? Vendor { get; set; }
    }
}
