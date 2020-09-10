using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
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

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    [LogAction]
    public class TodoController: BaseController<TodoController> {
        public TodoController(ILogger<TodoController> logger, IMapper mapper) : base(logger, mapper) { }

        public IActionResult Index() {
            return View(new TodoFilterViewModel());
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [LogAction]
    public class TodoController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly ITodoBusinessManager _todoBusinessManager;
        private readonly IAccountBusinessManager _accountBusinessService;

        public TodoController(IMapper mapper,
            IViewRenderService viewRenderService,
            ITodoBusinessManager todoBusinessManager,
            IAccountBusinessManager accountBusinessService) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _todoBusinessManager = todoBusinessManager;
            _accountBusinessService = accountBusinessService;
        }

        [HttpGet("GetTodoList", Name = "GetTodoList")]
        public async Task<PagerDto<TodoListViewModel>> GetTodoList([FromQuery] TodoFilterViewModel model) {
            var result = await _todoBusinessManager.GetTodoPage(_mapper.Map<TodoFilterDto>(model, opts =>
                opts.AfterMap((_, dest) => { var dto = dest as TodoFilterDto; dto.UserId = User.GetUserId(); dto.UserLogin = User.FindFirstValue(ClaimTypes.Name); })));
            var pager = new PagerDto<TodoListViewModel>(_mapper.Map<List<TodoListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DeleteTodoList", Name = "DeleteTodoList")]
        public async Task<ActionResult> DeleteTodoList([FromQuery] Guid[] id) {
            if(id.Length > 0) {
                var result = await _todoBusinessManager.DeleteTodo(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        [HttpGet("DetailsTodo", Name = "DetailsTodo")]
        public async Task<IActionResult> DetailsTodo([FromQuery] Guid id) {
            var item = await _todoBusinessManager.GetTodo(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<TodoListViewModel>(item));

            return Ok(html);
        }

        [HttpGet("AddTodo", Name = "AddTodo")]
        public async Task<IActionResult> AddTodo() {
            var curUserId = User.GetUserId();
            var users = await _accountBusinessService.GetUsersWithoutObservers();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "IsCreating", true },
                { "Users", users.Select(x=> new SelectListItem(x.UserName, x.Id, x.Id == curUserId)) }
            };

            var html = await _viewRenderService.RenderToStringAsync("_CreateUpdatePartial", new TodoViewModel(), viewData);
            return Ok(html);
        }

        [HttpPost("CreateTodo", Name = "CreateTodo")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _todoBusinessManager.CreateTodo(_mapper.Map<TodoDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<TodoViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("EditTodo", Name = "EditTodo")]
        public async Task<IActionResult> EditTodo([FromQuery] Guid id) {
            var item = await _todoBusinessManager.GetTodo(id);
            if(item == null)
                return NotFound();

            var users = await _accountBusinessService.GetUsersWithoutObservers();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "IsCreating", false },
                { "UserId", User.GetUserId() },
                { "Users", users.Select(x=> new SelectListItem(x.UserName, x.Id)) }
            };

            var html = await _viewRenderService.RenderToStringAsync("_CreateUpdatePartial", _mapper.Map<TodoViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPost("UpdateTodo", Name = "UpdateTodo")]
        public async Task<IActionResult> UpdateTodo([FromQuery] Guid id, [FromBody] TodoViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var dto = _mapper.Map<TodoDto>(model);

                var item = await _todoBusinessManager.UpdateTodo(id, dto);
                if(item == null)
                    throw new Exception("No records have been updated! Please, fill the required fields!");

                return Ok(_mapper.Map<TodoViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpPut("UpdateStatus", Name = "UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] Guid id) {
            try {
                var dto = await _todoBusinessManager.GetTodo(id);
                dto.IsCompleted = !dto.IsCompleted;

                var item = await _todoBusinessManager.UpdateTodo(id, dto);
                if(item == null)
                    throw new Exception("No records have been updated! Please, fill the required fields!");

                return Ok(_mapper.Map<TodoViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }
    }
}
