using System;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_SocketSlot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AuctionService.Interfaces;
using Services.DataTransferObjects.Objects;

namespace EPAM_API.Controllers
{
    [Route("api/auction")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly ISlotStorage _slotStorage;
        private readonly IUserProvider _userProvider;

        public AuctionController(IUserProvider userProvider, IAuctionService auctionService)
        {
            _userProvider = userProvider;
            _auctionService = auctionService;
            //_slotStorage = slotStorage;
        }

        [Authorize]
        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] AuctionDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var auction = await _auctionService.InsertAuctionAsync(request, _userProvider.GetUserId(), _userProvider.GetUserRole());
            return Ok(JsonSerializer.Serialize(auction));

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            return Ok(JsonSerializer.Serialize(auction));
        }

        [AllowAnonymous]
        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(string? filters, int? limit, int? offset)
        {
            var auctions = await _auctionService.GetAllAsync(filters, limit, offset);

            return Ok(JsonSerializer.Serialize(auctions));
        }

        [Authorize]
        [HttpGet, Route("my")]
        public async Task<IActionResult> GetMyAuctions(int? limit, int? offset)
        {
            var userId = _userProvider.GetUserId();
            var auctions = await _auctionService.GetAllAsync($"userId={userId}", limit, offset);

            return Ok(JsonSerializer.Serialize(auctions));
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if (User.IsInRole(Roles.User) || User.IsInRole(Roles.Premium) || User.IsInRole(Roles.Plus))
            {
                // TODO: compare the auction creator with current user
            }
            if (User.IsInRole(Roles.Moderator))
            {

            }
            if(User.IsInRole(Roles.Administrator))
            {
                // TODO: instant remove
            }

            return NotFound();
        }

        [HttpGet, Route("currentPrice")]
        public IActionResult GetCurrentPrice(Guid auctionId)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost, Route("bid")]
        public async Task<IActionResult> PlaceBid(Guid auctionId, decimal price)
        {
            var bid = await _auctionService.InsertBidAsync(auctionId, _userProvider.GetUserId(), price);
            await _slotStorage.NotifySlotsAsync(auctionId, JsonSerializer.Serialize(bid));
            return Ok();
        }

    }
}