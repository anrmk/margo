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
    public interface ICrudBusinessManager {
        #region COMPANY
        Task<CompanyDto> GetCompany(long id);
        Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> CreateCompany(CompanyGeneralDto dto);
        Task<CompanyDto> UpdateCompany(long id, CompanyGeneralDto dto);
        Task<bool> DeleteCompany(long id);
        #endregion

        #region COMPANY ADDRESS
        Task<CompanyAddressDto> GetCompanyAddress(long id);
        Task<CompanyAddressDto> CreateCompanyAddress(CompanyAddressDto dto);
        Task<CompanyAddressDto> UpdateCompanyAddress(long companyId, CompanyAddressDto dto);
        #endregion

        #region VENDOR
        Task<VendorDto> GetVendor(long id);
        Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter);
        Task<List<VendorDto>> GetVendors();
        Task<VendorDto> CreateVendor(VendorGeneralDto dto);
        Task<VendorDto> UpdateVendor(long id, VendorGeneralDto dto);
        Task<bool> DeleteVendor(long id);
        #endregion

        #region VENDOR ADDRESS
        Task<VendorAddressDto> GetVendorAddress(long id);
        Task<VendorAddressDto> UpdateVendorAddress(long companyId, VendorAddressDto dto);
        #endregion

        //#region VACCOUNT
        //Task<VaccountDto> GetVaccount(long id);
        //Task<VaccountDto> GetVaccountBySecurityId(long id);
        //Task<List<VaccountDto>> GetVaccounts();
        //Task<Pager<VaccountDto>> GetVaccountPager(VaccountFilterDto filter);
        //Task<VaccountDto> CreateVaccount(VaccountDto dto);
        //Task<VaccountDto> UpdateVaccount(long id, VaccountDto dto);
        //Task<bool> DeleteVaccount(long id);
        //#endregion

        //#region VACCOUNT SECURITY
        //Task<VaccountSecurityDto> GetVaccountSecurity(long id);
        //Task<VaccountSecurityDto> UpdateVaccountSecurity(long accountId, VaccountSecurityDto dto);
        //#endregion

        //#region VACCOUNT SECURITY QUESTION
        //Task<VaccountSecurityQuestionDto> GetVaccountSecurityQuestion(long id);
        //Task<List<VaccountSecurityQuestionDto>> GetVaccountSecurityQuestions(long securityId);
        //Task<VaccountSecurityQuestionDto> CreateVaccountSecurityQuestion(VaccountSecurityQuestionDto dto);
        //Task<VaccountSecurityQuestionDto> UpdateVaccountSecurityQuestion(long id, VaccountSecurityQuestionDto dto);
        //#endregion

        Task<InvoiceDto> GetInvoice(long id);
        Task<List<InvoiceDto>> GetInvoices();
        Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter);
        Task<InvoiceDto> CreateInvoice(InvoiceDto dto);
        Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto);
        Task<bool> DeleteInvoice(long id);

        #region COMPANY SECTIONS
        Task<CompanySectionDto> GetCompanySection(long id);
        Task<List<CompanySectionDto>> GetCompanySections(long companyId);
        Task<CompanySectionDto> CreateCompanySection(CompanySectionDto dto);
        Task<CompanySectionDto> UpdateCompanySection(long id, CompanySectionDto dto);
        Task<bool> DeleteCompanySection(long id);
        #endregion

        #region COMPANY SECTION FIELDS
        Task<CompanySectionFieldDto> GetCompanySectionField(long id);
        Task<CompanySectionFieldDto> CreateCompanySectionField(CompanySectionFieldDto dto);
        Task<CompanySectionFieldDto> UpdateCompanySectionField(long id, CompanySectionFieldDto dto);
        Task<bool> DeleteCompanySectionField(long id);
        #endregion

        #region SECTION
        Task<SectionDto> GetSection(long id);
        Task<Pager<SectionDto>> GetSectionPage(PagerFilter filter);
        Task<List<SectionDto>> GetSections();
        Task<SectionDto> CreateSection(SectionDto dto);
        Task<SectionDto> UpdateSection(long id, SectionDto dto);
        Task<bool> DeleteSection(long id);
        #endregion
    }

    public class CrudBusinessManager: BaseBusinessManager, ICrudBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        private readonly ICompanyAddressManager _companyAddressManager;
        private readonly ICompanySectionManager _companySectionManager;
        private readonly ICompanySectionFieldManager _companySectionFieldManager;

        private readonly IVendorManager _vendorManager;
        private readonly IVendorAddressManager _vendorAddressManager;

        //private readonly IVaccountManager _vaccountManager;
        //private readonly IVaccountSecurityManager _vaccountSecurityManager;
        //private readonly IVaccountSecurityQuestionManager _vaccountSecurityQuestionManager;

        private readonly IInvoiceManager _invoiceManager;

        //private readonly INsiBusinessManager _nsiBusinessManager;

        private readonly ISectionManager _sectionManager;


        public CrudBusinessManager(IMapper mapper,
            ISectionManager sectionManager,
            ICompanyManager companyManager, ICompanyAddressManager companyAddressManager, ICompanySectionManager companySectionManager, ICompanySectionFieldManager companySectionFieldManager,
            IVendorManager supplierManager, IVendorAddressManager vendorAddressManager,
            //IVaccountManager vaccountManager, IVaccountSecurityManager vaccountSecurityManager, IVaccountSecurityQuestionManager vaccountSecurityQuestionManager,
            IInvoiceManager invoiceManager) {
            _mapper = mapper;

            _sectionManager = sectionManager;

            _companyManager = companyManager;
            _companyAddressManager = companyAddressManager;
            _companySectionManager = companySectionManager;
            _companySectionFieldManager = companySectionFieldManager;

            _vendorManager = supplierManager;
            _vendorAddressManager = vendorAddressManager;

            //_vaccountManager = vaccountManager;
            //_vaccountSecurityManager = vaccountSecurityManager;
            //_vaccountSecurityQuestionManager = vaccountSecurityQuestionManager;

            _invoiceManager = invoiceManager;
        }

        #region COMPANY
        public async Task<CompanyDto> GetCompany(long id) {
            var result = await _companyManager.FindInclude(id);
            return _mapper.Map<CompanyDto>(result);
        }

        public async Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter) {
            #region Sort/Filter
            var sortby = filter.Sort ?? "Name";

            Expression<Func<CompanyEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));
            #endregion

            string[] include = new string[] { "Address" };

            var tuple = await _companyManager.Pager<CompanyEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<CompanyDto>(new List<CompanyDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<CompanyDto>>(list);
            return new Pager<CompanyDto>(result, count, page, filter.Limit);
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
            var result = 0;
            var entity = await _companyManager.Find(id);

            if(entity != null) {
                result = await _companyManager.Delete(entity);
            }

            var address = await _companyAddressManager.Find(entity.AddressId);
            if(address != null) {
                result = await _companyAddressManager.Delete(address);
            }

            return result != 0;
        }
        #endregion

        #region COMPANY ADRESS
        public async Task<CompanyAddressDto> GetCompanyAddress(long id) {
            var result = await _companyAddressManager.Find(id);
            return _mapper.Map<CompanyAddressDto>(result);
        }

        public async Task<CompanyAddressDto> CreateCompanyAddress(CompanyAddressDto dto) {
            var settings = await _companyAddressManager.Find(dto.Id);
            if(settings == null) {
                return null;
            }

            var newEntity = _mapper.Map<CompanyAddressEntity>(dto);
            var entity = await _companyAddressManager.Create(newEntity);
            return _mapper.Map<CompanyAddressDto>(entity);
        }

        public async Task<CompanyAddressDto> UpdateCompanyAddress(long companyId, CompanyAddressDto dto) {
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

        #region VENDOR
        public async Task<VendorDto> GetVendor(long id) {
            var result = await _vendorManager.FindInclude(id);
            return _mapper.Map<VendorDto>(result);
        }

        public async Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter) {
            #region Sort/Filter
            var sortby = filter.Sort ?? "No";

            Expression<Func<VendorEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.No.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));
            #endregion

            string[] include = new string[] { "Address" };

            var tuple = await _vendorManager.Pager<VendorEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<VendorDto>(new List<VendorDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<VendorDto>>(list);
            return new Pager<VendorDto>(result, count, page, filter.Limit);
        }

        public async Task<List<VendorDto>> GetVendors() {
            var result = await _vendorManager.FindAll();
            return _mapper.Map<List<VendorDto>>(result);
        }

        public async Task<VendorDto> CreateVendor(VendorGeneralDto dto) {
            var entity = await _vendorManager.Create(_mapper.Map<VendorEntity>(dto));
            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<VendorDto> UpdateVendor(long id, VendorGeneralDto dto) {
            var entity = await _vendorManager.Find(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _vendorManager.Update(newEntity);

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<bool> DeleteVendor(long id) {
            var result = 0;
            var entity = await _vendorManager.Find(id);

            if(entity == null) {
                result = await _vendorManager.Delete(entity);
            }

            var address = await _vendorAddressManager.Find(entity.AddressId);
            if(address != null) {
                result = await _vendorAddressManager.Delete(address);
            }

            return result != 0;
        }
        #endregion

        #region VENDOR ADRESS
        public async Task<VendorAddressDto> GetVendorAddress(long id) {
            var result = await _vendorAddressManager.Find(id);
            return _mapper.Map<VendorAddressDto>(result);
        }

        public async Task<VendorAddressDto> UpdateVendorAddress(long supplierId, VendorAddressDto dto) {
            var entity = await _vendorAddressManager.Find(dto.Id);

            if(entity == null) {
                entity = await _vendorAddressManager.Create(_mapper.Map<VendorAddressEntity>(dto));

                var supplier = await _vendorManager.Find(supplierId);
                supplier.AddressId = entity.Id;
                await _vendorManager.Update(supplier);
            } else {
                var updateEntity = _mapper.Map(dto, entity);
                entity = await _vendorAddressManager.Update(updateEntity);
            }

            return _mapper.Map<VendorAddressDto>(entity);
        }
        #endregion

        #region INVOICE
        public async Task<InvoiceDto> GetInvoice(long id) {
            var result = await _invoiceManager.FindInclude(id);
            return _mapper.Map<InvoiceDto>(result);
        }

        public async Task<List<InvoiceDto>> GetInvoices() {
            var result = await _invoiceManager.FindAll();
            return _mapper.Map<List<InvoiceDto>>(result);
        }

        public async Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
            #region Sort/Filter
            var sortby = filter.Sort ?? "No";

            Expression<Func<InvoiceEntity, bool>> where = x =>
                  (true)
               && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) ))
               ;
            #endregion

            string[] include = new string[] { };

            var tuple = await _invoiceManager.Pager<InvoiceEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<InvoiceDto>(new List<InvoiceDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<InvoiceDto>>(list);
            return new Pager<InvoiceDto>(result, count, page, filter.Limit);
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceDto dto) {
            var entity = _mapper.Map<InvoiceEntity>(dto);
            entity = await _invoiceManager.Create(entity);
            return _mapper.Map<InvoiceDto>(entity);
        }

        public async Task<bool> DeleteInvoice(long id) {
            var entity = await _invoiceManager.Find(id);
            if(entity == null) {
                return false;
            }
            int result = await _invoiceManager.Delete(entity);
            return result != 0;
        }

        public async Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto) {
            var entity = await _invoiceManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var entity1 = _mapper.Map(dto, entity);
            entity = await _invoiceManager.Update(entity1);
            return _mapper.Map<InvoiceDto>(entity);
        }
        #endregion

        #region VACCOUNT
        //public async Task<VaccountDto> GetVaccount(long id) {
        //    var result = await _vaccountManager.FindInclude(id);
        //    return _mapper.Map<VaccountDto>(result);
        //}

        //public async Task<VaccountDto> GetVaccountBySecurityId(long id) {
        //    var result = await _vaccountManager.FindBySecurityId(id);
        //    return _mapper.Map<VaccountDto>(result);
        //}

        //public async Task<Pager<VaccountDto>> GetVaccountPager(VaccountFilterDto filter) {
        //    var sortby = filter.Sort ?? "Id";

        //    Expression<Func<VaccountEntity, bool>> where = x =>
        //          (true)
        //       && (string.IsNullOrEmpty(filter.Search) || (x.UserName.ToLower().Contains(filter.Search.ToLower())))
        //       && ((filter.CompanyId == null) || filter.CompanyId == x.CompanyId)
        //       && ((filter.VendorId == null) || filter.VendorId == x.VendorId)
        //       ;

        //    string[] include = new string[] { "Company", "Vendor" };

        //    var tuple = await _vaccountManager.Pager<VaccountEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit, include);
        //    var list = tuple.Item1;
        //    var count = tuple.Item2;

        //    if(count == 0)
        //        return new Pager<VaccountDto>(new List<VaccountDto>(), 0, filter.Offset, filter.Limit);

        //    var page = (filter.Offset + filter.Limit) / filter.Limit;

        //    var result = _mapper.Map<List<VaccountDto>>(list);
        //    return new Pager<VaccountDto>(result, count, page, filter.Limit);
        //}

        //public async Task<List<VaccountDto>> GetVaccounts() {
        //    var result = await _vaccountManager.FindAll();
        //    return _mapper.Map<List<VaccountDto>>(result);
        //}

        //public async Task<VaccountDto> CreateVaccount(VaccountDto dto) {
        //    var entity = _mapper.Map<VaccountEntity>(dto);
        //    entity = await _vaccountManager.Create(entity);
        //    return _mapper.Map<VaccountDto>(entity);
        //}

        //public async Task<VaccountDto> UpdateVaccount(long id, VaccountDto dto) {
        //    var entity = await _vaccountManager.Find(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var newEntity = _mapper.Map(dto, entity);
        //    entity = await _vaccountManager.Update(newEntity);

        //    return _mapper.Map<VaccountDto>(entity);
        //}

        //public async Task<bool> DeleteVaccount(long id) {
        //    var result = 0;
        //    var entity = await _vaccountManager.Find(id);

        //    if(entity != null) {
        //        result = await _vaccountManager.Delete(entity);
        //    }

        //    return result != 0;
        //}
        #endregion

        #region VACCOUNT SECURITY
        //public async Task<VaccountSecurityDto> GetVaccountSecurity(long id) {
        //    var result = await _vaccountSecurityManager.FindInclude(id);
        //    return _mapper.Map<VaccountSecurityDto>(result);
        //}

        //public async Task<VaccountSecurityDto> CreateVaccountSecurity(VaccountSecurityDto dto) {
        //    var accountEntity = await _vaccountManager.Find(dto.AccountId);
        //    if(accountEntity == null) {
        //        return null;
        //    }

        //    var newEntity = _mapper.Map<VaccountSecurityEntity>(dto);
        //    var entity = await _vaccountSecurityManager.Create(newEntity);
        //    return _mapper.Map<VaccountSecurityDto>(entity);
        //}

        //public async Task<VaccountSecurityDto> UpdateVaccountSecurity(long accountId, VaccountSecurityDto dto) {
        //    var entity = await _vaccountSecurityManager.Find(dto.Id);

        //    if(entity == null) {
        //        entity = await _vaccountSecurityManager.Create(_mapper.Map<VaccountSecurityEntity>(dto));

        //        var account = await _vaccountManager.Find(accountId);
        //        account.SecurityId = entity.Id;
        //        await _vaccountManager.Update(account);
        //    } else {
        //        var updateEntity = _mapper.Map(dto, entity);
        //        entity = await _vaccountSecurityManager.Update(updateEntity);
        //    }

        //    return _mapper.Map<VaccountSecurityDto>(entity);
        //}
        #endregion

        #region VACCOUNT SECURITY QUESTION
        //public async Task<VaccountSecurityQuestionDto> GetVaccountSecurityQuestion(long id) {
        //    var result = await _vaccountSecurityQuestionManager.Find(id);
        //    return _mapper.Map<VaccountSecurityQuestionDto>(result);
        //}

        //public async Task<List<VaccountSecurityQuestionDto>> GetVaccountSecurityQuestions(long securityId) {
        //    var result = await _vaccountSecurityQuestionManager.FindBySecurityId(securityId);
        //    return _mapper.Map<List<VaccountSecurityQuestionDto>>(result);
        //}

        //public async Task<VaccountSecurityQuestionDto> CreateVaccountSecurityQuestion(VaccountSecurityQuestionDto dto) {
        //    var securityEntity = await _vaccountSecurityManager.Find(dto.SecurityId);
        //    if(securityEntity == null) {
        //        return null;
        //    }

        //    var newEntity = _mapper.Map<VaccountSecurityQuestionEntity>(dto);
        //    var entity = await _vaccountSecurityQuestionManager.Create(newEntity);
        //    return _mapper.Map<VaccountSecurityQuestionDto>(entity);
        //}

        //public async Task<VaccountSecurityQuestionDto> UpdateVaccountSecurityQuestion(long id, VaccountSecurityQuestionDto dto) {
        //    var entity = await _vaccountSecurityQuestionManager.Find(id);
        //    if(entity == null) {
        //        return null;
        //    }

        //    var updateEntity = _mapper.Map(dto, entity);
        //    entity = await _vaccountSecurityQuestionManager.Update(updateEntity);

        //    return _mapper.Map<VaccountSecurityQuestionDto>(entity);
        //}
        #endregion

        #region COMPANY SECTIONS
        public async Task<CompanySectionDto> GetCompanySection(long id) {
            var result = await _companySectionManager.FindInclude(id);
            return _mapper.Map<CompanySectionDto>(result);
        }

        public async Task<List<CompanySectionDto>> GetCompanySections(long companyId) {
            var result = await _companySectionManager.FindAll(companyId);
            return _mapper.Map<List<CompanySectionDto>>(result);
        }

        public async Task<CompanySectionDto> CreateCompanySection(CompanySectionDto dto) {
            var entity = await _companySectionManager.Create(_mapper.Map<CompanySectionEntity>(dto));
            return _mapper.Map<CompanySectionDto>(entity);
        }

        public async Task<CompanySectionDto> UpdateCompanySection(long id, CompanySectionDto dto) {
            var entity = await _companySectionManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companySectionManager.Update(newEntity);

            return _mapper.Map<CompanySectionDto>(entity);
        }

        public async Task<bool> DeleteCompanySection(long id) {
            var entity = await _companySectionManager.Find(id);

            if(entity != null) {
                var result = await _companySectionManager.Delete(entity);
                return result != 0;
            }

            return false;
        }
        #endregion

        #region COMPANY SECTION FIELDS
        public async Task<CompanySectionFieldDto> GetCompanySectionField(long id) {
            var result = await _companySectionFieldManager.Find(id);
            return _mapper.Map<CompanySectionFieldDto>(result);
        }

        public async Task<CompanySectionFieldDto> CreateCompanySectionField(CompanySectionFieldDto dto) {
            var entity = await _companySectionFieldManager.Create(_mapper.Map<CompanySectionFieldEntity>(dto));
            return _mapper.Map<CompanySectionFieldDto>(entity);
        }

        public async Task<CompanySectionFieldDto> UpdateCompanySectionField(long id, CompanySectionFieldDto dto) {
            var entity = await _companySectionFieldManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companySectionFieldManager.Update(newEntity);

            return _mapper.Map<CompanySectionFieldDto>(entity);
        }

        public async Task<bool> DeleteCompanySectionField(long id) {
            var entity = await _companySectionFieldManager.Find(id);

            if(entity != null) {
                var result = await _companySectionFieldManager.Delete(entity);
                return result != 0;
            }

            return false;
        }
        #endregion

        #region SECTION
        public async Task<SectionDto> GetSection(long id) {
            var result = await _sectionManager.Find(id);
            return _mapper.Map<SectionDto>(result);
        }

        public async Task<Pager<SectionDto>> GetSectionPage(PagerFilter filter) {
            #region Sort/Filter
            var sortby = filter.Sort ?? "Name";

            Expression<Func<SectionEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || x.Name.ToLower().Contains(filter.Search.ToLower()));
            #endregion

            var tuple = await _sectionManager.Pager<SectionEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<SectionDto>(new List<SectionDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<SectionDto>>(list);
            return new Pager<SectionDto>(result, count, page, filter.Limit);
        }

        public async Task<List<SectionDto>> GetSections() {
            var result = await _sectionManager.All();
            return _mapper.Map<List<SectionDto>>(result);
        }

        public async Task<SectionDto> CreateSection(SectionDto dto) {
            var entity = _mapper.Map<SectionEntity>(dto);
            entity = await _sectionManager.Create(entity);
            return _mapper.Map<SectionDto>(entity);
        }

        public async Task<SectionDto> UpdateSection(long id, SectionDto dto) {
            var entity = await _sectionManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _sectionManager.Update(newEntity);

            return _mapper.Map<SectionDto>(entity);
        }

        public async Task<bool> DeleteSection(long id) {
            var entity = await _sectionManager.Find(id);

            if(entity != null) {
                var result = await _sectionManager.Delete(entity);
                return result != 0;
            }

            return false;
        }
        #endregion
    }
}
