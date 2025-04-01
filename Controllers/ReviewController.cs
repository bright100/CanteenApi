using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.ReviewService;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performing review tasks
    /// </summary>
    /// <param name="reviewService">review service</param>
    /// <param name="mapper">Mapper service</param>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService reviewService, IMapper mapper) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Add Review
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/review/addreview
        /// </remarks>
        /// <param name="review">The review detail</param>
        /// <returns>True or False</returns>
        [HttpPost("addreview")]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> AddReview([FromBody] ReviewsDto review)
        {
            try
            {
                var created = await _reviewService.CreateReview(review);
                return created ? Ok(new ResponseWrapper<bool?> { ResponseCode = 200, ResponseMessage = "Review created Successfully", Data = created })
                    : BadRequest(new ResponseWrapper<bool?> { Data = null, ResponseCode = 400, ResponseMessage = "Review Creation Failed" });
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, title: "Opps Something went wrong", detail: ex.Message);
            }
        }

        /// <summary>
        /// Get review by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/review/getreview
        /// </remarks>
        /// <param name="id">The Review Id</param>
        /// <returns>A review</returns>
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getreviewbyid/{id}")]
        public async Task<ActionResult<ResponseWrapper<ReviewsResponseDto>>> GetReveiwsById(Guid id)
        {
            try
            {
                var review = await _reviewService.GetReviewById(id);
                return Ok(new ResponseWrapper<ReviewsResponseDto> { Data = _mapper.Map<ReviewsResponseDto>(review), ResponseCode = 200, ResponseMessage = "Data gotten Successfully" });

            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }

        /// <summary>
        /// Get review by employee name
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/review/getreview
        /// </remarks>
        /// <param name="employeeName">The employee name</param>
        /// <param name="vendorName">The vendor name</param>
        /// <param name="page">The starting page</param>
        /// <param name="pageSize">How many u want</param>
        /// <returns>A review</returns>
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getreviewbyemployeename/{employeeName}")]
        public async Task<ActionResult<ResponseWrapper<ReviewsResponseDto>>> GetReview(string employeeName, string vendorName, int page, int pageSize)
        {
            try
            {
                var review = await _reviewService.GetReview(employeeName, vendorName, page == 0 ? 1 : page, pageSize == 0 ? 1 : pageSize);
                return Ok(new ResponseWrapper<ReviewsResponseDto> { Data = _mapper.Map<ReviewsResponseDto>(review), ResponseCode = 200, ResponseMessage = "Data gotten Successfully" });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }

        /// <summary>
        /// Get review by vendor name
        /// </summary>
        /// <param name="vendorName">The Vendor Name</param>
        /// <param name="page">The starting page</param>
        /// <param name="pageSize">The Page Size</param>
        /// <returns>The Review detail.</returns>
        [HttpGet("getreviewbyvendorname/{vendorName}")]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<ReviewsResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<ReviewsResponseDto>>> GetReviewByVendorName(string vendorName, int page, int pageSize)
        {
            try
            {
                var review = await _reviewService.GetReviewByVendorName(vendorName, page == 0 ? 1 : page, pageSize == 0 ? 1 : pageSize);
                return Ok(new ResponseWrapper<ReviewsResponseDto> { Data = _mapper.Map<ReviewsResponseDto>(review), ResponseCode = 200, ResponseMessage = "Data gotten Successfully" });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }
    }
}
