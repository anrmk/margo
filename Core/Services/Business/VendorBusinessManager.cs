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
    public interface IVendorBusinessManager {
        Task<VendorDto> GetVendor(long id);
        Task<List<VendorDto>> GetVendors();
        Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter);
        Task<VendorDto> CreateVendor(VendorDto dto);
        Task<VendorDto> UpdateVendor(long id, VendorDto dto);
        Task<bool> DeleteVendors(long[] ids);

        Task<bool> DeleteFields(long[] ids);
    }

    public class VendorBusinessManager: BaseBusinessManager, IVendorBusinessManager {
        private readonly IMapper _mapper;
        private readonly IVendorManager _vendorManager;
        private readonly IVendorFieldManager _vendorFieldManager;

        public VendorBusinessManager(IMapper mapper, IVendorManager vendorManager,
            IVendorFieldManager vendorFieldManager) {
            _mapper = mapper;
            _vendorManager = vendorManager;
            _vendorFieldManager = vendorFieldManager;
        }

        public async Task<VendorDto> GetVendor(long id) {
            var result = await _vendorManager.FindInclude(id);
            return _mapper.Map<VendorDto>(result);
        }

        public async Task<List<VendorDto>> GetVendors() {
            var result = await _vendorManager.FindAll();
            return _mapper.Map<List<VendorDto>>(result);
        }

        public async Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter) {
            var sortby = "Id";

            Expression<Func<VendorEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.No.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));

            string[] include = new string[] { "Fields" };

            var tuple = await _vendorManager.Pager<VendorEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<VendorDto>(new List<VendorDto>(), 0, filter.Length, filter.Start);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<VendorDto>>(list);
            return new Pager<VendorDto>(result, count, page, filter.Length);
        }

        public async Task<VendorDto> CreateVendor(VendorDto dto) {
            var newEntity = _mapper.Map<VendorEntity>(dto);
            var entity = await _vendorManager.Create(newEntity);
            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<VendorDto> UpdateVendor(long id, VendorDto dto) {
            var entity = await _vendorManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _vendorManager.Update(newEntity);

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<bool> DeleteVendors(long[] ids) {
            var entities = await _vendorManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _vendorManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteFields(long[] ids) {
            var entities = await _vendorFieldManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _vendorFieldManager.Delete(entities);
            return result != 0;
        }
    }
}
