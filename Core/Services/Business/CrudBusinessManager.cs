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
    public interface ICrudBusinessManager {
        Task<CompanyDto> GetCompany(long id);
        Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto> CreateCompany(CompanyDto dto);
        Task<CompanyDto> UpdateCompany(long id, CompanyDto dto);
        Task<bool> DeleteCompany(long id);

        Task<SupplierDto> GetSupplier(long id);
        Task<Pager<SupplierDto>> GetSupplierPager(PagerFilter filter);
        Task<List<SupplierDto>> GetSuppliers();
        Task<List<SupplierDto>> GetSuppliers(long companyId);
        Task<SupplierDto> CreateSupplier(SupplierDto dto);
        Task<SupplierDto> UpdateSupplier(long id, SupplierDto dto);
        Task<bool> DeleteSupplier(long id);

        Task<InvoiceDto> GetInvoice(long id);
        Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter);
        Task<InvoiceDto> CreateInvoice(InvoiceDto dto);
        Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto);
        Task<bool> DeleteInvoice(long id);
    }

    public class CrudBusinessManager: BaseBusinessManager, ICrudBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;
        private readonly ISupplierManager _supplierManager;
        private readonly IInvoiceManager _invoiceManager;

        //private readonly INsiBusinessManager _nsiBusinessManager;

        public CrudBusinessManager(IMapper mapper, ICompanyManager companyManager, ISupplierManager supplierManager, IInvoiceManager invoiceManager) {
            _mapper = mapper;
            _companyManager = companyManager;
            _supplierManager = supplierManager;
            _invoiceManager = invoiceManager;
        }

        #region COMPANY
        public async Task<CompanyDto> GetCompany(long id) {
            var result = await _companyManager.FindInclude(id);
            return _mapper.Map<CompanyDto>(result);
        }

        public async Task<Pager<CompanyDto>> GetCompanyPage(PagerFilter filter) {
            #region Sort/Filter
            Expression<Func<CompanyEntity, string>> orderPredicate = filter.RandomSort ? x => Guid.NewGuid().ToString() : GetExpression<CompanyEntity>(filter.Sort ?? "No");

            Expression<Func<CompanyEntity, bool>> wherePredicate = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));
            #endregion

            string[] include = new string[] { "Address" };

            var tuple = await _companyManager.Pager<CompanyEntity>(wherePredicate, orderPredicate, filter.Offset, filter.Limit, include);
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

        public async Task<CompanyDto> CreateCompany(CompanyDto dto) {
            var entity = await _companyManager.Create(_mapper.Map<CompanyEntity>(dto));
            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<CompanyDto> UpdateCompany(long id, CompanyDto dto) {
            var entity = await _companyManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _companyManager.Update(newEntity);

            return _mapper.Map<CompanyDto>(entity);
        }

        public async Task<bool> DeleteCompany(long id) {
            var entity = await _companyManager.FindInclude(id);
            if(entity == null) {
                return false;
            }
            int result = await _companyManager.Delete(entity);
            return result != 0;
        }
        #endregion

        #region SUPPLIER
        public async Task<SupplierDto> GetSupplier(long id) {
            var result = await _supplierManager.FindInclude(id);
            return _mapper.Map<SupplierDto>(result);
        }

        public async Task<Pager<SupplierDto>> GetSupplierPager(PagerFilter filter) {
            #region Sort/Filter
            Expression<Func<SupplierEntity, string>> orderPredicate = filter.RandomSort ? x => Guid.NewGuid().ToString() : GetExpression<SupplierEntity>(filter.Sort ?? "No");

            Expression<Func<SupplierEntity, bool>> wherePredicate = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.No.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));
            #endregion

            string[] include = new string[] { "Company", "Address", "Activities" };

            var tuple = await _supplierManager.Pager<SupplierEntity>(wherePredicate, orderPredicate, filter.Offset, filter.Limit, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<SupplierDto>(new List<SupplierDto>(), 0, filter.Offset, filter.Limit);

            var page = (filter.Offset + filter.Limit) / filter.Limit;

            var result = _mapper.Map<List<SupplierDto>>(list);
            return new Pager<SupplierDto>(result, count, page, filter.Limit);
        }

        public async Task<List<SupplierDto>> GetSuppliers() {
            var result = await _supplierManager.FindAll();
            return _mapper.Map<List<SupplierDto>>(result);
        }

        public async Task<List<SupplierDto>> GetSuppliers(long companyId) {
            var result = await _supplierManager.FindAll(companyId);
            return _mapper.Map<List<SupplierDto>>(result);
        }

        public async Task<SupplierDto> CreateSupplier(SupplierDto dto) {
            var entity = _mapper.Map<SupplierEntity>(dto);
            entity = await _supplierManager.Create(entity);

            return _mapper.Map<SupplierDto>(entity);
        }

        public async Task<SupplierDto> UpdateSupplier(long id, SupplierDto dto) {
            if(id != dto.Id) {
                return null;
            }
            var entity = await _supplierManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _supplierManager.Update(newEntity);

            return _mapper.Map<SupplierDto>(entity);
        }

        public async Task<bool> DeleteSupplier(long id) {
            var entity = await _supplierManager.FindInclude(id);
            if(entity == null) {
                return false;
            }
            int result = await _supplierManager.Delete(entity);
            return result != 0;
        }
        #endregion

        #region INVOICE
        public async Task<InvoiceDto> GetInvoice(long id) {
            var result = await _invoiceManager.FindInclude(id);
            return _mapper.Map<InvoiceDto>(result);
        }

        public async Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
            #region Sort/Filter
            Expression<Func<InvoiceEntity, string>> orderPredicate = filter.RandomSort ? x => Guid.NewGuid().ToString() : GetExpression<InvoiceEntity>(filter.Sort ?? "No");

            Expression<Func<InvoiceEntity, bool>> wherePredicate = x =>
                  (true)
               && (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Supplier.Name.ToLower().Contains(filter.Search.ToLower())))
               && ((filter.CompanyId == null) || filter.CompanyId == x.CompanyId);
            #endregion

            string[] include = new string[] { "Company", "Supplier" };

            var tuple = await _invoiceManager.Pager<InvoiceEntity>(wherePredicate, orderPredicate, filter.Offset, filter.Limit, include);
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
            if(id != dto.Id) {
                return null;
            }
            var entity = await _invoiceManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var entity1 = _mapper.Map(dto, entity);
            entity = await _invoiceManager.Update(entity1);
            return _mapper.Map<InvoiceDto>(entity);
        }
        #endregion
    }
}
