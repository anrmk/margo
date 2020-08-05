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
    public interface IPaymentBusinessManager {
        Task<PaymentDto> GetPayment(long id);
        Task<List<PaymentDto>> GetPayments();
        Task<Pager<PaymentDto>> GetPaymentPager(PagerFilter filter);
        Task<PaymentDto> CreatePayment(PaymentDto dto);
        Task<PaymentDto> UpdatePayment(long id, PaymentDto dto);
        Task<bool> DeletePayments(long[] ids);
    }

    public class PaymentBusinessManager: BaseBusinessManager, IPaymentBusinessManager {
        private readonly IMapper _mapper;
        private readonly IPaymentManager _paymentManager;

        public PaymentBusinessManager(IMapper mapper, IPaymentManager paymentManager) {
            _mapper = mapper;
            _paymentManager = paymentManager;
        }

        public async Task<PaymentDto> GetPayment(long id) {
            var result = await _paymentManager.FindInclude(id);
            return _mapper.Map<PaymentDto>(result);
        }

        public async Task<List<PaymentDto>> GetPayments() {
            var entities = await _paymentManager.All();
            return _mapper.Map<List<PaymentDto>>(entities);
        }

        public async Task<Pager<PaymentDto>> GetPaymentPager(PagerFilter filter) {
            var sortby = "Id";

            string[] include = new string[] { "Invoice" };

            var (list, count) = await _paymentManager.Pager<PaymentEntity>(x => true, sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new Pager<PaymentDto>(new List<PaymentDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<PaymentDto>>(list);
            return new Pager<PaymentDto>(result, count, page, filter.Length);
        }

        public async Task<PaymentDto> CreatePayment(PaymentDto dto) {
            var newEntity = _mapper.Map<PaymentEntity>(dto);
            var entity = await _paymentManager.Create(newEntity);
            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<PaymentDto> UpdatePayment(long id, PaymentDto dto) {
            var entity = await _paymentManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _paymentManager.Update(newEntity);

            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<bool> DeletePayments(long[] ids) {
            var entities = await _paymentManager.FindByIds(ids);
            if(entities?.Any() != true)
                throw new Exception("Did not find any records.");

            int result = await _paymentManager.Delete(entities);
            return result != 0;
        }
    }
}
