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
        private readonly ISlotStorage _slotStorage;

        public WebsocketController(ISlotStorage slotStorage)
        {
            _slotStorage = slotStorage;
        }

        public async Task<IActionResult> Get(string streamName)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                return BadRequest();
            }

            var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketFinishedTcs = new TaskCompletionSource<object>();
            var streamParams = streamName.Split('@', StringSplitOptions.RemoveEmptyEntries);

            await _slotStorage.AddSubscriber(Guid.Parse(streamParams[0]), webSocket, socketFinishedTcs);
            await socketFinishedTcs.Task.ConfigureAwait(false);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client close connection", CancellationToken.None);

            return Ok();
        }

    }
}