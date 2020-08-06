using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IInvoiceBusinessManager {
        Task<InvoiceDto> GetInvoice(long id);
        Task<List<InvoiceDto>> GetInvoices();
        Task<List<InvoiceDto>> GetUnpaidInvoices();
        Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter);
        Task<InvoiceDto> CreateInvoice(InvoiceDto dto);
        Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto);
        Task<bool> DeleteInvoices(long[] ids);
    }

    public class InvoiceBusinessManager: BaseBusinessManager, IInvoiceBusinessManager {
        private readonly IMapper _mapper;
        private readonly IInvoiceManager _invoiceManager;

        public InvoiceBusinessManager(IMapper mapper, IInvoiceManager invoiceManager) {
            _mapper = mapper;
            _invoiceManager = invoiceManager;
        }

        public async Task<InvoiceDto> GetInvoice(long id) {
            var result = await _invoiceManager.FindInclude(id);
            return _mapper.Map<InvoiceDto>(result);
        }

        public async Task<List<InvoiceDto>> GetInvoices() {
            var entity = await _invoiceManager.FindAll();
            return _mapper.Map<List<InvoiceDto>>(entity);
        }

        public async Task<List<InvoiceDto>> GetUnpaidInvoices() {
            var entity = await _invoiceManager.FindAllUnpaid();
            return _mapper.Map<List<InvoiceDto>>(entity);
        }

        public async Task<Pager<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
            var sortby = "Id";

            string[] include = new string[] { "Account", "Account.Person", "Account.Company", "Payments" };

            var (list, count) = await _invoiceManager.Pager<InvoiceEntity>(
                x => (!filter.CompanyId.HasValue || x.Account.CompanyId == filter.CompanyId)
                    && (!filter.PersonId.HasValue || x.Account.PersonId == filter.PersonId)
                    && (!filter.VendorId.HasValue || x.Account.VendorId == filter.VendorId)
                    && (!filter.Unpaid || (x.Amount > 0 && !x.Payments.Any()) || (x.Amount - x.Payments.Sum(x => x.Amount) > 0)),
                sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new Pager<InvoiceDto>(new List<InvoiceDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<InvoiceDto>>(list);
            return new Pager<InvoiceDto>(result, count, page, filter.Length);
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceDto dto) {
            var newEntity = _mapper.Map<InvoiceEntity>(dto);
            var entity = await _invoiceManager.Create(newEntity);
            return _mapper.Map<InvoiceDto>(entity);
        }

        public async Task<InvoiceDto> UpdateInvoice(long id, InvoiceDto dto) {
            var entity = await _invoiceManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _invoiceManager.Update(newEntity);

            return _mapper.Map<InvoiceDto>(entity);
        }

        public async Task<bool> DeleteInvoices(long[] ids) {
            var entities = await _invoiceManager.FindByIds(ids);
            if(entities?.Any() != true)
                throw new Exception("Did not find any records.");

            int result = await _invoiceManager.Delete(entities);
            return result != 0;
        }
    }
}
