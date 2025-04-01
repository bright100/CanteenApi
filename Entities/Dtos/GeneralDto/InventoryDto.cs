using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Dto for The inventory
    /// </summary>
    public class InventoryDto
    {
        /// <summary>
        /// Gets The Inventory items
        /// </summary>
        public List<InventoryItem> InventoryItems { get; set; } = [];
    }
}
