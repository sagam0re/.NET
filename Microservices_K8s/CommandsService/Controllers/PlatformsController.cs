using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CommandsService.Data;


namespace CommandsService.Controllers
{
    [Route("api/c/[controller]/[action]")]
    [ApiController]
    public class PlatformsController : ControllerBase
	{
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult Test()
        {
            Console.WriteLine("--> Inbound POST # Commands Service");

            return Ok("Inbound test of from platforms controller");
        }
    }
}