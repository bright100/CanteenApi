using System.Text;
using System.Text.RegularExpressions;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using Newtonsoft.Json;

namespace LeadwaycanteenApi.util
{
    /// <summary>
    /// Utility class for validation
    /// </summary>
    public class ValidationUtility(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        private readonly IConfiguration _config = configuration ?? throw new ArgumentNullException(nameof(configuration));

        /// <summary>
        /// Validate if an email belongs to the Leadway domain
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if the email is a valid Leadway email, false otherwise</returns>
        public static bool ValidateLeadwayEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, @"^([\w\.\-]+)@leadway\.com$");
        }

        /// <summary>
        /// Checks if a user exists in the Leadway database
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>Employee information if validation succeeds, null otherwise</returns>
        public async Task<EmployeesResponse?> ValidateEmailWithLeadway(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || !ValidateLeadwayEmail(email))
                    return null;
                string passwordBase64 = ConvertSecureStringToBase64(password);

                var requestBody = new
                {
                    username = email,
                    password = passwordBase64
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var requestPayload = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient("LeadwayAuth");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_config["AdApi:Url"]),
                    Content = requestPayload
                };

                using var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var res = System.Text.Json.JsonSerializer.Deserialize<AdResponseDto>(body);
                    if (res == null) return null;
                    if (res.errorMsg == "")
                    {
                        return new EmployeesResponse
                        {
                            Email = res.username + email,
                            Bio = "",
                            FullName = res.fullName,
                            UserImageUrl = ""
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts a password to a Base64 encoded string
        /// </summary>
        private static string ConvertSecureStringToBase64(string password)
        {
            ArgumentNullException.ThrowIfNull(password);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }
    }
}