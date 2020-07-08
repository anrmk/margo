using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
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
    public class VendorController: BaseController<VendorController> {
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(ILogger<VendorController> logger, IMapper mapper,
            ICompanyBusinessManager companyBusinessManager,
            IVendorBusinessManager vendorBusinessManager) : base(logger, mapper) {
            _companyBusinessManager = companyBusinessManager;
            _vendorBusinessManager = vendorBusinessManager;
        }

        public ActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            return View(_mapper.Map<CompanyViewModel>(item));
        }

        public ActionResult Create() {
            var model = new VendorViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _vendorBusinessManager.CreateVendor(_mapper.Map<VendorDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Edit), new { id = item.Id });
                }
            } catch(Exception e) {
                BadRequest(e);
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            if(item == null) {
                return NotFound();
            }

            var sections = await _vendorBusinessManager.GetSections(id);
            ViewBag.Sections = _mapper.Map<List<VendorSectionViewModel>>(sections);

            var companies = await _companyBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            return View(_mapper.Map<VendorViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, VendorViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _vendorBusinessManager.UpdateVendor(id, _mapper.Map<VendorDto>(model));
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
        public async Task<ActionResult> EditAddress(long supplierId, VendorViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _vendorBusinessManager.UpdateVendorAddress(supplierId, _mapper.Map<VendorAddressDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            return RedirectToAction(nameof(Edit), new { Id = supplierId });
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _vendorBusinessManager.DeleteVendor(new long[id]);
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
    public class VendorController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly ISectionBusinessManager _businessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(IMapper mapper, IViewRenderService viewRenderService,
            ISectionBusinessManager businessManager,
            IVendorBusinessManager vendorBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _businessManager = businessManager;
            _vendorBusinessManager = vendorBusinessManager;
        }

        [HttpGet("GetVendors", Name = "GetVendors")]
        public async Task<Pager<VendorListViewModel>> GetVendors(PagerFilterViewModel model) {
            var result = await _vendorBusinessManager.GetVendorPager(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<VendorListViewModel>(_mapper.Map<List<VendorListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DeleteVendors", Name = "DeleteVendors")]
        public async Task<ActionResult> DeleteVendors([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _vendorBusinessManager.DeleteVendor(id);
                return Ok(new { Status = result, Data = id });
            }
            return BadRequest("No items selected");
        }

        [HttpGet("AddVendorSection", Name = "AddVendorSection")]
        public async Task<IActionResult> AddVendorSection([FromQuery] long id) {
            var result = await _vendorBusinessManager.GetVendor(id);
            if(result == null)
                return NotFound();

            var vendorSection = await _vendorBusinessManager.GetSections(id);
            var sections = await _businessManager.GetSections();
            var exist = sections.Where(x => !vendorSection.Any(y => x.Id == y.SectionId)).ToList();

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "SectionList", _mapper.Map<List<SectionViewModel>>(exist) }
            };

            if(exist.Count != 0) {
                var model = new VendorSectionViewModel() { VendorId = id };
                string html = _viewRenderService.RenderToStringAsync("_AddSectionPartial", model, viewDataDictionary).Result;
                return Ok(html);
            } else {
                return Ok("Nothing to display");
            }
        }

        [HttpPost("CreateVendorSection", Name = "CreateVendorSection")]
        public async Task<IActionResult> CreateVendorSection([FromBody] VendorSectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var result = await _vendorBusinessManager.CreateSection(_mapper.Map<VendorSectionDto>(model));
                    return Ok(_mapper.Map<VendorSectionViewModel>(result));
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok();
        }

    }
}