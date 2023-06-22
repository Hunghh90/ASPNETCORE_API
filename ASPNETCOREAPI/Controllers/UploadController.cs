using Microsoft.AspNetCore.Mvc;

namespace ASPNETCOREAPI.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {

        [HttpPost]
        [Route("image")]
        public ActionResult Index(IFormFile image)
        {
            if(image == null)
            {
                return BadRequest("Vui Long chon file");
            }
            var path = "wwwroot/uploads";
            var fileName = Guid.NewGuid().ToString()+Path.GetFileName(image.FileName);
            var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
            image.CopyTo(new FileStream(upload, FileMode.Create));
            var rs = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(rs)
;        }
    }
}
