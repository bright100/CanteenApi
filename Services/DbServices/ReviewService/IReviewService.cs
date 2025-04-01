using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;

namespace LeadwaycanteenApi.Services.DbServices.ReviewService
{
    /// <summary>
    /// Defines the contract for managing reviews for employees and vendors.
    /// Provides methods to create, retrieve, update, and delete reviews.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Retrieves all reviews with pagination support.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <param name="EmployeeName">The name of the employee to filter reviews by.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an enumeration of reviews.</returns>
        Task<IEnumerable<Reviews>> GetAllReviews(int page, int pageSize, string EmployeeName);

        /// <summary>
        /// Retrieves reviews for a specific employee and vendor with pagination support.
        /// </summary>
        /// <param name="EmployeeName">The name of the employee to filter reviews by.</param>
        /// <param name="VendorName">The name of the vendor to filter reviews by.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a review for the specified employee and vendor.</returns>
        Task<Reviews> GetReview(string EmployeeName, string VendorName, int page, int pageSize);

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result is the review matching the given ID.</returns>
        Task<Reviews> GetReviewById(Guid id);

        /// <summary>
        /// Creates a new review for an employee or vendor.
        /// </summary>
        /// <param name="review">The review data to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the review was successfully created.</returns>
        Task<bool> CreateReview(ReviewsDto review);

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="review">The updated review data.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the review was successfully updated.</returns>
        Task<bool> UpdateReview(ReviewsDto review);

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the review was successfully deleted.</returns>
        Task<bool> DeleteReviewById(int id);

        /// <summary>
        /// Deletes all reviews written by a specific employee.
        /// </summary>
        /// <param name="EmployeeName">The name of the employee whose reviews should be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task result indicates whether the reviews were successfully deleted.</returns>
        Task<bool> DeleteReviewByEmployeeName(string EmployeeName);

        /// <summary>
        /// Retrieves reviews for a specific vendor with pagination support.
        /// </summary>
        /// <param name="VendorName">The name of the vendor to filter reviews by.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a review for the specified vendor.</returns>
        Task<Reviews> GetReviewByVendorName(string VendorName, int page, int pageSize);
    }
}
