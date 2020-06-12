using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            return null;
        }
    }
}
