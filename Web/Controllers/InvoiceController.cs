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
    public class InvoiceController: BaseController<InvoiceController> {
        private readonly IPersonBusinessManager _personBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly IVendorBusinessManager _vendorsBusinessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper,
            IPersonBusinessManager personBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            IVendorBusinessManager vendorsBusinessManager)
            : base(logger, mapper) {
            _personBusinessManager = personBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _vendorsBusinessManager = vendorsBusinessManager;
        }

        public async Task<IActionResult> Index() {
            var persons = await _personBusinessManager.GetPersons();
            var companies = await _companyBusinessManager.GetCompanies();

            var personsUnified = persons.Select(x => (
                x.Id, x.FullName, "person"));
            var companiesUnified = companies.Select(x => (
                x.Id, x.Name, "company"));

            ViewBag.CompaniesAndPersons = personsUnified
                .Union(companiesUnified)
                .Select(x => new SelectListItem {
                    Text = x.Item2,
                    Value = $"{x.Item3}_{x.Item1}"
                });

            var vendors = await _vendorsBusinessManager.GetVendors();
            ViewBag.Vendors = vendors
                .Select(x => new SelectListItem {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            return View();
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IInvoiceBusinessManager _invoiceBusinessManager;
        private readonly IUccountBusinessManager _uccountBusinessManager;

        public InvoiceController(
            IMapper mapper,
            IViewRenderService viewRenderService,
            IInvoiceBusinessManager invoiceBusinessManager,
            IUccountBusinessManager uccountBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _invoiceBusinessManager = invoiceBusinessManager;
            _uccountBusinessManager = uccountBusinessManager;
        }

        [HttpGet("GetInvoices", Name = "GetInvoices")]
        public async Task<Pager<InvoiceListViewModel>> GetInvoices([FromQuery] InvoiceFilterViewModel model) {
            var result = await _invoiceBusinessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpGet("DetailsInvoice", Name = "DetailsInvoice")]
        public async Task<IActionResult> DetailsInvoice([FromQuery] long id) {
            var invoice = await _invoiceBusinessManager.GetInvoice(id);
            if(invoice == null)
                return NotFound();
            var html = await _viewRenderService.RenderToStringAsync(
                "_DetailsPartial",
                _mapper.Map<InvoiceListViewModel>(invoice));
            return Ok(html);
        }

        [HttpGet("AddInvoice", Name = "AddInvoice")]
        public async Task<IActionResult> AddInvoice() {
            var accounts = await _uccountBusinessManager.GetUccountsInclude();
            var viewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()) {
                {
                    "Accounts",
                    _mapper.Map<List<UccountListViewModel>>(accounts)
                        .Select(x=>new SelectListItem {
                            Text =x.Name,
                            Value =x.Id.ToString()
                        })
                }};

            var html = await _viewRenderService.RenderToStringAsync(
                "_CreatePartial", new InvoiceViewModel(), viewData);

            return Ok(html);
        }

        [HttpPost("CreateInvoice", Name = "CreateInvoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _invoiceBusinessManager.CreateInvoice(_mapper.Map<InvoiceDto>(model));
                if(item == null)
                    throw new Exception("No records have been created! Please, fill the required fields!");

                return Ok(_mapper.Map<InvoiceViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("EditInvoice", Name = "EditInvoice")]
        public async Task<IActionResult> EditInvoice([FromQuery] long id) {
            var item = await _invoiceBusinessManager.GetInvoice(id);
            if(item == null)
                return NotFound();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {{
                "AccountName",
                item.Account.Name
            }};

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<InvoiceViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdateInvoice", Name = "UpdateInvoice")]
        public async Task<IActionResult> UpdateInvoice([FromQuery] long id, [FromBody] InvoiceViewModel model) {
            try {
                if(!ModelState.IsValid) {
                    throw new Exception("Form is not valid!");
                }
                var item = await _invoiceBusinessManager.UpdateInvoice(id, _mapper.Map<InvoiceDto>(model));
                if(item == null)
                    throw new Exception("No records have been updated!");

                return Ok(_mapper.Map<InvoiceViewModel>(item));
            } catch(Exception e) {
                return BadRequest(e.Message ?? e.StackTrace);
            }
        }

        [HttpGet("DeleteInvoices", Name = "DeleteInvoices")]
        public async Task<IActionResult> DeleteInvoices([FromQuery] long[] id) {
            if(id.Length > 0) {
                var result = await _invoiceBusinessManager.DeleteInvoices(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }
    }
}
