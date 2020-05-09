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
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class VaccountController: BaseController<VaccountController> {

        private readonly ICrudBusinessManager _crudBusinessManager;

        public VaccountController(ILogger<VaccountController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context, ICrudBusinessManager businessManager) : base(logger, mapper, notificationHub, context) {
            _crudBusinessManager = businessManager;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Create() {
            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            return View(new VaccountViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VaccountViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateVaccount(_mapper.Map<VaccountDto>(model));
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

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetVaccount(id);
            if(item == null) {
                return NotFound();
            }

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            var securityQuestion = await _crudBusinessManager.GetVaccountSecurityQuestions(item.SecurityId ?? 0);
            ViewBag.SecurityQuestions = _mapper.Map<List<VaccountSecurityQuestionViewModel>>(securityQuestion);

            return View(_mapper.Map<VaccountViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, VaccountViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateVaccount(id, _mapper.Map<VaccountDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            return RedirectToAction(nameof(Edit), new { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _crudBusinessManager.DeleteVaccount(id);
                if(item == false) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));

            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSecurity(long accountId, VaccountSecurityViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateVaccountSecurity(accountId, _mapper.Map<VaccountSecurityDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            return RedirectToAction(nameof(Edit), new { Id = accountId });
        }

        public async Task<ActionResult> CreateSecurityQuestion(long securityId) {
            var security = await _crudBusinessManager.GetVaccountSecurity(securityId);
            if(security == null) {
                return NotFound();
            }

            var account = await _crudBusinessManager.GetVaccountBySecurityId(securityId);
            if(account == null) {
                return NotFound();
            }

            ViewBag.AccountId = account.Id;
            ViewBag.AccountName = account.UserName;

            return View(new VaccountSecurityQuestionViewModel() {
                SecurityId = securityId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSecurityQuestion(VaccountSecurityQuestionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateVaccountSecurityQuestion(_mapper.Map<VaccountSecurityQuestionDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(EditSecurityQuestion), new { Id = item.Id });
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("All", e.Message);
                BadRequest(e);
            }

            return View(model);
        }

        public async Task<ActionResult> EditSecurityQuestion(long id) {
            var item = await _crudBusinessManager.GetVaccountSecurityQuestion(id);
            if(item == null) {
                return NotFound();
            }

            var account = await _crudBusinessManager.GetVaccountBySecurityId(item.SecurityId ?? 0);
            if(account == null) {
                return NotFound();
            }

            ViewBag.AccountId = account.Id;
            ViewBag.AccountName = account.UserName;

            return View(_mapper.Map<VaccountSecurityQuestionViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSecurityQuestion(long id, VaccountSecurityQuestionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateVaccountSecurityQuestion(id, _mapper.Map<VaccountSecurityQuestionDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            var account = await _crudBusinessManager.GetVaccountBySecurityId(model.SecurityId ?? 0);
            if(account == null) {
                return NotFound();
            }

            ViewBag.AccountId = account.Id;
            ViewBag.AccountName = account.UserName;

            return View(model);
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    public class VaccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public VaccountController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<VaccountListViewModel>> GetVaccounts(VaccountFilterViewModel model) {
            var result = await _businessManager.GetVaccountPager(_mapper.Map<VaccountFilterDto>(model));
            var pager = new Pager<VaccountListViewModel>(_mapper.Map<List<VaccountListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
            return pager;
        }
    }
}