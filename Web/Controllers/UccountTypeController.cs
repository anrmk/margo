using AutoMapper;

using Core.Services;
using Core.Services.Business;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Mvc {
    public class UccountTypeController: BaseController<UccountTypeController> {
        private readonly IUccountBusinessManager _uccountBusinessManager;

        public UccountTypeController(ILogger<UccountTypeController> logger, IMapper mapper,
            IUccountBusinessManager uccountBusinessManager) : base(logger, mapper) {
            _uccountBusinessManager = uccountBusinessManager;

        }
        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    public class UccountTypeController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUccountBusinessManager _uccountBusinessManager;

        public UccountTypeController(IMapper mapper, IViewRenderService viewRenderService,
            IUccountBusinessManager uccountBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _uccountBusinessManager = uccountBusinessManager;
        }
    }
}