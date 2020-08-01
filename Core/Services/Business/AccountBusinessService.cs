using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Business {
    public interface IAccountBusinessService {
        Task<AspNetUserDto> GetUser(string id);
        Task<Pager<AspNetUserDto>> GetUserPage(PagerFilter filter);
        Task<AspNetUserDto> CreateUser(AspNetUserDto dto, string password);
        Task<AspNetUserDto> UpdateUser(string id, AspNetUserDto dto);

        Task<AspNetUserDto> UpdateUserProfile(string id, UserProfileDto dto);

        Task<List<AspNetRoleDto>> GetUserRoles();

        //Task<ApplicationUserDto> GetUserProfile(string name);
        //Task<ApplicationUserDto> UpdateUserProfile(string userId, ApplicationUserProfileDto model);
        //Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe);
        Task SignInAsync(string id, bool isPersistent);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(string id);


        Task<Pager<LogDto>> GetLogPager(LogFilterDto filter);
        Task<LogDto> GetLog(long id);
    }

    public class AccountBusinessService: BaseBusinessManager, IAccountBusinessService {
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AspNetUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;
        private readonly ILogManager _logManager;

        public AccountBusinessService(IMapper mapper,
            UserManager<AspNetUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AspNetUserEntity> signInManager,
            IUserProfileManager userProfileManager, ILogManager logManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
            _logManager = logManager;
        }
        #region ACCOUNT
        public async Task<AspNetUserDto> GetUser(string id) {
            var entity = await _userManager.Users.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if(entity == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(entity);
            var roles =  _roleManager.Roles.Where(x => userRoles.Any(y => x.Name.Equals(y))).ToList();
            
            var dto = _mapper.Map<AspNetUserDto>(entity);
            dto.Roles = _mapper.Map<List<AspNetRoleDto>>(roles);

            return dto;
        }

        public async Task<Pager<AspNetUserDto>> GetUserPage(PagerFilter filter) {
            var sortby = "UserName";

            var query = _userManager.Users
                .Include(x => x.Profile)
                .Where(x => (true)
                    && (string.IsNullOrEmpty(filter.Search) || x.UserName.ToLower().Contains(filter.Search.ToLower())));

            var count = await query.CountAsync();
            if(count == 0)
                return new Pager<AspNetUserDto>(new List<AspNetUserDto>(), 0, filter.Start, filter.Length);

            query = string.IsNullOrEmpty(sortby) ?
             query.OrderBy(x => Guid.NewGuid().ToString()).Skip(filter.Start) :
             SortExtension.OrderByDynamic(query, sortby, true).Skip(filter.Start);
            
            var roleNames = await _roleManager.Roles.ToListAsync();

            var list = await query.Take(filter.Length).ToListAsync();
            var result = new List<AspNetUserDto>();
            foreach(var user in list) {
                var userRoleNames = await _userManager.GetRolesAsync(user);
                var userRoles = roleNames.Where(x => userRoleNames.Any(y => x.Name.Equals(y, StringComparison.OrdinalIgnoreCase))).ToList();
                
                var model = _mapper.Map<AspNetUserDto>(user);
                model.Roles = _mapper.Map<List<AspNetRoleDto>>(userRoles);
                result.Add(model);
            }

            var page = (filter.Start + filter.Length) / filter.Length;

            return new Pager<AspNetUserDto>(result, count, page, filter.Length);
        }

        public async Task<AspNetUserDto> CreateUser(AspNetUserDto dto, string password) {
            try {
                var entity = _mapper.Map<AspNetUserEntity>(dto);
                var result = await _userManager.CreateAsync(entity, password);

                if(result.Succeeded) {
                    // Add user to new roles
                    var roleNames = await _roleManager.Roles.Where(x => dto.Roles.Any(y => y.Id == x.Id)).Select(x => x.Name).ToArrayAsync();
                    await _userManager.AddToRolesAsync(entity, roleNames);
                    return _mapper.Map<AspNetUserDto>(entity);
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

        public async Task<AspNetUserDto> UpdateUser(string id, AspNetUserDto dto) {
            var entity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            var result = await _userManager.UpdateAsync(newEntity);
            if(result.Succeeded) {
                // Edit user roles
                var allRoles = await _roleManager.Roles.ToListAsync();
                var userRoles = await _userManager.GetRolesAsync(newEntity);

                //remove
                var removeRoles = allRoles
                    .Where(x => userRoles.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                    .Where(x => !dto.Roles.Any(y => y.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x.Name);
                if(removeRoles.Count() != 0)
                    await _userManager.RemoveFromRolesAsync(newEntity, removeRoles);

                //add
                var addRoles = allRoles
                    .Where(x => dto.Roles.Any(y => y.Id.Equals(x.Id)))
                    .Where(x => !userRoles.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x.Name);
                //var roleNames = allRoles.Where(x => dto.Roles.Any(y => y.Id == y.Id)).Select(x => x.Name).ToList();
                //var addRoles = roleNames.Where(x => !userRoles.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase)));
                if(addRoles.Count() != 0)
                    await _userManager.AddToRolesAsync(newEntity, addRoles);

                return _mapper.Map<AspNetUserDto>(newEntity);
            }
            return null;
        }

        #endregion

        #region ROLES
        public async Task<List<AspNetRoleDto>> GetUserRoles() {
            var entity = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<AspNetRoleDto>>(entity);
        }
        #endregion

        public async Task<string> GenerateEmailConfirmationTokenAsync(string id) {
            var entity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
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

        public async Task SignInAsync(string id, bool isPersistent) {
            var entity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            await _signInManager.SignInAsync(entity, isPersistent);
        }

        public async Task SignOutAsync() {
            await _signInManager.SignOutAsync();
        }

        public async Task<AspNetUserDto> UpdateUserProfile(string id, UserProfileDto dto) {
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