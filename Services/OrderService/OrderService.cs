using Hangfire;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Hubs;
using LeadwaycanteenApi.Services.DbServices.OrderService;
using Microsoft.AspNetCore.SignalR;

namespace LeadwaycanteenApi.Services.OrderService
{
    /// <summary>
    /// Order service for managing orders different from the dbversion
    /// </summary>
    /// <param name="dbOrderService">the db service for CRUD of orders</param>
    /// <param name="hubContext">Hub Context</param>
    public class OrderService(IDbOrderService dbOrderService, IHubContext<OrdersHub> hubContext) : IOrderService
    {
        private readonly IDbOrderService _dbOrderService = dbOrderService;
        private readonly IHubContext<OrdersHub> _hubContext = hubContext;

        /// <summary>
        /// Creates a new order and schedules a background task for it to cancle the order after 2hrs
        /// </summary>
        /// <param name="fooditems">The food items to be added to the order</param>
        /// <param name="vendorName">The vendors name</param>
        /// <param name="employeeMail">The employees name</param>
        /// <returns>the recent order details</returns>
        public async Task<List<OrdersDto>> CreateOrder(List<OrdersInventoryItemRequestDto> fooditems, string vendorName, string employeeMail)
        {
            var result = await _dbOrderService.CreateOrder(fooditems, vendorName, employeeMail);
            if(!result) return [];

            var order = await _dbOrderService.GetRecentOrder(vendorName, employeeMail);
            if (!order.Any()) return [];

            await _hubContext.Clients.Client(vendorName).SendAsync("ReciveOrder", order);
            string jobId = BackgroundJob.Schedule<IDbOrderService>(x => x.CanceleOrder(order.FirstOrDefault().OrderId), TimeSpan.FromHours(2));
            await _dbOrderService.SetJobId(order.FirstOrDefault().OrderId, jobId);
            return [.. order];
        }
    }
}
