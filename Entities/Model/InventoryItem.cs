namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents an inventory item entity.
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// Unique identifier for the inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Amount of the inventory item.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Total amount of the inventory item.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Status of the food item.
        /// </summary>
        public FoodItemStatus FoodItemStatus { get; set; }

        /// <summary>
        /// Quantity of the inventory item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Name of the food item.
        /// </summary>
        public string FoodItemName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the food item.
        /// </summary>
        public string FoodItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// URL of the food item's image.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Class of the food item.
        /// </summary>
        public string FoodItemClass { get; set; } = string.Empty;

        /// <summary>
        /// The inventory that this item belongs to.
        /// </summary>
        public Inventory Inventory { get; set; } = new Inventory();

        /// <summary>
        /// Date and time when the inventory item was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the inventory item was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Enumerates the possible statuses of a food item.
    /// </summary>
    public enum FoodItemStatus
    {
        /// <summary>
        /// The food item is available.
        /// </summary>
        AVAILABLE,

        /// <summary>
        /// The food item is unavailable.
        /// </summary>
        UNAVAILABLE,
    }
}