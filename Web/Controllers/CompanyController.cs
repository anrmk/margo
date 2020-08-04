using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class CompanyController: BaseController<CompanyController> {
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly ISectionBusinessManager _sectionBusinessManager;

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper,
            ICompanyBusinessManager companyBusinessManager,
            ISectionBusinessManager sectionBusinessManager) : base(logger, mapper) {
            _companyBusinessManager = companyBusinessManager;
            _sectionBusinessManager = sectionBusinessManager;
        }

        public ActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }
            if(IsAjaxRequest) {
                return PartialView(_mapper.Map<CompanyViewModel>(item));
            }
            return View(_mapper.Map<CompanyViewModel>(item));
        }

        public ActionResult Create() {
            var model = new CompanyGeneralViewModel();
            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null) {
                return NotFound();
            }

            var sections = await _companyBusinessManager.GetSections(item.Id);
            ViewBag.Sections = _mapper.Map<List<CompanySectionViewModel>>(sections).ToList();

            return View(_mapper.Map<CompanyViewModel>(item));
        }

        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _companyBusinessManager.DeleteCompany(id);
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
        //public async Task<ActionResult> AddSection(long id) {
        //    var item = await _companyBusinessManager.GetCompany(id);
        //    if(item == null) {
        //        return NotFound(id);
        //    }

        //    var companySection = await _companyBusinessManager.GetSections(item.Id);
        //    var sections = await _sectionBusinessManager.GetSections();
        //    var exist = sections.Where(x => !companySection.Any(y => y.SectionId == x.Id));

        //    ViewBag.Sections = exist.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
        //    var modal = new CompanySectionViewModel() { CompanyId = id };

        //    return View("_AddSectionPartial", modal);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteSection(long id) {
        //    try {
        //        var section = await _companyBusinessManager.GetSection(id);
        //        if(section == null) {
        //            return NotFound();
        //        }

        //        if(section.Fields.Count > 0) {
        //            return BadRequest($"Delete all fields before remove section: {section.SectionName}");
        //        }

        //        var item = await _companyBusinessManager.DeleteSection(id);
        //        if(item == false) {
        //            return NotFound();
        //        }

        //        return RedirectToAction(nameof(Edit), new { Id = section.CompanyId });

        //    } catch(Exception er) {
        //        _logger.LogError(er, er.Message);
        //        return BadRequest(er);
        //    }
        //}
        //#endregion

        //#region COMPANY SECTION FIELD
        //public async Task<ActionResult> AddSectionField(long companySectionId) {
        //    var item = await _companyBusinessManager.GetSection(companySectionId);
        //    if(item == null) {
        //        return NotFound();
        //    }

        //    ViewBag.SectionName = item.SectionName;

        //    var model = new CompanySectionFieldViewModel() {
        //        CompanySectionId = item.Id
        //    };

        //    return View("_AddSectionFieldPartial", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddSectionField(CompanySectionFieldViewModel model) {
        //    try {
        //        if(ModelState.IsValid) {
        //            var companySection = await _companyBusinessManager.GetSection(model.CompanySectionId);
        //            if(companySection == null) {
        //                return BadRequest();
        //            }

        //            var item = await _companyBusinessManager.CreateSectionField(_mapper.Map<CompanySectionFieldDto>(model));
        //            if(item == null) {
        //                return BadRequest();
        //            }
        //            if(IsAjaxRequest) {
        //                return Ok(_mapper.Map<CompanySectionFieldViewModel>(item));
        //            } else {
        //                return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });
        //            }
        //        }
        //    } catch(Exception er) {
        //        _logger.LogError(er, er.Message);
        //        BadRequest(er);
        //    }

        //    return View(model);
        //}

        //public async Task<ActionResult> EditSectionField(long id) {
        //    var item = await _companyBusinessManager.GetSectionField(id);
        //    if(item == null) {
        //        return NotFound();
        //    }

        //    return View("_AddSectionFieldPartial", _mapper.Map<CompanySectionFieldViewModel>(item));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EditSectionField(long id, CompanySectionFieldViewModel model) {
        //    try {
        //        if(ModelState.IsValid) {
        //            var item = await _companyBusinessManager.UpdateSectionField(id, _mapper.Map<CompanySectionFieldDto>(model));
        //            if(item == null) {
        //                return BadRequest();
        //            }

        //            var companySection = await _companyBusinessManager.GetSection(model.CompanySectionId);
        //            if(companySection == null) {
        //                return BadRequest();
        //            }

        //            if(IsAjaxRequest) {
        //                return Ok(_mapper.Map<CompanySectionFieldViewModel>(item));
        //            } else {
        //                return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });
        //            }
        //        }
        //    } catch(Exception er) {
        //        _logger.LogError(er, er.Message);
        //        BadRequest(er);
        //    }
        //    return RedirectToAction(nameof(Edit), new { Id = id });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteSectionField(long id) {
        //    try {
        //        var sectionField = await _companyBusinessManager.GetSectionField(id);
        //        if(sectionField == null) {
        //            return NotFound();
        //        }

        //        var companySection = await _companyBusinessManager.GetSection(sectionField.SectionId);
        //        if(companySection == null) {
        //            return NotFound();
        //        }

        //        var item = await _companyBusinessManager.DeleteSectionField(id);
        //        if(item == false) {
        //            return NotFound();
        //        }
        //        return RedirectToAction(nameof(Edit), new { Id = companySection.CompanyId });

        //    } catch(Exception er) {
        //        _logger.LogError(er, er.Message);
        //        return BadRequest(er);
        //    }
        //}
        #endregion
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly ISectionBusinessManager _sectionBusinessManager;

        public CompanyController(IMapper mapper, IViewRenderService viewRenderService,
            ISectionBusinessManager sectionBusinessManager,
            ICompanyBusinessManager companyBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _sectionBusinessManager = sectionBusinessManager;
            _companyBusinessManager = companyBusinessManager;
        }

        [HttpGet("GetCompanies", Name = "GetCompanies")]
        public async Task<Pager<CompanyListViewModel>> GetCompanies([FromQuery] PagerFilterViewModel model) {
            var result = await _companyBusinessManager.GetCompanyPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<CompanyListViewModel>(_mapper.Map<List<CompanyListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DeleteCompanies", Name = "DeleteCompanies")]
        public async Task<ActionResult> DeleteCompanies([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _companyBusinessManager.DeleteCompany(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        #region SECTION
        [HttpGet("AddCompanySection", Name = "AddCompanySection")]
        public async Task<IActionResult> AddCompanySection(long id) {
            var result = await _companyBusinessManager.GetCompany(id);
            if(result == null)
                return NotFound();

            var companySections = await _companyBusinessManager.GetSections(id);
            var sections = await _sectionBusinessManager.GetSections();
            var exist = sections.Where(x => !companySections.Any(y => x.Id == y.SectionId)).ToList();

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "SectionList", _mapper.Map<List<SectionViewModel>>(exist) }
            };

            if(exist.Count != 0) {
                var model = new CompanySectionViewModel() { CompanyId = id };
                string html = _viewRenderService.RenderToStringAsync("_AddSectionPartial", model, viewDataDictionary).Result;
                return Ok(html);
            } else {
                return Ok("Nothing to display");
            }
        }

        [HttpPost("CreateCompanySection", Name = "CreateCompanySection")]
        public async Task<IActionResult> CreateCompanySection([FromBody] CompanySectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var result = await _companyBusinessManager.CreateSection(_mapper.Map<CompanySectionDto>(model));
                    return Ok(_mapper.Map<CompanySectionViewModel>(result));
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteCompanySection", Name = "DeleteCompanySection")]
        public async Task<ActionResult> DeleteCompanySection(long id) {
            var result = await _companyBusinessManager.DeleteSection(id);
            if(result)
                return Ok(id);

            return BadRequest("No items deleted");
        }

        #endregion

        [HttpPost("CreateCompanySectionField", Name = "CreateCompanySectionField")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompanySectionField([FromBody] CompanySectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _companyBusinessManager.CreateSection(_mapper.Map<CompanySectionDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    return Ok(_mapper.Map<CompanySectionViewModel>(item));
                }
            } catch(Exception er) {
                BadRequest(er.Message);
            }

            return BadRequest("");
        }

        [HttpGet("DetailsCompany", Name = "DetailsCompany")]
        public async Task<IActionResult> DetailsCompany([FromQuery] long id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<CompanyViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddCompany", Name = "AddCompany")]
        public async Task<IActionResult> AddCompany() {
            var html = await _viewRenderService.RenderToStringAsync("_CreatePartial", new CompanyViewModel());

            return Ok(html);
        }

        [HttpPost("CreateCompany", Name = "CreateCompany")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _companyBusinessManager.CreateCompany(_mapper.Map<CompanyDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<CompanyViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("EditCompany", Name = "EditCompany")]
        public async Task<IActionResult> EditCompany([FromQuery] long id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<CompanyViewModel>(item));
            return Ok(html);
        }

        [HttpPost("UpdateCompany", Name = "UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromQuery] long id, [FromBody] CompanyViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _companyBusinessManager.UpdateCompany(id, _mapper.Map<CompanyDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<CompanyViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

    }
}