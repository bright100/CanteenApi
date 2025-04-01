using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.VendorService
{
    /// <summary>
    /// Defines the contract for services related to managing vendors, including creating, retrieving, updating, and deactivating vendors.
    /// </summary>
    public interface IVendorService
    {
        /// <summary>
        /// Creates a new vendor in the system.
        /// </summary>
        /// <param name="vendor">The vendor object containing the information to be saved.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully created.</returns>
        public Task<bool> CreateVendor(Vendor vendor);

        /// <summary>
        /// Retrieves a vendor by email and password.
        /// </summary>
        /// <param name="Email">The email of the vendor.</param>
        /// <param name="password">The password of the vendor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a <see cref="Vendor"/> object representing the vendor, or null if not found.</returns>
        public Task<Vendor?> GetVendor(string Email, string password);

        /// <summary>
        /// Retrieves a vendor by email.
        /// </summary>
        /// <param name="Email">The email of the vendor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a <see cref="Vendor"/> object representing the vendor, or null if not found.</returns>
        public Task<Vendor?> GetVendor(string Email);

        /// <summary>
        /// Retrieves all vendors associated with a specific branch name.
        /// </summary>
        /// <param name="BranchName">The name of the branch for which vendors are being retrieved.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an <see cref="IEnumerable{VendorDto}"/> containing the list of vendors associated with the given branch name.</returns>
        public Task<IEnumerable<VendorDto>> GetVendorsByBranchName(string BranchName);

        /// <summary>
        /// Deletes a vendor by email.
        /// </summary>
        /// <param name="Email">The email of the vendor to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully deleted.</returns>
        public Task<bool> DeleteVendor(string Email);

        /// <summary>
        /// Updates an existing vendor's information.
        /// </summary>
        /// <param name="vendor">The vendor object containing the updated information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully updated.</returns>
        public Task<bool> UpdateVendor(VendorDto vendor);

        /// <summary>
        /// Retrieves all vendors in the system.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an <see cref="IEnumerable{VendorDto}"/> containing all vendors in the system.</returns>
        public Task<IEnumerable<VendorDto>> GetAllVendor();

        /// <summary>
        /// Activates a vendor's account by email.
        /// </summary>
        /// <param name="Email">The email of the vendor to be activated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully activated.</returns>
        public Task<bool> ActivaeVendor(string Email);

        /// <summary>
        /// Deactivates a vendor's account by email.
        /// </summary>
        /// <param name="Email">The email of the vendor to be deactivated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully deactivated.</returns>
        public Task<bool> DeactivateVendor(string Email);

        /// <summary>
        /// Suspends a vendor's account by email.
        /// </summary>
        /// <param name="Email">The email of the vendor to be suspended.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the vendor was successfully suspended.</returns>
        public Task<bool> SuspendVendor(string Email);

        /// <summary>
        /// Retrieves the inventory ID associated with a vendor's name.
        /// </summary>
        /// <param name="vendorname">The name of the vendor whose inventory ID is being retrieved.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the inventory ID associated with the vendor.</returns>
        public Task<Guid> GetInventoryId(string vendorname);

        /// <summary>
        /// Changes the branch for a vendor based on the email and the new branch name.
        /// </summary>
        /// <param name="Email">The email of the vendor whose branch is being changed.</param>
        /// <param name="branchName">The new branch name for the vendor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the branch was successfully changed.</returns>
        public Task<bool> ChangeBranch(string Email, string branchName);

        /// <summary>
        /// Stores a reset token for the vendor's account.
        /// </summary>
        /// <param name="vendorId">The unique identifier of the vendor.</param>
        /// <param name="token">The reset token to be stored.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the token was successfully stored.</returns>
        public Task<bool> StoreResetToken(Guid vendorId, string token);

        /// <summary>
        /// Retrieves a vendor by their name.
        /// </summary>
        /// <param name="vendorName">The name of the vendor to retrieve.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the <see cref="Vendor"/> object representing the vendor, or null if the vendor is not found.</returns>
        public Task<Vendor?> GetVendorByName(string vendorName);

        /// <summary>
        /// Opens a canteen for a vendor based on the vendor's name.
        /// </summary>
        /// <param name="vendorName">The name of the vendor whose canteen is being opened.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the canteen was successfully opened.</returns>
        public Task<bool> OpenCanteen(string vendorName);

        /// <summary>
        /// Closes a canteen for a vendor based on the vendor's name.
        /// </summary>
        /// <param name="vendorName">The name of the vendor whose canteen is being closed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the canteen was successfully closed.</returns>
        public Task<bool> CloseCanteen(string vendorName);

        /// <summary>
        /// Marks all vendors in the system as closed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task MarkAllVendorsAsClosed();
    }
}
