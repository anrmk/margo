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
    public class VendorController: BaseController<VendorController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public VendorController(ILogger<VendorController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context, ICrudBusinessManager businessManager) : base(logger, mapper, notificationHub, context) {
            _crudBusinessManager = businessManager;
        }

        // GET: Supplier
        public ActionResult Index() {
            return View();
        }

        // GET: Supplier/Details/5
        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetVendor(id);
            return View(_mapper.Map<CompanyViewModel>(item));
        }

        // GET: Supplier/Create
        public ActionResult Create() {
            var model = new VendorGeneralViewModel();
            return View(model);
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateVendor(_mapper.Map<VendorGeneralDto>(model));
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

        // GET: Supplier/Edit/5
        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetVendor(id);
            if(item == null) {
                return NotFound();
            }

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() }).ToList();

            return View(_mapper.Map<VendorViewModel>(item));
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, VendorGeneralViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateVendor(id, _mapper.Map<VendorGeneralDto>(model));
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
                    var item = await _crudBusinessManager.UpdateVendorAddress(supplierId, _mapper.Map<VendorAddressDto>(model));
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
                var item = await _crudBusinessManager.DeleteVendor(id);
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
    // [ApiController]
    public class VendorController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public VendorController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<VendorListViewModel>> GetSuppliers(PagerFilterViewModel model) {
            var result = await _businessManager.GetVendorPager(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<VendorListViewModel>(_mapper.Map<List<VendorListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
            return pager;
        }
    }
}