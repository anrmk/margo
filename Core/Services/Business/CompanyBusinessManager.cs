using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface ICompanyBusinessManager {
        //  COMPANY
        Task<CompanyDto> GetCompany(long id);
        Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> CreateCompany(CompanyGeneralDto dto);
        Task<CompanyDto> UpdateCompany(long id, CompanyGeneralDto dto);
        Task<bool> DeleteCompany(long id);
        Task<bool> DeleteCompany(long[] ids);

        //  COMPANY ADDRESS
        Task<CompanyAddressDto> GetAddress(long id);
        Task<CompanyAddressDto> CreateAddress(CompanyAddressDto dto);
        Task<CompanyAddressDto> UpdateAddress(long companyId, CompanyAddressDto dto);

        //  COMPANY SECTIONS
        Task<CompanySectionDto> GetSection(long id);
        Task<List<CompanySectionDto>> GetSections(long companyId);
        Task<CompanySectionDto> CreateSection(CompanySectionDto dto);
        Task<CompanySectionDto> UpdateSection(long id, CompanySectionDto dto);
        Task<bool> DeleteSection(long id);

        //  COMPANY SECTION FIELDS
        Task<List<CompanySectionFieldDto>> GetSectionFields(long sectionId);
        Task<CompanySectionFieldDto> GetSectionField(long id);
        Task<CompanySectionFieldDto> CreateSectionField(CompanySectionFieldDto dto);
        Task<CompanySectionFieldDto> UpdateSectionField(long id, CompanySectionFieldDto dto);
        Task<bool> DeleteSectionField(long id);
        Task<bool> DeleteSectionFields(long[] ids);
    }

    public class CompanyBusinessManager: ICompanyBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        private readonly ICompanyAddressManager _companyAddressManager;
        private readonly ICompanySectionManager _companySectionManager;
        private readonly ICompanySectionFieldManager _companySectionFieldManager;

        public CompanyBusinessManager(IMapper mapper,
           ISectionManager sectionManager,
           ICompanyManager companyManager,
           ICompanyAddressManager companyAddressManager,
           ICompanySectionManager companySectionManager,
           ICompanySectionFieldManager companySectionFieldManager) {
            _mapper = mapper;

            //_sectionManager = sectionManager;

            _companyManager = companyManager;
            _companyAddressManager = companyAddressManager;
            _companySectionManager = companySectionManager;
            _companySectionFieldManager = companySectionFieldManager;
        }

        #region COMPANY
        public async Task<CompanyDto> GetCompany(long id) {
            var result = await _companyManager.FindInclude(id);
            return _mapper.Map<CompanyDto>(result);
        }

        public async Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter) {
            var sortby = "Name";

            Expression<Func<CompanyEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));

            string[] include = new string[] { "Address" };

            var tuple = await _companyManager.Pager<CompanyEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<CompanyDto>(new List<CompanyDto>(), 0, filter.Length, filter.Start);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<CompanyDto>>(list);
            return new Pager<CompanyDto>(result, count, page, filter.Length);
        }

        public async Task<List<CompanyDto>> GetCompanies() {
            var result = await _companyManager.FindAll();
            return _mapper.Map<List<CompanyDto>>(result);
        }

        public async Task<CompanyDto> CreateCompany(CompanyGeneralDto dto) {
            var entity = await _companyManager.Create(_mapper.Map<CompanyEntity>(dto));
            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<CompanyDto> UpdateCompany(long id, CompanyGeneralDto dto) {
            var entity = await _companyManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companyManager.Update(newEntity);

            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<bool> DeleteCompany(long id) {
            return await DeleteCompany(new long[] { id });
        }

        public async Task<bool> DeleteCompany(long[] ids) {
            var entities = await _companyManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _companyManager.Delete(entities);
            return result != 0;
        }
        #endregion

        #region COMPANY ADRESS
        public async Task<CompanyAddressDto> GetAddress(long id) {
            var result = await _companyAddressManager.Find(id);
            return _mapper.Map<CompanyAddressDto>(result);
        }

        public async Task<CompanyAddressDto> CreateAddress(CompanyAddressDto dto) {
            var settings = await _companyAddressManager.Find(dto.Id);
            if(settings == null) {
                return null;
            }

            var newEntity = _mapper.Map<CompanyAddressEntity>(dto);
            var entity = await _companyAddressManager.Create(newEntity);
            return _mapper.Map<CompanyAddressDto>(entity);
        }

        public async Task<CompanyAddressDto> UpdateAddress(long companyId, CompanyAddressDto dto) {
            var entity = await _companyAddressManager.Find(dto.Id);

            if(entity == null) {
                entity = await _companyAddressManager.Create(_mapper.Map<CompanyAddressEntity>(dto));

                var company = await _companyManager.Find(companyId);
                company.AddressId = entity.Id;
                await _companyManager.Update(company);
            } else {
                var updateEntity = _mapper.Map(dto, entity);
                entity = await _companyAddressManager.Update(updateEntity);
            }

            return _mapper.Map<CompanyAddressDto>(entity);
        }
        #endregion

        #region COMPANY SECTIONS
        public async Task<CompanySectionDto> GetSection(long id) {
            var result = await _companySectionManager.FindInclude(id);
            return _mapper.Map<CompanySectionDto>(result);
        }

        public async Task<List<CompanySectionDto>> GetSections(long companyId) {
            var result = await _companySectionManager.FindAll(companyId);
            return _mapper.Map<List<CompanySectionDto>>(result);
        }

        public async Task<CompanySectionDto> CreateSection(CompanySectionDto dto) {
            var result = await _companySectionManager.Create(_mapper.Map<CompanySectionEntity>(dto));
            if(result != null) {
                var entity = await _companySectionManager.FindInclude(result.Id);
                return _mapper.Map<CompanySectionDto>(entity);
            }
            return null;
        }

        public async Task<CompanySectionDto> UpdateSection(long id, CompanySectionDto dto) {
            var entity = await _companySectionManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companySectionManager.Update(newEntity);

            return _mapper.Map<CompanySectionDto>(entity);
        }

        public async Task<bool> DeleteSection(long id) {
            var entity = await _companySectionManager.Find(id);

            if(entity != null) {
                var result = await _companySectionManager.Delete(entity);
                return result != 0;
            }

            return false;
        }
        #endregion

        #region COMPANY SECTION FIELDS
        public async Task<List<CompanySectionFieldDto>> GetSectionFields(long sectionId) {
            var result = await _companySectionFieldManager.FindAllBySectionId(sectionId);
            return _mapper.Map<List<CompanySectionFieldDto>>(result);
        }

        public async Task<CompanySectionFieldDto> GetSectionField(long id) {
            var result = await _companySectionFieldManager.Find(id);
            return _mapper.Map<CompanySectionFieldDto>(result);
        }

        public async Task<CompanySectionFieldDto> CreateSectionField(CompanySectionFieldDto dto) {
            var entity = await _companySectionFieldManager.Create(_mapper.Map<CompanySectionFieldEntity>(dto));
            return _mapper.Map<CompanySectionFieldDto>(entity);
        }

        public async Task<CompanySectionFieldDto> UpdateSectionField(long id, CompanySectionFieldDto dto) {
            var entity = await _companySectionFieldManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companySectionFieldManager.Update(newEntity);

            return _mapper.Map<CompanySectionFieldDto>(entity);
        }

        public async Task<bool> DeleteSectionField(long id) {
            return await DeleteSectionFields(new long[] { id });
        }

        public async Task<bool> DeleteSectionFields(long[] ids) {
            var entities = await _companySectionFieldManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _companySectionFieldManager.Delete(entities);
            return result != 0;
        }
        #endregion
    }
}
