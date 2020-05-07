using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class CompanyController: BaseController<CompanyController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context,
            ICrudBusinessManager crudBusinessManager) : base(logger, mapper, notificationHub, context) {
            _crudBusinessManager = crudBusinessManager;
        }

        public ActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<CompanyViewModel>(item));
        }

        public ActionResult Create() {
            var model = new CompanyGeneralViewModel();
            return View(model);
        }

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
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<CompanyViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, CompanyGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateCompany(id, _mapper.Map<CompanyGeneralDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    await ClientNotify($"Company Id: {item.Id}: This record was modified by {item.UpdatedBy} on {item.UpdatedDate.ToString()}");
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
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
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            return RedirectToAction(nameof(Edit), new { Id = companyId });
        }

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
    }
}