
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Represents a data transfer object for an inventory item.
    /// </summary>
    public class InventoryItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the inventory item.
        /// </summary>
        /// <remarks>
        /// The total amount is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount of the inventory item.
        /// </summary>
        /// <remarks>
        /// The amount is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the name of the food item.
        /// </summary>
        /// <remarks>
        /// The food item name is required, must not be null, and must be at least 3 characters long.
        /// </remarks>
        [Required, NotNull, MinLength(3)]
        public string FoodItemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the food item.
        /// </summary>
        public FoodItemStatus FoodItemStatus { get; set; } = FoodItemStatus.AVAILABLE;

        /// <summary>
        /// Gets or sets the quantity of the inventory item.
        /// </summary>
        /// <remarks>
        /// The quantity is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the food item.
        /// </summary>
        public string FoodItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the food item image.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class of the food item.
        /// </summary>
        public string FoodItemClass { get; set; } = "Protien";
    }

    /// <summary>
    /// Represents a data transfer object for an inventory item request.
    /// </summary>
    public class InventoryItemReqDto
    {
        /// <summary>
        /// Gets or sets the amount of the inventory item.
        /// </summary>
        /// <remarks>
        /// The amount is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the name of the food item.
        /// </summary>
        /// <remarks>
        /// The food item name is required, must not be null, and must be at least 3 characters long.
        /// </remarks>
        [Required, NotNull, MinLength(3)]
        public string FoodItemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the food item.
        /// </summary>
        public FoodItemStatus FoodItemStatus { get; set; } = FoodItemStatus.AVAILABLE;

        /// <summary>
        /// Gets or sets the quantity of the inventory item.
        /// </summary>
        /// <remarks>
        /// The quantity is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the food item.
        /// </summary>
        /// <remarks>
        /// The food item description is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public string FoodItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the food item image.
        /// </summary>
        /// <remarks>
        /// The image URL is required and must not be null.
        /// </remarks>
        [Required, NotNull]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class of the food item.
        /// </summary>
        public string FoodItemClass { get; set; } = "Protien";
    }
}
