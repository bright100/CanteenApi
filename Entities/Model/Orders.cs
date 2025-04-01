using LeadwaycanteenApi.Entities.Dtos.GeneralDto;

namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents an order entity.
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// Unique identifier for the order.
        /// </summary>
        public Guid OrderId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Status of the order.
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.ORDER_PENDING;

        /// <summary>
        /// Scan status of the order.
        /// </summary>
        public string ScanStatus { get; set; } = string.Empty;

        /// <summary>
        /// Subtotal of the order.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Collection of food items in the order.
        /// </summary>
        public List<OrdersFoodItemDto> OrderFoodItems { get; set; } = [];

        /// <summary>
        /// Date and time when the order was scanned.
        /// </summary>
        public DateTime ScanTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Payment method used for the order.
        /// </summary>
        public PaymentMethods PaymentMethod { get; set; } = PaymentMethods.CASH;

        /// <summary>
        /// Date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the order was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Total amount to pay for the order.
        /// </summary>
        public decimal TotalAmountToPay { get; set; }
        
        /// <summary>
        /// The background job id of the order
        /// </summary>
        public int JobId { get; set; }
    }

    /// <summary>
    /// Enumerates the possible statuses of an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// The order is pending.
        /// </summary>
        ORDER_PENDING,

        /// <summary>
        /// The order is complete.
        /// </summary>
        ORDER_COMPLETE,

        /// <summary>
        /// The order is canceled.
        /// </summary>
        ORDER_CANCELED
    }

    /// <summary>
    /// Enumerates the possible payment methods.
    /// </summary>
    public enum PaymentMethods
    {
        /// <summary>
        /// Cash payment method.
        /// </summary>
        CASH,

        /// <summary>
        /// Payment from salary.
        /// </summary>
        FROM_SALARY,

        /// <summary>
        /// Shared payment method.
        /// </summary>
        SHARED_PAYMENT
    }
}
