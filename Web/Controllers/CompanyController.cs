using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class CompanyController: BaseController<CompanyController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper, ApplicationContext context,
            ICrudBusinessManager crudBusinessManager) : base(logger, mapper, context) {
            this._crudBusinessManager = crudBusinessManager;
        }

        // GET: Company
        public ActionResult Index() {
            return View();
        }

        // GET: Company/Details/5
        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetCompany(id);
            return View(_mapper.Map<CompanyViewModel>(item));
        }

        // GET: Company/Create
        public ActionResult Create() {
            var model = new CompanyGeneralViewModel();
            return View(model);
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateCompany(_mapper.Map<CompanyGeneralDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Edit), new { Id = item.Id });
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("All", e.Message);
                BadRequest(e);
            }
            return View(model);
        }

        // GET: Company/Edit/5
        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<CompanyViewModel>(item));
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, CompanyGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateCompany(id, _mapper.Map<CompanyGeneralDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }
            return RedirectToAction(nameof(Edit), new { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddress(long companyId, CompanyAddressViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateCompanyAddress(companyId, _mapper.Map<CompanyAddressDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            return RedirectToAction(nameof(Edit), new { Id = companyId });
        }

        // POST: Company/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _crudBusinessManager.DeleteCompany(id);
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
    public class CompanyController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public CompanyController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<CompanyListViewModel>> GetCompanies(PagerFilterViewModel model) {
            var result = await _businessManager.GetCompanyPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<CompanyListViewModel>(_mapper.Map<List<CompanyListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
            return pager;
        }

        //[HttpGet]
        //[Route("{id}/suppliers")]
        //public async Task<List<SupplierListViewModel>> GetSuppliers(long companyId) {
        //    var result = await _businessManager.GetSuppliers(companyId);
        //    return _mapper.Map<List<SupplierListViewModel>>(result);
        //}
    }
}