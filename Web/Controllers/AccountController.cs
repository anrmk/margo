using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

using Web.Models.AccountViewModel;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    public class AccountController: BaseController<AccountController> {
        private readonly IAccountBusinessManager _accountBusinessManager;

        public AccountController(ILogger<AccountController> logger, IMapper mapper,
         IAccountBusinessManager accountBusinessManager) : base(logger, mapper) {
            _accountBusinessManager = accountBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Activity() {
            return View(new LogFilterViewModel());
        }

        public IActionResult Request() {
            return View();
        }

        public async Task<IActionResult> ActivityView(long id) {
            var item = await _accountBusinessManager.GetLog(id);
            if(item == null)
                return NotFound();

            return View(_mapper.Map<LogViewModel>(item));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid) {
                try {
                    var result = await _accountBusinessManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
                    if(result.Succeeded) {
                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if(result.RequiresTwoFactor) {
                        //return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                    }
                    if(result.IsLockedOut) {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    } else {
                        ModelState.AddModelError("All", "Invalid login attempt.");
                        return View(model);
                    }
                } catch(Exception e) {
                    ModelState.AddModelError("All", e.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _accountBusinessManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid) {
                try {
                    var user = new AspNetUserDto { UserName = model.Email, Email = model.Email };
                    var result = await _accountBusinessManager.CreateUser(user, model.Password);
                    if(result != null) {
                        _logger.LogInformation("User created a new account with password.");
                        var code = await _accountBusinessManager.GenerateEmailConfirmationTokenAsync(result.Id);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        await _accountBusinessManager.SignInAsync(result.Id, isPersistent: false);
                        _logger.LogInformation("User created a new account with password.");
                        return RedirectToLocal(returnUrl);
                    }
                } catch(Exception e) {
                    ModelState.AddModelError("All", e.Message);
                }
            }
            return View(model);
        }

        #region HELPERS
        private IActionResult RedirectToLocal(string returnUrl) {
            if(Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;

        private readonly IAccountBusinessManager _accountBusinessService;
        private readonly ICompanyBusinessManager _companyBusinessManager;

        public AccountController(IMapper mapper, IViewRenderService viewRenderService, IAccountBusinessManager accountBusinessService,
            ICompanyBusinessManager companyBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _accountBusinessService = accountBusinessService;
            _companyBusinessManager = companyBusinessManager;
        }

        [HttpGet("GetAppNetUsers", Name = "GetAppNetUsers")]
        public async Task<PagerDto<AspNetUserListViewModel>> GetAppNetUsers([FromQuery] PagerFilterViewModel model) {
            var result = await _accountBusinessService.GetUserPage(_mapper.Map<PagerFilterDto>(model));
            return new PagerDto<AspNetUserListViewModel>(_mapper.Map<List<AspNetUserListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpGet("DetailsAspNetUser", Name = "DetailsAspNetUser")]
        public async Task<IActionResult> DetailsAspNetUser([FromQuery] string id) {
            var item = await _accountBusinessService.GetUser(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsPartial", _mapper.Map<AspNetUserViewModel>(item));
            return Ok(html);
        }

        [HttpGet("AddAspNetUser", Name = "AddAspNetUser")]
        public async Task<IActionResult> AddAspNetUser() {
            var roles = await _accountBusinessService.GetUserRoles();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Roles", roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }) }
            };

            var html = await _viewRenderService.RenderToStringAsync("_CreatePartial", new AspNetUserViewModel(), viewData);
            return Ok(html);
        }

        [HttpPost("CreateAspNetUser", Name = "CreateAspNetUser")]
        public async Task<IActionResult> CreateAspNetUser([FromBody] AspNetUserViewModel model) {
            if(ModelState.IsValid) {
                var item = await _accountBusinessService.CreateUser(_mapper.Map<AspNetUserDto>(model), "1Q2w3E4r");
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<AspNetUserViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("EditAspNetUser", Name = "EditAspNetUser")]
        public async Task<IActionResult> EditPerson([FromQuery] string id) {
            var item = await _accountBusinessService.GetUser(id);
            if(item == null)
                return NotFound();

            var roles = await _accountBusinessService.GetUserRoles();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Roles", roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString()}) }
            };

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<AspNetUserViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdateAspNetUser", Name = "UpdateAspNetUser")]
        public async Task<IActionResult> UpdateAspNetUser([FromQuery] string id, [FromBody] AspNetUserViewModel model) {
            if(ModelState.IsValid) {
                var item = await _accountBusinessService.UpdateUser(id, _mapper.Map<AspNetUserDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<AspNetUserViewModel>(item));
            }
            return BadRequest();
        }

        [HttpGet("LockoutAspNetUser", Name = "LockoutAspNetUser")]
        public async Task<IActionResult> LockoutAspNetUser([FromQuery] string id, [FromQuery] bool locked) {
            var item = await _accountBusinessService.LockUser(id, locked);
            return Ok(item);
        }

        [HttpGet("EditAspNetUserProfile", Name = "EditAspNetUserProfile")]
        public async Task<IActionResult> EditAspNetUserProfile([FromQuery] long id) {
            var item = await _accountBusinessService.GetUserProfile(id);
            if(item == null)
                return NotFound();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                //    { "Roles", roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString()}) }
            };

            var html = await _viewRenderService.RenderToStringAsync("_EditProfilePartial", _mapper.Map<AspNetUserProfileViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdateAspNetUserProfile", Name = "UpdateAspNetUserProfile")]
        public async Task<IActionResult> UpdateAspNetUserProfile([FromQuery] long id, [FromBody] AspNetUserProfileViewModel model) {
            if(ModelState.IsValid) {
                var item = await _accountBusinessService.UpdateUserProfile(id, _mapper.Map<AspNetUserProfileDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<AspNetUserProfileViewModel>(item));
            }
            return BadRequest();
        }

        #region REQUEST
        [HttpGet("GetAspNetUserRequests", Name = "GetAspNetUserRequests")]
        public async Task<PagerDto<AspNetUserRequestListViewModel>> GetAspNetUserRequests([FromQuery] PagerFilterViewModel model) {
            var result = await _accountBusinessService.GetRequestPager(_mapper.Map<PagerFilterDto>(model));
            return new PagerDto<AspNetUserRequestListViewModel>(_mapper.Map<List<AspNetUserRequestListViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpGet("DetailsAspNetUserRequest", Name = "DetailsAspNetUserRequest")]
        public async Task<IActionResult> DetailsAspNetUserRequest([FromQuery] Guid id) {
            var item = await _accountBusinessService.GetRequest(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailRequestPartial", _mapper.Map<AspNetUserRequestViewModel>(item));
            return Ok(html);
        }

        [HttpGet("EditAspNetUserRequest", Name = "EditAspNetUserRequest")]
        public async Task<IActionResult> EditAspNetUserRequest([FromQuery] Guid id) {
            var item = await _accountBusinessService.GetRequest(id);
            if(item == null)
                return NotFound();

            if(item.ModelType == typeof(CompanyDto)) {
                var original = await _companyBusinessManager.GetCompany(item.ModelId);
                var unmodified = JsonSerializer.Deserialize(item.Model, item.ModelType);

                if(item.ModelType == typeof(CompanyDto)) {
                    var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                        { "Original", _mapper.Map<CompanyViewModel>(original) },
                        { "Unmodified", _mapper.Map<CompanyViewModel>(unmodified) }
                    };

                    var html = await _viewRenderService.RenderToStringAsync("_CompareCompanyPartial", _mapper.Map<AspNetUserRequestViewModel>(item), viewData);
                    return Ok(html);
                } else if(item.ModelType == typeof(UccountDto)) {

                } else {
                    throw new Exception("Unsupported type!");
                }
            }

            return Ok();
        }

        [HttpPost("ConfirmAspNetUserRequest", Name = "ConfirmAspNetUserRequest")]
        public async Task<IActionResult> ConfirmAspNetUserRequest([FromQuery] Guid id) {
            var item = await _accountBusinessService.GetRequest(id);
            if(item == null)
                return NotFound();

            var unmodified = JsonSerializer.Deserialize(item.Model, item.ModelType);

            if(item.ModelType == typeof(CompanyDto)) {
                var company = await _companyBusinessManager.UpdateCompany(item.ModelId, (CompanyDto)unmodified);
                if(company == null)
                    return NotFound();

                var result = await _accountBusinessService.DeleteRequest(id);
                return Ok(result);
            } else if(item.ModelType == typeof(UccountDto)) {

            }

            return BadRequest("No items selected");
        }

        [HttpPost("DeclineAspNetUserRequest", Name = "DeclineAspNetUserRequest")]
        public async Task<IActionResult> DeclineAspNetUserRequest([FromQuery] Guid id) {
            return await DeleteAspNetUserRequest(new Guid[] { id });
        }

        [HttpGet("DeleteAspNetUserRequest", Name = "DeleteAspNetUserRequest")]
        public async Task<IActionResult> DeleteAspNetUserRequest([FromQuery] Guid[] id) {
            var result = await _accountBusinessService.DeleteRequest(id);
            if(result)
                return Ok(id);

            return BadRequest("No items selected");
        }
        #endregion

        [HttpGet]
        [Route("activity")]
        public async Task<PagerDto<LogDto>> GetActivity([FromQuery] LogFilterViewModel model) {
            return await _accountBusinessService.GetLogPager(_mapper.Map<LogFilterDto>(model));
            //return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }

        [HttpGet("EditAspNetUserCompanyGrants", Name = "EditAspNetUserCompanyGrants")]
        public async Task<IActionResult> EditAspNetUserCompanyGrants([FromQuery] string id) {
            var item = await _accountBusinessService.GetUserCompanyGrants(id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_EditCompanyGrantsPartial", _mapper.Map<AspNetUserCompanyGrantsListViewModel>(item));
            return Ok(html);
        }

        [HttpPost("UpdateAspNetUserCompanyGrants", Name = "UpdateAspNetUserCompanyGrants")]
        public async Task<IActionResult> UpdateAspNetUserCompanyGrants([FromQuery] string id, [FromBody] AspNetUserCompanyGrantsListViewModel model) {
            if(ModelState.IsValid) {
                var item = await _accountBusinessService.UpdateUserCompanyGrants(id, _mapper.Map<AspNetUserCompanyGrantsListDto>(model));
                if(item == null)
                    return BadRequest();
                return Ok(_mapper.Map<AspNetUserCompanyGrantsListViewModel>(item));
            }
            return BadRequest();
        }
    }
}
