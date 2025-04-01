using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using Microsoft.AspNetCore.SignalR;

namespace LeadwaycanteenApi.Hubs
{
    /// <summary>
    /// Represents a SignalR hub for handling inventory-related messages.
    /// </summary>
    public class InventoryHub : Hub
    {
        /// <summary>
        /// Sends an inventory message to a specific client identified by connection ID.
        /// </summary>
        /// <param name="connectionId">The unique identifier for the client connection.</param>
        /// <param name="inventoryDto">The inventory data to be sent to the client.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMessage(string connectionId, InventoryDto inventoryDto)
        {
            await Clients.Client(connectionId).SendAsync("ReciveInventoryMessage", inventoryDto);
        }
    }

}
