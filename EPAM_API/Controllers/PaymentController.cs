using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Helpers;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAM_API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IPaymentService _paymentService;

        public PaymentController(IUserProvider userProvider, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _userProvider = userProvider;
        }

        [Authorize]
        [HttpPost, Route("proceed")]
        public IActionResult ProceedPayment(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult CreatePayment(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete, Route("create")]
        public IActionResult DeletePayment(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetPayment(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("getAll")]
        public IActionResult GetAllPayments(int? limit, int? offset)
        {
            return Ok();
        }

        #region Methods

        [Authorize]
        [HttpPost, Route("methods/create")]
        public IActionResult CreatePaymentMethod([FromBody]PaymentMethodDTO request)
        {
            _paymentService.AddPaymentMethod(_userProvider.GetUserId(), request);

            return Ok(JsonSerializer.Serialize(request));
        }

        [Authorize]
        [HttpDelete, Route("methods/delete")]
        public IActionResult DeletedPaymentMethod(int? paymentMethodId)
        {
            if (paymentMethodId == null)
            {
                return BadRequest($"Malformed request, {nameof(paymentMethodId)} must be set");
            }

            return Ok("record has successfully deleted");
        }

        [Authorize]
        [HttpGet, Route("methods/setDefault")]
        public IActionResult GetDefaultPaymentMethods(Guid id)
        {
            try
            {
                return Ok(JsonSerializer.Serialize(_paymentService.GetPaymentMethods(_userProvider.GetUserId())));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet, Route("methods/get")]
        public IActionResult GetPaymentMethods(Guid id)
        {
            try
            {
                return Ok(JsonSerializer.Serialize(_paymentService.GetPaymentMethods(_userProvider.GetUserId())));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet, Route("methods/getDefault")]
        public IActionResult GetDefaultPaymentMethods()
        {
            try
            {
                return Ok(JsonSerializer.Serialize(_paymentService.GetPaymentMethods(_userProvider.GetUserId())));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet, Route("methods/getAll")]
        public IActionResult GetPaymentMethods(int? limit, int? offset)
        {
            try
            {
                return Ok(JsonSerializer.Serialize(_paymentService.GetPaymentMethods(_userProvider.GetUserId())));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region Infos

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult GetPaymentInfo(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("info")]
        public IActionResult GetInfo(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("info/getAll")]
        public IActionResult GetAllInfo(int? limit, int? offset)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete, Route("info")]
        public IActionResult DeleteInfo(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("info/getDetails")]
        public IActionResult GetAllInfo(Guid id)
        {
            return Ok();
        }

        #endregion

    }
}