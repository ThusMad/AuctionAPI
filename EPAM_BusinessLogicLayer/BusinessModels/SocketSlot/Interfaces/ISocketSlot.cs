using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPAM_BusinessLogicLayer.BusinessModels.SocketSlot.Interfaces
{
    public interface ISocketSlot
    {
        void SubscribeToSlot(Guid id, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs);
        void NotifyAllSubscribers(Guid id, string message);
    }
}