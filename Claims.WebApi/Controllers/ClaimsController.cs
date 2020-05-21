using Microsoft.AspNetCore.Mvc;

namespace Claims.WebApi.Controllers
{
    [ApiController]
    [Route("claims")]
    public class ClaimsController: ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}