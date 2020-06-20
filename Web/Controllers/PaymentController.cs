using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class PaymentController: BaseController<PaymentController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public PaymentController(ILogger<PaymentController> logger, IMapper mapper,
            ICrudBusinessManager crudBusinessManager) : base(logger, mapper) {
            _crudBusinessManager = crudBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetPayment(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<PaymentViewModel>(item));
        }

        public async Task<IActionResult> Create(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            var rnd = new Random();

            var model = new PaymentViewModel() {
                Date = DateTime.Now,
                InvoiceId = item.Id,
                InvoiceNo = item.No,
                Amount = item.Amount - (item.PaymentAmount ?? 0),
                No = string.Format("{0}-{1}-{2}", DateTime.Now.ToString("yyyyMMdd"), rnd.Next(0, 99), Guid.NewGuid().ToString().Substring(0, 5))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreatePayment(_mapper.Map<PaymentDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    if(IsAjaxRequest) {
                        return Ok(_mapper.Map<PaymentDto>(item));
                    }
                    return RedirectToAction("Index", "Invoice", new { Id = item.Id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetPayment(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<PaymentViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, PaymentViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdatePayment(id, _mapper.Map<PaymentDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Edit), new { Id = id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _crudBusinessManager.DeletePayment(id);
                if(item == false) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));

            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    //[ApiController]
    public class PaymentController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _crudBusinessManager;
        public PaymentController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _crudBusinessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<PaymentViewModel>> GetPayments([FromQuery] PaymentFilterViewModel model) {
            var result = await _crudBusinessManager.GetPaymentPager(_mapper.Map<PaymentFilterDto>(model));
            return new Pager<PaymentViewModel>(_mapper.Map<List<PaymentViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> Delete([FromBody] List<long> id) {
            try {
                if(id.Count > 0) {
                    var result = await _crudBusinessManager.DeletePayment(id.ToArray());
                    return Ok(result);
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok(false);
        }
    }
}