using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;

namespace EPAM_API.Controllers
{
    [Route("ws")]
    [Controller]
    public class WebsocketController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public WebsocketController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }


        public async Task<IActionResult> Get(string streamName)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();
                var socketFinishedTcs = new TaskCompletionSource<object>();

                var streamParams = streamName.Split('@', StringSplitOptions.RemoveEmptyEntries);

                _auctionService.SubscribeToAuctionBidUpdatesAsync(Guid.Parse(streamParams[0]), webSocket, socketFinishedTcs);

                await socketFinishedTcs.Task;

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client close connection", CancellationToken.None);
                return Ok();
            }

            return BadRequest();
        }

    }
}