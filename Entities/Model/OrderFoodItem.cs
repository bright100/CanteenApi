namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents an order food item entity.
    /// </summary>
    public class OrderFoodItem
    {
        /// <summary>
        /// Unique identifier for the order food item.
        /// </summary>
        public Guid OrderFoodItemsId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Quantity of the food item ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Date and time when the order food item was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the order food item was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The food item that was ordered.
        /// </summary>
        public InventoryItem FoodItem { get; set; } = new InventoryItem();

        /// <summary>
        /// Total price of the ordered food item.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
