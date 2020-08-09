using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IPaymentBusinessManager {
        Task<PaymentDto> GetPayment(Guid id);
        Task<List<PaymentDto>> GetPayments();
        Task<PagerDto<PaymentDto>> GetPaymentPager(PagerFilterDto filter);
        Task<PaymentDto> CreatePayment(PaymentDto dto);
        Task<PaymentDto> UpdatePayment(Guid id, PaymentDto dto);
        Task<bool> DeletePayments(Guid[] ids);
    }

    public class PaymentBusinessManager: BaseBusinessManager, IPaymentBusinessManager {
        private readonly IMapper _mapper;
        private readonly IPaymentManager _paymentManager;

        public PaymentBusinessManager(IMapper mapper, IPaymentManager paymentManager) {
            _mapper = mapper;
            _paymentManager = paymentManager;
        }

        public async Task<PaymentDto> GetPayment(Guid id) {
            var result = await _paymentManager.FindInclude(id);
            return _mapper.Map<PaymentDto>(result);
        }

        public async Task<List<PaymentDto>> GetPayments() {
            var entities = await _paymentManager.All();
            return _mapper.Map<List<PaymentDto>>(entities);
        }

        public async Task<PagerDto<PaymentDto>> GetPaymentPager(PagerFilterDto filter) {
            var sortby = "Id";

            string[] include = new string[] { "Invoice" };

            var (list, count) = await _paymentManager.Pager<PaymentEntity>(x => true, sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new PagerDto<PaymentDto>(new List<PaymentDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<PaymentDto>>(list);
            return new PagerDto<PaymentDto>(result, count, page, filter.Length);
        }

        public async Task<PaymentDto> CreatePayment(PaymentDto dto) {
            var newEntity = _mapper.Map<PaymentEntity>(dto);
            var entity = await _paymentManager.Create(newEntity);
            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<PaymentDto> UpdatePayment(Guid id, PaymentDto dto) {
            var entity = await _paymentManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _paymentManager.Update(newEntity);

            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<bool> DeletePayments(Guid[] ids) {
            var entities = await _paymentManager.FindByIds(ids);
            if(entities?.Any() != true)
                throw new Exception("Did not find any records.");

            int result = await _paymentManager.Delete(entities);
            return result != 0;
        }
    }
}
