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

            //var sectionFields = await _uccountBusinessManager.GetSectionFields(item.SectionId ?? 0);
            //ViewBag.SectionFields = _mapper.Map<List<UccountSectionFieldViewModel>>(sectionFields).ToList();

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
        private readonly ISectionBusinessManager _sectionBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;


        public UccountController(IMapper mapper, IViewRenderService viewRenderService,
            IUccountBusinessManager uccountBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            ISectionBusinessManager sectionBusinessManager,
            IVendorBusinessManager vendorBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _uccountBusinessManager = uccountBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _sectionBusinessManager = sectionBusinessManager;
            _vendorBusinessManager = vendorBusinessManager;
        }

        [HttpGet("GetUccounts", Name = "GetUccounts")]
        public async Task<Pager<UccountListViewModel>> GetUccounts([FromQuery] PagerFilterViewModel model) {
            var result = await _uccountBusinessManager.GetUccountPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<UccountListViewModel>(_mapper.Map<List<UccountListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("AddUccount", Name = "AddUccount")]
        public async Task<IActionResult> AddUccount() {
            var companies = await _companyBusinessManager.GetCompanies();
            var vendors = await _vendorBusinessManager.GetVendors();
            var templates = await _sectionBusinessManager.GetSections();

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Companies", companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Vendors", vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Templates", templates.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() }
            };

            var model = new UccountViewModel();
            string html = _viewRenderService.RenderToStringAsync("_AddUccountPartial", model, viewDataDictionary).Result;
            return Ok(html);
        }

        [HttpPost("CreateUccount", Name = "CreateUccount")]
        public async Task<IActionResult> CreateUccount([FromBody] UccountViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var result = await _uccountBusinessManager.CreateUccount(_mapper.Map<UccountDto>(model));
                    return Ok(_mapper.Map<UccountViewModel>(result));
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
