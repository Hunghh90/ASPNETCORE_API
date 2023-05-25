using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartsController : Controller
    {
        private AspnetcoreApiContext _context;
        public CartsController(AspnetcoreApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var cart = _context.Carts.ToList<Cart>();
            return Ok(cart);
        }

        [HttpPost]
        public ActionResult Create(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return Created($"/get-by-id/id?={cart.UserId}", cart);
        }


        [HttpPut]
        public ActionResult Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch]
        public ActionResult AddPlus(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart == null)
                return NotFound();
            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
