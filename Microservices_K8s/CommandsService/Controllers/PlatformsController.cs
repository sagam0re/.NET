using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]/[action]")]
    [ApiController]
    public class PlatformsController : ControllerBase
	{
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult Test()
        {
            Console.WriteLine("--> Inbound POST # Commands Service");

            return Ok("Inbound test of from platforms controller");
        }
    }
}