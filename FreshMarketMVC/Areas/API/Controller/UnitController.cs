using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganicFoodMVC.DataAccess.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreshMarketMVC.Areas.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly ILogger<UnitController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnitController(ILogger<UnitController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Unit.GetAll());
        }
    }
}
