using FreshMarketMVC.Areas.API.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OrganicFoodMVC.DataAccess.Repository.IRepository;
using OrganicFoodMVC.Models;
using OrganicFoodMVC.Models.ViewModels;
using System;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreshMarketMVC.Areas.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Product.GetAll());
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]

        public IActionResult Get(string id)
        {
            if(!int.TryParse(id, out int Id))
                return BadRequest("Id không đúng định dạng");
            Product product = _unitOfWork.Product.Get(Id);
            return product == null? BadRequest("Product không tồn tại"): Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post([FromBody] ProductRequest p)
        {
            //valitdate missing
            if(string.IsNullOrEmpty(p.Name))
                return BadRequest("Nhập thiếu trường Name!");

            if (string.IsNullOrEmpty(p.Discription))
                return BadRequest("Nhập thiếu trường Discription!");

            if (string.IsNullOrEmpty(p.Quantity))
                return BadRequest("Nhập thiếu trường Quantity!");

            if (string.IsNullOrEmpty(p.Price))
                return BadRequest("Nhập thiếu trường Price!");

            if (string.IsNullOrEmpty(p.CategoryId))
                return BadRequest("Nhập thiếu trường CategoryId!");

            if (string.IsNullOrEmpty(p.BrandId))
                return BadRequest("Nhập thiếu trường BrandId!");

            if (string.IsNullOrEmpty(p.UnitId))
                return BadRequest("Nhập thiếu trường UnitId!");

            //validation type
            if(!int.TryParse(p.Quantity, out int Quantity))
                return BadRequest("Nhập không đúng định dạng trường Quantity!");

            if (!long.TryParse(p.Price, out long Price))
                return BadRequest("Nhập không đúng định dạng trường Price!");

            if (!int.TryParse(p.CategoryId, out int CategoryId))
                return BadRequest("Nhập không đúng định dạng trường CategoryId!");

            if (!int.TryParse(p.BrandId, out int BrandId))
                return BadRequest("Nhập không đúng định dạng trường BrandId!");

            if (!int.TryParse(p.UnitId, out int UnitId))
                return BadRequest("Nhập không đúng định dạng trường UnitId!");

            //validation database
            if (_unitOfWork.Category.Get(CategoryId) == null)
                return BadRequest("CategoryId không tồn tại!");

            if (_unitOfWork.Brand.Get(BrandId) == null)
                return BadRequest("BrandId không tồn tại!");

            if (_unitOfWork.Unit.Get(UnitId) == null)
                return BadRequest("UnitId không tồn tại!");

            Product product = new Product()
            {
                Name = p.Name,
                Discription = p.Discription,
                Quantity = Quantity,
                Price = Price,
                ImageUrl = "",
                CategoryId = CategoryId,
                BrandId = BrandId,
                UnitId = UnitId,
                CreateAt = DateTime.Now
            };

            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();
            return Ok("Thêm thành công");
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] ProductRequest p)
        {
            if (!int.TryParse(id, out int Id))
                return BadRequest("Id không đúng định dạng");
            Product product = _unitOfWork.Product.Get(Id);
            if(product == null) 
                return BadRequest("Product không tồn tại");

            //valitdate missing
            if (string.IsNullOrEmpty(p.Name))
                return BadRequest("Nhập thiếu trường Name!");

            if (string.IsNullOrEmpty(p.Discription))
                return BadRequest("Nhập thiếu trường Discription!");

            if (string.IsNullOrEmpty(p.Quantity))
                return BadRequest("Nhập thiếu trường Quantity!");

            if (string.IsNullOrEmpty(p.Price))
                return BadRequest("Nhập thiếu trường Price!");

            if (string.IsNullOrEmpty(p.CategoryId))
                return BadRequest("Nhập thiếu trường CategoryId!");

            if (string.IsNullOrEmpty(p.BrandId))
                return BadRequest("Nhập thiếu trường BrandId!");

            if (string.IsNullOrEmpty(p.UnitId))
                return BadRequest("Nhập thiếu trường UnitId!");

            //validation type
            if (!int.TryParse(p.Quantity, out int Quantity))
                return BadRequest("Nhập không đúng định dạng trường Quantity!");

            if (!long.TryParse(p.Price, out long Price))
                return BadRequest("Nhập không đúng định dạng trường Price!");

            if (!int.TryParse(p.CategoryId, out int CategoryId))
                return BadRequest("Nhập không đúng định dạng trường CategoryId!");

            if (!int.TryParse(p.BrandId, out int BrandId))
                return BadRequest("Nhập không đúng định dạng trường BrandId!");

            if (!int.TryParse(p.UnitId, out int UnitId))
                return BadRequest("Nhập không đúng định dạng trường UnitId!");

            //validation database
            if (_unitOfWork.Category.Get(CategoryId) == null)
                return BadRequest("CategoryId không tồn tại!");

            if (_unitOfWork.Brand.Get(BrandId) == null)
                return BadRequest("BrandId không tồn tại!");

            if (_unitOfWork.Unit.Get(UnitId) == null)
                return BadRequest("UnitId không tồn tại!");

            product.Name = p.Name;
            product.Discription = p.Discription;
            product.Quantity = Quantity;
            product.Price = Price;
            product.ImageUrl = "";
            product.CategoryId = CategoryId;
            product.BrandId = BrandId;
            product.UnitId = UnitId;
            product.CreateAt = DateTime.Now;
            
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            return Ok("Sửa sản phẩm thành công");
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!int.TryParse(id, out int Id))
                return BadRequest("Id không đúng định dạng");
            Product product = _unitOfWork.Product.Get(Id);
            if (product == null)
                return BadRequest("Product không tồn tại");

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Ok("Xóa sản phẩm thành công");
        }
    }
}
