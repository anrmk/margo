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
    public interface IUccountBusinessManager {
        //  UCCOUNT
        Task<UccountDto> GetUccount(long id);
        Task<Pager<UccountDto>> GetUccountPage(PagerFilter filter);
        Task<List<UccountDto>> GetUccounts();
        Task<UccountDto> CreateUccount(UccountDto dto);
        Task<UccountDto> UpdateUccount(long id, UccountDto dto);
        Task<bool> DeleteUccount(long id);
        Task<bool> DeleteUccount(long[] ids);

        //  UCCOUNT SECTION 
        Task<UccountSectionDto> GetSection(long id);

        Task<UccountSectionFieldDto> GetSectionField(long id);
        Task<List<UccountSectionFieldDto>> GetSectionFields(long sectionId);
    }
    public class UccountBusinessManager: IUccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly IUccountManager _uccountManager;
        private readonly IUccountSectionManager _uccountSectionManager;
        private readonly IUccountSectionFieldManager _uccountSectionFieldManager;

        private readonly ICompanyManager _companyManager;
        private readonly ISectionManager _sectionManager;

        public UccountBusinessManager(IMapper mapper,
            IUccountManager uccountManager,
            IUccountSectionManager uccountSectionManager,
            IUccountSectionFieldManager uccountSectionFieldManager,
            ICompanyManager companyManager,
            ISectionManager sectionManager) {
            _mapper = mapper;
            _uccountManager = uccountManager;
            _uccountSectionManager = uccountSectionManager;
            _uccountSectionFieldManager = uccountSectionFieldManager;
            _companyManager = companyManager;
            _sectionManager = sectionManager;
        }

        #region UCCOUNT

        public async Task<UccountDto> GetUccount(long id) {
            var result = await _uccountManager.Find(id);
            return _mapper.Map<UccountDto>(result);
        }

        public async Task<Pager<UccountDto>> GetUccountPage(PagerFilter filter) {
            var sortby = "Id";

            Expression<Func<UccountEntity, bool>> where = x =>
                   (true);
            //&& (string.IsNullOrEmpty(filter.Search) || (x.No.ToLower().Contains(filter.Search.ToLower()) || x.Name.ToLower().Contains(filter.Search.ToLower())));

            string[] include = new string[] { "Company", "Vendor", "Section" };

            var tuple = await _uccountManager.Pager<UccountEntity>(where, sortby, filter.Start, filter.Length, include);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<UccountDto>(new List<UccountDto>(), 0, filter.Start, filter.Length);

            var page = (filter.Start + filter.Length) / filter.Length;

            var result = _mapper.Map<List<UccountDto>>(list);
            return new Pager<UccountDto>(result, count, page, filter.Length);
        }

        public async Task<List<UccountDto>> GetUccounts() {
            var result = await _uccountManager.All();
            return _mapper.Map<List<UccountDto>>(result);
        }

        public async Task<UccountDto> CreateUccount(UccountDto dto) {
            //var template = await _sectionManager.FindInclude(dto.TemplateId);
            //if(template != null) {
            //var sectionEntity = new UccountSectionEntity() {
            //    Code = template.Code,
            //    Name = template.Name,
            //    Description = template.Description
            //};

            //sectionEntity = await _uccountSectionManager.Create(sectionEntity);

            //var fieldsEntity = template.Fields.Select(x => new UccountSectionFieldEntity() {
            //    Name = x.Name,
            //    SectionId = sectionEntity.Id,
            //    Type = x.Type,
            //    Value = ""
            //});
            //fieldsEntity = await _uccountSectionFieldManager.Create(fieldsEntity);

            //dto.SectionId = sectionEntity.Id;

            var entity = await _uccountManager.Create(_mapper.Map<UccountEntity>(dto));
            return _mapper.Map<UccountDto>(entity);
            //}
            //return null;
        }

        public async Task<UccountDto> UpdateUccount(long id, UccountDto dto) {
            var entity = await _uccountManager.Find(id);
            if(entity == null) {
                return null;
            }
            var newEntity = _mapper.Map(dto, entity);
            entity = await _uccountManager.Update(newEntity);

            return _mapper.Map<UccountDto>(entity);
        }

        public async Task<bool> DeleteUccount(long id) {
            return await DeleteUccount(new long[] { id });
        }

        public async Task<bool> DeleteUccount(long[] ids) {
            var entities = await _uccountManager.FindAll(ids);
            if(entities == null)
                throw new Exception("We did not find field records for this request!");

            int result = await _uccountManager.Delete(entities);
            return result != 0;
        }


        #endregion

        public async Task<UccountSectionDto> GetSection(long id) {
            var result = await _uccountSectionManager.Find(id);
            return _mapper.Map<UccountSectionDto>(result);
        }

        public async Task<UccountSectionFieldDto> GetSectionField(long id) {
            var result = await _uccountSectionFieldManager.Find(id);
            return _mapper.Map<UccountSectionFieldDto>(result);
        }

        public async Task<List<UccountSectionFieldDto>> GetSectionFields(long sectionId) {
            var result = await _uccountSectionFieldManager.FindAll(sectionId);
            return _mapper.Map<List<UccountSectionFieldDto>>(result);
        }
    }
}
