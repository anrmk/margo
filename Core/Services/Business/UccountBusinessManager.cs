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

namespace Core.Services.Business {
    public interface IUccountBusinessManager {
        //  UCCOUNT
        Task<UccountDto> GetUccount(Guid id);
        Task<PagerDto<UccountDto>> GetUccountPage(UccountFilterDto filter);
        Task<List<UccountDto>> GetUccounts();
        Task<List<UccountDto>> GetUccountsInclude();
        Task<List<UccountServiceDto>> GetServicesByCompanyId(Guid companyId);
        Task<UccountDto> CreateUccount(UccountDto dto);
        Task<UccountDto> UpdateUccount(Guid id, UccountDto dto, string userId);
        Task<bool> DeleteUccount(Guid id);
        Task<bool> DeleteUccount(Guid[] ids);
        Task<bool> DeleteService(Guid id);

        Task<UccountServiceDto> GetService(Guid serviceId);
        Task<List<UccountServiceDto>> GetServices(Guid accountId);
        Task<UccountServiceDto> CreateService(UccountServiceDto dto);

        Task<string> DisplayPassword(Guid fieldId);
    }
    public class UccountBusinessManager: IUccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly IUccountManager _uccountManager;
        private readonly IUccountServiceManager _uccountServiceManager;
        private readonly IUccountServiceFieldManager _uccountServiceFieldManager;

        private readonly IUccountVendorFieldManager _uccountVendorFieldManager;

        private readonly UccountServiceGrantManager _uccountServiceGrantsManager;
        private readonly IAspNetUserDenyAccessCategoryManager _userCategoryGrantsManager;

        public UccountBusinessManager(IMapper mapper,
            IUccountManager uccountManager,
            IUccountServiceManager uccountServiceManager,
            IUccountServiceFieldManager uccountServiceFieldManager,
            IUccountVendorFieldManager uccountVendorFieldManager,
            UccountServiceGrantManager uccountServiceGrantsManager,
            IAspNetUserDenyAccessCategoryManager userCategoryGrantsManager) {
            _mapper = mapper;
            _uccountManager = uccountManager;
            _uccountServiceManager = uccountServiceManager;
            _uccountServiceFieldManager = uccountServiceFieldManager;
            _uccountVendorFieldManager = uccountVendorFieldManager;
            _uccountServiceGrantsManager = uccountServiceGrantsManager;
            _userCategoryGrantsManager = userCategoryGrantsManager;
        }

        #region UCCOUNT
        public async Task<UccountDto> GetUccount(Guid id) {
            var result = await _uccountManager.FindInclude(id);

            // Filter services by user
            await _uccountServiceGrantsManager.FilterByUser(result);

            // Decrypt vendor password field
            foreach(var field in result.Fields) {
                if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                    field.Value = field.Value.Decrypt();
                }
            }

            // Decrypt service password field
            foreach(var service in result.Services) {
                foreach(var field in service.Fields) {
                    if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                        field.Value = field.Value.Decrypt();
                    }
                }
            }

            return _mapper.Map<UccountDto>(result);
        }

        public async Task<PagerDto<UccountDto>> GetUccountPage(UccountFilterDto filter) {
            var sortby = "Id";

            Expression<Func<UccountEntity, bool>> where = x =>
                (string.IsNullOrEmpty(filter.Search)
                    || x.Person.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.Company.Name.ToLower().Contains(filter.Search.ToLower()))
                && (!filter.VendorId.HasValue || x.VendorId.Equals(filter.VendorId))
                && (!filter.Kind.HasValue || x.Kind == filter.Kind)
                && (!filter.CustomerId.HasValue || x.CompanyId == filter.CustomerId || x.PersonId == filter.CustomerId)
                && (!x.Company.Grants.Any(z => z.UserId == filter.UserId));

            string[] include = new string[] { "Company", "Person", "Vendor", "Services" };

            var tuple = await _uccountManager.Pager<UccountEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerDto<UccountDto>(new List<UccountDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            // Filter services by user
            await _uccountServiceGrantsManager.FilterByUser(list);

            var result = _mapper.Map<List<UccountDto>>(list);
            return new PagerDto<UccountDto>(result, count, page, filter.Length);
        }

        public async Task<List<UccountDto>> GetUccounts() {
            var result = await _uccountManager.All();

            return _mapper.Map<List<UccountDto>>(result);
        }

        public async Task<List<UccountDto>> GetUccountsInclude() {
            var result = await _uccountManager.FindAll();
            return _mapper.Map<List<UccountDto>>(result);
        }

        public async Task<List<UccountServiceDto>> GetServicesByCompanyId(Guid companyId) {
            var result = await _uccountManager.FindByCompany(companyId);
            return _mapper.Map<List<UccountServiceDto>>(result);
        }

        public async Task<UccountDto> CreateUccount(UccountDto dto) {
            //Encrypt vendor password field
            foreach(var field in dto.Fields) {
                if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                    field.Value = field.Value.Encrypt();
                }
            }

            //Encrypt service passwod field
            foreach(var service in dto.Services) {
                foreach(var field in service.Fields) {
                    if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                        field.Value = field.Value.Encrypt();
                    }
                }
            }

            var uccountEntity = await _uccountManager.Create(_mapper.Map<UccountEntity>(dto));

            return _mapper.Map<UccountDto>(uccountEntity);
        }

        public async Task<UccountDto> UpdateUccount(Guid id, UccountDto dto, string userId) {
            var entity = await _uccountManager.FindInclude(id, false);
            if(entity == null) {
                return null;
            }

            var oldEntityServices = entity.Services.ToList();

            //Encrypt vendor password field
            foreach(var field in dto.Fields) {
                if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                    field.Value = field.Value.Encrypt();
                }
            }

            //Encrypt service passwod field
            foreach(var service in dto.Services) {
                foreach(var field in service.Fields) {
                    if(field.Type == Data.Enums.FieldEnum.PASSWORD) {
                        field.Value = field.Value.Encrypt();
                    }
                }
            }

            _mapper.Map(dto, entity);

            //Filter denied services by user
            var intersectionServices = CompareExtension.Intersect<UccountServiceEntity, Guid>(oldEntityServices, entity.Services);
            var oldServices = CompareExtension.Exclude<UccountServiceEntity, Guid>(oldEntityServices, intersectionServices);
            var newServices = CompareExtension.Exclude<UccountServiceEntity, Guid>(entity.Services, intersectionServices);

            var deniedCategories = (await _userCategoryGrantsManager.FindByUserId(userId)).Select(x => x.CategoryId).ToHashSet();

            foreach(var oldDeniedService in oldServices.Where(x => x.CategoryId.HasValue && deniedCategories.Contains(x.CategoryId.Value))) {
                entity.Services.Add(oldDeniedService);
            }

            foreach(var newDeniedService in newServices.Where(x => x.CategoryId.HasValue && deniedCategories.Contains(x.CategoryId.Value))) {
                entity.Services.Remove(newDeniedService);
            }

            entity = await _uccountManager.Update(entity);
            return _mapper.Map<UccountDto>(entity);
        }

        public async Task<bool> DeleteUccount(Guid id) {
            return await DeleteUccount(new Guid[] { id });
        }

        public async Task<bool> DeleteUccount(Guid[] ids) {
            var entities = await _uccountManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _uccountManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteService(Guid id) {
            var entity = await _uccountServiceManager.FindInclude(id);
            if(entity == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _uccountServiceManager.Delete(entity);
            return result != 0;
        }
        #endregion

        #region UCCOUNT SERVICES
        public async Task<UccountServiceDto> GetService(Guid serviceId) {
            var result = await _uccountServiceManager.FindIncludePublicData(serviceId);
            return _mapper.Map<UccountServiceDto>(result);
        }

        public async Task<List<UccountServiceDto>> GetServices(Guid accountId) {
            var result = await _uccountServiceManager.FindAll(accountId);
            return _mapper.Map<List<UccountServiceDto>>(result);
        }

        public async Task<UccountServiceDto> CreateService(UccountServiceDto dto) {
            var result = await _uccountServiceManager.Create(_mapper.Map<UccountServiceEntity>(dto));
            return _mapper.Map<UccountServiceDto>(result);
        }
        #endregion

        public async Task<string> DisplayPassword(Guid fieldId) {
            var serviceField = await _uccountServiceFieldManager.Find(fieldId);
            if(serviceField != null && serviceField.Type == Data.Enums.FieldEnum.PASSWORD) {
                return serviceField.Value.Decrypt();
            }

            var vendorField = await _uccountVendorFieldManager.Find(fieldId);
            if(vendorField != null && vendorField.Type == Data.Enums.FieldEnum.PASSWORD) {
                return vendorField.Value.Decrypt();
            }
            return "";

        }
    }
}
