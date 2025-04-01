using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.AuthService;
using LeadwaycanteenApi.Services.DbServices.AdminService;
using LeadwaycanteenApi.Services.DbServices.BranchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performin different admin tasks
    /// </summary>
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController(IAdminService adminService, IBranchService branchService, IAuthService authService, IMapper mapper) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;
        private readonly IAuthService _authService = authService;
        private readonly IBranchService _branchService = branchService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Add to admin
        /// </summary>
        /// <remarks>
        /// Sample request
        /// POST /api/admin/addadmin
        /// Returns a response wrapper with a success or failded message
        /// </remarks>
        /// <param name="email">The user email</param>
        /// <returns>true, false or null</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpPost("addadmin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> AddAdmin([EmailAddress]string email)
        {
            try
            {
                bool data = await _adminService.AddAdmin(email);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Added", Data = data })
                    : BadRequest("Could not add admin make sure user does not exist.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Creates a new vendor
        /// </summary>
        /// <remarks>
        /// Sample request
        /// POST /api/admin/createvendor
        /// Returns a response wrapper with a success or failded message
        /// </remarks>
        /// <param name="vendor">The vendor details</param>
        /// <param name="branchName">The branch name</param>
        /// <returns>true or false</returns>
        [HttpPost("createvendor")]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool?>>> CreateVendor(VendorDto vendor, [MinLength(5)]string branchName)
        {
            try
            {
                var branch = await _branchService.GetBranch(branchName);
                if (branch is null)
                    return BadRequest(new ResponseWrapper<bool?>() { ResponseCode = 400, ResponseMessage = "Branch does not exist", Data = null });

                Vendor vendor1 = _mapper.Map<Vendor>(vendor);
                vendor1.Branch = branch;
                var data = await _authService.CreateVendor(vendor1);

                return data is not null ?
                    Ok(new ResponseWrapper<bool?>() { ResponseCode = 201, ResponseMessage = "Vendor Registered Successfully", Data = data })
                    : BadRequest(new ResponseWrapper<bool?>() { ResponseCode = 400, ResponseMessage = "Vendor Registration Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }
    }
}
