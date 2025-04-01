using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.EmployeeService;
using LeadwaycanteenApi.Services.DbServices.JwtManager;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using LeadwaycanteenApi.Services.JwtService;
using LeadwaycanteenApi.util;
using LeadwaycanteenApi.Wrappers;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace LeadwaycanteenApi.Services.AuthService
{
    /// <summary>
    /// Controller for AuthServices
    /// </summary>
    /// <param name="config">configuration service</param>
    /// <param name="jwtService">token service</param>
    /// <param name="jwtManager">token manage service</param>
    /// <param name="mapper">mapper service</param>
    /// <param name="vendorService">vendor service</param>
    /// <param name="employeeService">employee service</param>
    /// <param name="validationUtility">validation service</param>
    public class AuthService(IConfiguration config, IJwtService jwtService, IJwtManager jwtManager, IMapper mapper, IVendorService vendorService, IEmployeeService employeeService, ValidationUtility validationUtility) : IAuthService
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IConfiguration _config = config;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IJwtManager _jwtManager = jwtManager;
        private readonly IMapper _mapper = mapper;
        private readonly IVendorService _vendorService = vendorService;
        private readonly ValidationUtility _validationUtility = validationUtility;


        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="password"></param>
        /// <returns>UserLoginResponse</returns>
        public async Task<UserLoginResponseDto?> Login([RegularExpression("^[a-z]+-[a-z]+@leadway\\.com$")]string Email, string password)
        {
            ConsoleClass<string>.LogInfo("Logged");
            var newHash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var storedUser = await new DbWrapper<Employee>(_config)
                .QueryFirstOrDefaultAsync("SELECT * FROM Employee WHERE Email = @Email AND PasswordHash = @PasswordHash", new { Email, PasswordHash = newHash });

            if (storedUser == null)
            {
                var staff = await _validationUtility.ValidateEmailWithLeadway(Email, password);
                if(staff == null) return null;

                storedUser = _mapper.Map<Employee>(await _employeeService.CreateEmployee(new EmployeesResponse()
                {
                    Email = Email,
                    FullName = staff.FullName,
                    Bio = "Employee",
                    Password = password,
                    UserImageUrl = "https://leadway.com"
                }));
            }

            if (storedUser == null) return null;

            var sat = await _jwtManager.GetStoredRefreshToken(Email, storedUser.Roles);
            var token = _jwtService.GenerateJwtToken(storedUser.Email, storedUser.Roles);
            if (token == null) return null;

            if (sat is not null)
            {
                _jwtManager.DeleteStoredRefreshToken(sat.TokenId);
            }

            await _jwtManager.StoreRefreshTokenAsync(new UserRefreshTokenModel()
            {
                Revoked = false,
                TokenHash = Encoding.UTF8.GetBytes(token.RefreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(31),
                Employee = storedUser
            });

            return new () { Accesstoken = token.AccessToken, Employee =  _mapper.Map<EmployeesResponse>(storedUser) };
        }

        /// <summary>
        /// Login a vendor
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="password"></param>
        /// <returns>Vendor details</returns>
        public async Task<VendorResponseDto?> LoginVendor(string Email, string password)
        {
            var newHash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var storedUser = await new DbWrapper<Vendor>(_config)
                .QueryFirstOrDefaultAsync("SELECT * FROM Vendor WHERE Email = @Email AND PasswordHash = @PasswordHash", new { Email, PasswordHash = newHash });

            if (storedUser == null)
                return null;

            var token = _jwtService.GenerateJwtToken(storedUser.Email, "Vendor");
            if (token == null) return null;
            await _jwtManager.StoreRefreshTokenAsync(new UserRefreshTokenModel()
            {
                Revoked = false,
                TokenHash = Encoding.UTF8.GetBytes(token.RefreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(31),
                Vendor = storedUser,
                TokenId = Guid.NewGuid(),
            });

            return new() { Accesstoken = token.AccessToken, Vendor = _mapper.Map<VendorDto>(storedUser) };
        }

        /// <summary>
        /// Refresh a token
        /// </summary>
        /// <param name="Email">user email address</param>
        /// <param name="token">access token</param>
        /// <param name="role">user role</param>
        /// <returns>UserLoginResponse</returns>
        public async Task<UserLoginResponseDto?> RefreshToken(string Email, string token, string role)
        {
            var principal = _jwtService.GetExpiredClaimsPrincipal(token);
            var email = principal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email);

            var srt = await _jwtManager.GetStoredRefreshToken(Email, role);

            if (srt == null || srt.ExpiresAt < DateTime.UtcNow) return null;
            _jwtManager.DeleteStoredRefreshToken(srt.TokenId);


            if (email?.Value != Email || srt.Revoked)
                return null;

            var retoken = _jwtService.GenerateRefreshToken(Email, role);

            if (retoken == null)
                return null;

            srt.TokenHash = Encoding.UTF8.GetBytes(retoken.RefreshToken);
            srt.ExpiresAt = DateTime.UtcNow.AddDays(31);
            srt.UpdatedAt = DateTime.UtcNow;

            await _jwtManager.StoreRefreshTokenAsync(srt);
            return _mapper.Map<UserLoginResponseDto>(retoken);
        }

        /// <summary>
        /// Create a vendor
        /// </summary>
        /// <param name="User">vendor details</param>
        /// <returns>true false or null</returns>
        public async Task<bool?> CreateVendor(Vendor User)
        {
            bool created = await _vendorService.CreateVendor(User);
            if (created)
                return true;
            return null;
        }

        /// <summary>
        /// Revoke a token
        /// </summary>
        /// <param name="Email">user email address</param>
        /// <param name="token">access token</param>
        /// <param name="role">user role</param>
        /// <returns>true or false</returns>
        public async Task<bool> RevokeToken(string Email, string token, string role)
        {
            var principal = _jwtService.GetExpiredClaimsPrincipal(token);
            var email = principal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email);

            var srt = await _jwtManager.GetStoredRefreshToken(Email, role);

            if (email?.Value != Email || srt == null || srt.ExpiresAt < DateTime.UtcNow)
                return false;

            await _jwtManager.RevokeRefreshTokenAsync(srt);
            return true;
        }

        /// <summary>
        /// Set vendor password
        /// </summary>
        /// <param name="Email">user email address</param>
        /// <param name="password">user password</param>
        /// <returns>true or false</returns>
        public Task<bool> SetVendorPassword(string Email, string password)
        {
            var passwordHash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password));
            DbWrapper<Vendor> vconnection = new(_config);
            var sql = "UPDATE Vendor SET PasswordHash = @PasswordHash WHERE Email = @Email";
            return vconnection.ExecuteAsync(sql, new { Email, PasswordHash = passwordHash });
        }

        /// <summary>
        /// Validates a reset token with whats stored in the database
        /// </summary>
        /// <param name="email">vendor email</param>
        /// <param name="passwordResetToken">the reset token</param>
        /// <returns>true or false</returns>
        public async Task<bool> ValidatePasswordResetToken(string email, string passwordResetToken)
        {
            DbWrapper<PasswordResetToken> dbWrapper = new(_config);
            var sql = "SELECT p.* FROM PasswordResetTokens JOIN Vendor v ON p.VendorId = v.VendorId WHERE v.Email = @email";
            var Token = await dbWrapper.QuerySingleOrDefaultAsync(sql, email);

            if (Token == null || Token.IsCancelled) return false;

            var sql2 = "UPDATE FROM PasswordResetTokens SET IsCancelled = 1 WHERE TokenId = @TokenId";
            var canceled = await dbWrapper.ExecuteAsync(sql2, Token.TokenId);

            return Token.Token == passwordResetToken && canceled;
        }
    }
}
