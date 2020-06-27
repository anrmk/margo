using AutoMapper;

using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IUccountBusinessManager {

    }
    public class UccountBusinessManager: IUccountBusinessManager {
        private readonly IMapper _mapper;
        private readonly ICompanyManager _companyManager;

        public UccountBusinessManager(IMapper mapper,
            ICompanyManager companyManager) {
            _mapper = mapper;
            _companyManager = companyManager;
        }
    }
}
