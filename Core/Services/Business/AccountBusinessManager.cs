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
    public interface IAccountBusinessManager {
        //  ACCOUNT
        Task<AspNetUserDto> GetUser(string id);
        Task<PagerDto<AspNetUserDto>> GetUserPage(PagerFilterDto filter);
        Task<AspNetUserDto> CreateUser(AspNetUserDto dto, string password);
        Task<AspNetUserDto> UpdateUser(string id, AspNetUserDto dto);
        Task<bool> LockUser(string id, bool locked);
        Task SignInAsync(string id, bool isPersistent);
        Task SignOutAsync();
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe);
        Task<string> GenerateEmailConfirmationTokenAsync(string id);

        //  ROLES
        Task<List<AspNetRoleDto>> GetUserRoles();

        //  PROFILE
        Task<AspNetUserProfileDto> GetUserProfile(long id);
        Task<AspNetUserProfileDto> UpdateUserProfile(long id, AspNetUserProfileDto dto);

        //  REQUESTS
        Task<AspNetUserRequestDto> GetRequest(Guid id);
        Task<AspNetUserRequestDto> GetRequest(string userName, Guid modelId);
        Task<PagerDto<AspNetUserRequestDto>> GetRequestPager(PagerFilterDto filter);
        Task<AspNetUserRequestDto> CreateRequest(AspNetUserRequestDto dto);
        Task<AspNetUserRequestDto> UpdateRequset(Guid id, AspNetUserRequestDto dto);
        Task<bool> DeleteRequest(Guid id);
        Task<bool> DeleteRequest(Guid[] ids);

        //  GRANTS
        Task<AspNetUserCompanyGrantsListDto> GetUserCompanyGrants(string id);
        Task<AspNetUserCompanyGrantsListDto> UpdateUserCompanyGrants(string id, AspNetUserCompanyGrantsListDto dto);

        //  LOGS
        Task<PagerDto<LogDto>> GetLogPager(LogFilterDto filter);
        Task<LogDto> GetLog(long id);
    }

    public class AccountBusinessManager: BaseBusinessManager, IAccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AspNetUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;
        private readonly IUserRequestManager _userRequestManager;
        private readonly IUserCompanyGrantsManager _userCompanyGrantsManager;
        private readonly ICompanyManager _companyManager;
        private readonly ILogManager _logManager;

        public AccountBusinessManager(IMapper mapper,
            UserManager<AspNetUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AspNetUserEntity> signInManager,
            IUserProfileManager userProfileManager, IUserRequestManager userRequestManager, IUserCompanyGrantsManager userCompanyGrantsManager, ICompanyManager companyManager, ILogManager logManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
            _userRequestManager = userRequestManager;
            _companyManager = companyManager;
            _logManager = logManager;
            _userCompanyGrantsManager = userCompanyGrantsManager;
        }

        #region ACCOUNT
        public async Task<AspNetUserDto> GetUser(string id) {
            var entity = await _userManager.Users
                .Include(x => x.Profile)
                .Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if(entity == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(entity);
            var roles = _roleManager.Roles.Where(x => userRoles.Any(y => x.Name.Equals(y))).ToList();

            var dto = _mapper.Map<AspNetUserDto>(entity);
            dto.Roles = _mapper.Map<List<AspNetRoleDto>>(roles);

            return dto;
        }

        public async Task<PagerDto<AspNetUserDto>> GetUserPage(PagerFilterDto filter) {
            var sortby = "UserName";

            var query = _userManager.Users
                .Include(x => x.Profile)
                .Where(x => (true)
                    && (string.IsNullOrEmpty(filter.Search) || x.UserName.ToLower().Contains(filter.Search.ToLower())));

            var count = await query.CountAsync();
            if(count == 0)
                return new PagerDto<AspNetUserDto>(new List<AspNetUserDto>(), 0, filter.Start, filter.Length);

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

            return new PagerDto<AspNetUserDto>(result, count, page, filter.Length);
        }

        public async Task<AspNetUserDto> CreateUser(AspNetUserDto dto, string password) {
            try {
                var entity = _mapper.Map<AspNetUserEntity>(dto);
                entity.Profile = new AspNetUserProfileEntity(); // init profile

                var result = await _userManager.CreateAsync(entity, password);

                if(result.Succeeded) {
                    // Add user to new roles
                    var allRoles = await _roleManager.Roles.ToListAsync();
                    var roleNames = allRoles
                        .Where(x => dto.Roles.Any(y => y.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)))
                        .Select(x => x.Name).ToList();

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

        public async Task<bool> LockUser(string id, bool locked) {
            var entity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(entity == null) {
                return false;
            }

            entity.LockoutEnd = locked ? new DateTimeOffset(new DateTime(2999, 01, 01)) : (DateTimeOffset?)null;
            var result = await _userManager.UpdateAsync(entity);
            return result.Succeeded ? locked : !locked;

        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string id) {
            var entity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            return await _userManager.GenerateEmailConfirmationTokenAsync(entity);
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
        #endregion

        #region ROLES
        public async Task<List<AspNetRoleDto>> GetUserRoles() {
            var entity = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<AspNetRoleDto>>(entity);
        }
        #endregion

        #region USER PROFILE
        public async Task<AspNetUserProfileDto> GetUserProfile(long id) {
            var entity = await _userProfileManager.Find(id);
            return _mapper.Map<AspNetUserProfileDto>(entity);
        }

        public async Task<AspNetUserProfileDto> UpdateUserProfile(long id, AspNetUserProfileDto dto) {
            var entity = await _userProfileManager.Find(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            var result = await _userProfileManager.Update(newEntity);

            return _mapper.Map<AspNetUserProfileDto>(result);
        }
        #endregion

        #region USER REQUESTS
        public async Task<AspNetUserRequestDto> GetRequest(Guid id) {
            var entity = await _userRequestManager.Find(id);
            return _mapper.Map<AspNetUserRequestDto>(entity);
        }

        public async Task<AspNetUserRequestDto> GetRequest(string userName, Guid modelId) {
            var entity = await _userRequestManager.FindInclude(userName, modelId);
            return _mapper.Map<AspNetUserRequestDto>(entity);
        }

        public async Task<PagerDto<AspNetUserRequestDto>> GetRequestPager(PagerFilterDto filter) {
            var sortby = "Id";

            Expression<Func<AspNetUserRequestEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search) || x.UpdatedBy.ToLower().Contains(filter.Search.ToLower()))
                //&& (!string.IsNullOrEmpty(x.UserName))
                ;

            var (list, count) = await _userRequestManager.Pager<AspNetUserRequestEntity>(where, sortby, filter.Start, filter.Length);
            if(count == 0)
                return new PagerDto<AspNetUserRequestDto>(new List<AspNetUserRequestDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            return new PagerDto<AspNetUserRequestDto>(_mapper.Map<List<AspNetUserRequestDto>>(list), count, page, filter.Length);
        }

        public async Task<AspNetUserRequestDto> CreateRequest(AspNetUserRequestDto dto) {
            var entity = await _userRequestManager.Create(_mapper.Map<AspNetUserRequestEntity>(dto));
            return _mapper.Map<AspNetUserRequestDto>(entity);
        }

        public async Task<AspNetUserRequestDto> UpdateRequset(Guid id, AspNetUserRequestDto dto) {
            var entity = await _userRequestManager.Find(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            var result = await _userRequestManager.Update(newEntity);

            return _mapper.Map<AspNetUserRequestDto>(result);
        }

        public async Task<bool> DeleteRequest(Guid id) {
            return await DeleteRequest(new Guid[] { id });
        }

        public async Task<bool> DeleteRequest(Guid[] ids) {
            var entities = await _userRequestManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _userRequestManager.Delete(entities);
            return result != 0;
        }
        #endregion

        #region COMPANY GRANTS
        public async Task<AspNetUserCompanyGrantsListDto> GetUserCompanyGrants(string id) {
            var entities = await _companyManager.FindAllGrantsByUser(id);
            var grants = _mapper.Map<List<AspNetUserCompanyGrantsDto>>(entities);
            return new AspNetUserCompanyGrantsListDto { UserId = id, Grants = grants };
        }

        public async Task<AspNetUserCompanyGrantsListDto> UpdateUserCompanyGrants(string id, AspNetUserCompanyGrantsListDto dto) {
            var grantEntities = _mapper.Map<IEnumerable<AspNetUserGrantEntity>>(dto.Grants);
            await _userCompanyGrantsManager.Delete(grantEntities.Where(x => x.Id != Guid.Empty));
            grantEntities = await _userCompanyGrantsManager.Create(grantEntities);

            var grants = _mapper.Map<List<AspNetUserCompanyGrantsDto>>(grantEntities);
            return new AspNetUserCompanyGrantsListDto { UserId = id, Grants = grants };
        }
        #endregion

        

        #region LOGS
        public async Task<PagerDto<LogDto>> GetLogPager(LogFilterDto filter) {
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
                return new PagerDto<LogDto>(new List<LogDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<LogDto>>(list);
            return new PagerDto<LogDto>(result, count, page, filter.Start);
        }

        public async Task<LogDto> GetLog(long id) {
            var item = await _logManager.Find(id);
            return _mapper.Map<LogDto>(item);
        }
        #endregion

    }
}
