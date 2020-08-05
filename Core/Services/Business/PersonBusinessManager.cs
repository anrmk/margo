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
    public interface IPersonBusinessManager {
        Task<PersonDto> GetPerson(long id);
        Task<List<PersonDto>> GetPersons();
        Task<Pager<PersonDto>> GetPersonPager(PagerFilter filter);
        Task<PersonDto> CreatePerson(PersonDto dto);
        Task<PersonDto> UpdatePerson(long id, PersonDto dto);
        Task<bool> DeletePersons(long[] ids);
    }

    public class PersonBusinessManager: BaseBusinessManager, IPersonBusinessManager {
        private readonly IMapper _mapper;
        private readonly IPersonManager _personManager;

        public PersonBusinessManager(IMapper mapper, IPersonManager personManager) {
            _mapper = mapper;
            _personManager = personManager;
        }

        public async Task<PersonDto> GetPerson(long id) {
            var result = await _personManager.FindInclude(id);
            return _mapper.Map<PersonDto>(result);
        }

        public async Task<List<PersonDto>> GetPersons() {
            var result = await _personManager.FindAll();
            return _mapper.Map<List<PersonDto>>(result);
        }

        public async Task<Pager<PersonDto>> GetPersonPager(PagerFilter filter) {
            var sortby = "Id";

            Expression<Func<PersonEntity, bool>> where = x =>
                    (string.IsNullOrEmpty(filter.Search)
                    || x.Name.ToLower().Contains(filter.Search.ToLower())
                    || x.Description.ToLower().Contains(filter.Search.ToLower()));

            string[] include = new string[] { "Accounts" };

            var (list, count) = await _personManager.Pager<PersonEntity>(where, sortby, filter.Start, filter.Length, include);

            if(count == 0)
                return new Pager<PersonDto>(new List<PersonDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<PersonDto>>(list);
            return new Pager<PersonDto>(result, count, page, filter.Length);
        }

        public async Task<PersonDto> CreatePerson(PersonDto dto) {
            var newEntity = _mapper.Map<PersonEntity>(dto);
            var entity = await _personManager.Create(newEntity);
            return _mapper.Map<PersonDto>(entity);
        }

        public async Task<PersonDto> UpdatePerson(long id, PersonDto dto) {
            var entity = await _personManager.FindInclude(id);
            if(entity == null) {
                return null;
            }

            var newEntity = _mapper.Map(dto, entity);
            entity = await _personManager.Update(newEntity);

            return _mapper.Map<PersonDto>(entity);
        }

        public async Task<bool> DeletePersons(long[] ids) {
            var entities = await _personManager.FindAll(ids);
            if(entities == null)
                throw new Exception("Did not find any records.");

            int result = await _personManager.Delete(entities);
            return result != 0;
        }
    }
}
