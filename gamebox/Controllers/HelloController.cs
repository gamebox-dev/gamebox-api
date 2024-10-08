using GameBox.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GameBox.Controllers
{
    /// <summary>
    /// Status check controller for the GameBox API
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        /// <summary>
        /// Returns the current status of the GameBox server
        /// </summary>
        /// <returns>"ok" if status is 200 ok</returns>
        [HttpGet(Name = "Hello")]
        public HealthStatus Get()
        {
            return new HealthStatus() { Status = "ok" };
        }
    }
}
