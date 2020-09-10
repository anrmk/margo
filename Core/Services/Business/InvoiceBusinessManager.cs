using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IInvoiceBusinessManager {
        Task<InvoiceDto> GetInvoice(Guid id);
        Task<List<InvoiceDto>> GetInvoices();
        Task<List<InvoiceDto>> GetInvoices(Guid accountId);
        Task<PagerDto<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter);
        Task<InvoiceDto> CreateInvoice(InvoiceDto dto);
        Task<List<InvoiceDto>> CreateInvoice(List<InvoiceDto> dto);
        Task<InvoiceDto> UpdateInvoice(Guid id, InvoiceDto dto);
        Task<bool> DeleteInvoices(Guid[] ids);

        Task<List<InvoiceServiceDto>> GetInvoiceServices(Guid id);
        Task<InvoiceServiceDto> CreateService(InvoiceServiceDto dto);
        Task<bool> DeleteService(Guid[] ids);
        Task<bool> DeleteService(Guid id);
    }

    public class InvoiceBusinessManager: BaseBusinessManager, IInvoiceBusinessManager {
        private readonly IMapper _mapper;
        private readonly IInvoiceManager _invoiceManager;
        private readonly IInvoiceServiceManager _invoiceServiceManager;

        public InvoiceBusinessManager(IMapper mapper, IInvoiceManager invoiceManager, IInvoiceServiceManager invoiceServiceManager) {
            _mapper = mapper;
            _invoiceManager = invoiceManager;
            _invoiceServiceManager = invoiceServiceManager;
        }

        public async Task<InvoiceDto> GetInvoice(Guid id) {
            var result = await _invoiceManager.FindInclude(id);
            return _mapper.Map<InvoiceDto>(result);
        }

        public async Task<List<InvoiceDto>> GetInvoices() {
            var entity = await _invoiceManager.FindAll();
            return _mapper.Map<List<InvoiceDto>>(entity);
        }

        public async Task<List<InvoiceDto>> GetInvoices(Guid accountId) {
            var entity = await _invoiceManager.FindAll(accountId);
            return _mapper.Map<List<InvoiceDto>>(entity);
        }

        public async Task<PagerDto<InvoiceDto>> GetInvoicePager(InvoiceFilterDto filter) {
            var sortby = "Id";

            string[] include = new string[] { "Account", "Account.Person", "Account.Company", "Payments" };

            var (list, count) = await _invoiceManager.Pager<InvoiceEntity>(
                x => (!filter.VendorId.HasValue || x.Account.VendorId == filter.VendorId)
                    && (!filter.CustomerId.HasValue || x.Account.CompanyId == filter.CustomerId || x.Account.PersonId == filter.CustomerId)
                    && (!filter.Unpaid || (x.Amount > 0 && !x.Payments.Any()) || (x.Amount - x.Payments.Sum(x => x.Amount) > 0))
                    && (!filter.Kind.HasValue || x.Account.Kind == filter.Kind),
                sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new PagerDto<InvoiceDto>(new List<InvoiceDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<InvoiceDto>>(list);
            return new PagerDto<InvoiceDto>(result, count, page, filter.Length);
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceDto dto) {
            var newEntity = _mapper.Map<InvoiceEntity>(dto);
            var entity = await _invoiceManager.Create(newEntity);

            var services = _mapper.Map<List<InvoiceServiceEntity>>(dto.Services);
            if(services.Count > 0) {
                services.ForEach(x => {
                    x.InvoiceId = entity.Id;
                    return;
                });

                await _invoiceServiceManager.Create(services);
            }
            return _mapper.Map<InvoiceDto>(entity);
        }

        public async Task<List<InvoiceDto>> CreateInvoice(List<InvoiceDto> dto) {
            var newEntity = _mapper.Map<List<InvoiceEntity>>(dto);
            var entity = await _invoiceManager.Create(newEntity.AsEnumerable());
            return _mapper.Map<List<InvoiceDto>>(entity.ToList());
        }

        public async Task<InvoiceDto> UpdateInvoice(Guid id, InvoiceDto dto) {
            var entity = await _invoiceManager.Find(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _invoiceManager.Update(newEntity);

            var sCreateDtos = dto.Services.Where(x => x.Id.Equals(Guid.Empty));
            var createServices = _mapper.Map<List<InvoiceServiceEntity>>(sCreateDtos);
            if(createServices.Count > 0) {
                await _invoiceServiceManager.Create(createServices);
            }

            var sUpdateDtos = dto.Services.Except(sCreateDtos);
            var updateServices = _mapper.Map<List<InvoiceServiceEntity>>(sUpdateDtos);
            if(updateServices.Count > 0) {
                await _invoiceServiceManager.Update(updateServices);
            }

            return _mapper.Map<InvoiceDto>(entity);
        }

        public async Task<bool> DeleteInvoices(Guid[] ids) {
            var entities = await _invoiceManager.FindByIds(ids);
            if(entities?.Any() != true)
                throw new Exception("Did not find any records.");

            int result = await _invoiceManager.Delete(entities);
            return result != 0;
        }

        #region SERVICES
        public async Task<List<InvoiceServiceDto>> GetInvoiceServices(Guid id) {
            var entities = await _invoiceServiceManager.FindBy(id);
            return _mapper.Map<List<InvoiceServiceDto>>(entities);
        }

        public async Task<InvoiceServiceDto> CreateService(InvoiceServiceDto dto) {
            var newEntity = _mapper.Map<InvoiceServiceEntity>(dto);
            var entity = await _invoiceServiceManager.Create(newEntity);
            return _mapper.Map<InvoiceServiceDto>(entity);
        }

        public async Task<bool> DeleteService(Guid[] ids) {
            var entities = await _invoiceServiceManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _invoiceServiceManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteService(Guid id) {
            return await DeleteService(new Guid[] { id });
        }


        #endregion
    }
}
