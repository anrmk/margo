using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Nsi;
using Core.Extension;

namespace Core.Services.Business {
    public interface INsiBusinessManager {
    }
    public class NsiBusinessManager: BaseBusinessManager, INsiBusinessManager {
        private readonly IMapper _mapper;

        public NsiBusinessManager(IMapper mapper) {
            _mapper = mapper;
        }
    }
}
