using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Enums;
using Core.Extension;
using Core.Filters;
using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.Models.AccountViewModel;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    [LogAction]
    public class AccountController: BaseController<AccountController> {
        private readonly IAccountBusinessManager _accountBusinessManager;

        public AccountController(ILogger<AccountController> logger, IMapper mapper,
         IAccountBusinessManager accountBusinessManager) : base(logger, mapper) {
            _accountBusinessManager = accountBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> Activity() {
            var userNameList = await _accountBusinessManager.GetUsers();
            ViewBag.UserNames = userNameList.Select(x => new SelectListItem() { Text = x.Profile != null ? $"{x.Profile.Name} {x.Profile.MiddleName} {x.Profile.SurName}" : x.NormalizedUserName, Value = x.Id.ToString() });

            var methodList = EnumExtension.GetAll<HttpMethodEnum>();
            ViewBag.Methods = methodList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            return View(new LogFilterViewModel { StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date });
        }

        public IActionResult Request() {
            return View();
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
    [LogAction]
    public class AccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IViewRenderService _viewRenderService;

        private readonly IAccountBusinessManager _accountBusinessService;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        private readonly ICategoryBusinessManager _categoryBusinessManager;

        public AccountController(IMapper mapper,
            IHubContext<NotificationHub> notificationHub,
            IViewRenderService viewRenderService, IAccountBusinessManager accountBusinessService,
            ICompanyBusinessManager companyBusinessManager,
            ICategoryBusinessManager categoryBusinessManager) {
            _mapper = mapper;
            _notificationHub = notificationHub;
            _viewRenderService = viewRenderService;
            _accountBusinessService = accountBusinessService;
            _companyBusinessManager = companyBusinessManager;
            _categoryBusinessManager = categoryBusinessManager;
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
            
            item.Profile ??= new AspNetUserProfileDto();

            var companies = await _companyBusinessManager.GetCompanies(true);
            var categories = await _categoryBusinessManager.GetCategories(true);

            var roles = await _accountBusinessService.GetUserRoles();
            var companyGrants = await _accountBusinessService.GetUnavailableCompanies(id);
            var categoryGrants = await _accountBusinessService.GetUnavailableCategory(id);

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
                { "Roles", roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString()}) },
                { "Companies", _mapper.Map<List<CompanyListViewModel>>(companies) },
                { "Categories", _mapper.Map<List<CategoryListViewModel>>(categories) },
                { "CompanyGrants", _mapper.Map<List<AspNetUserDenyAccessViewModel>>(companyGrants) },
                { "CategoryGrants", _mapper.Map<List<AspNetUserDenyAccessViewModel>>(categoryGrants) },
            };

            var html = await _viewRenderService.RenderToStringAsync("_EditPartial", _mapper.Map<AspNetUserViewModel>(item), viewData);
            return Ok(html);
        }

        [HttpPut("UpdateAspNetUser", Name = "UpdateAspNetUser")]
        public async Task<IActionResult> UpdateAspNetUser([FromQuery] string id, [FromBody] AspNetUserViewModel model) {
            try {
                if(!ModelState.IsValid)
                    throw new Exception("Form is not valid!");

                var item = await _accountBusinessService.UpdateUser(id, _mapper.Map<AspNetUserDto>(model));
                if(item == null)
                    return NotFound();

                return Ok(_mapper.Map<AspNetUserViewModel>(item));
            } catch(Exception er) {
                return BadRequest(er.Message ?? er.StackTrace);
            }
        }

        [HttpGet("LockoutAspNetUser", Name = "LockoutAspNetUser")]
        public async Task<IActionResult> LockoutAspNetUser([FromQuery] string id, [FromQuery] bool locked) {
            var item = await _accountBusinessService.LockUser(id, locked);
            if(item) {
                await _accountBusinessService.SignOutAsync(id);
                await _notificationHub.Clients.User(id).SendAsync("signout");
            }

            return Ok(item);
        }

        [HttpPut("UpdateAspNetUserProfile", Name = "UpdateAspNetUserProfile")]
        public async Task<IActionResult> UpdateAspNetUserProfile([FromQuery] long id, [FromBody] AspNetUserProfileViewModel model) {
            try {
                if(!ModelState.IsValid)
                    throw new Exception("Form is not valid!");

                var item = await _accountBusinessService.UpdateUserProfile(id, _mapper.Map<AspNetUserProfileDto>(model));
                if(item == null)
                    return NotFound();

                return Ok(_mapper.Map<AspNetUserProfileViewModel>(item));
            } catch(Exception er) {
                return BadRequest(er.Message ?? er.StackTrace);
            }
        }

        [HttpPut("UpdateAspNetUserCompanyAccess", Name = "UpdateAspNetUserCompanyAccess")]
        public async Task<IActionResult> UpdateAspNetUserCompanyAccess([FromQuery] string id, [FromBody] AspNetUserDenyAccessUpdateViewModel model) {
            try {
                if(!ModelState.IsValid)
                    throw new Exception("Form is not valid!");

                var item = await _accountBusinessService.UpdateUnavailableCompanies(id, (model.Ids ?? new List<Guid>()).ToList());

                if(item == null)
                    return NotFound();

                return Ok(_mapper.Map<List<AspNetUserDenyAccessViewModel>>(item));
            } catch(Exception er) {
                return BadRequest(er.Message ?? er.StackTrace);
            }
        }

        [HttpPut("UpdateAspNetUserCategoryGrants", Name = "UpdateAspNetUserCategoryGrants")]
        public async Task<IActionResult> UpdateAspNetUserCategoryGrants([FromQuery] string id, [FromBody] AspNetUserDenyAccessUpdateViewModel model) {
            try {
                if(!ModelState.IsValid)
                    throw new Exception("Form is not valid!");

                var item = await _accountBusinessService.UpdateUserCategoryGrants(id, (model.Ids ?? new List<Guid>()).ToList());

                if(item == null)
                    return NotFound();

                return Ok(_mapper.Map<List<AspNetUserDenyAccessViewModel>>(item));
            } catch(Exception er) {
                return BadRequest(er.Message ?? er.StackTrace);
            }
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

                if(result)
                    await _notificationHub.Clients.All.SendAsync("receiveMessage", new { Message = $"{company.Name} company data was applied!" });

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
            if(result) {
                return Ok(id);
            }
            return BadRequest("No items selected");
        }
        #endregion

        [HttpGet]
        public async Task<PagerDto<LogViewModel>> GetActivity([FromQuery] LogFilterViewModel model) {
            var result = await _accountBusinessService.GetLogPager(_mapper.Map<LogFilterDto>(model));
            return new PagerDto<LogViewModel>(_mapper.Map<List<LogViewModel>>(result.Data), result.RecordsTotal, result.Start, result.PageSize);
        }

        [HttpGet("DetailsActivity", Name = "DetailsActivity")]
        public async Task<IActionResult> DetailsActivity([FromQuery] LogFilterViewModel model) {
            var item = await _accountBusinessService.GetLog(model.StartDate, model.EndDate, model.Id);
            if(item == null)
                return NotFound();

            var html = await _viewRenderService.RenderToStringAsync("_DetailsActivityPartial", _mapper.Map<LogViewModel>(item));
            return Ok(html);
        }
    }
}
