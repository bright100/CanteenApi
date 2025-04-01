using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.OrderService
{
    /// <summary>
    /// Interface for managing orders in the system.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order based on the provided food items, vendor name, and employee email.
        /// </summary>
        /// <param name="fooditems">A list of <see cref="OrdersInventoryItemRequestDto"/> representing the food items to be added to the order.</param>
        /// <param name="vendorName">The name of the vendor for which the order is being created.</param>
        /// <param name="employeeMail">The email of the employee creating the order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of <see cref="OrdersDto"/> representing the created order details.</returns>
        public Task<List<OrdersDto>> CreateOrder(List<OrdersInventoryItemRequestDto> fooditems, string vendorName, string employeeMail);
    }
}
