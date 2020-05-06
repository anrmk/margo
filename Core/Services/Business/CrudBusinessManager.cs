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
        Task<VendorAddressDto> GetSupplierAddress(long id);
        Task<VendorAddressDto> CreateSupplierAddress(VendorAddressDto dto);
        Task<VendorAddressDto> UpdateSupplierAddress(long companyId, VendorAddressDto dto);
        #endregion

        #region VACCOUNT
        Task<VaccountDto> GetVaccount(long id);
        Task<Pager<VaccountDto>> GetVaccountPager(VaccountFilterDto filter);
        Task<VaccountDto> CreateVaccount(VaccountDto dto);
        Task<VaccountDto> UpdateVaccount(long id, VaccountDto dto);
        Task<bool> DeleteVaccount(long id);
        #endregion

        Task<InvoiceDto> GetInvoice(long id);
        Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter);
        Task<InvoiceDto> CreateInvoice(InvoiceDto dto);
        Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto);
        Task<bool> DeleteInvoice(long id);
    }

    public class CrudBusinessManager: BaseBusinessManager, ICrudBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        private readonly ICompanyAddressManager _companyAddressManager;

        private readonly IVendorManager _vendorManager;
        private readonly IVendorAddressManager _supplierAddressManager;

        private readonly IVaccountManager _vaccountManager;

        private readonly IInvoiceManager _invoiceManager;

        //private readonly INsiBusinessManager _nsiBusinessManager;

        public CrudBusinessManager(IMapper mapper,
            ICompanyManager companyManager, ICompanyAddressManager companyAddressManager,
            IVendorManager supplierManager, IVendorAddressManager supplierAddressManager,
            IVaccountManager vaccountManager,
            IInvoiceManager invoiceManager) {
            _mapper = mapper;
            _companyManager = companyManager;
            _companyAddressManager = companyAddressManager;
            _vendorManager = supplierManager;
            _supplierAddressManager = supplierAddressManager;
            _vaccountManager = vaccountManager;
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

        #region CUSTOMER ADRESS
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

            var address = await _supplierAddressManager.Find(entity.AddressId);
            if(address != null) {
                result = await _supplierAddressManager.Delete(address);
            }

            return result != 0;
        }
        #endregion

        #region SUPPLIER ADRESS
        public async Task<VendorAddressDto> GetSupplierAddress(long id) {
            var result = await _supplierAddressManager.Find(id);
            return _mapper.Map<VendorAddressDto>(result);
        }

        public async Task<VendorAddressDto> CreateSupplierAddress(VendorAddressDto dto) {
            var settings = await _supplierAddressManager.Find(dto.Id);
            if(settings == null) {
                return null;
            }

            var newEntity = _mapper.Map<VendorAddressEntity>(dto);
            var entity = await _supplierAddressManager.Create(newEntity);
            return _mapper.Map<VendorAddressDto>(entity);
        }

        public async Task<VendorAddressDto> UpdateSupplierAddress(long supplierId, VendorAddressDto dto) {
            var entity = await _supplierAddressManager.Find(dto.Id);

            if(entity == null) {
                entity = await _supplierAddressManager.Create(_mapper.Map<VendorAddressEntity>(dto));

                var supplier = await _vendorManager.Find(supplierId);
                supplier.AddressId = entity.Id;
                await _vendorManager.Update(supplier);
            } else {
                var updateEntity = _mapper.Map(dto, entity);
                entity = await _supplierAddressManager.Update(updateEntity);
            }

            return _mapper.Map<VendorAddressDto>(entity);
        }
        #endregion

        #region INVOICE
        public async Task<InvoiceDto> GetInvoice(long id) {
            var result = await _invoiceManager.FindInclude(id);
            return _mapper.Map<InvoiceDto>(result);
        }

        public async Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
            #region Sort/Filter
            var sortby = filter.Sort ?? "No";

            Expression<Func<InvoiceEntity, bool>> where = x =>
                  (true)
               && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Supplier.Name.ToLower().Contains(filter.Search.ToLower())))
               && ((filter.CompanyId == null) || filter.CompanyId == x.CompanyId);
            #endregion

            string[] include = new string[] { "Company", "Supplier" };

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
            var entity = await _invoiceManager.FindInclude(id);
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
        public async Task<VaccountDto> GetVaccount(long id) {
            var result = await _vaccountManager.Find(id);
            return _mapper.Map<VaccountDto>(result);
        }

        public async Task<Pager<VaccountDto>> GetVaccountPager(VaccountFilterDto filter) {
            var sortby = filter.Sort ?? "Id";

            Expression<Func<VaccountEntity, bool>> where = x =>
                  (true)
               && (string.IsNullOrEmpty(filter.Search) || (x.UserName.ToLower().Contains(filter.Search.ToLower())))
               && ((filter.CompanyId == null) || filter.CompanyId == x.CompanyId)
               && ((filter.VendorId == null) || filter.VendorId == x.VendorId)
               ;

            string[] include = new string[] { "Company", "Vendor" };

            var tuple = await _vaccountManager.Pager<VaccountEntity>(where, sortby, filter.Order.Equals("desc"), filter.Offset, filter.Limit, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<VaccountDto>(new List<VaccountDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<VaccountDto>>(list);
            return new Pager<VaccountDto>(result, count, page, filter.Limit);
        }

        public async Task<VaccountDto> CreateVaccount(VaccountDto dto) {
            var entity = _mapper.Map<VaccountEntity>(dto);
            entity = await _vaccountManager.Create(entity);
            return _mapper.Map<VaccountDto>(entity);
        }

        public async Task<VaccountDto> UpdateVaccount(long id, VaccountDto dto) {
            var entity = await _vaccountManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _vaccountManager.Update(newEntity);

            return _mapper.Map<VaccountDto>(entity);
        }

        public async Task<bool> DeleteVaccount(long id) {
            var result = 0;
            var entity = await _vaccountManager.Find(id);

            if(entity != null) {
                result = await _vaccountManager.Delete(entity);
            }

            return result != 0;
        }
        #endregion

    }
}
