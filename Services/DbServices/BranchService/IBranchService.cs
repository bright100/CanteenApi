using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.BranchService
{
    /// <summary>
    /// Interface for branch service
    /// </summary>
    public interface IBranchService
    {
        /// <summary>
        /// Create a new branch
        /// </summary>
        /// <param name="branch">Branch details</param>
        /// <returns>Branch details</returns>
        public Task<bool?> CreateBranch(BranchDto branch);

        /// <summary>
        /// Get a branch
        /// </summary>
        /// <param name="branchName">The branch name</param>
        /// <returns></returns>
        public Task<Branch?> GetBranch(string branchName);

        /// <summary>
        /// Update a branch
        /// </summary>
        /// <param name="branch">The branch details</param>
        /// <returns>true or false</returns>
        public Task<bool> UpdateBranch(BranchDto branch);

        /// <summary>
        /// Delete a branch
        /// </summary>
        /// <param name="branchName">The branch name</param>
        /// <returns>true or false</returns>
        public Task<bool> DeleteBranch(string branchName);
    }
}
