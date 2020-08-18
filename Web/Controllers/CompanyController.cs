using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Telegram.Bot.Requests.Abstractions;

using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class CompanyController: BaseController<CompanyController> {
        private readonly IPersonBusinessManager _personBusinessManager;

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper,
            IPersonBusinessManager personBusinessManager) : base(logger, mapper) {
            _personBusinessManager = personBusinessManager;
        }

        public async Task<IActionResult> Index() {
            var persons = await _personBusinessManager.GetPersons();
            ViewBag.Ceo = persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            var model = new CompanyFilterViewModel();
            return View(model);
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IViewRenderService _viewRenderService;

        private readonly IAccountBusinessManager _accountBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly IPersonBusinessManager _personBusinessManager;
        private readonly IUccountBusinessManager _uccountBusinessManager;
        private readonly ICategoryBusinessManager _categoryBusinessManager;

        public CompanyController(IMapper mapper,
            IHubContext<NotificationHub> notificationHub,
            IViewRenderService viewRenderService,
            IAccountBusinessManager accountBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            IPersonBusinessManager personBusinessManager,
            IUccountBusinessManager uccountBusinessManager,
            ICategoryBusinessManager categoryBusinessManager
            ) {
            _mapper = mapper;
            _notificationHub = notificationHub;
            _viewRenderService = viewRenderService;
            _accountBusinessManager = accountBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _uccountBusinessManager = uccountBusinessManager;
            _personBusinessManager = personBusinessManager;
            _categoryBusinessManager = categoryBusinessManager;
        }

        [HttpGet("GetCompanies", Name = "GetCompanies")]
        public async Task<PagerDto<CompanyListViewModel>> GetCompanies([FromQuery] CompanyFilterViewModel model) {
            var result = await _companyBusinessManager.GetCompanyPage(_mapper.Map<CompanyFilterDto>(model, opts =>
                opts.AfterMap((_, dest) => (dest as CompanyFilterDto).UserId = User.GetUserId())));
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

        [HttpGet("DeleteCompanyData", Name = "DeleteCompanyData")]
        public async Task<ActionResult> DeleteCompanyData([FromQuery] Guid id) {
            var result = await _companyBusinessManager.DeleteCompanyData(id);
            if(result)
                return Ok(id);

            return BadRequest("No items deleted");
        }

        [HttpGet("DetailsCompany", Name = "DetailsCompany")]
        public async Task<IActionResult> DetailsCompany([FromQuery] Guid id, int full = 1) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            string html;
            var data = await _companyBusinessManager.GetCompanyData(id);
            var mappedData = _mapper.Map<List<CompanyDataViewModel>>(data);
            var groupedData = from f in mappedData
                              group f by f.Name;

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "GroupedData", groupedData },
            };

            if(full == 1) {
                html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<CompanyViewModel>(item), viewData);
            } else {
                html = await _viewRenderService.RenderToStringAsync("_FullSizeDetailsPartial", _mapper.Map<CompanyViewModel>(item), viewData);
            }

            return Ok(html);
        }

        [HttpGet("AddCompany", Name = "AddCompany")]
        public async Task<IActionResult> AddCompany() {
            var persons = await _personBusinessManager.GetPersons();
            var categories = await _categoryBusinessManager.GetCategories();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Persons", persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() }
            };

            var html = await _viewRenderService.RenderToStringAsync("_CreatePartial", new CompanyViewModel(), viewData);

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

            var persons = await _personBusinessManager.GetPersons();
            var categories = await _categoryBusinessManager.GetCategories();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Persons", persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() },
                { "Categories", categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList() }
            };

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<CompanyViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPost("UpdateCompany", Name = "UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromQuery] Guid id, [FromBody] CompanyViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var dto = _mapper.Map<CompanyDto>(model);
                if(!User.IsInRole("Administrator")) {

                    var item = await CreateOrUpdateRequest(User.FindFirst(ClaimTypes.Name).Value, new AspNetUserRequestDto() {
                        ModelId = model.Id,
                        ModelType = dto.GetType(),
                        Model = JsonSerializer.Serialize(dto)
                    });

                    if(item != null)
                        await _notificationHub.Clients.Group("adminGroup").SendAsync("receiveMessage", new { Message = "Company data change request!" });

                    return Ok(new { Message = "Changes will be published after padding moderation!" });
                } else {
                    var item = await _companyBusinessManager.UpdateCompany(id, dto);
                    if(item == null)
                        throw new Exception("No records have been created! Please, fill the required fields!");

                    return Ok(_mapper.Map<CompanyViewModel>(item));
                }
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("AddCompanyData", Name = "AddCompanyData")]
        public async Task<IActionResult> AddCompanyData([FromQuery] Guid id) {
            var item = await _companyBusinessManager.GetCompany(id);
            if(item == null)
                return NotFound();

            var accounts = await _uccountBusinessManager.GetUccountsByCompanyId(id);
            var html = await _viewRenderService.RenderToStringAsync(
                "_AddCompanyDataPartial",
                _mapper.Map<CompanyDataListViewModel>(item),
                new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()) {
                {
                    "Accounts",
                    _mapper.Map<List<UccountListViewModel>>(accounts)
                        .Select(x => new SelectListItem {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        })
                        .ToArray()
                }});
            return Ok(html);
        }

        [HttpPost("CreateCompanyData", Name = "CreateCompanyData")]
        public async Task<IActionResult> CreateCompanyData([FromBody] CompanyDataListViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _companyBusinessManager.CreateCompanyData(_mapper.Map<CompanyDataListDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<CompanyViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("DeleteCompanySection", Name = "DeleteCompanySection")]
        public async Task<IActionResult> DeleteCompanySection([FromQuery] Guid id) {
            var result = await _companyBusinessManager.DeleteSection(id);
            if(result)
                return Ok(id);

            return BadRequest("No items selected");
        }

        private async Task<AspNetUserRequestDto> CreateOrUpdateRequest(string userName, AspNetUserRequestDto dto) {
            var item = await _accountBusinessManager.GetRequest(userName, dto.ModelId);
            if(item == null)
                return await _accountBusinessManager.CreateRequest(dto);
            else {
                dto.Id = item.Id;
                return await _accountBusinessManager.UpdateRequset(item.Id, dto);
            }

            //return Ok(new { Message = "Changes will be published after padding moderation!" });
        }
    }
}
