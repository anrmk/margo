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
        //  VENDOR
        Task<VendorDto> GetVendor(long id);
        Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter);
        Task<List<VendorDto>> GetVendors();
        Task<VendorDto> CreateVendor(VendorGeneralDto dto);
        Task<VendorDto> UpdateVendor(long id, VendorGeneralDto dto);
        Task<bool> DeleteVendor(long[] ids);

        //  VENDOR ADDRESS
        Task<VendorAddressDto> GetVendorAddress(long id);
        Task<VendorAddressDto> UpdateVendorAddress(long companyId, VendorAddressDto dto);

        //  VENDOR SECTION
        Task<List<VendorSectionDto>> GetVendorSections(long vendorId);
    }

    public class VendorBusinessManager: IVendorBusinessManager {
        private readonly IMapper _mapper;
        private readonly IVendorManager _vendorManager;
        private readonly IVendorAddressManager _vendorAddressManager;
        private readonly IVendorSectionManager _vendorSectionManager;
        private readonly IVendorSectionFieldManager _vendorSectionFieldManager;

        public VendorBusinessManager(IMapper mapper,
            IVendorManager supplierManager,
            IVendorAddressManager vendorAddressManager,
            IVendorSectionManager vendorSectionManager,
            IVendorSectionFieldManager vendorSectionFieldManager) {
            _mapper = mapper;
            _vendorManager = supplierManager;
            _vendorAddressManager = vendorAddressManager;
            _vendorSectionManager = vendorSectionManager;
            _vendorSectionFieldManager = vendorSectionFieldManager;
        }

        #region VENDOR
        public async Task<VendorDto> GetVendor(long id) {
            var result = await _vendorManager.FindInclude(id);
            return _mapper.Map<VendorDto>(result);
        }

        public async Task<Pager<VendorDto>> GetVendorPager(PagerFilter filter) {
            var sortby = "No";

            Expression<Func<VendorEntity, bool>> where = x =>
                   (true)
                && (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.No.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));

            string[] include = new string[] { "Address" };

            var tuple = await _vendorManager.Pager<VendorEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<VendorDto>(new List<VendorDto>(), 0, filter.Length, filter.Start);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<VendorDto>>(list);
            return new Pager<VendorDto>(result, count, page, filter.Length);
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

        public async Task<bool> DeleteVendor(long[] ids) {
            var companies = await _vendorManager.Find(ids);
            int result = await _vendorManager.Delete(companies);
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

        public async Task<List<VendorSectionDto>> GetVendorSections(long vendorId) {
            var result = await _vendorSectionManager.FindAll(vendorId);
            return _mapper.Map<List<VendorSectionDto>>(result);
        }
    }
}
