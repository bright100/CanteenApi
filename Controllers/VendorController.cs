using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performing vendor tasks
    /// </summary>
    /// <param name="vendorService"></param>
    /// <param name="mapper"></param>
    [Route("api/vendor")]
    [ApiController]
    public class VendorController(IVendorService vendorService, IMapper mapper) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Delete vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST /api/vendor/deletevendor
        /// Returns a response wrapper with success or failed message
        /// </remarks>
        /// <param name="email">The vendor email</param>
        /// <returns>bool</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("deletevendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> DeleteVendor([EmailAddress] string email)
        {
            try
            {
                bool data = await _vendorService.DeleteVendor(email);
                return await _vendorService.DeleteVendor(email) ? Ok(new { ResponseCode = 200, ResponseMessage = "Deleted", Data = data })
                   : BadRequest("Could not delete user make sure user exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }



        /// <summary>
        /// Get vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// POST /api/vendor/getvendor
        /// Returns a response wrapper with Vendor details
        /// </remarks>
        /// <param name="email">The vendor email</param>
        /// <param name="password">The vendors password</param>
        /// <returns>VendorResponse</returns>
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getvendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<VendorResponse>>> GetVendor([EmailAddress] string email, string password)
        {
            try
            {
                var Vendor = _mapper.Map<VendorResponse>(await _vendorService.GetVendor(email, password));
                return Vendor is not null ?
                    Ok(new { ResponseCode = 200, ResponseMessage = "Success", Data = Vendor }) :
                    BadRequest("Invalid User");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Update vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// PUT /api/vendor/updatevendor
        /// Returns a response wrapper with success or failed message
        /// </remarks>
        /// <param name="vendor">The vendor details</param>
        /// <returns>VendorResponse</returns>
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<VendorResponse?>), StatusCodes.Status500InternalServerError)]
        [HttpPut("updatevendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> UpdateVendor(VendorDto vendor)
        {
            try
            {
                bool data = await _vendorService.UpdateVendor(vendor);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Updated", Data = data })
                    : BadRequest("Could not update user make sure user exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Activate vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// PUT /api/vendor/acivatevendor
        /// Returns a response wrapper with success or failed message
        /// </remarks>
        /// <param name="email">The vendor email</param>
        /// <returns>bool or null</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpPut("activatevendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> ActivateVendor([EmailAddress] string email)
        {
            try
            {
                bool data = await _vendorService.ActivaeVendor(email);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Activated", Data = data })
                    : BadRequest("Could not activate user make sure user exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Deactivate vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// PUT /api/vendor/deactivatevendor
        /// Returns a response wrapper with success or failed message
        /// </remarks>
        /// <param name="email">The vendor email</param>
        /// <returns>bool or null</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpPut("deactivatevendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> DeactivateVendor([EmailAddress] string email)
        {
            try
            {
                bool data = await _vendorService.DeactivateVendor(email);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Deactivated", Data = data })
                    : BadRequest("Could not deactivate user make sure user exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Suspend vendor
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// DELETE /api/vendor/suspendvendor
        /// Returns a response wrapper with success or failed message
        /// </remarks>
        /// <param name="email">The vendor email</param>
        /// <returns>bool or null</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("suspendvendor")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<bool>>> SuspendVendor([EmailAddress] string email)
        {
            try
            {
                bool data = await _vendorService.SuspendVendor(email);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Suspended", Data = data })
                    : BadRequest("Could not suspend user make sure user exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Get all vendors
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// GET /api/vendor/getallvendors
        /// Returns a response wrapper with all vendors
        /// </remarks>
        /// <returns>List of all vendors</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getallvendors")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<List<VendorResponse>>>> GetAllVendor()
        {
            try
            {
                List<VendorResponse> vendors = _mapper.Map<List<VendorResponse>>(await _vendorService.GetAllVendor());
                return Ok(vendors);
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Changes the vendors branch
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /api/vendor/changevendorbranch
        /// </remarks>
        /// <param name="email">vendors email address</param>
        /// <param name="branchName">The branch name</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [HttpPut("changevendorbranch")]
        public async Task<ActionResult<ResponseWrapper<bool>>> ChangeVendorBranch([EmailAddress] string email, [MinLength(5)]string branchName)
        {
            try
            {
                bool data = await _vendorService.ChangeBranch(email, branchName);
                return data ? Ok(new { ResponseCode = 200, ResponseMessage = "Branch Changed", Data = data })
                    : BadRequest("Could not change branch make sure branch exists.");
            }
            catch (Exception)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: "Sorry something went wrong on our end");
            }
        }

        /// <summary>
        /// Gets the vendor by branch name
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/vendor/getvendorbybranchname
        /// </remarks>
        /// <param name="branchName">name of the branch</param>
        /// <returns>list of the vendors</returns>
        
        [HttpGet("getvendorbybranchname")]
        [ProducesResponseType(typeof(ResponseWrapper<List<VendorDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<List<VendorDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<List<VendorDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<List<VendorDto>>>> GetVendorByBranchName([MinLength(5)]string branchName)
        {
            var vendors = _mapper.Map<List<VendorDto>>(await _vendorService.GetVendorsByBranchName(branchName));
            return Ok(new { ResponseCode = 200, ResponseMessage = "Success", Data = vendors } );
        }

        /// <summary>
        /// Open canteen
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /api/vendor/opencanteen
        /// </remarks>
        /// <param name="vendorName">Vendor Name</param>
        /// <returns>True or False</returns>
        [HttpPut("openCanteen")]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> OpenCanteen(string vendorName)
        {
            try
            {
                var result = await _vendorService.OpenCanteen(vendorName);
                return result ? Ok(new ResponseWrapper<bool> { Data = result, ResponseCode = 200, ResponseMessage = "Success" }) 
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Sorry could not open canteen check if vendor exists"});
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Close canteen
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /api/vendor/opencanteen
        /// </remarks>
        /// <param name="vendorName"></param>
        /// <returns></returns>
        [HttpPut("closeCanteen")]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin, Vendor")]
        public async Task<ActionResult<ResponseWrapper<bool>>> CloseCanteen(string vendorName)
        {
            try
            {
                var response = await _vendorService.CloseCanteen(vendorName);
                return response ? Ok(new ResponseWrapper<bool> { Data = response, ResponseCode = 200, ResponseMessage = "Success" })
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Sorry could not close canteen check if vendor exists" });
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }
    }
}
