using gamebox.Models;
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
        public HealthStatus Get()
        {
            return new HealthStatus() { Status = "ok" };
        }
    }
}
