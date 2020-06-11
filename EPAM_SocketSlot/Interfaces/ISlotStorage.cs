using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPAM_SocketSlot.Interfaces
{
    public interface ISlotStorage
    {
        Task NotifySlotsAsync(Guid slotId, string data);
        Task AddSubscriber(Guid slotId, WebSocket webSocket, TaskCompletionSource<object> socketFinishedTcs);
        ISocketSlot? GetSlot(Guid slotId);
    }
}