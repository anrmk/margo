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
    public interface ISectionBusinessManager {
        //  SECTION
        Task<SectionDto> GetSection(long id);
        Task<Pager<SectionDto>> GetSectionPage(PagerFilter filter);
        Task<List<SectionDto>> GetSections();
        Task<SectionDto> CreateSection(SectionDto dto);
        Task<SectionDto> UpdateSection(long id, SectionDto dto);
        Task<bool> DeleteSection(long id);
        Task<bool> DeleteSections(long[] ids);

        //  SECTION FIELD
        Task<SectionFieldDto> GetSectionField(long id);
        Task<List<SectionFieldDto>> GetSectionFields(long sectionId);
        Task<SectionFieldDto> CreateSectionField(SectionFieldDto dto);
        Task<SectionFieldDto> UpdateSectionField(long id, SectionFieldDto dto);
        Task<bool> DeleteSectionField(long id);
    }

    public class SectionBusinessManager: BaseBusinessManager, ISectionBusinessManager {
        private readonly IMapper _mapper;
        private readonly ISectionManager _sectionManager;
        private readonly ISectionFieldManager _sectionFieldManager;

        public SectionBusinessManager(IMapper mapper, ISectionManager sectionManager, 
            ISectionFieldManager sectionFieldManager) {
            _mapper = mapper;
            _sectionManager = sectionManager;
            _sectionFieldManager = sectionFieldManager;
        }

        //#region COMPANY
        //public async Task<CompanyDto> GetCompany(long id) {
        //    var result = await _companyManager.FindInclude(id);
        //    return _mapper.Map<CompanyDto>(result);
        //}

        //public async Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter) {
        //    var sortby = "Name";

        //    Expression<Func<CompanyEntity, bool>> where = x =>
        //           (true)
        //           && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));

        //    string[] include = new string[] { "Address" };

        //    var tuple = await _companyManager.Pager<CompanyEntity>(where, sortby, filter.Start, filter.Length, include);
        //    var list = tuple.Item1;
        //    var count = tuple.Item2;

        //    if(count == 0)
        //        return new Pager<CompanyDto>(new List<CompanyDto>(), 0, filter.Length, filter.Start);

        //    var page = (filter.Start + filter.Length) / filter.Length;

        //    var result = _mapper.Map<List<CompanyDto>>(list);
        //    return new Pager<CompanyDto>(result, count, page, filter.Length);
        //}

        //public async Task<List<CompanyDto>> GetCompanies() {
        //    var result = await _companyManager.FindAll();
        //    return _mapper.Map<List<CompanyDto>>(result);
        //}

        //public async Task<CompanyDto> CreateCompany(CompanyGeneralDto dto) {
        //    var entity = await _companyManager.Create(_mapper.Map<CompanyEntity>(dto));
        //    return _mapper.Map<CompanyDto>(entity);
        //}

        //public async Task<CompanyDto> UpdateCompany(long id, CompanyGeneralDto dto) {
        //    var entity = await _companyManager.Find(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var newEntity = _mapper.Map(dto, entity);
        //    entity = await _companyManager.Update(newEntity);

        //    return _mapper.Map<CompanyDto>(entity);
        //}

        //public async Task<bool> DeleteCompany(long[] ids) {
        //    var companies = await _companyManager.FindAll(ids);
        //    int result = await _companyManager.Delete(companies);
        //    return result != 0;
        //}

        //public async Task<bool> DeleteCompany(long id) {
        //    var result = 0;
        //    var entity = await _companyManager.Find(id);

        //    if(entity != null) {
        //        result = await _companyManager.Delete(entity);
        //    }

        //    var address = await _companyAddressManager.Find(entity.AddressId);
        //    if(address != null) {
        //        result = await _companyAddressManager.Delete(address);
        //    }

        //    return result != 0;
        //}
        //#endregion

        //#region COMPANY ADRESS
        //public async Task<CompanyAddressDto> GetCompanyAddress(long id) {
        //    var result = await _companyAddressManager.Find(id);
        //    return _mapper.Map<CompanyAddressDto>(result);
        //}

        //public async Task<CompanyAddressDto> CreateCompanyAddress(CompanyAddressDto dto) {
        //    var settings = await _companyAddressManager.Find(dto.Id);
        //    if(settings == null) {
        //        return null;
        //    }

        //    var newEntity = _mapper.Map<CompanyAddressEntity>(dto);
        //    var entity = await _companyAddressManager.Create(newEntity);
        //    return _mapper.Map<CompanyAddressDto>(entity);
        //}

        //public async Task<CompanyAddressDto> UpdateCompanyAddress(long companyId, CompanyAddressDto dto) {
        //    var entity = await _companyAddressManager.Find(dto.Id);

        //    if(entity == null) {
        //        entity = await _companyAddressManager.Create(_mapper.Map<CompanyAddressEntity>(dto));

        //        var company = await _companyManager.Find(companyId);
        //        company.AddressId = entity.Id;
        //        await _companyManager.Update(company);
        //    } else {
        //        var updateEntity = _mapper.Map(dto, entity);
        //        entity = await _companyAddressManager.Update(updateEntity);
        //    }

        //    return _mapper.Map<CompanyAddressDto>(entity);
        //}
        //#endregion

        #region INVOICE
        //public async Task<InvoiceDto> GetInvoice(long id) {
        //    var result = await _invoiceManager.FindInclude(id);
        //    return _mapper.Map<InvoiceDto>(result);
        //}

        //public async Task<List<InvoiceDto>> GetInvoices() {
        //    var result = await _invoiceManager.FindAll();
        //    return _mapper.Map<List<InvoiceDto>>(result);
        //}

        //public async Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
        //    #region Sort/Filter
        //    var sortby = "No";

        //    Expression<Func<InvoiceEntity, bool>> where = x =>
        //          (true)
        //       && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower())))
        //       && (!filter.CompanyId.HasValue || x.CompanyId == filter.CompanyId)
        //       && (!filter.Unpaid || x.IsPayd != filter.Unpaid)
        //       ;
        //    #endregion

        //    string[] include = new string[] { "Company", "Vendor", "Payments" };

        //    var tuple = await _invoiceManager.Pager<InvoiceEntity>(where, sortby, filter.Start, filter.Length, include);
        //    var list = tuple.Item1;
        //    var count = tuple.Item2;

        //    if(count == 0)
        //        return new Pager<InvoiceDto>(new List<InvoiceDto>(), 0, filter.Start, filter.Length);

        //    var page = (filter.Start + filter.Length) / filter.Length;

        //    var result = _mapper.Map<List<InvoiceDto>>(list);
        //    return new Pager<InvoiceDto>(result, count, page, filter.Start);
        //}

        //public async Task<InvoiceDto> CreateInvoice(InvoiceDto dto) {
        //    var entity = _mapper.Map<InvoiceEntity>(dto);
        //    entity = await _invoiceManager.Create(entity);
        //    return _mapper.Map<InvoiceDto>(entity);
        //}

        //public async Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto) {
        //    var entity = await _invoiceManager.FindInclude(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var entity1 = _mapper.Map(dto, entity);
        //    entity = await _invoiceManager.Update(entity1);
        //    return _mapper.Map<InvoiceDto>(entity);
        //}

        //public async Task<bool> DeleteInvoice(long id) {
        //    var entity = await _invoiceManager.Find(id);
        //    if(entity == null) {
        //        return false;
        //    }
        //    int result = await _invoiceManager.Delete(entity);
        //    return result != 0;
        //}

        //public async Task<bool> DeleteInvoice(long[] ids) {
        //    var invoices = await _invoiceManager.FindByIds(ids);

        //    var hasPayments = invoices.Any(x => x.Payments != null && x.Payments.Count > 0);
        //    if(hasPayments)
        //        throw new Exception("Delete a payment or overpayment from an invoices!");

        //    int result = await _invoiceManager.Delete(invoices);
        //    return result != 0;
        //}

        #endregion

        #region PAYMENTS
        //public async Task<PaymentDto> GetPayment(long id) {
        //    var result = await _paymentManager.FindInclude(id);
        //    return _mapper.Map<PaymentDto>(result);
        //}

        //public async Task<Pager<PaymentDto>> GetPaymentPager(PaymentFilterDto filter) {
        //    #region Sort/Filter
        //    var sortby = "Date";

        //    Expression<Func<PaymentEntity, bool>> where = x =>
        //          (true)
        //       && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower())))
        //       && (!filter.InvoiceId.HasValue || x.InvoiceId == filter.InvoiceId)
        //       && (!filter.DateFrom.HasValue || x.Date >= filter.DateFrom)
        //       && (!filter.DateTo.HasValue || x.Date <= filter.DateTo)
        //       ;
        //    #endregion

        //    string[] include = new string[] { "Invoice" };

        //    var tuple = await _paymentManager.Pager<PaymentEntity>(where, sortby, filter.Start, filter.Length, include);
        //    var list = tuple.Item1;
        //    var count = tuple.Item2;

        //    if(count == 0)
        //        return new Pager<PaymentDto>(new List<PaymentDto>(), 0, filter.Start, filter.Length);

        //    var page = (filter.Start + filter.Length) / filter.Length;

        //    var result = _mapper.Map<List<PaymentDto>>(list);
        //    return new Pager<PaymentDto>(result, count, page, filter.Start);
        //}

        //public async Task<PaymentDto> CreatePayment(PaymentDto dto) {
        //    var item = await _invoiceManager.FindInclude(dto.InvoiceId ?? 0);
        //    if(item == null) {
        //        return null;
        //    }

        //    var entity = _mapper.Map<PaymentEntity>(dto);
        //    entity = await _paymentManager.Create(entity);

        //    return _mapper.Map<PaymentDto>(entity);
        //}

        //public async Task<PaymentDto> UpdatePayment(long id, PaymentDto dto) {
        //    var entity = await _paymentManager.FindInclude(id);
        //    if(entity == null) {
        //        return null;
        //    }
        //    var entity1 = _mapper.Map(dto, entity);
        //    entity = await _paymentManager.Update(entity1);
        //    return _mapper.Map<PaymentDto>(entity);
        //}

        //public async Task<bool> DeletePayment(long id) {
        //    var entity = await _paymentManager.Find(id);
        //    if(entity == null)
        //        throw new Exception("We did not find payment records for this request.!");

        //    int result = await _paymentManager.Delete(entity);
        //    return result != 0;
        //}

        //public async Task<bool> DeletePayment(long[] ids) {
        //    var entity = await _paymentManager.Filter(x => ids.Contains(x.Id));
        //    if(entity == null)
        //        throw new Exception("We did not find payment records for this request.!");

        //    int result = await _paymentManager.Delete(entity);
        //    return result != 0;
        //}
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

        #region SECTION
        public async Task<SectionDto> GetSection(long id) {
            var result = await _sectionManager.Find(id);
            return _mapper.Map<SectionDto>(result);
        }

        public async Task<Pager<SectionDto>> GetSectionPage(PagerFilter filter) {
            var sortby = "Name";

            Expression<Func<SectionEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || x.Name.ToLower().Contains(filter.Search.ToLower()));

            var tuple = await _sectionManager.Pager<SectionEntity>(where, sortby, filter.Start, filter.Length);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<SectionDto>(new List<SectionDto>(), 0, filter.Length, filter.Start);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<SectionDto>>(list);
            return new Pager<SectionDto>(result, count, page, filter.Length);
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
            return await DeleteSections(new long[] { id });
        }

        public async Task<bool> DeleteSections(long[] ids) {
            var entities = await _sectionManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _sectionManager.Delete(entities);
            return result != 0;
        }
        #endregion

        #region SECTION FIELDS
        public async Task<SectionFieldDto> GetSectionField(long id) {
            var result = await _sectionFieldManager.Find(id);
            return _mapper.Map<SectionFieldDto>(result);
        }

        public async Task<List<SectionFieldDto>> GetSectionFields(long sectionId) {
            var result = await _sectionFieldManager.FindAll(sectionId);
            return _mapper.Map<List<SectionFieldDto>>(result);
        }

        public async Task<SectionFieldDto> CreateSectionField(SectionFieldDto dto) {
            var entity = _mapper.Map<SectionFieldEntity>(dto);
            entity = await _sectionFieldManager.Create(entity);
            return _mapper.Map<SectionFieldDto>(entity);
        }

        public Task<SectionFieldDto> UpdateSectionField(long id, SectionFieldDto dto) {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteSectionField(long id) {
            var entity = await _sectionFieldManager.Find(id);
            if(entity == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _sectionFieldManager.Delete(entity);
            return result != 0;
        }
        #endregion
    }
}
