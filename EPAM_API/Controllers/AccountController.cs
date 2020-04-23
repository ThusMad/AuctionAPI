using System;
using System.Threading.Tasks;
using EPAM_API.Models;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EPAM_API.Controllers
{
    [Route("api/account")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IUserProvider userProvider,  ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountService = accountService;
            _userProvider = userProvider;

            if (_logger == null || _accountService == null)
            {
                throw new Exception($"Service wasn't injected in {nameof(AccountController)} ctor");
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAccount([FromBody]RegistrationDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userDto = await _accountService.CreateUserAsync(request, new[] { Roles.User });

            return Ok(JsonSerializer.Serialize(userDto));
        }

        [Authorize]
        [HttpPatch]
        public IActionResult Update([FromBody]ApplicationUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Malformed request");
            }

            if (request.Id != _userProvider.GetUserId().ToString())
            {
                return BadRequest("Can't update foreign user");
            }

            _accountService.UpdateUser(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _accountService.GetUserByIdAsync(userId);
            if (user != null)
            {
                return Ok(JsonSerializer.Serialize(user));
            }

            return StatusCode(404, $"User with following {nameof(userId)} = {userId} was not found");
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int? limit, int? offset)
        {
            return Ok();
        }
    }
}