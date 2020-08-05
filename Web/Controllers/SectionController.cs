using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Enums;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class SectionController: BaseController<SectionController> {
        private readonly ISectionBusinessManager _sectionBusinessManager;

        public SectionController(ILogger<SectionController> logger, IMapper mapper,
           ISectionBusinessManager sectionBusinessManager) : base(logger, mapper) {
            _sectionBusinessManager = sectionBusinessManager;
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Create() {
            var model = new SectionViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _sectionBusinessManager.CreateSection(_mapper.Map<SectionDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Edit), new { Id = item.Id });
                }
            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                BadRequest(e);
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _sectionBusinessManager.GetSection(id);
            if(item == null) {
                return NotFound();
            }

            var fields = await _sectionBusinessManager.GetSectionFields(id);
            ViewBag.SectionFields = _mapper.Map<List<SectionFieldViewModel>>(fields);

            return View(_mapper.Map<SectionViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, SectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _sectionBusinessManager.UpdateSection(id, _mapper.Map<SectionDto>(model));
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
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _sectionBusinessManager.DeleteSection(id);
                if(item == false) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));

            } catch(Exception e) {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly ISectionBusinessManager _sectionBusinessManager;

        public SectionController(IMapper mapper, IViewRenderService viewRenderService,
            ISectionBusinessManager sectionBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _sectionBusinessManager = sectionBusinessManager;
        }

        [HttpGet("GetFieldTypes", Name = "GetFieldTypes")]
        public IActionResult GetFieldTypes() {
            var cast = Enum.GetValues(typeof(FieldEnum)).Cast<FieldEnum>().Select(x => new { Name = Enum.GetName(typeof(FieldEnum), x), Id = x, Title = x.GetAttribute<DisplayAttribute>().Name }).ToList();
            return Ok(cast);
        }

        [HttpGet("GetSections", Name = "GetSections")]
        public async Task<Pager<SectionListViewModel>> GetSections([FromQuery] PagerFilterViewModel model) {
            var result = await _sectionBusinessManager.GetSectionPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<SectionListViewModel>(_mapper.Map<List<SectionListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DeleteSections", Name = "DeleteSections")]
        public async Task<ActionResult> DeleteSections([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _sectionBusinessManager.DeleteSections(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        //[HttpGet("GetSectionFields", Name = "GetSectionFields")]
        //public async Task<List<SectionFieldViewModel>> GetSectionFields(long id) {
        //    var result = await _sectionBusinessManager.GetSectionFields(id);
        //    return _mapper.Map<List<SectionFieldViewModel>>(result);
        //}

        [HttpGet("GetSectionFields", Name = "GetSectionFields")]
        public async Task<Pager<SectionFieldViewModel>> GetSectionFields([FromQuery] SectionFieldsFilterViewModel model) {
            var result = await _sectionBusinessManager.GetSectionFieldsPage(_mapper.Map<SectionFieldsFilterDto>(model));
            var pager = new Pager<SectionFieldViewModel>(_mapper.Map<List<SectionFieldViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("AddSectionField", Name = "AddSectionField")]
        public async Task<IActionResult> AddSectionField(long id) {
            var result = await _sectionBusinessManager.GetSection(id);
            if(result == null)
                return NotFound();

            //var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
            //    { "Section", _mapper.Map<List<SectionViewModel>>(result) }
            //};

            var model = new SectionFieldViewModel() { SectionId = id };
            string html = _viewRenderService.RenderToStringAsync("_AddSectionFieldPartial", model).Result;
            return Ok(html);
        }

        [HttpPost("CreateSectionField", Name = "CreateSectionField")]
        public async Task<IActionResult> CreateSectionField([FromBody] SectionFieldViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var result = await _sectionBusinessManager.CreateSectionField(_mapper.Map<SectionFieldDto>(model));
                    return Ok(_mapper.Map<SectionFieldViewModel>(result));
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok();
        }
    }
}