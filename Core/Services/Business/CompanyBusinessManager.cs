using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface ICompanyBusinessManager {
        //  COMPANY
        Task<CompanyDto> GetCompany(Guid id);
        Task<PagerDto<CompanyDto>> GetCompanyPage(PagerFilterDto filter);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> CreateCompany(CompanyDto dto);
        Task<CompanyDto> UpdateCompany(Guid id, CompanyDto dto);
        Task<bool> DeleteCompany(Guid id);
        Task<bool> DeleteCompany(Guid[] ids);

        // COMPANY DATA
        Task<List<CompanyDataDto>> GetCompanyData(Guid id);
        Task<CompanyDto> CreateCompanyData(CompanyDataListDto dto);
        Task<bool> DeleteCompanyData(Guid id);

        //  COMPANY SECTIONS
        //Task<CompanySectionDto> GetSection(long id);
        //Task<List<CompanySectionDto>> GetSections(Guid companyId);
        //Task<CompanySectionDto> CreateSection(CompanySectionDto dto);
        //Task<CompanySectionDto> UpdateSection(long id, CompanySectionDto dto);
        //Task<bool> DeleteSection(long id);

        ////  COMPANY SECTION FIELDS
        //Task<List<CompanySectionFieldDto>> GetSectionFields(long sectionId);
        //Task<CompanySectionFieldDto> GetSectionField(long id);
        //Task<CompanySectionFieldDto> CreateSectionField(CompanySectionFieldDto dto);
        //Task<CompanySectionFieldDto> UpdateSectionField(long id, CompanySectionFieldDto dto);
        //Task<bool> DeleteSectionField(long id);
        //Task<bool> DeleteSectionFields(long[] ids);
    }

    public class CompanyBusinessManager: ICompanyBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        // private readonly ICompanySectionManager _companySectionManager;
        // private readonly ICompanySectionFieldManager _companySectionFieldManager;
        private readonly ICompanyDataManager _companyDataManager;

        public CompanyBusinessManager(IMapper mapper,
        //    ISectionManager sectionManager,
           ICompanyManager companyManager,
        //    ICompanySectionManager companySectionManager,
        //    ICompanySectionFieldManager companySectionFieldManager,
           ICompanyDataManager companyDataManager) {
            _mapper = mapper;

            //_sectionManager = sectionManager;

            _companyManager = companyManager;
            // _companySectionManager = companySectionManager;
            // _companySectionFieldManager = companySectionFieldManager;
            _companyDataManager = companyDataManager;
        }

        #region COMPANY
        public async Task<CompanyDto> GetCompany(Guid id) {
            var result = await _companyManager.FindInclude(id);
            return _mapper.Map<CompanyDto>(result);
        }

        public async Task<PagerDto<CompanyDto>> GetCompanyPage(PagerFilterDto filter) {
            var sortby = "Id";

            Expression<Func<CompanyEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search)
                        || (x.Name.ToLower().Contains(filter.Search.ToLower())
                        || x.EIN.ToLower().Contains(filter.Search.ToLower())
                        || x.DB.ToLower().Contains(filter.Search.ToLower())
                        ));

            string[] include = new string[] { "CEO" };

            var tuple = await _companyManager.Pager<CompanyEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerDto<CompanyDto>(new List<CompanyDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<CompanyDto>>(list);
            return new PagerDto<CompanyDto>(result, count, page, filter.Length);
        }

        public async Task<List<CompanyDto>> GetCompanies() {
            var result = await _companyManager.FindAll();
            return _mapper.Map<List<CompanyDto>>(result);
        }

        public async Task<CompanyDto> CreateCompany(CompanyDto dto) {
            var entity = await _companyManager.Create(_mapper.Map<CompanyEntity>(dto));
            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<CompanyDto> UpdateCompany(Guid id, CompanyDto dto) {
            var entity = await _companyManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companyManager.Update(newEntity);

            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<bool> DeleteCompany(Guid id) {
            return await DeleteCompany(new Guid[] { id });
        }

        public async Task<bool> DeleteCompany(Guid[] ids) {
            var entities = await _companyManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _companyManager.Delete(entities);
            return result != 0;
        }
        #endregion

        #region COMPANY DATA
        public async Task<List<CompanyDataDto>> GetCompanyData(Guid id) {
            var result = await _companyDataManager.FindAllData(id);
            return _mapper.Map<List<CompanyDataDto>>(result);
        }

        public async Task<CompanyDto> CreateCompanyData(CompanyDataListDto dto) {
            var entity = await _companyManager.FindInclude(dto.CompanyId);
            if(entity == null)
                throw new Exception("We did not find company for this request!");

            entity.Data = (await _companyDataManager.Create(_mapper.Map<List<CompanyDataEntity>>(dto.Data))).ToList();
            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<bool> DeleteCompanyData(Guid id) {
            var entity = await _companyDataManager.FindInclude(id);
            if(entity == null)
                throw new Exception("We did not find data records for this request!");

            int result = await _companyDataManager.Delete(entity);
            return result != 0;
        }
        #endregion

        #region COMPANY SECTIONS
        //public async Task<CompanySectionDto> GetSection(long id) {
        //    var result = await _companySectionManager.FindInclude(id);
        //    return _mapper.Map<CompanySectionDto>(result);
        //}

        //public async Task<List<CompanySectionDto>> GetSections(Guid companyId) {
        //    var result = await _companySectionManager.FindAll(companyId);
        //    return _mapper.Map<List<CompanySectionDto>>(result);
        //}

        //public async Task<CompanySectionDto> CreateSection(CompanySectionDto dto) {
        //    var result = await _companySectionManager.Create(_mapper.Map<CompanySectionEntity>(dto));
        //    if(result != null) {
        //        var entity = await _companySectionManager.FindInclude(result.Id);
        //        return _mapper.Map<CompanySectionDto>(entity);
        //    }
        //    return null;
        //}

        //public async Task<CompanySectionDto> UpdateSection(long id, CompanySectionDto dto) {
        //    var entity = await _companySectionManager.Find(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var newEntity = _mapper.Map(dto, entity);
        //    entity = await _companySectionManager.Update(newEntity);

        //    return _mapper.Map<CompanySectionDto>(entity);
        //}

        //public async Task<bool> DeleteSection(long id) {
        //    var entity = await _companySectionManager.Find(id);

        //    if(entity != null) {
        //        var result = await _companySectionManager.Delete(entity);
        //        return result != 0;
        //    }

        //    return false;
        //}
        #endregion

        #region COMPANY SECTION FIELDS
        //public async Task<List<CompanySectionFieldDto>> GetSectionFields(long sectionId) {
        //    var result = await _companySectionFieldManager.FindAllBySectionId(sectionId);
        //    return _mapper.Map<List<CompanySectionFieldDto>>(result);
        //}

        //public async Task<CompanySectionFieldDto> GetSectionField(long id) {
        //    var result = await _companySectionFieldManager.Find(id);
        //    return _mapper.Map<CompanySectionFieldDto>(result);
        //}

        //public async Task<CompanySectionFieldDto> CreateSectionField(CompanySectionFieldDto dto) {
        //    var entity = await _companySectionFieldManager.Create(_mapper.Map<CompanySectionFieldEntity>(dto));
        //    return _mapper.Map<CompanySectionFieldDto>(entity);
        //}

        //public async Task<CompanySectionFieldDto> UpdateSectionField(long id, CompanySectionFieldDto dto) {
        //    var entity = await _companySectionFieldManager.Find(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var newEntity = _mapper.Map(dto, entity);
        //    entity = await _companySectionFieldManager.Update(newEntity);

        //    return _mapper.Map<CompanySectionFieldDto>(entity);
        //}

        //public async Task<bool> DeleteSectionField(long id) {
        //    return await DeleteSectionFields(new long[] { id });
        //}

        //public async Task<bool> DeleteSectionFields(long[] ids) {
        //    var entities = await _companySectionFieldManager.FindAll(ids);
        //    if(entities == null)
        //        throw new Exception("We did not find field records for this request!");

        //    int result = await _companySectionFieldManager.Delete(entities);
        //    return result != 0;
        //}
        #endregion
    }
}
