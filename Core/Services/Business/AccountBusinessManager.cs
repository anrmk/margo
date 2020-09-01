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
        Task<List<AspNetUserDto>> GetUsers();
        Task<AspNetUserDto> GetUser(string id);
        Task<PagerDto<AspNetUserDto>> GetUserPage(PagerFilterDto filter);
        Task<AspNetUserDto> CreateUser(AspNetUserDto dto, string password);
        Task<AspNetUserDto> UpdateUser(string id, AspNetUserDto dto);
        Task<bool> LockUser(string id, bool locked);
        Task SignInAsync(string id, bool isPersistent);
        Task SignOutAsync();
        Task SignOutAsync(string id);
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

        //  LOGS
        Task<PagerDto<LogDto>> GetLogPager(LogFilterDto filter);
        Task<LogDto> GetLog(DateTime startDate, DateTime endDate, Guid id);

        // ACCESS
        Task<List<AspNetUserDenyAccessCompanyDto>> GetUnavailableCompanies(string id);
        Task<List<AspNetUserDenyAccessCompanyDto>> UpdateUnavailableCompanies(string id, List<Guid> companyIds);

        Task<List<AspNetUserDenyAccessCategoryDto>> UpdateUserCategoryGrants(string id, List<Guid> categoryIds);
        Task<List<AspNetUserDenyAccessCategoryDto>> GetUnavailableCategory(string id);

        Task<List<AspNetUserCompanyFavouriteDto>> GetFavouriteCompanies(string userId);
        Task<List<AspNetUserCompanyFavouriteDto>> UpdateFavouriteCompanies(string userId, IEnumerable<AspNetUserCompanyFavouriteDto> favourites);
    }

    public class AccountBusinessManager: BaseBusinessManager, IAccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AspNetUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;
        private readonly IUserRequestManager _userRequestManager;
        private readonly ILogManager _logManager;

        private readonly IAspNetUserDenyAccessCompanyManager _userDenyAccessCompanyManager;
        private readonly IAspNetUserDenyAccessCategoryManager _userDenyAccessCategoryManager;

        private readonly IUserFavouriteCompanyManager _favouriteCompanyManager;

        public AccountBusinessManager(IMapper mapper,
            UserManager<AspNetUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AspNetUserEntity> signInManager,
            IUserProfileManager userProfileManager,
            IUserRequestManager userRequestManager,
            IAspNetUserDenyAccessCompanyManager userDenyAccessCompanyManager,
            IAspNetUserDenyAccessCategoryManager userDenyAccessCategoryManager,
             ILogManager logManager,
             IUserFavouriteCompanyManager favouriteCompanyManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
            _userRequestManager = userRequestManager;
            _logManager = logManager;
            _userDenyAccessCompanyManager = userDenyAccessCompanyManager;
            _userDenyAccessCategoryManager = userDenyAccessCategoryManager;
            _favouriteCompanyManager = favouriteCompanyManager;
        }

        #region ACCOUNT
        public async Task<List<AspNetUserDto>> GetUsers() {
            var entities = await _userManager.Users.Include(x => x.Profile).ToListAsync();
            return _mapper.Map<List<AspNetUserDto>>(entities); ;
        }

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

        public async Task SignOutAsync(string id) {
            var entity = await _userManager.Users
                .Include(x => x.Profile)
                .Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if(entity != null)
                await _userManager.UpdateSecurityStampAsync(entity);
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

        #region LOGS
        public async Task<PagerDto<LogDto>> GetLogPager(LogFilterDto filter) {
            Func<LogDto, object> sortBy = x => x.Logged;
            Func<LogDto, bool> where = x =>
                (string.IsNullOrEmpty(filter.UserName) || string.IsNullOrEmpty(x.UserName) || x.UserId.ToLower().Contains(filter.UserName))
                && (string.IsNullOrEmpty(filter.Search) || string.IsNullOrEmpty(x.Message) || x.Message.ToLower().Contains(filter.Search.ToLower()))
                && (string.IsNullOrEmpty(filter.Controller) || string.IsNullOrEmpty(x.Controller) || x.Controller.ToLower().Contains(filter.Controller.ToLower()))
                && (string.IsNullOrEmpty(filter.Action) || string.IsNullOrEmpty(x.Action) || x.Action.ToLower().Contains(filter.Action.ToLower()))
                && (string.IsNullOrEmpty(filter.Method) || string.IsNullOrEmpty(x.Method) || x.Method.ToLower().Contains(filter.Method.ToLower()));

            var tuple = await _logManager.Pager(filter.StartDate, filter.EndDate, where, sortBy, false, filter.Start, filter.Length, filter.IsException);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerDto<LogDto>(new List<LogDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;
            return new PagerDto<LogDto>(list, count, page, filter.Length);
        }

        public async Task<LogDto> GetLog(DateTime startDate, DateTime endDate, Guid id) {
            return await _logManager.Find(startDate, endDate, id);
        }
        #endregion

        #region ACCESS
        public async Task<List<AspNetUserDenyAccessCompanyDto>> GetUnavailableCompanies(string id) {
            var entities = await _userDenyAccessCompanyManager.FindByUserId(id);
            return _mapper.Map<List<AspNetUserDenyAccessCompanyDto>>(entities);
        }

        public async Task<List<AspNetUserDenyAccessCompanyDto>> UpdateUnavailableCompanies(string id, List<Guid> companyIds) {
            var entities = await _userDenyAccessCompanyManager.FindByUserId(id);

            var intersectionEntities = entities.Where(x => companyIds.Contains(x.CompanyId)).ToList();
            var deleteEntities = entities.Except(intersectionEntities); // to delete
            var createEntities = companyIds.Where(x => !intersectionEntities.Any(y => y.CompanyId == x)).Select(x => new AspNetUserDenyAccessCompanyEntity() {
                UserId = id,
                CompanyId = x
            }); // to insert

            if(deleteEntities.Count() > 0)
                await _userDenyAccessCompanyManager.Delete(deleteEntities);

            if(createEntities.Count() > 0)
                await _userDenyAccessCompanyManager.Create(createEntities);

            entities = await _userDenyAccessCompanyManager.FindByUserId(id);

            return _mapper.Map<List<AspNetUserDenyAccessCompanyDto>>(entities);
        }

        public async Task<List<AspNetUserDenyAccessCategoryDto>> UpdateUserCategoryGrants(string id, List<Guid> categoryIds) {
            var entities = await _userDenyAccessCategoryManager.FindByUserId(id);

            var intersectionEntities = entities.Where(x => categoryIds.Contains(x.CategoryId)).ToList();
            var deleteEntities = entities.Except(intersectionEntities); // to delete
            var createEntities = categoryIds.Where(x => !intersectionEntities.Any(y => y.CategoryId == x)).Select(x => new AspNetUserDenyAccessCategoryEntity() {
                UserId = id,
                CategoryId = x
            }); // to insert

            if(deleteEntities.Count() > 0)
                await _userDenyAccessCategoryManager.Delete(deleteEntities);

            if(createEntities.Count() > 0)
                await _userDenyAccessCategoryManager.Create(createEntities);

            entities = await _userDenyAccessCategoryManager.FindByUserId(id);

            return _mapper.Map<List<AspNetUserDenyAccessCategoryDto>>(entities);
        }

        public async Task<List<AspNetUserDenyAccessCategoryDto>> GetUnavailableCategory(string id) {
            var entities = await _userDenyAccessCategoryManager.FindByUserId(id);
            return _mapper.Map<List<AspNetUserDenyAccessCategoryDto>>(entities);
        }
        #endregion

        public async Task<List<AspNetUserCompanyFavouriteDto>> GetFavouriteCompanies(string userId) {
            var result = await _favouriteCompanyManager.FindByUserId(userId);
            return _mapper.Map<List<AspNetUserCompanyFavouriteDto>>(result);
        }

        public async Task<List<AspNetUserCompanyFavouriteDto>> UpdateFavouriteCompanies(string userId, IEnumerable<AspNetUserCompanyFavouriteDto> favourites) {
            var entities = await _favouriteCompanyManager.FindByUserId(userId);

            var intersectionEntities = entities.Where(x => favourites.Any(z => z.CompanyId == x.CompanyId && z.Sort == x.Sort)).ToList();
            var deleteEntities = entities.Except(intersectionEntities);
            var createEntities = favourites.Where(x => !intersectionEntities.Any(y => y.CompanyId == x.CompanyId)).Select(x => new AspNetUserCompanyFavouriteEntity() {
                UserId = userId,
                CompanyId = x.CompanyId,
                Sort = x.Sort
            });

            if(deleteEntities.Count() > 0)
                await _favouriteCompanyManager.Delete(deleteEntities);

            if(createEntities.Count() > 0)
                await _favouriteCompanyManager.Create(createEntities);

            entities = await _favouriteCompanyManager.FindByUserId(userId);

            return _mapper.Map<List<AspNetUserCompanyFavouriteDto>>(entities);
        }
    }
}
