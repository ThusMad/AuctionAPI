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
using Services.TokenService.Interfaces;
using Services.UploadService.Interfaces;

namespace EPAM_API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUploadService _uploadService;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ITokenService tokenService, IUploadService uploadService, IUserProvider userProvider,  ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountService = accountService;
            _tokenService = tokenService;
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
        [HttpPatch]
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

        [Authorize]
        [HttpGet, Route("fee")]
        public async Task<IActionResult> GetFee()
        {
            var id = _userProvider.GetUserId();
            var fee = await _accountService.GetUserFeeAsync(id);

            return Ok(JsonSerializer.Serialize(fee));
        }

        [Authorize]
        [HttpGet, Route("role")]
        public async Task<IActionResult> GetRole()
        {
            var id = _userProvider.GetUserId();
            var role = await _accountService.GetUserRoleAsync(id);

            return Ok(JsonSerializer.Serialize(role));
        }

        [AllowAnonymous]
        [HttpGet, Route("preview")]
        public async Task<IActionResult> GetPreview(Guid userId)
        {
            var user = await _accountService.GetUserByIdAsync<ApplicationUserPreviewDTO>(userId);

            return Ok(JsonSerializer.Serialize(user));
        }

        [Authorize]
        [HttpGet, Route("detailed")]
        public async Task<IActionResult> GetDetailed(Guid userId)
        {
            if (_userProvider.GetUserId() != userId)
            {
                return Forbid(JwtBearerDefaults.AuthenticationScheme);
            }

            var user = await _accountService.GetUserByIdAsync<ApplicationUserDto>(userId);
            return Ok(JsonSerializer.Serialize(user));

        }

        [Authorize]
        [HttpDelete, Route("deleteAccount")]
        public async Task<IActionResult> Delete()
        {
            var userId = _userProvider.GetUserId();
            await _tokenService.RemoveTokenFromUserAsync(userId);

            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Cre");

            _logger.Log(LogLevel.Debug, $"user {userId} logged out");

            await _accountService.RemoveUserAsync(userId);

            return Ok();
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet, Route("getAll")]
        public async Task<IActionResult> GetAll(int? limit, int? offset)
        {
            var users = await _accountService.GetAllUsersAsync(limit, offset);

            return Ok(JsonSerializer.Serialize(users));
        }

        [Authorize]
        [HttpPost, Route("addProfilePicture"), DisableRequestSizeLimit]
        public async Task<IActionResult> AddProfilePicture(IFormFile file)
        {
            var userId = _userProvider.GetUserId();
            var imagePath = await _uploadService.UploadAsync(file);

            try
            {
                await _accountService.AttachProfilePictureAsync(userId, imagePath);
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