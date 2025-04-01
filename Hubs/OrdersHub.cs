using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using Microsoft.AspNetCore.SignalR;

namespace LeadwaycanteenApi.Hubs
{
    /// <summary>
    /// Represents a SignalR hub for handling orders-related messages and client connections.
    /// </summary>
    public class OrdersHub : Hub
    {
        /// <summary>
        /// Sends an order message to a specific client identified by connection ID.
        /// </summary>
        /// <param name="connectionId">The unique identifier for the client connection.</param>
        /// <param name="order">The order data to be sent to the client.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMessage(string connectionId, OrdersDto order)
        {
            await Clients.Client(connectionId).SendAsync("ReciveMessage", order);
        }

        /// <summary>
        /// Called when a client connects to the hub.
        /// Sends a notification to all connected clients with the connection ID of the newly connected client.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a client disconnects from the hub.
        /// Sends a notification to all connected clients with the connection ID of the disconnected client.
        /// </summary>
        /// <param name="exception">An optional exception that caused the disconnection.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }

}
