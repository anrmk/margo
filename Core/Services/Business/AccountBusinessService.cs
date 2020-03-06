using System;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

using Microsoft.AspNetCore.Identity;

namespace Core.Services.Business {
    public interface IAccountBusinessService {
        Task<ApplicationUserEntity> CreateUser(ApplicationUserDto dto, string password);
        Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto);
        //Task<ApplicationUserDto> GetUserProfile(string name);
        //Task<ApplicationUserDto> UpdateUserProfile(string userId, ApplicationUserProfileDto model);
        //Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe);
        Task SignInAsync(ApplicationUserEntity entity, bool isPersistent);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserEntity entity);
    }

    public class AccountBusinessService: BaseBusinessManager, IAccountBusinessService {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;

        public AccountBusinessService(IMapper mapper,
            UserManager<ApplicationUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUserEntity> signInManager,
            IUserProfileManager userProfileManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
        }

        public async Task<ApplicationUserEntity> CreateUser(ApplicationUserDto dto, string password) {
            var user = new ApplicationUserEntity() {
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserEntity entity) {
            return await _userManager.GenerateEmailConfirmationTokenAsync(entity);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe) {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
        }

        public async Task SignInAsync(ApplicationUserEntity entity, bool isPersistent) {
            await _signInManager.SignInAsync(entity, isPersistent);
        }

        public async Task SignOutAsync() {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto) {
            try {
                var item1 = await _userProfileManager.Create(new ApplicationUserProfileEntity() {
                    Name = dto.Name,
                    SurName = dto.SurName,
                    MiddleName = dto.MiddleName
                });
                return null;
            } catch(Exception e) {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}