using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Services.BalanceService.Interfaces;

namespace EPAM_API.Controllers
{
    [Route("api/balance")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IBalanceService _balanceService;

        public BalanceController(IUserProvider userProvider, IBalanceService balanceService)
        {
            _userProvider = userProvider;
            _balanceService = balanceService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _userProvider.GetUserId();
            var balance = await _balanceService.GetUserBalanceAsync(userId);

            return Ok(JsonSerializer.Serialize(balance));
        }

        [Authorize]
        [HttpGet, Route("get")]
        public async Task<IActionResult> GetById(Guid balanceId)
        {
            var userId = _userProvider.GetUserId();
            var balance = await _balanceService.GetBalanceByIdAsync(balanceId, userId);

            return Ok(JsonSerializer.Serialize(balance));
        }

        [Authorize]
        [HttpPost, Route("refill")]
        public async Task<IActionResult> RefillBalance(Guid methodId, decimal amount)
        {
            var userId = _userProvider.GetUserId();
            await _balanceService.RefillBalanceAsync(userId, methodId, amount);

            return Ok();
        }

        [Authorize]
        [HttpGet, Route("transactions")]
        public async Task<IActionResult> GetTransactions(int? limit, int? offset)
        {
            var userId = _userProvider.GetUserId();
            var transactions = await _balanceService.GetAllBalanceTransactionsAsync(userId, limit, offset);
            transactions.Reverse();
            return Ok(JsonSerializer.Serialize(transactions));
        }
    }
}
