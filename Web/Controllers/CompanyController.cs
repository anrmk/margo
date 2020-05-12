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

            var sections = await _crudBusinessManager.GetCompanySections(item.Id);
            ViewBag.Sections = _mapper.Map<List<CompanySectionViewModel>>(sections).ToList();

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

        #region COMPANY SECTIONS
        public async Task<ActionResult> AddSection(long id) {
            var item = await _crudBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }

            var sections = await _crudBusinessManager.GetSections();
            var companySection = await _crudBusinessManager.GetCompanySections(item.Id);
            var exist = sections.Where(x => !companySection.Any(y => y.SectionId == x.Id));

            ViewBag.Sections = exist.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            var modal = new CompanySectionViewModel() { CompanyId = id };

            return View("_AddSectionPartial", modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSection(CompanySectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateCompanySection(_mapper.Map<CompanySectionDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    if(IsAjaxRequest) {
                        return Ok(_mapper.Map<CompanySectionViewModel>(item));
                    } else {
                        return RedirectToAction(nameof(Edit), new { Id = item.CompanyId });
                    }
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSection(long id) {
            try {
                var section = await _crudBusinessManager.GetCompanySection(id);
                if(section == null) {
                    return NotFound();
                }

                if(section.Fields.Count > 0) {
                    return BadRequest($"Delete all fields before remove section: {section.SectionName}");
                }

                var item = await _crudBusinessManager.DeleteCompanySection(id);
                if(item == false) {
                    return NotFound();
                }

                return RedirectToAction(nameof(Edit), new { Id = section.CompanyId });

            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
        }
        #endregion

        #region COMPANY SECTION FIELD
        public async Task<ActionResult> AddSectionField(long companySectionId) {
            var item = await _crudBusinessManager.GetCompanySection(companySectionId);
            if(item == null) {
                return NotFound();
            }

            ViewBag.SectionName = item.SectionName;

            var model = new CompanySectionFieldViewModel() {
                CompanySectionId = item.Id
            };

            return View("_AddSectionFieldPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSectionField(CompanySectionFieldViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var companySection = await _crudBusinessManager.GetCompanySection(model.CompanySectionId);
                    if(companySection == null) {
                        return BadRequest();
                    }

                    var item = await _crudBusinessManager.CreateCompanySectionField(_mapper.Map<CompanySectionFieldDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    if(IsAjaxRequest) {
                        return Ok(_mapper.Map<CompanySectionFieldViewModel>(item));
                    } else {
                        return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });
                    }
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            return View(model);
        }

        public async Task<ActionResult> EditSectionField(long id) {
            var item = await _crudBusinessManager.GetCompanySectionField(id);
            if(item == null) {
                return NotFound();
            }

            return View("_AddSectionFieldPartial", _mapper.Map<CompanySectionFieldViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSectionField(long id, CompanySectionFieldViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateCompanySectionField(id, _mapper.Map<CompanySectionFieldDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    var companySection = await _crudBusinessManager.GetCompanySection(model.CompanySectionId);
                    if(companySection == null) {
                        return BadRequest();
                    }

                    if(IsAjaxRequest) {
                        return Ok(_mapper.Map<CompanySectionFieldViewModel>(item));
                    } else {
                        return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });
                    }
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }
            return RedirectToAction(nameof(Edit), new { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSectionField(long id) {
            try {
                var sectionField = await _crudBusinessManager.GetCompanySectionField(id);
                if(sectionField == null) {
                    return NotFound();
                }

                var companySection = await _crudBusinessManager.GetCompanySection(sectionField.CompanySectionId);
                if(companySection == null) {
                    return NotFound();
                }

                var item = await _crudBusinessManager.DeleteCompanySectionField(id);
                if(item == false) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });

            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
        }
        #endregion
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
        //[Route("section/{sectionId}/fields")]
        //public async Task<List<CompanySectionFieldViewModel>> GetFields(long sectionId) {
        //    return null;
        //}
    }
}