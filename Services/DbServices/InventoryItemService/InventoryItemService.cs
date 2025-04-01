using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;

namespace LeadwaycanteenApi.Services.DbServices.InventoryItemService
{
    /// <summary>
    /// Service for managing inventory items, including creating, adding, removing, updating, and retrieving inventory items.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="InventoryItemService"/> class with the provided configuration.
    /// </remarks>
    /// <param name="config">The configuration object containing the application settings.</param>
    public class InventoryItemService(IConfiguration config) : IInventoryItemService
    {
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Adds a food item to the inventory for a specific vendor.
        /// </summary>
        /// <param name="vendorname">The name of the vendor to which the food item is being added.</param>
        /// <param name="fooditemname">The name of the food item being added.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating success or failure.</returns>
        public async Task<bool> AddItemToInventory(string vendorname, string fooditemname)
        {
            try
            {
                DbWrapper<InventoryItem> dbWrapper = new(_config);
                var sql = @"UPDATE InventoryItem SET InventoryId = i.InventoryId 
                        FROM Inventory i 
                        WHERE i.VendorId = (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName) 
                        AND FoodItemName = @FoodItemName";
                return await dbWrapper.ExecuteAsync(sql, new { VendorName = vendorname, FoodItemName = fooditemname });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a new inventory item in the database.
        /// </summary>
        /// <param name="inventoryItem">The inventory item details to be created, provided as an <see cref="InventoryItemReqDto"/> object.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating success or failure.</returns>
        public async Task<bool?> CreateInventoryItem(InventoryItemReqDto inventoryItem)
        {
            try
            {
                DbWrapper<InventoryItem> dbWrapper = new(_config);
                var sql = @"INSERT INTO InventoryItem (TotalAmount, FoodItemStatus, Quantity, FoodItemName, FoodItemDescription, ImageUrl, FoodItemClass, Amount) 
                        VALUES (@TotalAmount, @FoodItemStatus, @Quantity, @FoodItemName, @FoodItemDescription, @ImageUrl, @FoodItemClass, @Amount)";
                return await dbWrapper.ExecuteAsync(sql, new
                {
                    TotalAmount = inventoryItem.Amount * inventoryItem.Quantity,
                    FoodItemStatus = inventoryItem.FoodItemStatus == FoodItemStatus.AVAILABLE ? "Available" : "UnAvailable",
                    inventoryItem.Quantity,
                    inventoryItem.FoodItemName,
                    inventoryItem.FoodItemDescription,
                    inventoryItem.ImageUrl,
                    inventoryItem.FoodItemClass,
                    inventoryItem.Amount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves all inventory items for a specific vendor with pagination.
        /// </summary>
        /// <param name="VendorName">The name of the vendor whose inventory items are being retrieved.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items to return per page.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of <see cref="InventoryResponseDto"/> representing the retrieved inventory items.</returns>
        public async Task<List<InventoryResponseDto>> GetAllItems(string VendorName, int page, int pageSize)
        {
            try
            {
                DbWrapper<InventoryResponseDto> dbWrapper = new(_config);
                var sql = @"SELECT * FROM InventoryItem 
                        WHERE InventoryId = (SELECT InventoryId FROM Inventory WHERE VendorId = (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName))
                        ORDER BY CreatedAt DESC 
                        OFFSET (@page - 1) * @pageSize ROWS 
                        FETCH NEXT @pageSize ROWS ONLY";
                return (await dbWrapper.QueryAsync(sql, new { VendorName, page, pageSize })).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        /// <summary>
        /// Retrieves a specific inventory item by vendor name and item name.
        /// </summary>
        /// <param name="VendorName">The name of the vendor whose inventory is being queried.</param>
        /// <param name="ItemName">The name of the food item being retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of <see cref="InventoryItemDto"/> representing the matching inventory items.</returns>
        public async Task<List<InventoryItemDto>> GetItem(string VendorName, string ItemName)
        {
            try
            {
                DbWrapper<InventoryItemDto> dbWrapper = new(_config);
                var sql = @"SELECT ii.* 
                        FROM InventoryItem ii 
                        JOIN Inventory i ON ii.InventoryId = i.InventoryId 
                        JOIN Vendor v ON i.VendorId = v.VendorId 
                        WHERE v.VendorName = @VendorName 
                        AND ii.FoodItemName = @ItemName 
                        AND v.CanteenStatus = 'Open'";
                return (List<InventoryItemDto>)await dbWrapper.QueryAsync(sql, new { VendorName, ItemName });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        /// <summary>
        /// Retrieves a specific inventory item by its name.
        /// </summary>
        /// <param name="itemName">The name of the food item being retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="InventoryItem"/> representing the item details, or null if not found.</returns>
        public async Task<InventoryItem?> GetItem(string itemName)
        {
            try
            {
                DbWrapper<InventoryItem> dbWrapper = new(_config);
                var sql = "SELECT * FROM InventoryItem WHERE FoodItemName = @FoodItemName";
                return await dbWrapper.QueryFirstOrDefaultAsync(sql, new { FoodItem = itemName });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Removes a food item from the inventory of a specific vendor.
        /// </summary>
        /// <param name="VendorName">The name of the vendor whose inventory item is being removed.</param>
        /// <param name="FoodItemName">The name of the food item to be removed.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the removal was successful.</returns>
        public async Task<bool?> RemoveItemFromInventory(string VendorName, string FoodItemName)
        {
            DbWrapper<InventoryItem> dbWrapper = new(_config);
            var sql = @"DELETE ii 
                    FROM InventoryItem ii 
                    JOIN Inventory i ON ii.InventoryId = i.InventoryId 
                    JOIN Vendor v ON i.VendorId = v.VendorId 
                    WHERE v.VendorName = @VendorName 
                    AND ii.FoodItemName = @FoodItemName";
            return await dbWrapper.ExecuteAsync(sql, new { VendorName, FoodItemName });
        }

        /// <summary>
        /// Marks the status (available or unavailable) of a specific food item.
        /// </summary>
        /// <param name="foodItemName">The name of the food item to update.</param>
        /// <param name="vendorName">The name of the vendor who owns the food item.</param>
        /// <param name="foodItemStatus">The status to set for the food item.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the status update was successful.</returns>
        public async Task<bool> MarkFoodItemStatus(string foodItemName, string vendorName, FoodItemStatus foodItemStatus)
        {
            DbWrapper<InventoryItem> dbWrapper = new(_config);
            var sql = @"UPDATE InventoryItem 
                    SET FoodItemStatus = @FoodItemStatus 
                    WHERE FoodItemName = @FoodItemName 
                    AND InventoryId = (SELECT InventoryId FROM Inventory WHERE VendorId = (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName))";
            return await dbWrapper.ExecuteAsync(sql, new { FoodItemName = foodItemName, VendorName = vendorName, FoodItemStatus = foodItemStatus == FoodItemStatus.AVAILABLE ? "Available" : "Unavailable" });
        }

        /// <summary>
        /// Adds a specified quantity of a food item to the inventory of a vendor.
        /// </summary>
        /// <param name="itemName">The name of the food item to update.</param>
        /// <param name="vendorName">The name of the vendor who owns the food item.</param>
        /// <param name="quantity">The quantity of the food item to add.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating whether the update was successful.</returns>
        public async Task<bool> AddToFoodItem(string itemName, string vendorName, int quantity)
        {
            DbWrapper<InventoryItem> dbWrapper = new(_config);
            var sql = @"UPDATE InventoryItem 
                    SET Quantity = (Quantity + @Quantity) 
                    WHERE FoodItemName = @FoodItemName 
                    AND InventoryId = (SELECT InventoryId FROM Inventory WHERE VendorId = (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName))";
            return await dbWrapper.ExecuteAsync(sql, new { Quantity = quantity, FoodItemName = itemName, VendorName = vendorName });
        }
    }
}
