using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
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

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper, ICompanyBusinessManager companyBusinessManager) : base(logger, mapper) {
            _companyBusinessManager = companyBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly ICompanyBusinessManager _companyBusinessManager;

        public CompanyController(IMapper mapper, IViewRenderService viewRenderService,
            ICompanyBusinessManager companyBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _companyBusinessManager = companyBusinessManager;
        }

        [HttpGet("GetCompanies", Name = "GetCompanies")]
        public async Task<PagerDto<CompanyListViewModel>> GetCompanies([FromQuery] PagerFilterViewModel model) {
            var result = await _companyBusinessManager.GetCompanyPage(_mapper.Map<PagerFilterDto>(model));
            var pager = new PagerDto<CompanyListViewModel>(_mapper.Map<List<CompanyListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DeleteCompanies", Name = "DeleteCompanies")]
        public async Task<ActionResult> DeleteCompanies([FromQuery] Guid[] id) {
            if(id.Length > 0) {
                var result = await _companyBusinessManager.DeleteCompany(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        [HttpGet("DetailsCompany", Name = "DetailsCompany")]
        public async Task<IActionResult> DetailsCompany([FromQuery] Guid id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<CompanyViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddCompany", Name = "AddCompany")]
        public async Task<IActionResult> AddCompany() {
            var html = await _viewRenderService.RenderToStringAsync("_CreatePartial", new CompanyViewModel() { Founded = DateTime.Now });

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
        public async Task<IActionResult> EditCompany([FromQuery] Guid id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<CompanyViewModel>(item));
            return Ok(html);
        }

        [HttpPost("UpdateCompany", Name = "UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromQuery] Guid id, [FromBody] CompanyViewModel model) {
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