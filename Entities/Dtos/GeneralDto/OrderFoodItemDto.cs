using System.ComponentModel.DataAnnotations;

namespace LeadwaycanteenApi.Entities.Dtos.GeneralDto
{
    /// <summary>
    /// Data transfer object for ordering food items.
    /// </summary>
    public class OrderFoodItemDto
    {
        /// <summary>
        /// Gets or sets the quantity of the food item.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the food item being ordered.
        /// </summary>
        public OrdersInventoryItemDto FoodItem { get; set; } = new OrdersInventoryItemDto();

        /// <summary>
        /// Gets or sets the subtotal for the ordered food item.
        /// </summary>
        public decimal SubTotal { get; set; }
    }
}
