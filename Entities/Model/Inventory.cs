namespace LeadwaycanteenApi.Entities.Model
{
    /// <summary>
    /// Represents an inventory entity.
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// Unique identifier for the inventory.
        /// </summary>
        public Guid InventoryId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and time when the inventory was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the inventory was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of inventory items.
        /// </summary>
        public List<InventoryItem> InventoryItems { get; set; } = [];
    }
}
