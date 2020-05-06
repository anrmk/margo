
using AutoMapper;

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
