using GameBox.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GameBox.Controllers
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
