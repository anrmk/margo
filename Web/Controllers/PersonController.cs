using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class PersonController: BaseController<PersonController> {
        private readonly IPersonBusinessManager _personBusinessManager;

        public PersonController(ILogger<PersonController> logger, IMapper mapper,
            IPersonBusinessManager personBusinessManager) : base(logger, mapper) {
            _personBusinessManager = personBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IPersonBusinessManager _personBusinessManager;

        public PersonController(IMapper mapper, IViewRenderService viewRenderService,
            ISectionBusinessManager businessManager,
            IPersonBusinessManager personBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _personBusinessManager = personBusinessManager;
        }

        [HttpGet("GetPersons", Name = "GetPersons")]
        public async Task<Pager<PersonListViewModel>> GetPersons([FromQuery] PagerFilterViewModel model) {
            var result = await _personBusinessManager.GetPersonPager(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<PersonListViewModel>(_mapper.Map<List<PersonListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DetailsPerson", Name = "DetailsPerson")]
        public async Task<IActionResult> DetailsPerson([FromQuery] long id) {
            var item = await _personBusinessManager.GetPerson(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("Details", _mapper.Map<PersonViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddPerson", Name = "AddPerson")]
        public async Task<IActionResult> AddPerson() {
            var html = await _viewRenderService.RenderToStringAsync("Create", new PersonViewModel());
            return Ok(html);
        }

        [HttpPost("CreatePerson", Name = "CreatePerson")]
        public async Task<IActionResult> CreatePerson([FromBody] PersonViewModel model) {
            if(ModelState.IsValid) {
                var item = await _personBusinessManager.CreatePerson(_mapper.Map<PersonDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<PersonViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("EditPerson", Name = "EditPerson")]
        public async Task<IActionResult> EditPerson([FromQuery] long id) {
            var item = await _personBusinessManager.GetPerson(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("Edit", _mapper.Map<PersonViewModel>(item));
            return Ok(html);
        }

        [HttpPut("UpdatePerson", Name = "UpdatePerson")]
        public async Task<IActionResult> UpdatePerson([FromQuery] long id, [FromBody] PersonViewModel model) {
            if(ModelState.IsValid) {
                var item = await _personBusinessManager.UpdatePerson(id, _mapper.Map<PersonDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<PersonViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("DeletePersons", Name = "DeletePersons")]
        public async Task<IActionResult> DeletePersons([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _personBusinessManager.DeletePersons(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }
    }
}
