using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.AuthService;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using LeadwaycanteenApi.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performing authentication tasks 
    /// </summary>
    /// <param name="authService">The authentication service</param>
    /// <param name="mapper">Automapper</param>
    /// <param name="vendorService">The vendor service</param>
    /// <param name="emailService">The email service</param>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IMapper mapper, IVendorService vendorService, IEmailService emailService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private readonly IVendorService _vendorService = vendorService;

        /// <summary>
        /// Login vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST /api/auth/loginvendor
        /// Returns a response wrapper with Vendor details and access token
        /// </remarks>
        /// <param name="user">The vendor email and password</param>
        /// <returns>VendorResponseDto</returns>
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponseDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponseDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponseDto?>), StatusCodes.Status500InternalServerError)]
        [HttpGet("loginvendor")]
        public async Task<ActionResult<ResponseWrapper<VendorResponseDto?>>> VendorLogin([FromQuery] UserLoginDto user)
        {
            try
            {
                var data = await _authService.LoginVendor(user.Email, user.Password);

                return data is not null ?
                    Ok(new ResponseWrapper<VendorResponseDto?> { ResponseCode = 200, ResponseMessage = "User Logged In", Data = new() { Vendor = data.Vendor, Accesstoken = data.Accesstoken } })
                    : BadRequest(new ResponseWrapper<VendorResponseDto?>() { ResponseCode = 400, ResponseMessage = "User Login Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);

            }
        }

        /// <summary>
        /// Login staff
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST /api/auth/login
        /// Returns a response wrapper with Employee details and access token
        /// </remarks>
        /// <param name="user">The Employee email and password</param>
        /// <returns>EmployeeResponse</returns>
        [ProducesResponseType(typeof(ResponseWrapper<EmployeeResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<EmployeeResponse?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<EmployeeResponse?>), StatusCodes.Status500InternalServerError)]
        [HttpGet("login")]
        public async Task<ActionResult<ResponseWrapper<EmployeeResponseDto?>>> Login([FromQuery] EmployeeLoginDto user)
        {
            try
            {
                var data = await _authService.Login(user.Email, user.Password);

                return data is not null ?
                    Ok(new ResponseWrapper<EmployeeResponseDto?>() { ResponseCode = 200, ResponseMessage = "Login Successfull", Data = new EmployeeResponseDto() { Accesstoken = data.Accesstoken, Employee = _mapper.Map<EmployeeResponse>(data.Employee) } })
                    : BadRequest(new ResponseWrapper<EmployeeResponse?>() { ResponseCode = 400, ResponseMessage = "User Login Failed", Data = null });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Set vendor password
        /// </summary>
        /// <remarks>
        /// Sample request
        /// POST /api/auth/setvendorpassword
        /// Returns a response wrapper with Employee details and access token
        /// </remarks>
        /// <param name="user">Contains the user email and password</param>
        /// <returns>bool or null</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpPost("setvendorpassword")]
        [Authorize(Roles = "Vendor, Admin")]
        public async Task<ActionResult<ResponseWrapper<bool?>>> SetVendorPassword([FromQuery]UserLoginDto user)
        {
            try
            {
                var vendor = await _vendorService.GetVendor(user.Email);
                if(vendor is null)
                    return BadRequest(new ResponseWrapper<bool?>() { ResponseCode = 204, ResponseMessage = "Vendor Not Found", Data = null });

                var data = await _authService.SetVendorPassword(user.Email, user.Password);
                return data ?
                    Ok(new ResponseWrapper<bool?>() { ResponseCode = 200, ResponseMessage = "Password Set Successfully", Data = data })
                    : BadRequest(new ResponseWrapper<bool?>() { ResponseCode = 400, ResponseMessage = "Password Set Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Refreshes users access token
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/refresh
        /// </remarks>
        /// <param name="email">user email</param>
        /// <param name="oldAccessToken">old acess token</param>
        /// <param name="role">user role</param>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<UserLoginResponseDto?>>> Refresh([EmailAddress]string email, string oldAccessToken, string role)
        {
            try
            {
                var refreshToken = await _authService.RefreshToken(email, oldAccessToken, role);
                return refreshToken is null ? 
                    Unauthorized(new ResponseWrapper<UserLoginResponseDto?> { ResponseCode = 201, ResponseMessage = "User not authorized", Data = null })
                : Ok(new ResponseWrapper<UserLoginResponseDto> { Data = refreshToken, ResponseMessage = "Token Ready", ResponseCode = 200 });
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong could not refresh", detail: ex.Message);
            }
        }

        /// <summary>
        /// Sends a reset token
        /// </summary>
        /// <param name="email">vendors email</param>
        /// <returns>true or false</returns>
        [HttpPost("sendresettoken")]
        [Authorize(Roles = "Vendor")]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> SendResetTokenMail([EmailAddress]string email)
        {
            try
            {
                return await _emailService.SendMessageViaSmtp(email) ? Ok(new ResponseWrapper<bool> { Data = true, ResponseCode = 200, ResponseMessage = "Sent" })
                    : NotFound(new ResponseWrapper<bool> { ResponseMessage = "Could not send", ResponseCode = 404, Data = false });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong could not refresh", detail: ex.Message);
            }
        }

        /// <summary>
        /// Validates a reset token
        /// </summary>
        /// <param name="email">vendors email</param>
        /// <param name="resetToken">the reset token to be validated</param>
        /// <returns>true or false</returns>
        [HttpGet("validateresettoken")]
        [Authorize(Roles = "Vendor")]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<UserLoginResponseDto?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> ValidateResetToken([EmailAddress] string  email, string resetToken)
        {
            try
            {
                return await _authService.ValidatePasswordResetToken(email, resetToken) ? Ok(new ResponseWrapper<bool> { Data = true, ResponseCode = 200, ResponseMessage = "Valid" })
                    : NotFound(new ResponseWrapper<bool> { Data = false, ResponseMessage = "😒😒🤨 Invalid Attempt", ResponseCode = 404 });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong could not refresh", detail: ex.Message);
            }
        }
    }
}
