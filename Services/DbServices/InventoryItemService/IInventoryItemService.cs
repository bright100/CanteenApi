using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.InventoryItemService
{
    /// <summary>
    /// Defines the contract for inventory item services, including creating, adding, removing, retrieving, and updating inventory items.
    /// </summary>
    public interface IInventoryItemService
    {
        /// <summary>
        /// Creates a new inventory item.
        /// </summary>
        /// <param name="inventoryItem">An <see cref="InventoryItemReqDto"/> object representing the details of the inventory item to be created.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool?> CreateInventoryItem(InventoryItemReqDto inventoryItem);

        /// <summary>
        /// Adds a food item to the inventory for a specific vendor.
        /// </summary>
        /// <param name="vendorname">The name of the vendor to whom the item will be added.</param>
        /// <param name="fooditemname">The name of the food item to be added to the inventory.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> AddItemToInventory(string vendorname, string fooditemname);

        /// <summary>
        /// Removes a food item from the inventory for a specific vendor.
        /// </summary>
        /// <param name="VendorName">The name of the vendor from whom the item will be removed.</param>
        /// <param name="FoodItemName">The name of the food item to be removed from the inventory.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool?> RemoveItemFromInventory(string VendorName, string FoodItemName);

        /// <summary>
        /// Retrieves a specific inventory item for a vendor by name.
        /// </summary>
        /// <param name="VendorName">The name of the vendor to retrieve the item from.</param>
        /// <param name="ItemName">The name of the food item to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of <see cref="InventoryItemDto"/> objects representing the inventory item details.</returns>
        public Task<List<InventoryItemDto>> GetItem(string VendorName, string ItemName);

        /// <summary>
        /// Retrieves a specific inventory item by item name.
        /// </summary>
        /// <param name="itemName">The name of the food item to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="InventoryItem"/> representing the inventory item details, or null if not found.</returns>
        public Task<InventoryItem?> GetItem(string itemName);

        /// <summary>
        /// Retrieves a paginated list of all items in a vendor's inventory.
        /// </summary>
        /// <param name="VendorName">The name of the vendor whose items are to be retrieved.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of <see cref="InventoryResponseDto"/> objects representing the inventory items.</returns>
        public Task<List<InventoryResponseDto>> GetAllItems(string VendorName, int page, int pageSize);

        /// <summary>
        /// Marks the status of a food item in the inventory.
        /// </summary>
        /// <param name="foodItemName">The name of the food item to update the status for.</param>
        /// <param name="vendorName">The name of the vendor to whom the food item belongs.</param>
        /// <param name="foodItemStatus">The new status to be set for the food item.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> MarkFoodItemStatus(string foodItemName, string vendorName, FoodItemStatus foodItemStatus);

        /// <summary>
        /// Adds a specified quantity of a food item to the inventory for a vendor.
        /// </summary>
        /// <param name="itemName">The name of the food item to be added.</param>
        /// <param name="vendorName">The name of the vendor to whom the item will be added.</param>
        /// <param name="quantity">The quantity of the food item to be added to the inventory.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the operation was successful.</returns>
        public Task<bool> AddToFoodItem(string itemName, string vendorName, int quantity);
    }

}
