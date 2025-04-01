using LeadwaycanteenApi.Entities.Model;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.ResponseDto
{
    /// <summary>
    /// Data transfer object for Inventory response information.
    /// </summary>
    public class InventoryResponseDto
    {
        /// <summary>
        /// Unique identifier for the Inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Total amount of the Inventory item.
        /// </summary>
        [Required, NotNull]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Amount of the Inventory item.
        /// </summary>
        [Required, NotNull]
        public decimal Amount { get; set; }

        /// <summary>
        /// Name of the Food item.
        /// </summary>
        [Required, NotNull, MinLength(3)]
        public string FoodItemName { get; set; } = string.Empty;

        /// <summary>
        /// Status of the Food item.
        /// </summary>
        public FoodItemStatus FoodItemStatus { get; set; } = FoodItemStatus.AVAILABLE;

        /// <summary>
        /// Description of the Food item.
        /// </summary>
        [Required, NotNull]
        public string FoodItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// URL of the Food item image.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Classification of the Food item (e.g. "Protein").
        /// </summary>
        public string FoodItemClass { get; set; } = "Protein";
    }
}
