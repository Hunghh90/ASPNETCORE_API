using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly AspnetcoreApiContext _context;
        public UserController(AspnetcoreApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var user = _context.Users.ToList<User>();
            return Ok(user);
        }

        [HttpGet]
        [Route("get-by-id")]
        public ActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Created($"/get-by-id/id?={user.Id}", user);
        }

        [HttpPut]
        public ActionResult Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
