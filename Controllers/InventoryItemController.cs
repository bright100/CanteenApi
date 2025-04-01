using System.ComponentModel.DataAnnotations;
using System.Net;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.InventoryItemService;
using Microsoft.AspNetCore.Mvc;

namespace LeadwaycanteenApi.Controllers
{
    /// <summary>
    /// Inventory Item Controller
    /// </summary>
    /// <param name="inventoryItemService">inventory service</param>
    [Route("api/inventoryitem")]
    [ApiController]
    public class InventoryItemController(IInventoryItemService inventoryItemService) : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService = inventoryItemService;

        /// <summary>
        /// Get all inventory items
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/inventoryitem/getitembyid/{inventoryId}
        /// </remarks>
        /// <param name="vendorname"></param>
        /// <param name="page">starting page</param>
        /// <param name="pageSize">how many u want</param>
        /// <returns>Returns a list of inventory items</returns>

        [HttpGet("getall/{vendorname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseWrapper<List<InventoryResponseDto>>>> GetAllInventoryItems([MinLength(5)] string vendorname, int page, int pageSize)
        {
            var inventoryItems = await _inventoryItemService.GetAllItems(vendorname, page == 0 ? 1: page, pageSize == 0 ? 1 : pageSize);
            return Ok(new ResponseWrapper<List<InventoryResponseDto>> { ResponseCode = 200, ResponseMessage = "Data gotten Successfully", Data = inventoryItems });
        }


        /// <summary>
        /// Create inventory item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/inventoryitem/createinventoryitem
        /// </remarks>
        /// <param name="inventoryItem">details of item to be created</param>
        /// <returns>return true,  false or null</returns>
        [HttpPost("createinventoryitem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> CreateInventoryItem(InventoryItemReqDto inventoryItem)
        {
            try
            {
                var result = await _inventoryItemService.CreateInventoryItem(inventoryItem);
                return result is not null ? Ok(new ResponseWrapper<bool?> { ResponseCode = 200, ResponseMessage = "Data created Successfully", Data = result })
                    : BadRequest(new ResponseWrapper<bool?> { Data = null, ResponseCode = 400, ResponseMessage = "Error Creating Item Check the item for missing fields" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<bool?> { Data = null, ResponseCode = 500, ResponseMessage = ex.Message });
            }
        }

        /// <summary>
        /// Add item to inventory
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/inventoryitem/additemtoinventory/{inventoryId}
        /// </remarks>
        /// <param name="fooditemname">food item name</param>
        /// <param name="vendorname">vendor name</param>
        /// <returns>true, false or null</returns>
        [HttpPost("additemtoinventory/{vendorname}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool?>>> AddItemToInventory([MinLength(5)] string vendorname, [MinLength(3)] string fooditemname)
        {
            try
            {
                var result = await _inventoryItemService.AddItemToInventory(vendorname, fooditemname);
                return result ? Ok(new ResponseWrapper<bool?> { ResponseCode = 201, ResponseMessage = "Data created Added Successfully", Data = result })
                    : BadRequest(new ResponseWrapper<bool?> { Data = null, ResponseCode = 400, ResponseMessage = "Error Creating Item Check the item for missing fields" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<bool?> { Data = null, ResponseCode = 500, ResponseMessage = ex.Message });
            }
        }

        /// <summary>
        /// Create and add
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/inventoryitem/createandadd/{vendorname}
        /// </remarks>
        /// <param name="fooditem">food item details</param>
        /// <param name="vendorname">vendor name</param>
        /// <returns>true, false or null</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("createandadd/{vendorname}")]
        public async Task<ActionResult<ResponseWrapper<bool?>>> CreateAndAddToInventory(InventoryItemReqDto fooditem, [MinLength(5)] string vendorname)
        {
            try
            {
                var result = await _inventoryItemService.CreateInventoryItem(fooditem);
                if (result == false || result is null)
                    return BadRequest(new ResponseWrapper<bool?> { Data = null, ResponseCode = 400, ResponseMessage = "Could not create item check your item." });

                var added = await _inventoryItemService.AddItemToInventory(vendorname, fooditem.FoodItemName);
                return added ? Ok(new ResponseWrapper<bool?> { ResponseMessage = "Request Successfull", ResponseCode = 200, Data = added }) :
                    BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Could not add." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<bool?> { Data = null, ResponseCode = 500, ResponseMessage = ex.Message });
            }
        }

        /// <summary>
        /// Get item from inventory
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/inventoryitem/getitem?itemname=vendorname=
        /// </remarks>
        /// <param name="itemname">item name</param>
        /// <param name="vendorname">vendor name</param>
        /// <returns>returns a list of inventory items</returns>
        [HttpGet("getitem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseWrapper<List<InventoryItemDto>>>> GetItem([MinLength(3)] string itemname, [MinLength(5)] string vendorname)
        {
            try
            {
                var inventoryItems = await _inventoryItemService.GetItem(vendorname, itemname);
                return Ok(new ResponseWrapper<List<InventoryItemDto>> { ResponseCode = 200, ResponseMessage = "Data gotten Successfully", Data = inventoryItems });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<bool?> { Data = null, ResponseCode = 500, ResponseMessage = ex.Message });
            }
        }


        /// <summary>
        /// Delete an item from the inventory
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /api/inventoryitem/deleteitem/{itemname}/{vendorname}
        /// </remarks>
        /// <param name="itemname">item name</param>
        /// <param name="vendorname">the vendor name</param>
        /// <returns>true, false or null</returns>
        [HttpDelete("deletitem/{itemname}/{vendorname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool?>>> DeleteItem([MinLength(3)] string itemname, string vendorname)
        {
            try
            {
                var deleted = await _inventoryItemService.RemoveItemFromInventory(vendorname, itemname);
                if (deleted == null)
                    throw new HttpRequestException("Could not find item");

                return (bool)deleted
                    ? Ok(new ResponseWrapper<bool?> { ResponseCode = 200, ResponseMessage = "Item deleted Successfully", Data = deleted })
                    : BadRequest(new ResponseWrapper<bool?> { Data = null, ResponseCode = 404, ResponseMessage = "Could not find item" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<bool?> { Data = null, ResponseCode = 500, ResponseMessage = ex.Message });
            }
        }

        /// <summary>
        /// Set food item status
        /// </summary>
        /// <remarks>
        /// In the food item status 0 repesents Available and 1 represents Unavailable
        /// Sample Resquest:
        /// PUT /api/inventoryitem/setfooditemstatus/{itemname}/{vendorname}
        /// </remarks>
        /// <param name="itemname">Food item name</param>
        /// <param name="vendorname">The vendor name</param>
        /// <param name="foodItemStatus">The food item status</param>
        /// <returns></returns>
        [HttpPut("setfooditemstatus/{itemname}/{vendorname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseWrapper<bool>>> SetFoodItemStatus([MinLength(3)] string itemname, string vendorname, FoodItemStatus foodItemStatus)
        {
            try
            {
                var response = await _inventoryItemService.MarkFoodItemStatus(itemname, vendorname, foodItemStatus);
                return response ? Ok(new ResponseWrapper<bool> { Data = response, ResponseCode = 200, ResponseMessage = "Success" })
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Could not set status" });
            }
            catch(Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Sorry Something went wrong");
            }
        }

        /// <summary>
        /// incerease food item quantity
        /// </summary>
        /// <param name="vendorName">The vendor name</param>
        /// <param name="itemName">The Food item name</param>
        /// <param name="quantity">The amount to be added</param>
        /// <returns>True or False</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("addtofooditem/{vendorName}/{itemName}/{quantity}")]
        public async Task<ActionResult<ResponseWrapper<bool>>> AddToFoodItem(string vendorName, string itemName, int quantity)
        {
            try
            {
                var response = await _inventoryItemService.AddToFoodItem(itemName, vendorName, quantity);
                return response ? Ok(new ResponseWrapper<bool> { Data = response, ResponseCode = 200, ResponseMessage = "Success" })
                    : BadRequest(new ResponseWrapper<bool> { Data = false, ResponseCode = 400, ResponseMessage = "Could not set quantity" });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message, title: "Opps Something went wrong");
            }
        }
    }
}
