using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly AspnetcoreApiContext _context;
        public ProductController(AspnetcoreApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var products = _context.Products.Include(products => products.Category).ToArray();
            return Ok(products);
        }

        [HttpGet]
        [Route("get-by-id")]
        public ActionResult GetById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created($"/get-by-id/id?={product.Id}", product);
        }

        [HttpPut]
        public ActionResult Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public ActionResult Search(string? q, int? limit, int? page)
        {
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) * limit); // số lượng bỏ qua
            var products = _context.Products.Where(p => p.Name.Contains(q)).Skip(offset).Take((int)limit).ToArray(); //Skip = số lượng bỏ qua, Take = Số lượng muốn lấy
            return Ok(products);
        }
    }
}
