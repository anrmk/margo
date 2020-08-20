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
        Task<PagerDto<CompanyDto>> GetCompanyPage(CompanyFilterDto filter);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> CreateCompany(CompanyDto dto);
        Task<CompanyDto> UpdateCompany(Guid id, CompanyDto dto);
        Task<bool> DeleteCompany(Guid id);
        Task<bool> DeleteCompany(Guid[] ids);
        Task<bool> DeleteSection(Guid id);

        // COMPANY DATA
        Task<List<CompanyDataDto>> GetCompanyData(Guid id);
        Task<CompanyDto> CreateCompanyData(CompanyDataListDto dto);
        Task<bool> DeleteCompanyData(Guid id);
    }

    public class CompanyBusinessManager: ICompanyBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        private readonly ICompanySectionManager _companySectionManager;
        private readonly ICompanyDataManager _companyDataManager;

        public CompanyBusinessManager(IMapper mapper,
           ICompanyManager companyManager,
           ICompanySectionManager companySectionManager,
           ICompanyDataManager companyDataManager) {
            _mapper = mapper;
            _companyManager = companyManager;
            _companySectionManager = companySectionManager;
            _companyDataManager = companyDataManager;
        }

        #region COMPANY
        public async Task<CompanyDto> GetCompany(Guid id) {
            var result = await _companyManager.FindInclude(id);
            return _mapper.Map<CompanyDto>(result);
        }

        public async Task<PagerDto<CompanyDto>> GetCompanyPage(CompanyFilterDto filter) {
            var sortby = "Id";

            Expression<Func<CompanyEntity, bool>> where = x =>
                (string.IsNullOrEmpty(filter.Search)
                    || (x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.EIN.ToLower().Contains(filter.Search.ToLower())
                    || x.DB.ToLower().Contains(filter.Search.ToLower())
                    || x.CEO.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.CEO.SurName.ToLower().Contains(filter.Search.ToLower())
                    ))
                && (!filter.CEOId.HasValue || x.CEOId == filter.CEOId)
                && (!x.Grants.Any(z => z.UserId == filter.UserId));

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
            var entity = await _companyManager.FindInclude(id);
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

        public async Task<bool> DeleteSection(Guid id) {
            var entity = await _companySectionManager.FindInclude(id);
            if(entity == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _companySectionManager.Delete(entity);
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
    }
}
