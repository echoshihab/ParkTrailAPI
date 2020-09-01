using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkApi.Models;
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

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
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

        [HttpPost]
        public IActionResult CreateNationalPark(NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if(_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "This Park already exists");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

      

            if(!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error saving {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
        
            //because of savechanges(), we can see nationalparkobj.id because entity framework
            //by defeault follows each INSERT with SELECT_SCOPE_IDENTITY()
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);

        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);



            if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error updating {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }

}