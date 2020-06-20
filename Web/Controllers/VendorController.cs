using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class VendorController: BaseController<VendorController> {
        private readonly ICrudBusinessManager _crudBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(ILogger<VendorController> logger, IMapper mapper,
            ICrudBusinessManager businessManager,
            IVendorBusinessManager vendorBusinessManager) : base(logger, mapper) {
            _crudBusinessManager = businessManager;
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
            var model = new VendorGeneralViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _vendorBusinessManager.CreateVendor(_mapper.Map<VendorGeneralDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Edit), new { id = item.Id });
                }

            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("All", e.Message);
                BadRequest(e);
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            if(item == null) {
                return NotFound();
            }

            var sections = await _vendorBusinessManager.GetVendorSections(id);
            ViewBag.Sections = _mapper.Map<List<VendorSectionViewModel>>(sections);

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            return View(_mapper.Map<VendorViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, VendorGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _vendorBusinessManager.UpdateVendor(id, _mapper.Map<VendorGeneralDto>(model));
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
        public async Task<ActionResult> EditAddress(long supplierId, VendorAddressViewModel model) {
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
        private readonly ICrudBusinessManager _businessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(IMapper mapper, ICrudBusinessManager businessManager,
            IVendorBusinessManager vendorBusinessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
            _vendorBusinessManager = vendorBusinessManager;
        }

        [HttpGet("GetVendors", Name = "GetVendors")]
        public async Task<Pager<VendorListViewModel>> GetVendors(PagerFilterViewModel model) {
            var result = await _vendorBusinessManager.GetVendorPager(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<VendorListViewModel>(_mapper.Map<List<VendorListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }
    }
}