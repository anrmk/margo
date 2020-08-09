using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Enums;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class PaymentController: BaseController<PaymentController> {
        public PaymentController(ILogger<PaymentController> logger, IMapper mapper)
            : base(logger, mapper) { }

        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IPaymentBusinessManager _paymentBusinessManager;
        private readonly IInvoiceBusinessManager _invoiceBusinessManager;

        public PaymentController(
            IMapper mapper,
            IViewRenderService viewRenderService,
            IPaymentBusinessManager paymentBusinessManager,
            IInvoiceBusinessManager invoiceBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _paymentBusinessManager = paymentBusinessManager;
            _invoiceBusinessManager = invoiceBusinessManager;
        }

        [HttpGet("GetPayments", Name = "GetPayments")]
        public async Task<PagerDto<PaymentListViewModel>> GetPayments([FromQuery] PaymentFilterViewModel model) {
            var result = await _paymentBusinessManager.GetPaymentPager(_mapper.Map<PaymentFilterDto>(model));
            return new PagerDto<PaymentListViewModel>(
                _mapper.Map<List<PaymentListViewModel>>(result.Data),
                result.RecordsTotal,
                result.Start,
                result.PageSize);
        }

        [HttpGet("DetailsPayment", Name = "DetailsPayment")]
        public async Task<IActionResult> DetailsPayment([FromQuery] Guid id) {
            var payment = await _paymentBusinessManager.GetPayment(id);
            if(payment == null)
                return NotFound();
            var html = await _viewRenderService.RenderToStringAsync(
                "_DetailsPartial",
                _mapper.Map<PaymentListViewModel>(payment));
            return Ok(html);
        }

        [HttpGet("AddPayment", Name = "AddPayment")]
        public async Task<IActionResult> AddPayment() {
            var invoices = await _invoiceBusinessManager.GetInvoices();
            var viewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()) {
                {
                    "Invoices",
                    _mapper.Map<List<InvoiceViewModel>>(invoices)
                        .Select(x=> new SelectListItem {
                            Text = x.No,
                            Value = x.Id.ToString()
                        })
                },
                {
                    "PaymentMethods",
                    EnumExtension.GetAll<PaymentMethodEnum>()
                        .Select(x=> new SelectListItem {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        })
                }};

            var html = await _viewRenderService.RenderToStringAsync(
                "_CreatePartial", new PaymentViewModel(), viewData);

            return Ok(html);
        }

        [HttpPost("CreatePayment", Name = "CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _paymentBusinessManager.CreatePayment(_mapper.Map<PaymentDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<PaymentViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("EditPayment", Name = "EditPayment")]
        public async Task<IActionResult> EditPayment([FromQuery] Guid id) {
            var item = await _paymentBusinessManager.GetPayment(id);
            if(item == null)
                return NotFound();

            var viewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()) {
                {
                    "PaymentMethods",
                    EnumExtension.GetAll<PaymentMethodEnum>()
                        .Select(x=> new SelectListItem {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        })
                }};

            var html = await _viewRenderService.RenderToStringAsync(
                "_EditPartial", _mapper.Map<PaymentViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdatePayment", Name = "UpdatePayment")]
        public async Task<IActionResult> UpdatePayment([FromQuery] Guid id, [FromBody] PaymentViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _paymentBusinessManager.UpdatePayment(id, _mapper.Map<PaymentDto>(model));
                if(item == null)
                    throw new Exception("No records have been updated!");

                return Ok(_mapper.Map<PaymentViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("DeletePayments", Name = "DeletePayments")]
        public async Task<IActionResult> DeletePayments([FromQuery] Guid[] id) {
            try {
                if(id.Length > 0) {
                    var result = await _paymentBusinessManager.DeletePayments(id);
                    return Ok(result);
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok(false);
        }
    }
}
