using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class SectionController: BaseController<SectionController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public SectionController(ILogger<SectionController> logger, IMapper mapper, 
           ICrudBusinessManager crudBusinessManager) : base(logger, mapper) {
            _crudBusinessManager = crudBusinessManager;
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
                    var item = await _crudBusinessManager.CreateSection(_mapper.Map<SectionDto>(model));
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
            var item = await _crudBusinessManager.GetSection(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<SectionViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, SectionViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateSection(id, _mapper.Map<SectionDto>(model));
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
                var item = await _crudBusinessManager.DeleteSection(id);
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
    //[ApiController]
    public class SectionController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;

        public SectionController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _businessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<SectionListViewModel>> GetSections(PagerFilterViewModel model) {
            var result = await _businessManager.GetSectionPage(_mapper.Map<PagerFilter>(model));
            var pager = new Pager<SectionListViewModel>(_mapper.Map<List<SectionListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }
    }
}