using LeadwaycanteenApi.Entities.Dtos.GeneralDto;

namespace LeadwaycanteenApi.Services.DbServices.OrderService
{
    /// <summary>
    /// Defines the contract for managing orders in the system.
    /// Provides methods to create, retrieve, update, and delete orders,
    /// as well as manage the food items within those orders.
    /// </summary>
    public interface IDbOrderService
    {
        /// <summary>
        /// Creates a new order with a list of food items for a specified vendor and employee.
        /// </summary>
        /// <param name="foodItemList">A list of food items to include in the order.</param>
        /// <param name="vendorName">The name of the vendor placing the order.</param>
        /// <param name="employeeMail">The email of the employee placing the order.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the order was successfully created.</returns>
        Task<bool> CreateOrder(List<OrdersInventoryItemRequestDto> foodItemList, string vendorName, string employeeMail);

        /// <summary>
        /// Retrieves a specific order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an enumeration of orders matching the given order ID.</returns>
        Task<IEnumerable<OrdersDto>> GetOrder(Guid orderId);

        /// <summary>
        /// Retrieves all orders with pagination support.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of orders per page.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an enumeration of orders with pagination applied.</returns>
        Task<IEnumerable<OrdersDto>> GetAllOrders(int page, int pageSize);

        /// <summary>
        /// Retrieves the most recent orders for a specific vendor and employee.
        /// </summary>
        /// <param name="vendorName">The name of the vendor.</param>
        /// <param name="employeeMail">The email of the employee.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an enumeration of the most recent orders for the specified vendor and employee.</returns>
        Task<IEnumerable<OrdersDto>> GetRecentOrder(string vendorName, string employeeMail);

        /// <summary>
        /// Marks an order as completed.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to mark as completed.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the operation was successful.</returns>
        Task<bool> MarkOrderAsCompleted(Guid orderId);

        /// <summary>
        /// Deletes a specific order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the order was successfully deleted.</returns>
        Task<bool> DeleteOrder(Guid orderId);

        /// <summary>
        /// Adds a food item to an existing order.
        /// </summary>
        /// <param name="foodItem">The food item to add to the order.</param>
        /// <param name="orderId">The unique identifier of the order to which the food item will be added.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the food item was successfully added.</returns>
        Task<bool> AddFoodItemToOrder(OrdersInventoryItemRequestDto foodItem, Guid orderId);

        /// <summary>
        /// Removes a food item from an existing order.
        /// </summary>
        /// <param name="foodItemname">The name of the food item to remove from the order.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the food item was successfully removed.</returns>
        Task<bool> RemoveFoodItemFromOrder(string foodItemname);

        /// <summary>
        /// Cancels an existing order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to cancel.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the order was successfully canceled.</returns>
        Task<bool> CanceleOrder(Guid orderId);

        /// <summary>
        /// Sets the order as successfully scanned (perhaps during some verification process).
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to mark as scanned successfully.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the order was successfully marked as scanned.</returns>
        Task<bool> SetScanSuccess(Guid orderId);

        /// <summary>
        /// Retrieves all orders for a specific vendor with pagination support.
        /// </summary>
        /// <param name="vendorName">The name of the vendor to filter orders by.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of orders per page.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an enumeration of orders for the specified vendor, with pagination applied.</returns>
        Task<IEnumerable<OrdersDto>> GetAllVendorsOrders(string vendorName, int page, int pageSize);

        /// <summary>
        /// Sets the background jobId of the order
        /// </summary>
        /// <param name="orderId">The orderId</param>
        /// <param name="jobId">The Job Id</param>
        /// <returns>True or False</returns>
        Task<bool> SetJobId(Guid orderId, string jobId);
    }

}
