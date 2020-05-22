using System;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;
using Core.Services.Business;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Web.Controllers.Mvc;
using Web.Hubs;
using Web.Models.AccountViewModel;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class AccountController: BaseController<AccountController> {
        private readonly IAccountBusinessService _accountBusinessService;

        public AccountController(ILogger<AccountController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context,
         IAccountBusinessService accountBusinessService) : base(logger, mapper, notificationHub, context) {
            _accountBusinessService = accountBusinessService;
        }

        public async Task<IActionResult> Activity() {
            return View(new LogFilterViewModel());
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
                    var result = await _accountBusinessService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
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
            await _accountBusinessService.SignOutAsync();
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
                    var user = new ApplicationUserDto { UserName = model.Email, Email = model.Email };
                    var result = await _accountBusinessService.CreateUser(user, model.Password);
                    if(result != null) {
                        _logger.LogInformation("User created a new account with password.");
                        var code = await _accountBusinessService.GenerateEmailConfirmationTokenAsync(result);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        await _accountBusinessService.SignInAsync(result, isPersistent: false);
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
    public class AccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ICrudBusinessManager _crudBusinessManager;
        private readonly IAccountBusinessService _accountBusinessService;

        public AccountController(IMapper mapper, ICrudBusinessManager businessManager, IAccountBusinessService accountBusinessService) {
            _mapper = mapper;
            _crudBusinessManager = businessManager;
            _accountBusinessService = accountBusinessService;
            //_telegramBotClient = telegramBotClient;
        }

        [HttpGet]
        [Route("logs")]
        public async Task<Pager<LogDto>> GetLog([FromQuery] LogFilterViewModel model) {
            return await _accountBusinessService.GetLogPager(_mapper.Map<LogFilterDto>(model));
            //return new Pager<InvoiceListViewModel>(_mapper.Map<List<InvoiceListViewModel>>(result.Items), result.TotalItems, result.CurrentPage, result.PageSize);
        }
    }
}