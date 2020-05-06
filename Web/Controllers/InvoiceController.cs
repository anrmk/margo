using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Extensions;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class InvoiceController: BaseController<InvoiceController> {
        private readonly INsiBusinessManager _nsiBusinessManager;
        private readonly ICrudBusinessManager _crudBusinessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper, ApplicationContext context,
            INsiBusinessManager nsiBusinessManager, ICrudBusinessManager crudBusinessManager) : base(logger, mapper, context) {
            _nsiBusinessManager = nsiBusinessManager;
            _crudBusinessManager = crudBusinessManager;
        }

        public ActionResult Index() {
            var model = new InvoiceFilterViewModel();
            return View(model);
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<InvoiceViewModel>(item));
        }

        public async Task<ActionResult> Create() {
            var accounts = await _crudBusinessManager.GetVaccounts();
            ViewBag.Accounts = accounts.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList(); ;

            return View(new InvoiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateInvoice(_mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Edit), new { Id = item.Id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }

            var accounts = await _crudBusinessManager.GetVaccounts();
            ViewBag.Accounts = accounts.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList(); ;

            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            var accounts = await _crudBusinessManager.GetVaccounts();
            ViewBag.Accounts = accounts.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList(); ;

            return View(_mapper.Map<InvoiceViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateInvoice(id, _mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Edit), new { Id = id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            var accounts = await _crudBusinessManager.GetVaccounts();
            ViewBag.Accounts = accounts.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id.ToString() }).ToList(); ;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _crudBusinessManager.DeleteInvoice(id);
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
    public class InvoiceController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public InvoiceController(IMapper mapper, IViewRenderService viewRenderService,
            ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<InvoiceListViewModel>> GetInvoices(InvoiceFilterViewModel model) {
            var result = await _businessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }
    }
}