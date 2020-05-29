using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPAM_SocketSlot.Interfaces
{
    public interface ISocketSlot
    {
        void AddSubscriber(WebSocket webSocket, TaskCompletionSource<object> socketFinishedTcs);
        void NotifyAllSubscribers(string message);
    }
}