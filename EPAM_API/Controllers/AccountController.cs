using System;
using System.Linq;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            if (_userProvider == null || _accountService == null)
            {
                throw new NotInjectedException($"Service wasn't injected in {nameof(AccountController)} ctor");
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("create")]
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
        [HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody]ApplicationUserPatchModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Malformed request");
            }

            var id = _userProvider.GetUserId();

            await _accountService.UpdateUserAsync(id, request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _accountService.GetUserByIdAsync<ApplicationUserPreviewDTO>(userId);

            return Ok(JsonSerializer.Serialize(user));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _accountService.DeleteUserAsync(_userProvider.GetUserId(), "no reason");

            return Ok();
        }

        [Authorize]
        [HttpGet, Route("detail")]
        public async Task<IActionResult> GetDetailed(Guid userId)
        {
            if (_userProvider.GetUserId() != userId)
            {
                return Forbid(JwtBearerDefaults.AuthenticationScheme);
            }

            var user = await _accountService.GetUserByIdAsync<ApplicationUserDto>(userId);
            return Ok(JsonSerializer.Serialize(user));

        }

        [Authorize(Roles = Roles.User)]
        [HttpGet, Route("getAll")]
        public IActionResult GetAll(int? limit, int? offset)
        {
            var users = _accountService.GetAllUsers(limit, offset);
            var applicationUserDtos = users as ApplicationUserDto[] ?? users.ToArray();

            if (!applicationUserDtos.Any())
            {
                return NotFound("There is no users in provided range");
            }

            return Ok(JsonSerializer.Serialize(applicationUserDtos));
        }
    }
}