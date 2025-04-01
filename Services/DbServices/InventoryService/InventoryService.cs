using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Wrappers;

namespace LeadwaycanteenApi.Services.DbServices.InventoryService
{
    /// <summary>
    /// Service for managing inventory operations for vendors.
    /// Implements the <see cref="IInventoryService"/> interface.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryService"/> class.
        /// </summary>
        /// <param name="config">The configuration object that provides access to configuration settings.</param>
        public InventoryService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Creates an inventory for the specified vendor.
        /// </summary>
        /// <param name="VendorId">The unique identifier of the vendor for whom the inventory is being created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see cref="bool"/> indicating success or failure of the inventory creation.</returns>
        public async Task<bool?> CreateInventory(Guid VendorId)
        {
            DbWrapper<InventoryDto> dbWrapper = new(_config);
            var sql = "INSERT INTO Inventory (VendorId) VALUES (@VendorId)";
            return await dbWrapper.ExecuteAsync(sql, new { VendorId });
        }
    }

}
