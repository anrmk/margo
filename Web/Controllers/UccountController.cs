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
using Core.Data.Enums;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class UccountController: BaseController<UccountController> {
        private readonly IUccountBusinessManager _uccountBusinessManager;
        public UccountController(ILogger<UccountController> logger, IMapper mapper,
            IUccountBusinessManager uccountBusinessManager) : base(logger, mapper) {
            _uccountBusinessManager = uccountBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        //public async Task<IActionResult> Details(long id) {
        //    return View();
        //}

        public async Task<IActionResult> Edit(long id) {
            var item = await _uccountBusinessManager.GetUccount(id);
            if(item == null) {
                return NotFound();
            }

            var services = await _uccountBusinessManager.GetServices(id);
            ViewBag.Services = _mapper.Map<List<UccountServiceViewModel>>(services).ToList();

            return View(_mapper.Map<UccountViewModel>(item));
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<Pager<UccountListViewModel>> GetUccounts([FromQuery] PagerFilterViewModel model) {
            var result = await _uccountBusinessManager.GetUccountPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<UccountListViewModel>(_mapper.Map<List<UccountListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DetailsUccount", Name = "DetailsUccount")]
        public async Task<IActionResult> DetailsUccount([FromQuery] long id) {
            var item = await _uccountBusinessManager.GetUccountWith(id);
            if (item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("Details", _mapper.Map<UccountViewModel>(item));
            return Ok(html);
        }

        public async Task<IActionResult> AddUccount([FromQuery] UccountTypes kind) {
            string html;
            var model = new UccountViewModel();
            var vendors = await _vendorBusinessManager.GetVendors();
            var categories = await _categoryBusinessManager.GetCategories();

            if (kind == UccountTypes.PERSONAL) {
                var persons = await _personBusinessManager.GetPersons();
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                    { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Persons", persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Kind", kind }
                };
                html = _viewRenderService.RenderToStringAsync("_AddPersonUccountPartial", model, viewDataDictionary).Result;
            } else {
                var companies = await _companyBusinessManager.GetCompanies();
                var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                    { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Companies", companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                    { "Kind", kind }
                };
                html = _viewRenderService.RenderToStringAsync("_AddBusinessUccountPartial", model, viewDataDictionary).Result;
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

        [HttpGet("DeleteUccounts", Name = "DeleteUccounts")]
        public async Task<ActionResult> DeleteUccounts([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _uccountBusinessManager.DeleteUccount(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }
    }
}
