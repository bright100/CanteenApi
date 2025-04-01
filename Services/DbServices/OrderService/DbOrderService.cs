using Dapper;
using Hangfire;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Model;
using LeadwaycanteenApi.Services.DbServices.VendorService;
using LeadwaycanteenApi.util;
using LeadwaycanteenApi.Wrappers;
using Microsoft.Data.SqlClient;

namespace LeadwaycanteenApi.Services.DbServices.OrderService
{
    /// <summary>
    /// Serice for managing orders
    /// </summary>
    /// <param name="config">Configuration Interface</param>
    /// <param name="vendorService">Vendor Interface</param>
    public class DbOrderService(IConfiguration config, IVendorService vendorService) : IDbOrderService
    {
        private readonly IConfiguration _config = config;
        private readonly IVendorService _vendorService = vendorService;
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(config), "Connection string 'DefaultConnection' not found.");

        /// <summary>
        /// Add a food item to an order
        /// </summary>
        /// <param name="foodItem">a model representing the food item to be added and the order to add it to</param>
        /// <param name="orderId"></param>
        /// <returns>true if successfull or false if not</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddFoodItemToOrder(OrdersInventoryItemRequestDto foodItem, Guid orderId)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var transanction = await connection.BeginTransactionAsync();
            try
            {
                var createOrderItem = "INSERT INTO OrderFoodItems (Quantity, InventoryItemId, OrderId) VALUES(@Quantity, @InventoryItemId, @orderId)";
                var res = await connection.ExecuteAsync(createOrderItem, new { foodItem.Quantity, foodItem.InventoryItemId, orderId }, transanction);

                var updateInventorySql = "UPDATE InventoryItem SET Quantity = Quantity - @Quantity WHERE InventoryItemId = @InventoryItemId AND Quantity >= @Quantity";
                var updateInventory = await connection.ExecuteAsync(updateInventorySql, new { foodItem.Quantity, foodItem.InventoryItemId }, transanction);

                if(updateInventory == 0 || res == 0)
                    return false;

                await transanction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transanction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// /Canceles   ann Order
        /// </summary>
        /// <param name="orderId">The Order</param>
        /// <returns>true or fasle</returns>
        public async Task<bool> CanceleOrder(Guid orderId)
        {
            DbWrapper<Orders> dbWrapper = new(_config);
            var canceleOrder = "UPDATE Orders SET OrderStatus = 'Order Cancelled'";
            return await dbWrapper.ExecuteAsync(canceleOrder, new { orderId });
        }

        /// <summary>
        /// Initializes an order
        /// </summary>
        /// <param name="foodItemList">list of fooditems to be added to the order</param>
        /// <param name="vendorName">the vendor Id</param>
        /// <param name="employeeMail">the employee mail</param>
        /// <returns>true or false</returns>
        public async Task<bool> CreateOrder(List<OrdersInventoryItemRequestDto> foodItemList, string vendorName, string employeeMail)
        {
            ConsoleClass<bool>.LogInfo("Logged");
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var vendor = await _vendorService.GetVendorByName(vendorName);
                if (vendor == null || vendor.Status != VendorStatus.ACTIVE || vendor.CanteenStatus == "Closed")
                    return false;

                var orderSql = @"INSERT INTO Orders (OrderId, VendorId, TotalAmountToPay, EmployeeId, SubTotal)
                                 VALUES (@OrderId, (SELECT VendorId FROM Vendor WHERE VendorName = @VendorName), @TotalAmountToPay, (SELECT EmployeeId FROM Employee WHERE Email = @EmployeeMail), @SubTotal)";

                //if the food item is not available it will return null
                var getprice = "SELECT Amount FROM InventoryItem WHERE InventoryItemId = @InventoryItemId AND FoodItemStatus = 'Available'";

                var orderParams = new
                {
                    OrderId = Guid.NewGuid(),
                    VendorName = vendorName,
                    EmployeeMail = employeeMail,
                    TotalAmountToPay = foodItemList.Sum( x => x.Quantity * connection.QuerySingle<decimal>(getprice, new { x.InventoryItemId }, transaction)),
                    SubTotal = foodItemList.Sum(x => x.Quantity * x.Amount)
                };
                await connection.ExecuteAsync(orderSql, orderParams, transaction);

                //if the food item quantity is more than the item available it will fail
                var orderFoodItemSql = @"INSERT INTO OrderFoodItems (OrderId, InventoryItemId, Quantity) 
                                         VALUES (@OrderId, @InventoryItemId, @Quantity) 
                                         SELECT Quantity FROM InventoryItem 
                                         WHERE InventoryItemId = @InventoryItemId AND Quantity >= @Quantity
                                         AND InventoryId = (SELECT InventoryId From Inventory WHERE VendorId = @VendorId)";

                foreach (var item in foodItemList)
                {
                    var orderFoodItemParams = new
                    {
                        orderParams.OrderId,
                        item.Quantity,
                        item.InventoryItemId,
                        vendor.VendorId,
                    };

                    var added = await connection.QueryAsync(orderFoodItemSql, orderFoodItemParams, transaction);
                    if (!added.Any())
                        throw new InvalidOperationException("No such item for this vendor");
                }

                var updateInventorySql = "UPDATE InventoryItem SET Quantity = Quantity - @Quantity WHERE InventoryItemId = @InventoryItemId AND Quantity >= @Quantity";
                foreach (var item in foodItemList)
                {
                    var updateInventoryParams = new
                    {
                        item.Quantity,
                        item.InventoryItemId,
                    };
                    var result = await connection.ExecuteAsync(updateInventorySql, updateInventoryParams, transaction);
                    if (result == 0)
                        return false;
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
        }
        /// <summary>
        /// Deletes an Order
        /// </summary>
        /// <param name="orderId">the order id</param>
        /// <returns>true or false</returns>
        public async Task<bool> DeleteOrder(Guid orderId)
        {
            var connection = new SqlConnection(_connectionString);
            var transaction = await connection.BeginTransactionAsync();

            var sqlfordelete = "DELETE FROM InventoryItems WHERE OrderId = @orderId";
            var sql = "DELETE FROM Orders WHERE OrderId = @orderId;";

            try
            {
                await connection.ExecuteAsync(sqlfordelete, new { orderId }, transaction);
                await connection.ExecuteAsync(sql, new { orderId }, transaction);
                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the Order Information
        /// </summary>
        /// <param name="orderId">the Id of the Order in question</param>
        /// <returns>The order information or null if not found</returns>
        public async Task<IEnumerable<OrdersDto>> GetOrder(Guid orderId)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT o.*, f.*, i.* FROM Orders o 
                        INNER JOIN OrderFoodItems f ON o.OrderId = f.OrderId 
                        INNER JOIN InventoryItem i ON f.InventoryItemId = i.InventoryItemId
                        WHERE o.OrderId = @orderId;";

            Dictionary<Guid, OrdersDto> foodOrders = [];
            var orders = await connection.QueryAsync<OrdersDto, OrdersFoodItemDto, OrdersInventoryItemReDto, OrdersDto>(sql, (o, of, iv) =>
            {
                if(!foodOrders.TryGetValue(o.OrderId, out var order))
                {
                    order = o;
                    order.OrderFoodItems = [];
                    order.OrderFoodItems.Add(of);
                    order.OrderFoodItems.ForEach(o => o.FoodItem = iv);
                    foodOrders.Add(o.OrderId, order);
                }

                order.OrderFoodItems.Add(of);
                order.OrderFoodItems.ForEach(o => o.FoodItem = iv);
                return order;
            }, new { orderId }, splitOn: "OrderId, InventoryItemId");

            return orders;
        }

        /// <summary>
        /// Get the recent food order
        /// </summary>
        /// <param name="vendorName">the vendor name</param>
        /// <param name="employeeMail">the employee name</param>
        /// <returns>an order item</returns>
        public async Task<IEnumerable<OrdersDto>> GetRecentOrder(string vendorName, string employeeMail)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT TOP 1 o.*, f.*, i.* 
                            FROM Orders o INNER JOIN OrderFoodItems f ON o.OrderId = f.OrderId
                                          INNER JOIN InventoryItem i ON f.InventoryItemId = i.InventoryItemId
                                          INNER JOIN Vendor v ON o.VendorId = v.VendorId  
                                          INNER JOIN Employee e ON o.EmployeeId = e.EmployeeId      
                                          WHERE v.VendorName = @vendorName AND e.Email = @employeeMail ORDER BY o.CreatedAt DESC";

            var orders = await connection.QueryAsync<OrdersDto, OrdersFoodItemDto, OrdersInventoryItemReDto, OrdersDto>(sql, (o, of, iv) =>
            {
                o.OrderFoodItems = [];
                o.OrderFoodItems.Add(of);
                o.OrderFoodItems.ForEach(o => o.FoodItem = iv);
                return o;
            }, new { vendorName, employeeMail }, splitOn: "OrderId, InventoryItemId");

            return orders;
        }

        /// <summary>
        /// Marks an Order as Completed
        /// </summary>
        /// <param name="orderId">The id of the Order in question</param>
        /// <returns>true of false</returns>
        public async Task<bool> MarkOrderAsCompleted(Guid orderId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();

            var sql = "UPDATE Orders SET OrderStatus = 'Order Complete' WHERE OrderId = @orderId";
            var getJobId = "SELECT JobId FROM Orders WHERE OrderId = @orderId";

            try
            {
                await connection.ExecuteAsync(sql, new { orderId }, transaction);
                var jobId = await connection.QueryAsync<string>(getJobId, new { orderId }, transaction);
                BackgroundJob.Delete(jobId.FirstOrDefault());
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes a food item from an order
        /// </summary>
        /// <param name="foodItemname">the name of the food item in question</param>
        /// <returns>true or false</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> RemoveFoodItemFromOrder(string foodItemname)
        {
            DbWrapper<Orders> dbOrderFoodItem = new(_config);
            var sql = "DELETE FROM Orders WHERE (SELECT o.OrderFoodItemsId FROM OrderFoodItems o WHERE (SELECT FoodItemName FROM InventoryItem) = @fooditemname) = OrderId";
            return await dbOrderFoodItem.ExecuteAsync(sql, new { foodItemname });
        }

        /// <summary>
        /// Sets the Scan status to succsessful
        /// </summary>
        /// <param name="orderId">the id of the order</param>
        /// <returns>true or false</returns>
        public async Task<bool> SetScanSuccess(Guid orderId)
        {
            DbWrapper<Orders> dbWrapper = new(_config);
            var sql = "UPDATE Orders SET ScanStatus = 'Successfull', ScanTime = GETDATE() WHERE OrderId = @orderId";
            return await dbWrapper.ExecuteAsync(sql, new { orderId });
        }
    
        /// <summary>
        /// Gets the price of an inventoryItem
        /// </summary>
        /// <param name="InventoryItemId">The Id of the InventoryItem</param>
        /// <returns>The price</returns>
        public async Task<decimal> GetPrice(Guid InventoryItemId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QuerySingleAsync<decimal>("SELECT Amount AS TotalAmount From InventoryItem i INNER JOIN Inventory iv ON i.InventoryId = iv.InventoryId INNER JOIN Vendor v ON iv.VendorId = iv.VendorId WHERE v.VendorName = 'Vendor One' AND i.InventoryItemId = @InventoryItemId;", new { InventoryItemId });
        }

        /// <summary>
        /// Gets all the orders
        /// </summary>
        /// <param name="page">The starting page</param>
        /// <param name="pageSize">How many u want</param>
        /// <returns>A list of Orders</returns>
        public async Task<IEnumerable<OrdersDto>> GetAllOrders(int page, int pageSize)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT o.*, f.*, i.* FROM Orders o 
                        INNER JOIN OrderFoodItems f ON o.OrderId = f.OrderId 
                        INNER JOIN InventoryItem i ON f.InventoryItemId = i.InventoryItemId
                        ORDER BY o.CreatedAt DESC
                        OFFSET (@page - 1) * @pageSize ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";

            return await connection.QueryAsync<OrdersDto, OrdersFoodItemDto, OrdersInventoryItemReDto, OrdersDto>(sql, (o, of, iv) =>
            {
                o.OrderFoodItems = [];
                o.OrderFoodItems.Add(of);
                o.OrderFoodItems.ForEach(o => o.FoodItem = iv);
                return o;
            }, new { page, pageSize }, splitOn: "OrderId, InventoryItemId");
        }
        
        /// <summary>
        /// Gets all the orders by a vendor
        /// </summary>
        /// <param name="vendorName">The vendor name</param>
        /// <param name="page">The starting page</param>
        /// <param name="pageSize">How many you want</param>
        /// <returns>A list of orders</returns>
        public async Task<IEnumerable<OrdersDto>> GetAllVendorsOrders(string vendorName, int page, int pageSize)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT o.*, f.*, i.* FROM Orders o 
                        INNER JOIN OrderFoodItems f ON o.OrderId = f.OrderId 
                        INNER JOIN InventoryItem i ON f.InventoryItemId = i.InventoryItemId
                        WHERE o.VendorId = (SELECT VendorId FROM Vendor WHERE VendorName = @vendorName)
                        ORDER BY o.CreatedAt DESC
                        OFFSET (@page - 1) * @pageSize ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";

            return await connection.QueryAsync<OrdersDto, OrdersFoodItemDto, OrdersInventoryItemReDto, OrdersDto>(sql, (o, of, iv) =>
            {
                o.OrderFoodItems = [];
                o.OrderFoodItems.Add(of);
                o.OrderFoodItems.ForEach(o => o.FoodItem = iv);
                return o;
            }, new { page, pageSize, vendorName }, splitOn: "OrderId, InventoryItemId");
        }

        /// <summary>
        /// Sets the job id for the background job for canceling the order after two hours
        /// </summary>
        /// <param name="orderId">The orderId</param>
        /// <param name="jobId">The jobId to be saved</param>
        /// <returns>True or false</returns>

        public Task<bool> SetJobId(Guid orderId, string jobId)
        {
            DbWrapper<Orders> dbWrapper = new(_config);
            var sql = "UPDATE Orders SET JobId = @JobId WHERE OrderId = @orderId";
            return dbWrapper.ExecuteAsync(sql, new { orderId, jobId });
        }
    }
}
