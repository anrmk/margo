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
        private readonly ICrudBusinessManager _businessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper, ApplicationContext context,
            INsiBusinessManager nsiBusinessManager, ICrudBusinessManager businessManager) : base(logger, mapper, context) {
            _nsiBusinessManager = nsiBusinessManager;
            _businessManager = businessManager;
        }

        // GET: Invoice
        public async Task<ActionResult> Index() {
            var companies = await _businessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var model = new InvoiceFilterViewModel();
            return View(model);
        }

        // GET: Invoice/Details/5
        public async Task<ActionResult> Details(long id) {
            var item = await _businessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<InvoiceViewModel>(item);

            return View(model);
        }

        // GET: Invoice/Create
        public ActionResult Create() {
            var model = new InvoiceViewModel();

            return View(model);
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _businessManager.CreateInvoice(_mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
            }

            return View(model);
        }

        // GET: Invoice/Edit/5
        public async Task<ActionResult> Edit(long id) {
            var item = await _businessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<InvoiceViewModel>(item));
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _businessManager.UpdateInvoice(id, _mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
            }

            return View(model);
        }

        // GET: Invoice/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _businessManager.DeleteInvoice(id);
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
    [ApiController]
    public class InvoiceController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public InvoiceController(IMapper mapper, IViewRenderService viewRenderService,
            ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<InvoiceListViewModel>> GetInvoices([FromQuery] InvoiceFilterViewModel model) {
            var result = await _businessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }
    }
}