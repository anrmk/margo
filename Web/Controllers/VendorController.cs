using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Filters;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    [LogAction]
    public class VendorController: BaseController<VendorController> {
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(ILogger<VendorController> logger, IMapper mapper,
            IVendorBusinessManager vendorBusinessManager) : base(logger, mapper) {
            _vendorBusinessManager = vendorBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [LogAction]
    public class VendorController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public VendorController(IMapper mapper, IViewRenderService viewRenderService,
            //ISectionBusinessManager businessManager,
            IVendorBusinessManager vendorBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _vendorBusinessManager = vendorBusinessManager;
        }

        [HttpGet("GetVendors", Name = "GetVendors")]
        public async Task<PagerDto<VendorListViewModel>> GetVendors([FromQuery] PagerFilterViewModel model) {
            var result = await _vendorBusinessManager.GetVendorPager(_mapper.Map<PagerFilterDto>(model));
            var pager = new PagerDto<VendorListViewModel>(_mapper.Map<List<VendorListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
            return pager;
        }

        [HttpGet("DetailsVendor", Name = "DetailsVendor")]
        public async Task<IActionResult> DetailsVendor([FromQuery] Guid id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<VendorViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddVendor", Name = "AddVendor")]
        public async Task<IActionResult> AddVendor() {
            var html = await _viewRenderService.RenderToStringAsync("_CreatePartial", new VendorViewModel());
            return Ok(html);
        }

        [HttpPost("CreateVendor", Name = "CreateVendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorViewModel model) {
            if(ModelState.IsValid) {
                var item = await _vendorBusinessManager.CreateVendor(_mapper.Map<VendorDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<VendorViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("EditVendor", Name = "EditVendor")]
        public async Task<IActionResult> EditVendor([FromQuery] Guid id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<VendorViewModel>(item));
            return Ok(html);
        }

        [HttpPut("UpdateVendor", Name = "UpdateVendor")]
        public async Task<IActionResult> UpdateVendor([FromQuery] Guid id, [FromBody] VendorViewModel model) {
            if(ModelState.IsValid) {
                var item = await _vendorBusinessManager.UpdateVendor(id, _mapper.Map<VendorDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<VendorViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("DeleteVendors", Name = "DeleteVendors")]
        public async Task<IActionResult> DeleteVendors([FromQuery] Guid[] id) {
            if(id.Length > 0) {
                var result = await _vendorBusinessManager.DeleteVendors(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }

        [HttpGet("GetVendor", Name = "GetVendor")]
        public async Task<IActionResult> GetVendor([FromQuery] Guid id) {
            var item = await _vendorBusinessManager.GetVendor(id);
            if(item == null)
                return NotFound();
            return Ok(_mapper.Map<VendorViewModel>(item));
        }

        [HttpGet("DeleteVendorField", Name = "DeleteVendorField")]
        public async Task<IActionResult> DeleteVendorField([FromQuery] Guid id) {
            var result = await _vendorBusinessManager.DeleteFields(new Guid[] { id });
            if(result)
                return Ok(id);
            return BadRequest("No item selected");
        }
    }
}
