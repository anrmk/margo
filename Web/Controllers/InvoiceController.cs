﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;

using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class InvoiceController: BaseController<InvoiceController> {
        private readonly INsiBusinessManager _nsiBusinessManager;
        private readonly ICrudBusinessManager _crudBusinessManager;

        public InvoiceController(ILogger<InvoiceController> logger, IMapper mapper,
            INsiBusinessManager nsiBusinessManager, ICrudBusinessManager crudBusinessManager) : base(logger, mapper) {
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
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _crudBusinessManager;

        public InvoiceController(IMapper mapper, ICrudBusinessManager businessManager) {
            _mapper = mapper;
            _crudBusinessManager = businessManager;
        }

        [HttpGet]
        public async Task<Pager<InvoiceListViewModel>> GetInvoices([FromQuery] InvoiceFilterViewModel model) {
            var result = await _crudBusinessManager.GetInvoicePager(_mapper.Map<InvoiceFilterDto>(model));
            return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> Delete([FromBody] List<long> id) {
            try {
                if(id.Count > 0) {
                    var result = await _crudBusinessManager.DeleteInvoice(id.ToArray());
                    return Ok(result);
                }
            } catch(Exception er) {
                return BadRequest(er.Message);
            }
            return Ok(false);
        }
    }
}