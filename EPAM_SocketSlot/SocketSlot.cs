using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_SocketSlot.Interfaces;

namespace EPAM_SocketSlot
{
    class SocketSlot : ISocketSlot
    {
        private readonly List<WebSocket> _sockets;

        public SocketSlot()
        {
            _sockets = new List<WebSocket>();
        }

        public void AddSubscriber(WebSocket webSocket, TaskCompletionSource<object> socketFinishedTcs)
        {
            lock (_sockets)
            {
                _sockets?.Add(webSocket);
            }

            while (webSocket.State == WebSocketState.Open)
            {
                Thread.Sleep(1000 * 5);
            }

            Debug.WriteLine("closing websocket");

            lock (_sockets)
            {
                _sockets?.Remove(webSocket);
            }

            socketFinishedTcs.SetResult(new object());
        }

        public void NotifyAllSubscribers(string message)
        {
            lock (_sockets)
            {
                if (!_sockets.Any()) return;

                _sockets.ForEach(socket =>
                    Task.Run(async () => await Utility.SendStringToWebsocket(socket, message, CancellationToken.None)));
            }
        }
    }
}
