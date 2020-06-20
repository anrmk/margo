using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;

using Microsoft.AspNetCore.Identity;

namespace Core.Services.Business {
    public interface IAccountBusinessService {
        Task<AppNetUserEntity> CreateUser(ApplicationUserDto dto, string password);
        Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto);
        //Task<ApplicationUserDto> GetUserProfile(string name);
        //Task<ApplicationUserDto> UpdateUserProfile(string userId, ApplicationUserProfileDto model);
        //Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe);
        Task SignInAsync(AppNetUserEntity entity, bool isPersistent);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(AppNetUserEntity entity);

        Task<Pager<LogDto>> GetLogPager(LogFilterDto filter);
        Task<LogDto> GetLog(long id);

    }

    public class AccountBusinessService: BaseBusinessManager, IAccountBusinessService {
        private readonly IMapper _mapper;
        private readonly UserManager<AppNetUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppNetUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;
        private readonly ILogManager _logManager;

        public AccountBusinessService(IMapper mapper,
            UserManager<AppNetUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppNetUserEntity> signInManager,
            IUserProfileManager userProfileManager, ILogManager logManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
            _logManager = logManager;
        }

        public async Task<AppNetUserEntity> CreateUser(ApplicationUserDto dto, string password) {
            var user = new AppNetUserEntity() {
                UserName = dto.UserName,
                NormalizedUserName = dto.NormalizedUserName,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberConfirmed = dto.PhoneNumberConfirmed
            };

            try {
                var result = await _userManager.CreateAsync(user, password);

                if(result.Succeeded) {
                    // Add user to new roles
                    // var roleNames = await _roleManager.Roles.Where(x => model.Roles.Contains(x.Id)).Select(x => x.Name).ToArrayAsync();
                    //var res2 = await _userManager.AddToRolesAsync(user.Id, roleNames);
                    return user;
                } else {
                    foreach(var error in result.Errors) {
                        //ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            } catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }
            return null;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppNetUserEntity entity) {
            return await _userManager.GenerateEmailConfirmationTokenAsync(entity);
        }

        public async Task<Pager<LogDto>> GetLogPager(LogFilterDto filter) {
            var sortby = "Logged";

            Expression<Func<LogEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search) || x.Message.ToLower().Contains(filter.Search.ToLower()))
                && (!string.IsNullOrEmpty(x.UserName))
                    ;

            var tuple = await _logManager.Pager<LogEntity>(where, sortby, filter.Start, filter.Length);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<LogDto>(new List<LogDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<LogDto>>(list);
            return new Pager<LogDto>(result, count, page, filter.Start);
        }

        public async Task<LogDto> GetLog(long id) {
            var item = await _logManager.Find(id);
            return _mapper.Map<LogDto>(item);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe) {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
        }

        public async Task SignInAsync(AppNetUserEntity entity, bool isPersistent) {
            await _signInManager.SignInAsync(entity, isPersistent);
        }

        public async Task SignOutAsync() {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto) {
            try {
                var item1 = await _userProfileManager.Create(new AppNetUserProfileEntity() {
                    Name = dto.Name,
                    SurName = dto.SurName,
                    MiddleName = dto.MiddleName
                });
                return null;
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}