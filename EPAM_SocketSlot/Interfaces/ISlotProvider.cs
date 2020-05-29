using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPAM_SocketSlot.Interfaces
{
    public interface ISlotProvider
    {
        void SubscribeToSlot(Guid id, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs);
    }
}