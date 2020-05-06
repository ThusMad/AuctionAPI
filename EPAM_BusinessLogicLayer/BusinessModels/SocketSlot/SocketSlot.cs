using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.BusinessModels.SocketSlot.Interfaces;

namespace EPAM_BusinessLogicLayer.BusinessModels.SocketSlot
{
    class SocketSlot : ISocketSlot
    {
        private readonly ConcurrentDictionary<Guid, List<WebSocket>> _bidUpdateSockets;

        public SocketSlot()
        {
            _bidUpdateSockets = new ConcurrentDictionary<Guid, List<WebSocket>>();
        }

        public void SubscribeToSlot(Guid id, WebSocket ws, TaskCompletionSource<object> socketFinishedTcs)
        {
            if (!_bidUpdateSockets.ContainsKey(id))
            {
                while (!_bidUpdateSockets.TryAdd(id, new List<WebSocket>()))
                {
                    Thread.Sleep(10);
                }
            }

            _bidUpdateSockets[id].Add(ws);

            while (ws.State == WebSocketState.Open)
            {
                Thread.Sleep(1000 * 15);
            }

            Debug.WriteLine("closing websocket");

            List<WebSocket> sockets;

            while (!_bidUpdateSockets.TryGetValue(id, out sockets))
            {
                Thread.Sleep(10);
            }

            sockets?.Remove(ws);
            socketFinishedTcs.SetResult(new object());
        }

        public void NotifyAllSubscribers(Guid id, string message)
        {
            if (_bidUpdateSockets.ContainsKey(id))
            {
                foreach (var socket in _bidUpdateSockets[id])
                {
                    Task.Run(async () => Utility.SendStringToWebsocket(socket, message, CancellationToken.None));
                }
            }
        }
    }
}
