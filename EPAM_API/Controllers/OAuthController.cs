using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPAM_API.Models;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EPAM_API.Controllers
{
    [Route("api/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<OAuthController> _logger;

        public OAuthController(ITokenProvider tokenProvider, IAccountService accountService, ITokenService tokenService, IUserProvider userProvider, ILogger<OAuthController> logger)
        {
            _tokenProvider = tokenProvider;
            _accountService = accountService;
            _tokenService = tokenService;
            _userProvider = userProvider;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost, Route("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateToken([FromBody]AuthenticationDTO authenticationRequest)
        {
            if (!await _accountService.IsValidUsernameAndPasswordCombinationAsync(authenticationRequest.Username, authenticationRequest.Password))
            {
                return BadRequest(new ErrorDetails()
                {
                    Message = "Username or password is incorrect",
                    StatusCode = 400
                }.ToString());
            }

            var accessToken = await _tokenProvider.GenerateAccessToken(authenticationRequest.Username);
            var refreshToken = _tokenProvider.GenerateRefreshToken();

            await _tokenService.UpdateRefreshTokenAsync(authenticationRequest.Username, refreshToken, 15);

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id",
                accessToken,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(40),
                    Path = "/"
                });
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Cre",
                refreshToken,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(40),
                    Path = "/"
                });

            _logger.Log(LogLevel.Debug, $"user {authenticationRequest.Username} obtain token");

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet, Route("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Remove(0, 7);
            var refreshToken = HttpContext.Request.Headers["RefreshToken"].FirstOrDefault();

            if (token == null || refreshToken == null)
            {
                return Unauthorized(new ErrorDetails()
                {
                    Message = "Credentials not provided for request",
                    StatusCode = 401
                });
            }

            var userId = _tokenProvider.GetUserIdFromExpiredToken(token);

            if (!_tokenService.CheckTokenIdentity(userId, refreshToken))
            {
                return BadRequest("Refresh token for current user don't match with stored");
            }

            var newToken = _tokenProvider.RefreshAccessToken(token, out var newRefreshToken);

            await _tokenService.UpdateRefreshTokenAsync(userId, newRefreshToken);

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id",
                newToken,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(60)
                });
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Cre",
                newRefreshToken,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(60)
                });

            _logger.Log(LogLevel.Debug, $"user {userId} refreshed tokens");

            return Ok();
        }

        [Authorize]
        [HttpGet, Route("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            var userId = _userProvider.GetUserId();

            await _tokenService.RemoveTokenFromUserAsync(userId);

            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Cre");

            _logger.Log(LogLevel.Debug, $"user {userId} logged out");

            return Ok();
        }
    }
}