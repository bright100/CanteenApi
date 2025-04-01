using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for inventory items in an order.
    /// </summary>
    public class OrdersInventoryItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the inventory item.
        /// </summary>
        [Required, NotNull]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the inventory item.
        /// </summary>
        [Required, NotNull]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the name of the food item.
        /// </summary>
        [Required, NotNull, MinLength(3)]
        public string FoodItemName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data transfer object for inventory items in an order (read-only).
    /// </summary>
    public class OrdersInventoryItemReDto
    {
        /// <summary>
        /// Gets the name of the food item.
        /// </summary>
        [Required, NotNull, MinLength(3)]
        public string FoodItemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the unique identifier for the inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Gets the amount of the inventory item.
        /// </summary>
        [Required, NotNull]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Data transfer object for inventory item requests.
    /// </summary>
    public class OrdersInventoryItemRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the inventory item.
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the inventory item.
        /// </summary>
        [NotNull, Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the amount of the inventory item.
        /// </summary>
        public decimal Amount { get; set; }
    }
}