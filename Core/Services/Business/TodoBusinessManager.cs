using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Data.Enums;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface ITodoBusinessManager {
        Task<TodoDto> GetTodo(Guid id);
        Task<PagerDto<TodoDto>> GetTodoPage(TodoFilterDto filter);
        Task<TodoDto> CreateTodo(TodoDto dto);
        Task<TodoDto> UpdateTodo(Guid id, TodoDto dto);
        Task<bool> DeleteTodo(Guid[] ids);
    }

    public class TodoBusinessManager: ITodoBusinessManager {
        private readonly IMapper _mapper;
        private readonly ITodoManager _todoManager;

        public TodoBusinessManager(IMapper mapper,
           ITodoManager todoManager) {
            _mapper = mapper;
            _todoManager = todoManager;
        }

        public async Task<TodoDto> GetTodo(Guid id) {
            var result = await _todoManager.FindInclude(id);
            return _mapper.Map<TodoDto>(result);
        }

        public async Task<PagerDto<TodoDto>> GetTodoPage(TodoFilterDto filter) {
            var sortby = filter.SortingBy switch {
                TodoSortingEnum.Priority=>"Priority",
                TodoSortingEnum.DateCreation=> "CreatedDate",
                _ => string.Empty
            };

            if(filter.DateTo.HasValue) {
                filter.DateTo = filter.DateTo.Value.AddDays(1).AddTicks(-1);
            }

            Expression<Func<TodoEntity, bool>> where = x =>
                (string.IsNullOrEmpty(filter.Text) || x.Description.ToLower().Contains(filter.Text.ToLower()))
                && (!filter.Type.HasValue
                    || (filter.Type == TodoUserTypeEnum.Mine
                        ? (x.UserId == filter.UserId && x.CreatedBy == filter.UserLogin)
                        : filter.Type == TodoUserTypeEnum.ToMe
                            ? (x.UserId == filter.UserId && x.CreatedBy != filter.UserLogin)
                            : (x.UserId != filter.UserId && x.CreatedBy == filter.UserLogin)))
                && (filter.IncludeCompleted || !filter.IncludeCompleted && !x.IsCompleted)
                && (!filter.Priority.HasValue || filter.Priority == x.Priority)
                && (!filter.DateFrom.HasValue || filter.DateFrom <= x.CreatedDate)
                && (!filter.DateTo.HasValue || filter.DateTo >= x.CreatedDate);

            string[] include = new string[] { "User" };

            var (list, count) = await _todoManager.Pager<TodoEntity>(where, sortby, filter.Start, filter.Length, include);
            if(count == 0)
                return new PagerDto<TodoDto>(new List<TodoDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<TodoDto>>(list);
            return new PagerDto<TodoDto>(result, count, page, filter.Length);
        }

        public async Task<TodoDto> CreateTodo(TodoDto dto) {
            var entity = await _todoManager.Create(_mapper.Map<TodoEntity>(dto));
            return _mapper.Map<TodoDto>(entity);
        }

        public async Task<TodoDto> UpdateTodo(Guid id, TodoDto dto) {
            var entity = await _todoManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _todoManager.Update(newEntity);

            return _mapper.Map<TodoDto>(entity);
        }

        public async Task<bool> DeleteTodo(Guid[] ids) {
            var entities = await _todoManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _todoManager.Delete(entities);
            return result != 0;
        }
    }
}
