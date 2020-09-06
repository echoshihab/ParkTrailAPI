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
    [ProducesResponseType(StatusCodes.Status400BadRequest]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParksController(IMapper mapper, INationalParkRepository  nationalParkRepository)
        {
            _mapper = mapper;
            _nationalParkRepository = nationalParkRepository;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<NationalParkDto>))]

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


        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId">The Id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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
        /// <summary>
        /// Creates a New National Park
        /// </summary>
        /// <param name="nationalParkDto">National Park DTO</param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Updates National Park
        /// </summary>
        /// <param name="nationalParkId">Id of National Park</param>
        /// <param name="nationalParkDto">National Park DTO</param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes National Parl
        /// </summary>
        /// <param name="nationalParkId">Id of National Park</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _nationalParkRepository.GetNationalPark(nationalParkId);



            if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error deleting {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }

}