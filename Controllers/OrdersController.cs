using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.OrderService;
using LeadwaycanteenApi.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Controller for performing order tasks
    /// </summary>
    [Route("api/order")]
    [ApiController]
    public class OrdersController(IOrderService orderService, IDbOrderService dbOrderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IDbOrderService _dbOrderService = dbOrderService;

        /// <summary>
        /// Creates an Order
        /// </summary>
        /// <param name="fooditems">A list of food items</param>
        /// <param name="vendorName">The vendor Id</param>
        /// <param name="employeeMail">The employee Mail</param>
        /// <returns>Order information</returns>
        [HttpPost("createorder")]
        [ProducesResponseType(typeof(ResponseWrapper<OrdersDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<OrdersDto?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<OrdersDto?>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<OrdersDto?>>> CreateOrder(List<OrdersInventoryItemRequestDto> fooditems, string vendorName, string employeeMail)
        {
            try
            {
                var result = await _orderService.CreateOrder(fooditems, vendorName, employeeMail);
                return result.Count != 0 ? Ok(new ResponseWrapper<List<OrdersDto>> { Data = result, ResponseCode = 200, ResponseMessage = "Created" }) :
                    BadRequest(new ResponseWrapper<OrdersDto?> { ResponseMessage = "Could not create order", ResponseCode = 400, Data = null});
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Ooopps Something went wrong");
            }
        }

        /// <summary>
        /// Canceles an Order
        /// </summary>
        /// <param name="OrderId">The order id</param>
        /// <returns>True or False</returns>
        [HttpPut("cancleorder")]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> CancelOrder(Guid OrderId)
        {
            try
            {
                return await _dbOrderService.CanceleOrder(OrderId) ? Ok(new ResponseWrapper<bool> { Data = true, ResponseCode = 200, ResponseMessage = "Order Cancled" }) 
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseMessage = " Could not find such Oder", ResponseCode = 400 });
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps somehing went wrong");
            }
        }

        /// <summary>
        /// Adds a food item
        /// </summary>
        /// <param name="foodItem">Food item detail</param>
        /// <param name="orderId">The id of the order</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPost("addfooditem")]
        public async Task<ActionResult<ResponseWrapper<bool>>> AddFoodItemToOrder(OrdersInventoryItemRequestDto foodItem, Guid orderId)
        {
            try
            {
                var result = await _dbOrderService.AddFoodItemToOrder(foodItem, orderId);
                return result ? Ok(new ResponseWrapper<bool> { Data = true, ResponseCode = 200, ResponseMessage = "Successfully Added" })
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseMessage = "Could not add check item", ResponseCode = 400 });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="Orderid">The id of the order</param>
        /// <returns>An order value</returns>
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getorder/{Orderid}")]
        public async Task<ActionResult<ResponseWrapper<List<OrdersDto>>>> GetOrder(Guid Orderid)
        {
            try
            {
                var order = (await _dbOrderService.GetOrder(Orderid)).ToList();
                return order is not null ? Ok(new ResponseWrapper<List<OrdersDto>> { Data = order,  ResponseCode = 200, ResponseMessage = "Request Succsessfull"})
                    : BadRequest(new ResponseWrapper<OrdersDto> { ResponseMessage = "Could not get order check id", ResponseCode = 400, Data = null});
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }

        /// <summary>
        /// Removes an item from an order
        /// </summary>
        /// <param name="fooditemname">The food item name</param>
        /// <returns>True or False</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("removeitemfromorder/{fooditemname}")]
        public async Task<ActionResult<ResponseWrapper<bool>>> RemoveItemFromOrder(string fooditemname)
        {
            try
            {
                var removed = await _dbOrderService.RemoveFoodItemFromOrder(fooditemname);
                return removed ? Ok(new ResponseWrapper<bool> { ResponseMessage = "Request Successfull", ResponseCode = 200, Data = removed })
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Could not get item check name" });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }

        /// <summary>
        /// Complete Order
        /// </summary>
        /// <param name="orderId">The Order Id</param>
        /// <returns>True or False</returns>
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<bool>), StatusCodes.Status500InternalServerError)]
        [HttpPut("markorderascompleted/{orderId}")]
        public async Task<ActionResult<ResponseWrapper<bool>>> MarkOrderAsCompleted(Guid orderId)
        {
            try
            {
                var marked = await _dbOrderService.MarkOrderAsCompleted(orderId);
                return marked ? Ok(new ResponseWrapper<bool> { Data = marked, ResponseCode = 200, ResponseMessage = "Marked as completed" }) 
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseMessage = "Could not complete order", ResponseCode = 400 });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }

        /// <summary>
        /// Gets recent order
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/orders/getrecentorder
        /// </remarks>
        /// <param name="vendorname">The vendor Name</param>
        /// <param name="employeeMail">The employee Name</param>
        /// <returns>The recent order</returns>
        [HttpGet("getrecentorder/{vendorname}/{employeeMail}")]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<List<OrdersDto>>>> GetRecentOrder(string vendorname, string employeeMail)
        {
            try
            {
                var response = await _dbOrderService.GetRecentOrder(vendorname, employeeMail);
                return response is not null ? Ok(new ResponseWrapper<List<OrdersDto>> { Data = [.. response], ResponseCode = 200, ResponseMessage = "Request Successfull" }) 
                    : BadRequest(new ResponseWrapper<List<OrdersDto>> { Data = [], ResponseCode = 400, ResponseMessage = "Could not get order" });
            }
            catch(Exception ex) {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <remarks>
        /// page size and page must be grater than 0
        /// Sample Request:
        /// GET /api/orders/getallorders/1/1
        /// </remarks>
        /// <param name="page">start</param>
        /// <param name="pageSize">how many data u need</param>
        /// <returns>a list of orders in that range</returns>
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getallorders/{page}/{pageSize}")]
        public async Task<ActionResult<ResponseWrapper<List<OrdersDto>>>> GetAllOrders(int page, int pageSize)
        {
            try
            {
                var res = await _dbOrderService.GetAllOrders(page == 0 ? 1 : page, pageSize == 0 ? 1 : pageSize);
                return Ok(new ResponseWrapper<List<OrdersDto>> { Data = [.. res], ResponseCode = 200, ResponseMessage = "Request Successfull" });
            }catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }

        /// <summary>
        /// Gets all vendor orders
        /// </summary>
        /// <remarks>
        /// Note the page size must not be less than 0
        /// </remarks>
        /// <param name="vendorname">the vendor name</param>
        /// <param name="page">the page start</param>
        /// <param name="pageSize">how many data u need</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseWrapper<List<OrdersDto>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("getallordersbyvendor/{vendorname}/{page}/{pageSize}")]
        public async Task<ActionResult<ResponseWrapper<List<OrdersDto>>>> GetAllOrdersByVendor(string vendorname, int page, int pageSize) 
        {
            try
            {
                var res = await _dbOrderService.GetAllVendorsOrders(vendorname, page == 0 ? 1 : page, pageSize == 0 ? 1 : pageSize);
                return Ok(new ResponseWrapper<List<OrdersDto>> { Data = [.. res], ResponseCode = 200, ResponseMessage = "Request Successfull" });
            }catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps something went wrong");
            }
        }
    }
}
