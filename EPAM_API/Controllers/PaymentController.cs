using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Helpers;
using EPAM_API.Services.Interfaces;
using EPAM_BusinessLogicLayer.DataTransferObjects;
using EPAM_BusinessLogicLayer.Services.Interfaces;
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
        public async Task<IActionResult> CreatePaymentMethod([FromBody]PaymentMethodDTO request)
        {
            await _paymentService.InsertPaymentMethodAsync(_userProvider.GetUserId(), request);

            return Ok(JsonSerializer.Serialize(request));
        }

        [Authorize]
        [HttpDelete, Route("methods/delete")]
        public async Task<IActionResult> DeletedPaymentMethod(Guid id)
        {
            await _paymentService.DeletePaymentMethodAsync(id, _userProvider.GetUserId());

            return Ok();
        }

        [Authorize]
        [HttpPost, Route("methods/setDefault")]
        public async Task<IActionResult> SetDefaultPaymentMethods(Guid methodId)
        {
            await _paymentService.SetDefaultPaymentMethodAsync(_userProvider.GetUserId(), methodId);
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("methods/get")]
        public async Task<IActionResult> GetPaymentMethods(Guid id)
        {
            var method = await _paymentService.GetPaymentMethodAsync(id, _userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(method));
        }

        [Authorize]
        [HttpGet, Route("methods/getDefault")]
        public async Task<IActionResult> GetDefaultPaymentMethods()
        {
            var method = await _paymentService.GetDefaultPaymentMethodAsync(_userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(method));
        }

        [Authorize]
        [HttpGet, Route("methods/getAll")]
        public async Task<IActionResult> GetPaymentMethods(int? limit, int? offset)
        {
            var methods = await _paymentService.GetPaymentMethodsAsync(_userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(methods));
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