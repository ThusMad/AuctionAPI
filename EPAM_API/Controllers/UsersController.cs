using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EPAM_API.Controllers
{
    [Route("api/users")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost, Route("authenticate")]
        public IActionResult Authenticate(long? timestamp, int? recvWindow, [FromBody]UserDTO request)
        {
            try
            {
                var user = _userService.GetUser(request.Username, request.Password);

                return Ok(JsonSerializer.Serialize(user));
            }
            catch (ValidationException e)
            {
                Debug.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}