using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class SupplierController: BaseController<SupplierController> {
        private readonly ICrudBusinessManager _businessManager;

        public SupplierController(ILogger<SupplierController> logger, IMapper mapper, ApplicationContext context, ICrudBusinessManager businessManager) : base(logger, mapper, context) {
            _businessManager = businessManager;
        }

        // GET: Supplier
        public ActionResult Index() {
            return View();
        }

        // GET: Supplier/Details/5
        public async Task<ActionResult> Details(long id) {
            var item = await _businessManager.GetSupplier(id);
            return View(_mapper.Map<CompanyViewModel>(item));
        }

        // GET: Supplier/Create
        public async Task<ActionResult> Create() {
            var model = new SupplierViewModel();

            var companies = await _businessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            return View(model);
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SupplierViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _businessManager.CreateSupplier(_mapper.Map<SupplierDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Edit), new { id = item.Id });
                }

            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }
            return View(model);
        }

        // GET: Supplier/Edit/5
        public async Task<ActionResult> Edit(long id) {
            var item = await _businessManager.GetSupplier(id);
            if(item == null) {
                return NotFound();
            }

            var companies = await _businessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            return View(_mapper.Map<SupplierViewModel>(item));
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, SupplierViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _businessManager.UpdateSupplier(id, _mapper.Map<SupplierDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Edit), new { id = item.Id});
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            var companies = await _businessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            return View(model);
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _businessManager.DeleteSupplier(id);
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
    public class SupplierController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public SupplierController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<SupplierListViewModel>> GetSuppliers(PagerFilterViewModel model) {
            var result = await _businessManager.GetSupplierPager(_mapper.Map<PagerFilter>(model));
            return new Pager<SupplierListViewModel>(_mapper.Map<List<SupplierListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }
    }
}