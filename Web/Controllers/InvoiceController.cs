using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Filters;
using Core.Services;
using Core.Services.Business;
using Core.Services.Integration;

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
    public class InvoiceController: BaseController<InvoiceController> {
        private readonly IPersonBusinessManager _personBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly IVendorBusinessManager _vendorBusinessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper,
            IPersonBusinessManager personBusinessManager,
            ICompanyBusinessManager companyBusinessManager,
            IVendorBusinessManager vendorBusinessManager)
            : base(logger, mapper) {
            _personBusinessManager = personBusinessManager;
            _companyBusinessManager = companyBusinessManager;
            _vendorBusinessManager = vendorBusinessManager;
        }

        public async Task<IActionResult> Index() {
            var vendors = await _vendorBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });


            var persons = await _personBusinessManager.GetPersons();
            var companies = await _companyBusinessManager.GetCompanies();

            ViewBag.Customers = persons.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
              .Union(companies.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));

            var model = new InvoiceFilterViewModel();
            return View(model);
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [LogAction]
    public class InvoiceController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IInvoiceBusinessManager _invoiceBusinessManager;
        private readonly IUccountBusinessManager _uccountBusinessManager;
        private readonly IToyotaFinancialService _toyotaFinancialService;

        public InvoiceController(
            IMapper mapper,
            IViewRenderService viewRenderService,
            IInvoiceBusinessManager invoiceBusinessManager,
            IUccountBusinessManager uccountBusinessManager,
            IToyotaFinancialService toyotaFinancialService) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _invoiceBusinessManager = invoiceBusinessManager;
            _uccountBusinessManager = uccountBusinessManager;
            _toyotaFinancialService = toyotaFinancialService;
        }

        [HttpGet("GetInvoices", Name = "GetInvoices")]
        public async Task<PagerDto<InvoiceListViewModel>> GetInvoices([FromQuery] InvoiceFilterViewModel model) {
            var result = await _invoiceBusinessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new PagerDto<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpGet("DetailsInvoice", Name = "DetailsInvoice")]
        public async Task<IActionResult> DetailsInvoice([FromQuery] Guid id) {
            var invoice = await _invoiceBusinessManager.GetInvoice(id);
            if(invoice == null)
                return NotFound();
            var html = await _viewRenderService.RenderToStringAsync(
                "_DetailsPartial",
                _mapper.Map<InvoiceListViewModel>(invoice));
            return Ok(html);
        }

        [HttpGet("GetInvoice", Name = "GetInvoice")]
        public async Task<IActionResult> GetInvoice([FromQuery] Guid id) {
            var invoice = await _invoiceBusinessManager.GetInvoice(id);
            if(invoice == null)
                return NotFound();
            return Ok(_mapper.Map<InvoiceListViewModel>(invoice));
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

        [HttpGet("SyncInvoice", Name = "SyncInvoice")]
        public async Task<IActionResult> SyncInvoice() {
            var accounts = await _uccountBusinessManager.GetUccountsInclude();
            var financeAccount = accounts.Where(x => x.VendorId == new Guid("40F1266A-B2C6-45AC-5976-08D84DDE544D")).ToList(); //Toyota Finance

            var invoiceList = new List<InvoiceDto>();

            foreach(var account in financeAccount) {
                var invoices = await _toyotaFinancialService.Execute(account);
                if(invoices != null) {
                    var accountInvoices = await _invoiceBusinessManager.GetInvoices(account.Id);
                    var createInvoiceList = invoices.Where(x => !accountInvoices.Any(y => x.DueDate.Equals(y.DueDate) && x.Amount.Equals(y.Amount))).ToList();
                    if(createInvoiceList.Count > 0)
                        invoiceList.AddRange(invoices);
                }
            }
            if(invoiceList.Count > 0) {
                var result = await _invoiceBusinessManager.CreateInvoice(invoiceList);
                return Ok(result);
            } else
                return Ok();
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
        public async Task<IActionResult> EditInvoice([FromQuery] Guid id) {
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
        public async Task<IActionResult> UpdateInvoice([FromQuery] Guid id, [FromBody] InvoiceViewModel model) {
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
        public async Task<IActionResult> DeleteInvoices([FromQuery] Guid[] id) {
            if(id.Length > 0) {
                var result = await _invoiceBusinessManager.DeleteInvoices(id);
                if(result)
                    return Ok(id);
            }
            return BadRequest("No items selected");
        }
    }
}
