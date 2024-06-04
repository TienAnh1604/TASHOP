using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganicFoodMVC.DataAccess.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreshMarketMVC.Areas.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Category.GetAll());
        }
    }
}
