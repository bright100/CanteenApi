using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for orders.
    /// </summary>
    public class OrdersDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order.
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.ORDER_PENDING;

        /// <summary>
        /// Gets or sets the date and time when the order was scanned.
        /// </summary>
        public DateTime ScanTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the list of food items in the order.
        /// </summary>
        public List<OrdersFoodItemDto> OrderFoodItems { get; set; } = [];

        /// <summary>
        /// Gets or sets the payment method used for the order.
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total amount to be paid for the order.
        /// </summary>
        public decimal TotalAmountToPay { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
