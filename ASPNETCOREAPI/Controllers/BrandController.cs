using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandController : Controller
    {
        private readonly AspnetcoreApiContext _context;
        public BrandController(AspnetcoreApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var brand = _context.Brands.ToList<Brand>();
            return Ok(brand);
        }

        [HttpGet]
        [Route("get-by-id")]
        public ActionResult GetById(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return Created($"/get-by-id/id?={brand.Id}", brand);
        }

        [HttpPut]
        public ActionResult Update(Brand brand)
        {
            _context.Brands.Update(brand);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
                return NotFound();
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
