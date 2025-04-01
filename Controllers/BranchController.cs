using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.BranchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performing branch tasks
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BranchController(IBranchService branchService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBranchService _branchService = branchService;

        /// <summary>
        /// Create a new branch
        /// </summary>
        /// <remarks>
        /// Sample request
        /// POST /api/branch/createbranch
        /// </remarks>
        /// <param name="branch">The branch data</param>
        /// <returns>A response of sucessfull or failed</returns>
        [HttpPost("createbranch")]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool?>>> CreateBranch(BranchDto branch)
        {
            try
            {
                var data = await _branchService.CreateBranch(branch);
                return data is not null ?
                    Ok(new ResponseWrapper<bool?>() { ResponseCode = 201, ResponseMessage = "Branch Created Successfully", Data = data })
                    : BadRequest(new ResponseWrapper<bool?>() { ResponseCode = 400, ResponseMessage = "Branch Creation Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Get a branch
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/branch/getbranch
        /// </remarks>
        /// <param name="branchName">Branch name</param>
        /// <returns>branch details</returns>
        [HttpGet("getbranch")]
        [ProducesResponseType(typeof(ResponseWrapper<BranchDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<BranchDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<BranchDto?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<BranchDto?>>> GetBranch(string branchName)
        {
            try
            {
                var data = await _branchService.GetBranch(branchName);
                return data is not null ?
                    Ok(new ResponseWrapper<BranchDto?>() { ResponseCode = 200, ResponseMessage = "Branch Found", Data = _mapper.Map<BranchDto>(data) })
                    : BadRequest(new ResponseWrapper<BranchDto?>() { ResponseCode = 400, ResponseMessage = "Branch Not Found", Data = _mapper.Map<BranchDto>(data) });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Update a branch
        /// </summary>
        /// <remarks>
        /// Sample request
        /// PUT /api/branch/deletebranch
        /// </remarks>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpPut("updatebranch")]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> UpdateBranch(BranchDto branch)
        {
            try
            {
                var data = await _branchService.UpdateBranch(branch);
                return data ?
                    Ok(new ResponseWrapper<bool>() { ResponseCode = 200, ResponseMessage = "Branch Updated Successfully", Data = data })
                    : BadRequest(new ResponseWrapper<bool>() { ResponseCode = 400, ResponseMessage = "Branch Update Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Delete a branch
        /// </summary>
        /// <remarks>
        /// Sample erequest
        /// DELETE /api/branch/deletebranch
        /// </remarks>
        /// <param name="branchName"></param>
        /// <returns></returns>
        [HttpDelete("deletebranch")]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> DeleteBranch(string branchName)
        {
            try
            {
                var data = await _branchService.DeleteBranch(branchName);
                return data ?
                    Ok(new ResponseWrapper<bool>() { ResponseCode = 200, ResponseMessage = "Branch Deleted Successfully", Data = data })
                    : BadRequest(new ResponseWrapper<bool>() { ResponseCode = 400, ResponseMessage = "Branch Deletion Failed", Data = data });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }
    }
}
