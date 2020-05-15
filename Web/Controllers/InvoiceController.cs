using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Extensions;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class InvoiceController: BaseController<InvoiceController> {
        private readonly INsiBusinessManager _nsiBusinessManager;
        private readonly ICrudBusinessManager _crudBusinessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context,
            INsiBusinessManager nsiBusinessManager, ICrudBusinessManager crudBusinessManager) : base(logger, mapper, notificationHub, context) {
            _nsiBusinessManager = nsiBusinessManager;
            _crudBusinessManager = crudBusinessManager;
        }

        public async Task<ActionResult> Index() {
            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var model = new InvoiceFilterViewModel() {
                CompanyId = null
            };

            return View(model);
        }

        public async Task<ActionResult> Details(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            return View(_mapper.Map<InvoiceViewModel>(item));
        }

        public async Task<ActionResult> Create() {
            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.CreateInvoice(_mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Edit), new { Id = item.Id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            return View(model);
        }

        public async Task<ActionResult> Edit(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            return View(_mapper.Map<InvoiceViewModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, InvoiceViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var item = await _crudBusinessManager.UpdateInvoice(id, _mapper.Map<InvoiceDto>(model));
                    if(item == null) {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Edit), new { Id = id });
                }
            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                BadRequest(er);
            }

            var vendors = await _crudBusinessManager.GetVendors();
            ViewBag.Vendors = vendors.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            var companies = await _crudBusinessManager.GetCompanies();
            ViewBag.Companies = companies.Select(x => new SelectListItem() { Text = x.General.Name, Value = x.Id.ToString() });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id) {
            try {
                var item = await _crudBusinessManager.DeleteInvoice(id);
                if(item == false) {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));

            } catch(Exception er) {
                _logger.LogError(er, er.Message);
                return BadRequest(er);
            }
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    //[ApiController]
    public class InvoiceController: ControllerBase {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _businessManager;
        private readonly ITelegramBotClient _telegramBotClient;

        public InvoiceController(IConfiguration configuration, IMapper mapper, IViewRenderService viewRenderService,
            ICrudBusinessManager businessManager) {
            _configuration = configuration;
            _mapper = mapper;
            _businessManager = businessManager;
            //_telegramBotClient = telegramBotClient;
        }

        [HttpGet]
        public async Task<Pager<InvoiceListViewModel>> GetInvoices(InvoiceFilterViewModel model) {
            var result = await _businessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }

        [HttpGet("SendNotification", Name = "ApiSendNotification")]
        public async Task<ActionResult> SendNotification() {
            var me = _telegramBotClient.GetMeAsync().Result;

            var message = "Hello Margo *world* \n lalala";

            var chatId = int.Parse(_configuration.GetConnectionString("TelegramChatId"));

            //var result = await _telegramBotClient.SendTextMessageAsync(new ChatId(_configuration.GetConnectionString("TelegramChatId")), message, ParseMode.Markdown);
            //await _telegramBotClient.SendInvoiceAsync(chatId, "Invoice Title", "Invoice description hdafljlj kjl", "", "providerToken", "startParameter", "USD");

            return Ok();
        }

        [HttpGet("{id}/pay", Name = "Pay")]
        public async Task<ActionResult> Pay(long id) {
            var result = await _businessManager.PayInvoice(id);
            return Ok(_mapper.Map<InvoiceViewModel>(result));
        }
    }
}