using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface ICategoryBusinessManager {
        Task<CategoryDto> GetCategory(long id);
        Task<List<CategoryDto>> GetCategories();
        Task<Pager<CategoryDto>> GetCategoryPage(PagerFilter filter);
        Task<CategoryDto> CreateCategory(CategoryDto dto);
        Task<CategoryDto> UpdateCategory(long id, CategoryDto dto);
        
        Task<bool> DeleteCategories(long[] ids);
        Task<bool> DeleteFields(long[] ids);
    }

    public class CategoryBusinessManager: BaseBusinessManager, ICategoryBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICategoryManager _categoryManager;
        private readonly ICategoryFieldManager _categoryFieldManager;

        public CategoryBusinessManager(IMapper mapper, ICategoryManager categoryManager,
           ICategoryFieldManager categoryFieldManager) {
            _mapper = mapper;
            _categoryManager = categoryManager;
            _categoryFieldManager = categoryFieldManager;
        }

        public async Task<CategoryDto> GetCategory(long id) {
            var result = await _categoryManager.FindInclude(id);
            return _mapper.Map<CategoryDto>(result);
        }

        public async Task<List<CategoryDto>> GetCategories() {
            var result = await _categoryManager.FindAll();
            return _mapper.Map<List<CategoryDto>>(result);
        }

        public async Task<Pager<CategoryDto>> GetCategoryPage(PagerFilter filter) {
            var sortby = "Name";

            Expression<Func<CategoryEntity, bool>> where = x =>
                   (true)
                   && (string.IsNullOrEmpty(filter.Search) || (x.Name.ToLower().Contains(filter.Search.ToLower())));

            string[] include = new string[] { "Fields" };

            var tuple = await _categoryManager.Pager<CategoryEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<CategoryDto>(new List<CategoryDto>(), 0, filter.Length, filter.Start);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<CategoryDto>>(list);
            return new Pager<CategoryDto>(result, count, page, filter.Length);
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto dto) {
            var newEntity = _mapper.Map<CategoryEntity>(dto);
            var entity = await _categoryManager.Create(newEntity);
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> UpdateCategory(long id, CategoryDto dto) {
            var entity = await _categoryManager.FindInclude(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _categoryManager.Update(newEntity);

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> DeleteCategories(long[] ids) {
            var entities = await _categoryManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _categoryManager.Delete(entities);
            return result != 0;
        }

        public async Task<bool> DeleteFields(long[] ids) {
            var entities = await _categoryFieldManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find records for this request!");

            int result = await _categoryFieldManager.Delete(entities);
            return result != 0;
        }
    }
}
