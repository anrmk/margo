using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IUccountBusinessManager {
        //  UCCOUNT
        Task<UccountDto> GetUccount(Guid id);
        Task<PagerDto<UccountDto>> GetUccountPage(UccountFilterDto filter);
        Task<List<UccountDto>> GetUccounts();
        Task<List<UccountDto>> GetUccountsInclude();
        Task<UccountDto> CreateUccount(UccountDto dto);
        Task<UccountDto> UpdateUccount(Guid id, UccountDto dto);
        Task<bool> DeleteUccount(Guid id);
        Task<bool> DeleteUccount(Guid[] ids);
        Task<bool> DeleteService(Guid id);

        //  UCCOUNT SECTION 
        //Task<UccountSectionDto> GetSection(long id);

        //Task<UccountSectionFieldDto> GetSectionField(long id);
        //Task<List<UccountSectionFieldDto>> GetSectionFields(long sectionId);

        Task<List<UccountServiceDto>> GetServices(Guid accountId);
        Task<UccountServiceDto> CreateService(UccountServiceDto dto);
    }
    public class UccountBusinessManager: IUccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly IUccountManager _uccountManager;
        //private readonly IUccountSectionManager _uccountSectionManager;
        //private readonly IUccountSectionFieldManager _uccountSectionFieldManager;
        private readonly IUccountServiceManager _uccountServiceManager;
        private readonly IUccountServiceFieldManager _uccountServiceFieldManager;

        private readonly ICompanyManager _companyManager;
        //private readonly ISectionManager _sectionManager;
        private readonly IVendorManager _vendorManager;

        public UccountBusinessManager(IMapper mapper,
            IUccountManager uccountManager,
            //IUccountSectionManager uccountSectionManager,
            //IUccountSectionFieldManager uccountSectionFieldManager,
            IUccountServiceManager uccountServiceManager,
            IUccountServiceFieldManager uccountServiceFieldManager,
            ICompanyManager companyManager,
            //ISectionManager sectionManager,
            IVendorManager vendorManager) {
            _mapper = mapper;
            _uccountManager = uccountManager;
            //_uccountSectionManager = uccountSectionManager;
            //_uccountSectionFieldManager = uccountSectionFieldManager;
            _uccountServiceManager = uccountServiceManager;
            _uccountServiceFieldManager = uccountServiceFieldManager;
            _companyManager = companyManager;
            //_sectionManager = sectionManager;
            _vendorManager = vendorManager;
        }

        #region UCCOUNT

        public async Task<UccountDto> GetUccount(Guid id) {
            var result = await _uccountManager.FindInclude(id);
            return _mapper.Map<UccountDto>(result);
        }

        public async Task<PagerDto<UccountDto>> GetUccountPage(UccountFilterDto filter) {
            var sortby = "Id";

            Expression<Func<UccountEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search)
                        || ( x.Person.Name.ToLower().Contains(filter.Search.ToLower()))
                        || (x.Company.Name.ToLower().Contains(filter.Search.ToLower()))
                        )
                   && (!filter.VendorId.HasValue || x.VendorId.Equals(filter.VendorId))
                   && (!filter.Kind.HasValue || x.Kind == filter.Kind)
                   && (!filter.CustomerId.HasValue || x.CompanyId == filter.CustomerId || x.PersonId == filter.CustomerId)
                   ;

            string[] include = new string[] { "Company", "Person", "Vendor", "Services" };

            var tuple = await _uccountManager.Pager<UccountEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerDto<UccountDto>(new List<UccountDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

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

        public async Task<UccountDto> CreateUccount(UccountDto dto) {
            var uccountEntity = await _uccountManager.Create(_mapper.Map<UccountEntity>(dto));

            return _mapper.Map<UccountDto>(uccountEntity);
        }

        public async Task<UccountDto> UpdateUccount(Guid id, UccountDto dto) {
            var entity = await _uccountManager.FindInclude(id);

            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _uccountManager.Update(newEntity);

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
        /*
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
        }*/

        public async Task<List<UccountServiceDto>> GetServices(Guid accountId) {
            var result = await _uccountServiceManager.FindAll(accountId);
            return _mapper.Map<List<UccountServiceDto>>(result);
        }

        public async Task<UccountServiceDto> CreateService(UccountServiceDto dto) {
            var result = await _uccountServiceManager.Create(_mapper.Map<UccountServiceEntity>(dto));
            return _mapper.Map<UccountServiceDto>(result);
        }
    }
}
