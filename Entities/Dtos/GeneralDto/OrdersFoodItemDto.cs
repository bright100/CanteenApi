using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for food items in an order.
    /// </summary>
    public class OrdersFoodItemDto
    {
        /// <summary>
        /// Gets or sets the quantity of the food item.
        /// </summary>
        [Required, NotNull]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the food item details.
        /// </summary>
        public OrdersInventoryItemReDto FoodItem { get; set; } = new OrdersInventoryItemReDto();
    }
}
