using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EPAM_API.Helpers;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPAM_API.Controllers
{
    [Route("api/auction")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IUserProvider _userProvider;

        public AuctionController(IUserProvider userProvider, IAuctionService auctionService)
        {
            _userProvider = userProvider;
            _auctionService = auctionService;
        }

        [Authorize]
        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] AuctionDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var auction = await _auctionService.CreateAuction(request, _userProvider.GetUserId(), _userProvider.GetUserRole());
            return Ok(JsonSerializer.Serialize(auction));

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            var auction = _auctionService.GetById(id);
            if (auction == null)
            {
                return NotFound($"Auction with following id={id} not found");
            }

            return Ok(JsonSerializer.Serialize(auction));
        }

        [AllowAnonymous]
        [HttpGet, Route("getAll")]
        public IActionResult GetAll(string filters, int? limit, int? offset)
        {
            var auctions = _auctionService.GetAll(limit, offset);

            return Ok(JsonSerializer.Serialize(auctions));
        }

        [AllowAnonymous]
        [HttpGet, Route("getByCategory")]
        public IActionResult GetByCategory(string category, int? limit, int? offset)
        {
            var auctions = _auctionService.GetAll(limit, offset);

            return Ok(JsonSerializer.Serialize(auctions));
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if (User.IsInRole(Role.User) || User.IsInRole(Role.GoldUser) || User.IsInRole(Role.PlusUser))
            {
                // TODO: compare the auction creator with current user
            }
            if (User.IsInRole(Role.Moderator))
            {

            }
            if(User.IsInRole(Role.Admin))
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
        public IActionResult PlaceBid([FromBody] BidDTO request)
        {
            _auctionService.PlaceBid(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet, Route("getCategories")]
        public IActionResult GetCategories(int? limit, int? offset)
        {
            var categories = _auctionService.GetCategories(limit, offset);

            return Ok(JsonSerializer.Serialize(categories));
        }

        [Authorize]
        [HttpPost, Route("addCategories")]
        public async Task<IActionResult> AddCategories([FromBody] IEnumerable<AuctionCategoryDto> categories)
        {
            await _auctionService.AddCategoriesAsync(categories);

            return Ok();
        }

    }
}