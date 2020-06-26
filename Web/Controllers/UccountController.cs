using AutoMapper;
using Core.Services;
using Core.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    [Authorize]
    public class UccountController: BaseController<UccountController> {
        private readonly IUccountBusinessManager _uccountBusinessManager;
        public UccountController(ILogger<UccountController> logger, IMapper mapper,
            IUccountBusinessManager uccountBusinessManager) : base(logger, mapper) {
            _uccountBusinessManager = uccountBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }
    }
}

namespace Web.Controllers.Api {
    public class UccountController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUccountBusinessManager _uccountBusinessManager;

        public UccountController(IMapper mapper, IViewRenderService viewRenderService,
            IUccountBusinessManager uccountBusinessManager) {
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _uccountBusinessManager = uccountBusinessManager;
        }
    }
}
