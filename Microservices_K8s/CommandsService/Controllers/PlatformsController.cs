using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;


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

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Plaforms from CommandsServicec");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult Test()
        {
            Console.WriteLine("--> Inbound POST # Commands Service");

            return Ok("Inbound test of from platforms controller");
        }
    }
}