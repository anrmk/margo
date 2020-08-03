using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Data.Enums;
using Core.Extension;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IUccountBusinessManager {
        //  UCCOUNT
        Task<UccountDto> GetUccount(long id);
        Task<Pager<UccountDto>> GetUccountPage(PagerFilter filter);
        Task<List<UccountDto>> GetUccounts();
        Task<List<UccountDto>> GetUccountsInclude();
        Task<UccountDto> CreateUccount(UccountDto dto);
        Task<UccountDto> UpdateUccount(long id, UccountDto dto);
        Task<bool> DeleteUccount(long id);
        Task<bool> DeleteUccount(long[] ids);

        //  UCCOUNT SECTION 
        Task<UccountSectionDto> GetSection(long id);

        Task<UccountSectionFieldDto> GetSectionField(long id);
        Task<List<UccountSectionFieldDto>> GetSectionFields(long sectionId);

        Task<List<UccountServiceDto>> GetServices(long accountId);
        Task<UccountServiceDto> CreateService(UccountServiceDto dto);
    }
    public class UccountBusinessManager: IUccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly IUccountManager _uccountManager;
        private readonly IUccountSectionManager _uccountSectionManager;
        private readonly IUccountSectionFieldManager _uccountSectionFieldManager;
        private readonly IUccountServiceManager _uccountServiceManager;
        private readonly IUccountServiceFieldManager _uccountServiceFieldManager;

        private readonly ICompanyManager _companyManager;
        private readonly ISectionManager _sectionManager;
        private readonly IVendorManager _vendorManager;

        public UccountBusinessManager(IMapper mapper,
            IUccountManager uccountManager,
            IUccountSectionManager uccountSectionManager,
            IUccountSectionFieldManager uccountSectionFieldManager,
            IUccountServiceManager uccountServiceManager,
            IUccountServiceFieldManager uccountServiceFieldManager,
            ICompanyManager companyManager,
            ISectionManager sectionManager,
            IVendorManager vendorManager) {
            _mapper = mapper;
            _uccountManager = uccountManager;
            _uccountSectionManager = uccountSectionManager;
            _uccountSectionFieldManager = uccountSectionFieldManager;
            _uccountServiceManager = uccountServiceManager;
            _uccountServiceFieldManager = uccountServiceFieldManager;
            _companyManager = companyManager;
            _sectionManager = sectionManager;
            _vendorManager = vendorManager;
        }

        #region UCCOUNT

        public async Task<UccountDto> GetUccount(long id) {
            var result = await _uccountManager.FindInclude(id);
            return _mapper.Map<UccountDto>(result);
        }

        public async Task<Pager<UccountDto>> GetUccountPage(PagerFilter filter) {
            var sortby = "Id";

            Expression<Func<UccountEntity, bool>> where = x =>
                   (true);
            //&& (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));

            string[] include = new string[] { "Company", "Person", "Vendor", "Services" };

            var tuple = await _uccountManager.Pager<UccountEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<UccountDto>(new List<UccountDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<UccountDto>>(list);
            return new Pager<UccountDto>(result, count, page, filter.Length);
        }

        public async Task<List<UccountDto>> GetUccounts() {
            var result = await _uccountManager.All();
            return _mapper.Map<List<UccountDto>>(result);
        }

        public async Task<List<UccountDto>> GetUccountsInclude() {
            var result = await _uccountManager.FindAll();
            return _mapper.Map<List<UccountDto>>(result);
        }

        public async Task<UccountDto> CreateUccount(UccountDto dto) {
            var uccountEntity = await _uccountManager.Create(_mapper.Map<UccountEntity>(dto));

            return _mapper.Map<UccountDto>(uccountEntity);
        }

        public async Task<UccountDto> UpdateUccount(long id, UccountDto dto) {
            var entity = await _uccountManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _uccountManager.Update(newEntity);
            entity.Services = new List<UccountServiceEntity>();

            foreach(var service in dto.Services) {
                var serviceEntity = await _uccountServiceManager.Find(id);
                if(serviceEntity == null) {
                    continue;
                }

                var newServiceEntity = _mapper.Map(service, serviceEntity);
                entity.Services.Add(await _uccountServiceManager.Update(newServiceEntity));
            }

            return _mapper.Map<UccountDto>(entity);
        }

        public async Task<bool> DeleteUccount(long id) {
            return await DeleteUccount(new long[] { id });
        }

        public async Task<bool> DeleteUccount(long[] ids) {
            var entities = await _uccountManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _uccountManager.Delete(entities);
            return result != 0;
        }


        #endregion

        public async Task<UccountSectionDto> GetSection(long id) {
            var result = await _uccountSectionManager.Find(id);
            return _mapper.Map<UccountSectionDto>(result);
        }

        public async Task<UccountSectionFieldDto> GetSectionField(long id) {
            var result = await _uccountSectionFieldManager.Find(id);
            return _mapper.Map<UccountSectionFieldDto>(result);
        }

        public async Task<List<UccountSectionFieldDto>> GetSectionFields(long sectionId) {
            var result = await _uccountSectionFieldManager.FindAll(sectionId);
            return _mapper.Map<List<UccountSectionFieldDto>>(result);
        }

        public async Task<List<UccountServiceDto>> GetServices(long accountId) {
            var result = await _uccountServiceManager.FindAll(accountId);
            return _mapper.Map<List<UccountServiceDto>>(result);
        }

        public async Task<UccountServiceDto> CreateService(UccountServiceDto dto) {
            var result = await _uccountServiceManager.Create(_mapper.Map<UccountServiceEntity>(dto));
            return _mapper.Map<UccountServiceDto>(result);
        }
    }
}
