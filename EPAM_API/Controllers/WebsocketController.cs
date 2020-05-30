using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using EPAM_SocketSlot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPAM_API.Controllers
{
    [Route("ws")]
    [Controller]
    public class WebsocketController : ControllerBase
    {
        private ISlotStorage slotStorage;
        private ISlotFactory slotFactory;

        public WebsocketController(ISlotFactory slotFactory, ISlotStorage slotStorage)
        {
            this.slotFactory = slotFactory;
            this.slotStorage = slotStorage;
        }

        public async Task<IActionResult> Get(string streamName)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();
                var socketFinishedTcs = new TaskCompletionSource<object>();
                var streamParams = streamName.Split('@', StringSplitOptions.RemoveEmptyEntries);

                var slot = slotStorage.GetSlot(Guid.Parse(streamParams[0]));

                if (slot != null)
                {
                    slot.AddSubscriber(webSocket, socketFinishedTcs);
                }
                else
                {
                    var newSlot = slotFactory.CreateInstance();
                    await slotStorage.AddSlot(Guid.Parse(streamParams[0]), newSlot);
                    newSlot.AddSubscriber(webSocket, socketFinishedTcs);
                }

                await socketFinishedTcs.Task.ConfigureAwait(false);

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client close connection", CancellationToken.None);
                return Ok();
            }

            return BadRequest();
        }

    }
}