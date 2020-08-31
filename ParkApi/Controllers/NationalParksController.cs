using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkApi.Models.Dtos;
using ParkApi.Models.Repository.IRepository;

namespace ParkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParksController(IMapper mapper, INationalParkRepository  nationalParkRepository)
        {
            _mapper = mapper;
            _nationalParkRepository = nationalParkRepository;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _nationalParkRepository.GetNationalParks();
            var nationalParksDto = new List<NationalParkDto>();

            foreach(var park in nationalParks)
            {
                nationalParksDto.Add(_mapper.Map<NationalParkDto>(park));
            }

            return Ok(nationalParksDto);
        }

        [HttpGet("{nationalParkId:int}")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);

            if(nationalPark == null)
            {
                return NotFound();
            }

            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);

            return Ok(nationalParkDto);


        }
    }
}