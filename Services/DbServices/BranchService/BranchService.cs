using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;

namespace LeadwaycanteenApi.Services.DbServices.BranchService
{
    /// <summary>
    /// Db Interface for branch service
    /// </summary>
    public class BranchService(IConfiguration config) : IBranchService
    {
        private readonly IConfiguration _config = config;
        /// <summary>
        /// Create a new branch in the database
        /// </summary>
        /// <param name="branch">Branch details</param>
        /// <returns>Branch details</returns>
        public async Task<bool?> CreateBranch(BranchDto branch)
        {
            DbWrapper<BranchDto> dbWrapper = new(_config);
            var sql = "INSERT INTO Branch (BranchName, BranchAddress, State, LGA, Note) " +
                      "VALUES (@BranchName, @BranchAddress, @State, @Lga, @Note)";

            return await dbWrapper.ExecuteAsync(sql, branch);
        }

        /// <summary>
        /// Delte a branch from the database
        /// </summary>
        /// <param name="branchName">The branch name</param>
        /// <returns>true or false</returns>s
        public async Task<bool> DeleteBranch(string branchName)
        {
            DbWrapper<BranchDto> dbWrapper = new(_config);
            var sql = "DELETE FROM Branch WHERE BranchName = @BranchName";
            return await dbWrapper.ExecuteAsync(sql, new { BranchName = branchName });
        }

        /// <summary>
        /// Get a branch from the database
        /// </summary>
        /// <param name="branchName">The branch name</param>
        /// <returns>The branch details</returns>
        public async Task<Branch?> GetBranch(string branchName)
        {
            DbWrapper<Branch> dbWrapper = new(_config);
            var sql = "SELECT * FROM Branch WHERE BranchName = @BranchName";
            return await dbWrapper.QueryFirstOrDefaultAsync(sql, new { BranchName = branchName });
        }

        /// <summary>
        /// Update a branch in the database
        /// </summary>
        /// <param name="branch">The branch details</param>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateBranch(BranchDto branch)
        {
            DbWrapper<BranchDto> dbWrapper = new(_config);
            var sql = "UPDATE Branch SET BranchAddress = @BranchAddress, State = @State, Lga = @Lga, Note = @Note WHERE BranchName = @BranchName";
            return await dbWrapper.ExecuteAsync(sql, branch);
        }
    }
}
