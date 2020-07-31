using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class CategoryController: BaseController<CategoryController> {
        private readonly ICategoryBusinessManager _categoryBussinessManager;

        public CategoryController(ILogger<CategoryController> logger, IMapper mapper,
            ICategoryBusinessManager categoryBussinessManager) : base(logger, mapper) {
            _categoryBussinessManager = categoryBussinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        //public async Task<IActionResult> Edit(long id) {
        //    var item = await _categoryBussinessManager.GetCategory(id);
        //    if(item == null)
        //        return NotFound();

        //    var categories = await _categoryBussinessManager.GetCategories();
        //    categories.Remove(item);
        //    ViewData["Categories"] = categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

        //    return View(_mapper.Map<CategoryViewModel>(item));
        //}
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICategoryBusinessManager _categoryBusinessManager;

        public CategoryController(IMapper mapper, IViewRenderService viewRenderService,
            ICategoryBusinessManager categoryBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _categoryBusinessManager = categoryBusinessManager;
        }

        [HttpGet("GetCategories", Name = "GetCategories")]
        public async Task<Pager<CategoryListViewModel>> GetCategories([FromQuery] PagerFilterViewModel model) {
            var result = await _categoryBusinessManager.GetCategoryPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<CategoryListViewModel>(_mapper.Map<List<CategoryListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DetailsCategory", Name = "DetailsCategory")]
        public async Task<IActionResult> DetailsCategory([FromQuery] long id) {
            var item = await _categoryBusinessManager.GetCategory(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("Details", _mapper.Map<CategoryViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddCategory", Name = "AddCategory")]
        public async Task<IActionResult> AddCategory() {
            var categories = await _categoryBusinessManager.GetCategories();
            var categoryList = categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Categories", categoryList }
            };

            var html = await _viewRenderService.RenderToStringAsync("Create", new CategoryViewModel(), viewData);
            return Ok(html);
        }

        [HttpPost("CreateCategory", Name = "CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel model) {
            if(ModelState.IsValid) {
                var item = await _categoryBusinessManager.CreateCategory(_mapper.Map<CategoryDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<CategoryViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("GetCategory", Name = "GetCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] long id) {
            var item = await _categoryBusinessManager.GetCategory(id);
            if(item == null)
                return NotFound();
            return Ok(_mapper.Map<CategoryViewModel>(item));
        }

        [HttpGet("EditCategory", Name = "EditCategory")]
        public async Task<IActionResult> EditCategory([FromQuery] long id) {
            var item = await _categoryBusinessManager.GetCategory(id);
            if(item == null)
                return NotFound();

            var categories = await _categoryBusinessManager.GetCategories();
            categories.Remove(item);
            var categoryList = categories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Categories", categoryList }
            };

            var html = await _viewRenderService.RenderToStringAsync("Edit", _mapper.Map<CategoryViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdateCategory", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromQuery] long id, [FromBody] CategoryViewModel model) {
            if(ModelState.IsValid) {
                var item = await _categoryBusinessManager.UpdateCategory(id, _mapper.Map<CategoryDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<CategoryViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("DeleteCategories", Name = "DeleteCategories")]
        public async Task<IActionResult> DeleteCategories([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _categoryBusinessManager.DeleteCategories(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        [HttpGet("DeleteCategoryField", Name = "DeleteCategoryField")]
        public async Task<IActionResult> DeleteCategoryField([FromQuery] long id) {
            var result = await _categoryBusinessManager.DeleteFields(new long[] { id });
            if(result)
                return Ok(id);
            return BadRequest("No item selected");
        }
    }
}
