using LeadwaycanteenApi.Entities.Dtos.GeneralDto;

namespace LeadwaycanteenApi.Services.DbServices.InventoryService
{
    /// <summary>
    /// Interface for managing inventory operations related to a specific vendor.
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Creates an inventory for a specific vendor.
        /// </summary>
        /// <param name="VendorId">The unique identifier of the vendor for whom the inventory is being created.</param>
        /// <returns>A task representing the asynchronous operation, with a result of <see cref="bool"/> indicating success or failure of the inventory creation.</returns>
        public Task<bool?> CreateInventory(Guid VendorId);
    }
}
