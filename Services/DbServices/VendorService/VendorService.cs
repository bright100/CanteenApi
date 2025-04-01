using System.Text;
using Hangfire;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.BranchService;
using LeadwaycanteenApi.Services.DbServices.InventoryService;
using LeadwaycanteenApi.Wrappers;

namespace LeadwaycanteenApi.Services.DbServices.VendorService
{
    /// <summary>
    /// Service for managing vendors
    /// </summary>
    /// <param name="config">configuration</param>
    /// <param name="branchService">The branch service</param>
    /// <param name="inventoryService">The inventory service</param>
    public class VendorService(IConfiguration config, IBranchService branchService, IInventoryService inventoryService) : IVendorService
    {
        private readonly IBranchService _branchService = branchService;
        private readonly IInventoryService _inventoryService = inventoryService;
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Create a new vendor
        /// </summary>
        /// <param name="vendor">vendor details</param>
        /// <returns>true or false</returns>
        public async Task<bool> CreateVendor(Vendor vendor)
        {
            try
            {
                DbWrapper<VendorDto> dbWrapper = new(_config);
                var sql = "INSERT INTO Vendor (VendorId, Email, VendorName, Phone, Address, FirstName, LastName, AccountNumber, BankName, AccountName, PasswordHash, VendorsContractForm, LetterOfAgreement, CompanyRegistrationDocument, Others, BranchId, CanteenStatus) " +
                    "VALUES (@VendorId, @Email, @VendorName, @Phone, @Address, @FirstName, @LastName, @AccountNumber, @BankName, @AccountName, @PasswordHash, @VendorsContractForm, @LetterOfAgreement, @CompanyRegistrationDocument, @Others, @BranchId, 'Closed')";

                var done = await dbWrapper.ExecuteAsync(sql, new { vendor.VendorId, vendor.Email, vendor.VendorName, vendor.Phone, vendor.Address, vendor.FirstName, vendor.LastName, vendor.AccountNumber, vendor.BankName, vendor.AccountName, vendor.PasswordHash, vendor.VendorsContractForm, vendor.LetterOfAgreement, vendor.CompanyRegistrationDocument, vendor.Others, vendor.Branch.BranchId });
                if (done)
                {
                    var inventory_created = await _inventoryService.CreateInventory(vendor.VendorId);
                    if(inventory_created == null) return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a vendor
        /// </summary>
        /// <param name="Email">vendor email</param>
        /// <returns>true or false</returns>
        public async Task<bool> DeleteVendor(string Email)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = "DELETE FROM Vendor WHERE Email = @Email";
            return await dbWrapper.ExecuteAsync(sql, Email);
        }

        /// <summary>
        /// Update a vendor
        /// </summary>
        /// <param name="vendor">vendor details</param>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateVendor(VendorDto vendor)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = 
                "UPDATE Vendor SET (@Email, @VendorName, @Phone, @Address, @FirstName, @LastName, @AccountNumber, @BankName, @AccountName) WHERE VendorId = @VendorId";
            return await dbWrapper.ExecuteAsync(sql, vendor);
        }

        /// <summary>
        /// Get all vendors
        /// </summary>
        /// <returns>List of vendors</returns>
        public async Task<IEnumerable<VendorDto>> GetAllVendor()
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = "SELECT * FROM Vendor";
            return await dbWrapper.QueryAsync(sql, null);
        }

        /// <summary>
        /// Activate a vendor
        /// </summary>
        /// <param name="Email">vendor email</param>
        /// <returns>true or false</returns>
        public async Task<bool> ActivaeVendor(string Email)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = "UPDATE Vendor SET Status = @Status WHERE Email = @Email";
            return await dbWrapper.ExecuteAsync(sql, new { Status = "Active", Email });
        }

        /// <summary>
        /// Deactivate a vendor
        /// </summary>
        /// <param name="Email">vendor email</param>
        /// <returns>true or false</returns>
        public async Task<bool> DeactivateVendor(string Email)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = "UPDATE Vendor SET Status = @Status WHERE Email = @Email";
            return await dbWrapper.ExecuteAsync(sql, new { Status = "Inactive", Email });
        }

        /// <summary>
        /// Suspend a vendor
        /// </summary>
        /// <param name="Email">vendor email</param>
        /// <returns>true or false</returns>
        public async Task<bool> SuspendVendor(string Email)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var sql = "UPDATE Vendor SET Status = @Status WHERE Email = @Email";
            return await dbWrapper.ExecuteAsync(sql, new { Status = "Suspended", Email });
        }

        /// <summary>
        /// Changes the vendor branch
        /// </summary>
        /// <param name="Email">The vendors branch</param>
        /// <param name="branchName">The new branch name</param>
        /// <returns></returns>
        public async Task<bool> ChangeBranch(string Email, string branchName)
        {
            DbWrapper<VendorDto> dbWrapper = new(_config);
            var branch = await _branchService.GetBranch(branchName);
            if (branch == null) return false;
            var sql = "UPDATE Vendor SET BranchId = @BranchId WHERE Email = @Email";
            return await dbWrapper.ExecuteAsync(sql, new { Email, branch.BranchId });
        }

        /// <summary>
        /// gets vendor by the branch name the belong to
        /// </summary>
        /// <param name="BranchName">the branch name</param>
        /// <returns>list of vendors</returns>
        public async Task<IEnumerable<VendorDto>> GetVendorsByBranchName(string BranchName)
        {
            DbWrapper<VendorDto> vendors = new(_config);
            var sql = "SELECT * FROM Vendor v JOIN Branch b ON b.BranchId = v.BranchId WHERE b.BranchName = @BranchName";
            return await vendors.QueryAsync(sql, new { BranchName });
        }

        /// <summary>
        /// Gets the inventory id for the vendor
        /// </summary>
        /// <param name="vendorname">the vendor name</param>
        /// <returns>the inventory id</returns>
        public async Task<Guid> GetInventoryId(string vendorname)
        {
            DbWrapper<Inventory> dbWrapper = new(_config);
            var sql = "SELECT i.* FROM Vendor v JOIN Inventory i ON v.VendorId = i.VendorId WHERE v.VendorName = @VendorName";
            var inventory = await dbWrapper.QuerySingleOrDefaultAsync(sql, new { vendorname });

            if (inventory == null) return Guid.Empty;
            return inventory.InventoryId;
        }

        /// <summary>
        /// Stores vendor reset token
        /// </summary>
        /// <param name="vendorId">vendor Id</param>
        /// <param name="token">the token to store</param>
        /// <returns>true or false</returns>
        public async Task<bool> StoreResetToken(Guid vendorId, string token)
        {
            DbWrapper<PasswordResetDto> dbWrapper = new(_config);
            var sql = "INSERT INTO PasswordResetTokens (TokenExpiresAt, Token, VendorId) VALUES(@TokenExpiresAt, @Token, @VendorId)";
            return await dbWrapper.ExecuteAsync(sql, new { TokenExpiresAt = DateTime.UtcNow.AddMinutes(10), Token = token, VendorId = vendorId });
        }

        /// <summary>
        /// Gets a vendor by name
        /// </summary>
        /// <param name="vendorName">The vendor name</param>
        /// <returns>A vendor</returns>
        public async Task<Vendor?> GetVendorByName(string vendorName)
        {
            DbWrapper<Vendor> dbWrapper = new(_config);
            var sql = "SELECT * FROM Vendor WHERE VendorName = @VendorName";
            return await dbWrapper.QuerySingleOrDefaultAsync(sql, new { VendorName = vendorName });
        }

        /// <summary>
        /// Gets a vendor
        /// </summary>
        /// <param name="Email">The vendor email</param>
        /// <returns>Returns a vendor</returns>
        public async Task<Vendor?> GetVendor(string Email)
        {
            DbWrapper<Vendor> dbWrapper = new(_config);
            var sql = "SELECT * FROM Vendor WHERE Email = @Email";
            return await dbWrapper.QueryFirstOrDefaultAsync(sql, new { Email });
        }


        /// <summary>
        /// Get a vendor
        /// </summary>
        /// <param name="Email">vendor email</param>
        /// <param name="password">vendors password</param>
        /// <returns>vendor details</returns>
        public async Task<Vendor?> GetVendor(string Email, string password)
        {
            DbWrapper<Vendor> dbWrapper = new(_config);
            var sql = "SELECT * FROM Vendor WHERE Email = @Email AND PasswordHash = @passwordHash";
            return await dbWrapper.QueryFirstOrDefaultAsync(sql, new { Email, passwordHash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password)) });
        }

        /// <summary>
        /// Open a canteen
        /// </summary>
        /// <param name="vendorName"></param>
        /// <returns>True or false</returns>
        public async Task<bool> OpenCanteen(string vendorName)
        {
            DbWrapper<Vendor> dbWrapper = new(_config);
            var sql = "UPDATE Vendor SET CanteenStatus = @Status WHERE VendorName = @VendorName";
            var done = await dbWrapper.ExecuteAsync(sql, new { Status = "Open", VendorName = vendorName });
            if (!done)
                return false;

            BackgroundJob.Schedule<VendorService>(x => x.CloseCanteen(vendorName), TimeSpan.FromDays(7));
            return true;
        }

        /// <summary>
        /// Close a canteen
        /// </summary>
        /// <param name="vendorName"></param>
        /// <returns>True or false</returns>
        public async Task<bool> CloseCanteen(string vendorName)
        {
            DbWrapper<Vendor> dbWrapper = new(_config);
            var sql = "UPDATE Vendor SET CanteenStatus = @Status WHERE VendorName = @VendorName";
            return await dbWrapper.ExecuteAsync(sql, new { Status = "Closed", VendorName = vendorName });
        }

        /// <summary>
        /// Mark all vendors as closed
        /// </summary>
        /// <returns></returns>
        public async Task MarkAllVendorsAsClosed()
        {
            DbWrapper<Vendor> dbWrappers = new(_config);
            var vendors = await dbWrappers.QueryAsync("SELECT * FROM Vendor");
            foreach (var item in vendors)
            {
                await CloseCanteen(item.VendorName);
            }
        }
    }
}