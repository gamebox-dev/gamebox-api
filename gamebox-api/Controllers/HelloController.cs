using gamebox_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace gamebox_api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet(Name = "Hello")]
        public Hello Get()
        {
            return new Hello() { Status = "ok" };
        }
    }
}
