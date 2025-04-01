using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LeadwaycanteenApi.Services.EmailService
{
    /// <summary>
    /// Service for sending mails
    /// </summary>
    /// <param name="config">Configuration interface</param>
    /// <param name="vendorService">vendor service</param>
    public class EmailService(IConfiguration config, IVendorService vendorService) : IEmailService
    {
        private readonly IConfiguration _config = config;
        private readonly IVendorService _vendorService = vendorService;
     
        /// <summary>
        /// Send verification mail
        /// </summary>
        /// <param name="employee">The admin</param>
        /// <returns>True or false</returns>
        public async Task<bool> SendVerificationMail(Employee employee)
        {
            var code = GenerateBase64Code(50);

            string messg = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Password Reset</title>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
      color: #333;
    }}
    .container {{
      width: 100%;
      max-width: 600px;
      margin: 0 auto;
      background-color: #fff;
      padding: 20px;
      border-radius: 8px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }}
    .header {{
      text-align: center;
      margin-bottom: 20px;
    }}
    .button {{
      display: inline-block;
      background-color: #F24822;
      color: white;
      padding: 14px 20px;
      text-align: center;
      font-size: 16px;
      border-radius: 5px;
      text-decoration: none;
      width: 100%;
      max-width: 250px;
      margin: 20px auto;
      transition: background-color 0.3s;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <div class=""header"">
      <h2>Email Verification</h2>
    </div>
    <p>Dear User,</p>
    <h2>Welcome To the Leadway Canteen App</h2>
    <p>Verify your email</p>
    <a href=""https://yourwebsite.com/reset-password?token={code}"" class=""button"">Reset Your Password</a>
    <p>Thank you,</p>
    <p>The Support Team</p>
  </div>
</body>
</html>";

            try
            {
                var smtpSettings = _config.GetSection("SmtpSettings");

                var fromEmail = smtpSettings["FromEmail"];
                var smtpServer = smtpSettings["SmtpServer"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Password Reset",
                    Body = messg,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(employee.Email);

                using var smtpClient = new SmtpClient(smtpServer, port);
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// generates base64 Codes
        /// </summary>
        /// <param name="byteLength">code length</param>
        /// <returns>a base64 code</returns>
        public static string GenerateBase64Code(int byteLength)
        {
            byte[] bytes = new byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes).Replace("+", "-".Replace("/", "_").Replace("=", ""));
        }

        /// <summary>
        /// Generates a password reset token
        /// </summary>
        /// <param name="email">users email</param>
        /// <returns>a reset token</returns>
        public string GeneratePasswordResetToken(string email)
        {
            byte[] randomBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var tokenData = new
            {
                email,
                exp = DateTime.UtcNow.AddHours(24).Ticks,
                rnd = Convert.ToBase64String(randomBytes)
            };

            string jsonData = JsonSerializer.Serialize(tokenData);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

            byte[] encryptedData = EncryptData(jsonBytes, _config["jwtSettings:SecretKey"]);

            return (Convert.ToBase64String(encryptedData)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", ""));
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="data">the data to be encryted</param>
        /// <param name="key">encryption key</param>
        /// <returns>a byte of chars</returns>
        public static byte[] EncryptData(byte[] data, string key)
        {
            using Aes aes = Aes.Create();
            aes.GenerateIV();

            using (var deriveBytes = new Rfc2898DeriveBytes(key, aes.IV, 100000, HashAlgorithmName.SHA256))
            {
                aes.Key = deriveBytes.GetBytes(32);
            }

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        /// <summary>
        /// Email method for sending mails with SMTP
        /// </summary>
        /// <param name="toEmail">To who?</param>
        /// <returns>Tru or false</returns>
        public async Task<bool> SendMessageViaSmtp(string toEmail)
        {
            var vendor = await _vendorService.GetVendor(toEmail);
            if (vendor == null)
                return false;

            var token = GeneratePasswordResetToken(toEmail);
            string emailBody = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Password Reset</title>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
      color: #333;
    }}
    .container {{
      width: 100%;
      max-width: 600px;
      margin: 0 auto;
      background-color: #fff;
      padding: 20px;
      border-radius: 8px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }}
    .header {{
      text-align: center;
      margin-bottom: 20px;
    }}
    .button {{
      display: inline-block;
      background-color: #F24822;
      color: white;
      padding: 14px 20px;
      text-align: center;
      font-size: 16px;
      border-radius: 5px;
      text-decoration: none;
      width: 100%;
      max-width: 250px;
      margin: 20px auto;
      transition: background-color 0.3s;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <div class=""header"">
      <h2>Password Reset Request</h2>
    </div>
    <p>Dear {vendor.FirstName + " " + vendor.LastName},</p>
    <p>We received a request to reset your password. To proceed, please click the button below:</p>
    <a href=""https://yourwebsite.com/reset-password?token={token}"" class=""button"">Reset Your Password</a>
    <p>If you did not request a password reset, please ignore this email.</p>
    <p>Thank you,</p>
    <p>The Support Team</p>
  </div>
</body>
</html>";
            if (toEmail == "")
                return false;

            try
            {
                var smtpSettings = _config.GetSection("SmtpSettings");

                var fromEmail = smtpSettings["FromEmail"];
                var smtpServer = smtpSettings["SmtpServer"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Password Reset",
                    Body = emailBody,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                using var smtpClient = new SmtpClient(smtpServer, port);
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}