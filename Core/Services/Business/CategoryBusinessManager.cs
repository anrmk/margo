using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface ICategoryBusinessManager {
        Task<CategoryDto> GetCategory(Guid id);
        Task<List<CategoryDto>> GetCategories(bool ignoreFilters = false);
        Task<PagerDto<CategoryDto>> GetCategoryPage(CategoryFilterDto filter);
        Task<CategoryDto> CreateCategory(CategoryDto dto);
        Task<CategoryDto> UpdateCategory(Guid id, CategoryDto dto);
        Task<bool> DeleteCategories(Guid[] ids);

        Task<bool> DeleteFields(Guid[] ids);
    }

    public class CategoryBusinessManager: BaseBusinessManager, ICategoryBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICategoryManager _categoryManager;
        private readonly ICategoryFieldManager _categoryFieldManager;
        private readonly IVendorManager _vendorManager;

        public CategoryBusinessManager(IMapper mapper, ICategoryManager categoryManager,
           ICategoryFieldManager categoryFieldManager,
           IVendorManager vendorManager) {
            _mapper = mapper;
            _categoryManager = categoryManager;
            _categoryFieldManager = categoryFieldManager;
            _vendorManager = vendorManager;
        }

        public async Task<CategoryDto> GetCategory(Guid id) {
            var result = await _categoryManager.FindInclude(id);
            return _mapper.Map<CategoryDto>(result);
        }

        public async Task<List<CategoryDto>> GetCategories(bool ignoreFilters) {
            var result = await _categoryManager.FindAll(ignoreFilters);
            return _mapper.Map<List<CategoryDto>>(result);
        }

        public async Task<PagerDto<CategoryDto>> GetCategoryPage(CategoryFilterDto filter) {
            var sortby = "Name";

            Expression<Func<CategoryEntity, bool>> where = x =>
                (string.IsNullOrEmpty(filter.Search) || (x.Name.ToLower().Contains(filter.Search.ToLower())))
                && (!x.Grants.Any(z => z.UserId == filter.UserId));

            string[] include = new string[] { "Fields" };

            var (list, count) = await _categoryManager.Pager<CategoryEntity>(where, sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new PagerDto<CategoryDto>(new List<CategoryDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<CategoryDto>>(list);
            return new PagerDto<CategoryDto>(result, count, page, filter.Length);
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto dto) {
            var newEntity = _mapper.Map<CategoryEntity>(dto);
            var entity = await _categoryManager.Create(newEntity);
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> UpdateCategory(Guid id, CategoryDto dto) {
            var entity = await _categoryManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _categoryManager.Update(newEntity);

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> DeleteCategories(Guid[] ids) {
            var entities = await _categoryManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _categoryManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteFields(Guid[] ids) {
            var entities = await _categoryFieldManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _categoryFieldManager.Delete(entities);
            return result != 0;
        }
    }
}
