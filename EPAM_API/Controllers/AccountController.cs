using System;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.BusinessModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.AccountService.Interfaces;
using Services.DataTransferObjects.Objects;
using Services.Infrastructure.Exceptions;
using Services.UploadService.Interfaces;

namespace EPAM_API.Controllers
{
    [Route("api/account")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUploadService _uploadService;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IUploadService uploadService, IUserProvider userProvider,  ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountService = accountService;
            _uploadService = uploadService;
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

            var userDto = await _accountService.InsertUserAsync(request, new[] { Roles.User });

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var id = _userProvider.GetUserId();
            var user = await _accountService.GetUserByIdAsync<ApplicationUserDto>(id);

            return Ok(JsonSerializer.Serialize(user));
        }

        [AllowAnonymous]
        [HttpGet, Route("view")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _accountService.GetUserByIdAsync<ApplicationUserPreviewDTO>(userId);

            return Ok(JsonSerializer.Serialize(user));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _accountService.RemoveUserAsync(_userProvider.GetUserId());

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
        public async Task<IActionResult> GetAll(int? limit, int? offset)
        {
            var users = await _accountService.GetAllUsersAsync(limit, offset);

            return Ok(JsonSerializer.Serialize(users));
        }

        [HttpPost("attach"), DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
        {
            var userId = _userProvider.GetUserId();
            var imagePath = await _uploadService.UploadAsync(file);

            try
            {
                await _accountService.AttachProfilePicture(userId, imagePath);
            }
            catch (Exception)
            {
                await _uploadService.RemoveAsync(imagePath);
                throw;
            }

            return Ok(JsonSerializer.Serialize(imagePath));
        }
    }
}