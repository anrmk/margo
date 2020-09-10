using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Enums;
using Core.Extension;
using Core.Filters;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Web.Utils;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    [LogAction]
    public class UccountController: BaseController<UccountController> {
        private readonly IUccountBusinessManager _uccountBusinessManager;
        private readonly IPersonBusinessManager _personBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;
        private readonly ICategoryBusinessManager _categoryBusinessManager;
        public UccountController(ILogger<UccountController> logger, IMapper mapper,
            IUccountBusinessManager uccountBusinessManager,
            IPersonBusinessManager personBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            IVendorBusinessManager vendorBusinessManager,
            ICategoryBusinessManager categoryBusinessManager) : base(logger, mapper) {
            _uccountBusinessManager = uccountBusinessManager;
            _personBusinessManager = personBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _vendorBusinessManager = vendorBusinessManager;
            _categoryBusinessManager = categoryBusinessManager;
        }

        public async Task<IActionResult> Index() {
            var vendors = await _vendorBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            var persons = await _personBusinessManager.GetPersons();
            var companies = await _companyBusinessManager.GetCompanies();
            //var stateGroups = Enum.GetNames(typeof(UccountTypes)).Select(x => new SelectListGroup() { Name = x }).ToList();

            ViewBag.Customers = persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                .Union(companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));

            var categories = await _categoryBusinessManager.GetCategories();
            ViewBag.Categories = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

            var model = new UccountFilterViewModel();
            return View(model);
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [LogAction]
    public class UccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUccountBusinessManager _uccountBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly ICategoryBusinessManager _categoryBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;
        private readonly IPersonBusinessManager _personBusinessManager;

        public UccountController(IMapper mapper, IViewRenderService viewRenderService,
            IUccountBusinessManager uccountBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            ICategoryBusinessManager categoryBusinessManager,
            IVendorBusinessManager vendorBusinessManager,
            IPersonBusinessManager personBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _uccountBusinessManager = uccountBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _categoryBusinessManager = categoryBusinessManager;
            _vendorBusinessManager = vendorBusinessManager;
            _personBusinessManager = personBusinessManager;
        }

        [HttpGet("GetUccounts", Name = "GetUccounts")]
        public async Task<PagerDto<UccountListViewModel>> GetUccounts([FromQuery] UccountFilterViewModel model) {
            var result = await _uccountBusinessManager.GetUccountPage(_mapper.Map<UccountFilterDto>(model, opts =>
                opts.AfterMap((_, dest) => (dest as UccountFilterDto).UserId = User.GetUserId())));
            var pager = new PagerDto<UccountListViewModel>(_mapper.Map<List<UccountListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DetailsUccount", Name = "DetailsUccount")]
        public async Task<IActionResult> DetailsUccount([FromQuery] Guid id) {
            var item = await _uccountBusinessManager.GetUccount(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<UccountViewModel>(item));
            return Ok(html);
        }

        [HttpGet("GetUccount", Name = "GetUccount")]
        public async Task<IActionResult> GetUccount([FromQuery] Guid id) {
            var item = await _uccountBusinessManager.GetUccount(id);
            if(item == null)
                return NotFound();

            return Ok(_mapper.Map<UccountViewModel>(item));
        }

        [HttpGet("GetUccountService", Name = "GetUccountService")]
        public async Task<IActionResult> GetUccountService([FromQuery] Guid id) {
            var item = await _uccountBusinessManager.GetService(id);
            if(item == null)
                return NotFound();

            return Ok(_mapper.Map<UccountServiceViewModel>(item));
        }

        public async Task<IActionResult> AddUccount([FromQuery] UccountTypes kind) {
            string html;
            var model = new UccountViewModel();
            var vendors = await _vendorBusinessManager.GetVendors();
            var categories = await _categoryBusinessManager.GetCategories();

            if(kind == UccountTypes.PERSONAL) {
                var persons = await _personBusinessManager.GetPersons();
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                    { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Persons", persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Kind", kind }
                };
                html = _viewRenderService.RenderToStringAsync("_CreatePartial", model, viewDataDictionary).Result;
            } else {
                var companies = await _companyBusinessManager.GetCompanies();
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                    { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Companies", companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Kind", kind }
                };
                html = _viewRenderService.RenderToStringAsync("_CreatePartial", model, viewDataDictionary).Result;
            }
            return Ok(html);
        }

        [HttpPost("CreateUccount", Name = "CreateUccount")]
        public async Task<IActionResult> CreateUccount([FromBody] UccountViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var result = await _uccountBusinessManager.CreateUccount(_mapper.Map<UccountDto>(model));
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok();
        }

        [HttpPut("UpdateUccount", Name = "UpdateUccount")]
        public async Task<IActionResult> UpdateUccount([FromQuery] Guid id, [FromBody] UccountViewModel model) {
            if(ModelState.IsValid) {
                var item = await _uccountBusinessManager.UpdateUccount(id, _mapper.Map<UccountDto>(model), User.GetUserId());
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<UccountViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("EditUccount", Name = "EditUccount")]
        public async Task<IActionResult> EditUccount([FromQuery] Guid id) {
            var item = await _uccountBusinessManager.GetUccount(id);
            if(item == null)
                return NotFound();

            var mappedItem = _mapper.Map<UccountViewModel>(item);
            var persons = await _personBusinessManager.GetPersons();
            var companies = await _companyBusinessManager.GetCompanies();
            var vendors = await _vendorBusinessManager.GetVendors();
            var vendorCategories = await _vendorBusinessManager.GetVendorCategories(item.VendorId);
            var services = await _uccountBusinessManager.GetServices(id);
            var groupedServices = services
                .GroupBy(x => x.CategoryId)
                .Select(x => new UccountGroupedServiceViewModel() {
                    Id = Guid.NewGuid(),
                    Name = x.FirstOrDefault().Name,
                    CategoryId = x.FirstOrDefault().CategoryId,
                    Groups = x.Select(g => new UccountGroupedServiceFieldsViewModel() {
                        Id = Guid.NewGuid(),
                        ServiceId = g.Id,
                        ServiceName = g.Name,
                        Fields = _mapper.Map<List<UccountServiceFieldViewModel>>(g.Fields)
                    }).ToList()
                }).ToList();

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Persons", persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Companies", companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Categories", _mapper.Map<List<VendorCategoryViewModel>>(vendorCategories) },
                { "Services", groupedServices},
                { "Kind", item.Kind }
            };

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", mappedItem, viewDataDictionary);
            return Ok(html);
        }

        [HttpGet("DeleteUccounts", Name = "DeleteUccounts")]
        public async Task<IActionResult> DeleteUccounts([FromQuery] Guid[] id) {
            if(id.Length > 0) {
                var result = await _uccountBusinessManager.DeleteUccount(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        [HttpGet("DeleteService", Name = "DeleteService")]
        public async Task<IActionResult> DeleteService([FromQuery] Guid id) {
            var result = await _uccountBusinessManager.DeleteService(id);
            if(result)
                return Ok(id);

            return BadRequest("No items selected");
        }

        [HttpGet("DisplayPassword", Name = "DisplayPassword")]
        public async Task<IActionResult> DisplayPassword([FromQuery] Guid id) {
            var result = await _uccountBusinessManager.DisplayPassword(id);
            return Ok(result);
        }
    }
}
