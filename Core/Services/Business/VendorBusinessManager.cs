﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IVendorBusinessManager {
        Task<VendorDto> GetVendor(Guid id);
        Task<List<VendorDto>> GetVendors();
        Task<List<VendorCategoryDto>> GetVendorCategories(Guid vendorId);
        Task<VendorCategoryDto> GetVendorCategory(Guid id);
        Task<PagerDto<VendorDto>> GetVendorPager(PagerFilterDto filter);
        Task<VendorDto> CreateVendor(VendorDto dto);
        Task<VendorDto> UpdateVendor(Guid id, VendorDto dto);
        Task<bool> DeleteVendors(Guid[] ids);

        Task<bool> DeleteFields(Guid[] ids);
    }

    public class VendorBusinessManager: BaseBusinessManager, IVendorBusinessManager {
        private readonly IMapper _mapper;
        private readonly IVendorManager _vendorManager;
        private readonly IVendorFieldManager _vendorFieldManager;
        private readonly IVendorCategoryManager _vendorCategoryManager;

        public VendorBusinessManager(IMapper mapper, IVendorManager vendorManager,
            IVendorFieldManager vendorFieldManager,
            IVendorCategoryManager vendorCategoryManager) {
            _mapper = mapper;
            _vendorManager = vendorManager;
            _vendorFieldManager = vendorFieldManager;
            _vendorCategoryManager = vendorCategoryManager;
        }

        public async Task<VendorDto> GetVendor(Guid id) {
            var result = await _vendorManager.FindInclude(id);
            return _mapper.Map<VendorDto>(result);
        }

        public async Task<List<VendorDto>> GetVendors() {
            var result = await _vendorManager.FindAll();
            return _mapper.Map<List<VendorDto>>(result);
        }

        public async Task<VendorCategoryDto> GetVendorCategory(Guid id) {
            var result = await _vendorCategoryManager.FindInclude(id);
            return _mapper.Map<VendorCategoryDto>(result);
        }

        public async Task<List<VendorCategoryDto>> GetVendorCategories(Guid vendorId) {
            var result = await _vendorCategoryManager.FindAll(vendorId);
            return _mapper.Map<List<VendorCategoryDto>>(result);
        }

        public async Task<PagerDto<VendorDto>> GetVendorPager(PagerFilterDto filter) {
            var sortby = "Id";

            Expression<Func<VendorEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));

            string[] include = new string[] { "Fields" };

            var tuple = await _vendorManager.Pager<VendorEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerDto<VendorDto>(new List<VendorDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<VendorDto>>(list);
            return new PagerDto<VendorDto>(result, count, page, filter.Length);
        }

        public async Task<VendorDto> CreateVendor(VendorDto dto) {
            var newEntity = _mapper.Map<VendorEntity>(dto);
            var entity = await _vendorManager.Create(newEntity);
            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<VendorDto> UpdateVendor(Guid id, VendorDto dto) {
            var entity = await _vendorManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _vendorManager.Update(newEntity);

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<bool> DeleteVendors(Guid[] ids) {
            var entities = await _vendorManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _vendorManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteFields(Guid[] ids) {
            var entities = await _vendorFieldManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _vendorFieldManager.Delete(entities);
            return result != 0;
        }
    }
}
