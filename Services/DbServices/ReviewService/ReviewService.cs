using Dapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Wrappers;
using Microsoft.Data.SqlClient;

namespace LeadwaycanteenApi.Services.DbServices.ReviewService
{
    /// <summary>
    /// Service for managing reviews, including creating, deleting, retrieving, and updating reviews for vendors and employees.
    /// </summary>
    /// <param name="config">The configuration object containing the connection string for the database.</param>
    /// <exception cref="ArgumentNullException">Thrown when the connection string 'DefaultConnection' is not found in the configuration.</exception>
    public class ReviewService(IConfiguration config) : IReviewService
    {
        /// <summary>
        /// configuration
        /// </summary>
        public readonly IConfiguration _config = config;
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException(nameof(config), "Connection string 'DefaultConnection' not found.");

        /// <summary>
        /// Creates a new review for a vendor by an employee.
        /// </summary>
        /// <param name="review">The review data to be inserted into the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the review was successfully created.</returns>
        public async Task<bool> CreateReview(ReviewsDto review)
        {
            DbWrapper<ReviewsDto> dbWrapper = new(_config);
            var sql = @"INSERT INTO Reviews (Rating, Review, VendorId, EmployeeId) VALUES (@Rating, @Review, (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName), (SELECT EmployeeId FROM Employee WHERE FullName = @EmployeeName))";
            return await dbWrapper.ExecuteAsync(sql, new { review.VendorName, review.EmployeeName, review.Rating, review.Review });
        }

        /// <summary>
        /// Deletes reviews associated with an employee based on the employee's name.
        /// </summary>
        /// <param name="EmployeeName">The name of the employee whose reviews are to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the reviews were successfully deleted.</returns>
        public async Task<bool> DeleteReviewByEmployeeName(string EmployeeName)
        {
            DbWrapper<ReviewsDto> dbWrapper = new(_config);
            var sql = @"DELETE FROM Reviews WHERE EmployeeId = (SELECT EmployeeId FROM Employee WHERE EmployeeName = @EmployeeName)";
            return await dbWrapper.ExecuteAsync(sql, EmployeeName);
        }

        /// <summary>
        /// Deletes a review by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The ID of the review to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the review was successfully deleted.</returns>
        public async Task<bool> DeleteReviewById(int id)
        {
            DbWrapper<ReaderWriterLock> dbWrapper = new(_config);
            var sql = @"DELETE FROM Reviews WHERE ReviewId = @ReviewId";
            return await dbWrapper.ExecuteAsync(sql, id);
        }

        /// <summary>
        /// Retrieves a list of all reviews associated with a specific employee, paginated by page and page size.
        /// </summary>
        /// <param name="page">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <param name="EmployeeName">The name of the employee whose reviews are to be fetched.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains an <see cref="IEnumerable{Reviews}"/> containing the paginated list of reviews.</returns>
        public async Task<IEnumerable<Reviews>> GetAllReviews(int page, int pageSize, string EmployeeName)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT * FROM Reviews JOIN Employee ON Reviews.EmployeeId = Employee.EmployeeId 
                        INNER JOIN Vendor ON Reviews.VendorId = Vendor.VendorId WHERE EmployeeId = (SELECT EmployeeId FROM Employee WHERE EmployeeName = @EmployeeName)
                        ORDER BY ReviewId DESC LIMIT @pageSize OFFSET @page";

            Dictionary<string, Reviews> reviewDict = [];

            var reviews = await connection.QueryAsync<Reviews, Employee, Vendor, Reviews>(sql, (rev, emp, ven) =>
            {
                if (!reviewDict.TryGetValue(rev.ReviewId.ToString(), out var review))
                {
                    review = rev;
                    review.Employee = emp;
                    review.Vendor = ven;
                }
                review.Employee = emp;
                review.Vendor = ven;
                reviewDict.Add(rev.ReviewId.ToString(), rev);
                return review;
            }, new { EmployeeName, page, pageSize }, splitOn: "EmployeeId, VendorId");

            return reviews;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The ID of the review to retrieve.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a <see cref="Reviews"/> object representing the review, or null if the review was not found.</returns>
        public async Task<Reviews> GetReviewById(Guid id)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT * FROM Reviews JOIN Employee ON Reviews.EmployeeId = Employee.EmployeeId WHERE ReviewId = @ReviewId";

            Dictionary<Guid, Reviews> reviewsDict = [];
            var review = await connection.QueryAsync<Reviews, Employee, Reviews>(sql, (rev, emp) =>
            {
                if(!reviewsDict.TryGetValue(id, out var review))
                {
                    review = rev;
                    review.Employee = emp;
                }
                review.Employee = emp;
                reviewsDict.Add(id, rev);
                return review;
            }, new { ReviewId = id }, splitOn: "EmployeeId");

            return review.First();
        }

        /// <summary>
        /// Retrieves a review for a specific employee and vendor, paginated by page and page size.
        /// </summary>
        /// <param name="EmployeeName">The name of the employee associated with the review.</param>
        /// <param name="VendorName">The name of the vendor associated with the review.</param>
        /// <param name="page">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a <see cref="Reviews"/> object representing the review, or null if the review was not found.</returns>
        public async Task<Reviews> GetReview(string EmployeeName, string VendorName, int page, int pageSize)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT * FROM Reviews r
                        INNER JOIN Employee e ON r.EmployeeId = e.EmployeeId 
                        INNER JOIN Vendor v ON r.VendorId = v.VendorId 
                        WHERE e.FullName = @EmployeeName 
                        AND v.VendorName = @VendorName
                        ORDER BY ReviewId
                        DESC OFFSET (@page - 1) * 1 ROWS
                        FETCH NEXT @pageSize ROWS ONLY";

            Dictionary<string, Reviews> reviewDict = [];
            var reviews = await connection.QueryAsync<Reviews, Employee, Vendor, Reviews>(sql, (rev, emp, ven) =>
            {   
                if(!reviewDict.TryGetValue(EmployeeName, out var review))
                {
                    review = rev;
                    review.Employee = emp;
                    review.Vendor = ven;
                }
                review.Employee = emp;
                review.Vendor = ven;
                reviewDict.Add(EmployeeName, rev);
                return review;
            }, new { EmployeeName, page, pageSize, VendorName }, splitOn: "EmployeeId, VendorId");

            return reviews.First();
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="review">The review data to be updated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean indicating whether the review was successfully updated.</returns>
        public Task<bool> UpdateReview(ReviewsDto review)
        {
            DbWrapper<ReviewsDto> dbWrapper = new(_config);
            var sql = @"UPDATE Reviews SET Rating = @Rating, ReviewText = @ReviewText, ReviewDate = @ReviewDate, ReviewTime = @ReviewTime WHERE ReviewId = @ReviewId";
            return dbWrapper.ExecuteAsync(sql, review);
        }

        /// <summary>
        /// Retrieves all reviews for a specific vendor, paginated by page and page size.
        /// </summary>
        /// <param name="VendorName">The name of the vendor whose reviews are to be fetched.</param>
        /// <param name="page">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of reviews per page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a <see cref="Reviews"/> object representing the review, or null if the review was not found.</returns>
        public async Task<Reviews> GetReviewByVendorName(string VendorName, int page, int pageSize)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM Reviews JOIN Employee ON Reviews.EmployeeId = Employee.EmployeeId
                        INNER JOIN Vendor ON Reviews.VendorId = Vendor.VendorId
                        WHERE Vendor.VendorName = @VendorName
                        ORDER BY ReviewId
                        DESC OFFSET (@page - 1) * 1 ROWS
                        FETCH NEXT @pageSize ROWS ONLY";

            Dictionary<string , Reviews> reviewDict = [];

            var reviews = await connection.QueryAsync<Reviews, Employee, Vendor, Reviews>(sql, (rev, emp, ven) =>
            {
                if(!reviewDict.TryGetValue(VendorName, out var review))
                {
                    review = rev;
                    review.Employee = emp;
                    review.Vendor = ven;
                }
                review.Vendor = ven;
                review.Employee = emp;
                reviewDict.Add(VendorName, rev);
                return review;
            }, new { VendorName, page, pageSize }, splitOn: "VendorId");

            return reviews.First();
        }
    }
}
