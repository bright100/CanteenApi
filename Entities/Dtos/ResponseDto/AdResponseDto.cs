namespace LeadwaycanteenApi.Entities.Dtos.ResponseDto
{
    /// <summary>
    /// Data transfer object for Ad response information.
    /// </summary>
    public class AdResponseDto
    {
        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string fullName { get; set; } = string.Empty;

        /// <summary>
        /// Username of the user.
        /// </summary>
        public string username { get; set; } = string.Empty;

        /// <summary>
        /// Telephone number of the user.
        /// </summary>
        public string telephoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Error message associated with the Ad response.
        /// </summary>
        public string errorMsg { get; set; } = string.Empty;
    }
}
