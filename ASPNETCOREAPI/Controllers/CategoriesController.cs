using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly AspnetcoreApiContext _context;
        public CategoriesController(AspnetcoreApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var category = _context.Categories.ToList<Category>();
            return Ok(category);
        }

        [HttpGet]
        [Route("get-by-id")]
        public ActionResult GetById(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id)
                .Include(category => category.Products).First();
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Created($"/get-by-id/id?={category.Id}", category);
        }

        [HttpPut]
        public ActionResult Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
