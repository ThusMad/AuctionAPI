using System;
using System.Text.Json;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DataTransferObjects.Objects;
using Services.PaymentService.Interfaces;

namespace EPAM_API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IPaymentService _paymentService;

        public PaymentController(IUserProvider userProvider, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _userProvider = userProvider;
        }

        [HttpPost, Route("proceed")]
        public IActionResult ProceedPayment(Guid id)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost, Route("create")]
        public async Task<IActionResult> CreatePayment(Guid auctionId)
        {
            var userId = _userProvider.GetUserId();
            var payment = await _paymentService.InsertAuctionPaymentAsync(userId, auctionId);
            return Ok(JsonSerializer.Serialize(payment));
        }

        [HttpDelete]
        public IActionResult DeletePayment(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetPayment(Guid id)
        {
            return Ok();
        }

        [HttpGet, Route("getAll")]
        public IActionResult GetAllPayments(int? limit, int? offset)
        {
            return Ok();
        }

        [HttpGet, Route("statistic")]
        public async Task<IActionResult> GetPaymentStatistic()
        {
            var userId = _userProvider.GetUserId();
            var statistic = await _paymentService.GetPaymentStatisticAsync(userId);

            return Ok(JsonSerializer.Serialize(statistic));
        }

        #region Methods

        [HttpPost, Route("methods/create")]
        public async Task<IActionResult> CreatePaymentMethod([FromBody]PaymentMethodDTO request)
        {
            await _paymentService.InsertPaymentMethodAsync(_userProvider.GetUserId(), request);

            return Ok(JsonSerializer.Serialize(request));
        }

        [HttpDelete, Route("methods/delete")]
        public async Task<IActionResult> DeletedPaymentMethod(Guid id)
        {
            await _paymentService.DeletePaymentMethodAsync(id, _userProvider.GetUserId());

            return Ok();
        }

        [HttpPost, Route("methods/setDefault")]
        public async Task<IActionResult> SetDefaultPaymentMethods([FromQuery]Guid methodId)
        {
            await _paymentService.SetDefaultPaymentMethodAsync(_userProvider.GetUserId(), methodId);
            return Ok();
        }

        [HttpGet, Route("methods/get")]
        public async Task<IActionResult> GetPaymentMethod(Guid methodId)
        {
            var method = await _paymentService.GetPaymentMethodAsync(methodId, _userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(method));
        }

        [HttpGet, Route("methods/getDefault")]
        public async Task<IActionResult> GetDefaultPaymentMethods()
        {
            var method = await _paymentService.GetDefaultPaymentMethodAsync(_userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(method));
        }

        [HttpGet, Route("methods/getAll")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var methods = await _paymentService.GetPaymentMethodsAsync(_userProvider.GetUserId());
            return Ok(JsonSerializer.Serialize(methods));
        }

        #endregion

        #region Infos

        [HttpPost, Route("getInfo")]
        public IActionResult GetPaymentInfo(Guid id)
        {
            return Ok();
        }

        [HttpGet, Route("info")]
        public IActionResult GetInfo(Guid id)
        {
            return Ok();
        }

        [HttpGet, Route("info/getAll")]
        public IActionResult GetAllInfo(int? limit, int? offset)
        {
            return Ok();
        }

        [HttpDelete, Route("info")]
        public IActionResult DeleteInfo(Guid id)
        {
            return Ok();
        }

        [HttpGet, Route("info/getDetails")]
        public IActionResult GetAllInfo(Guid id)
        {
            return Ok();
        }

        #endregion

    }
}