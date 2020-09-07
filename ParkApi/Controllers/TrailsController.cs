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
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkTrailAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailsController(IMapper mapper, ITrailRepository  trailRepository)
        {
            _mapper = mapper;
            _trailRepository = trailRepository;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<TrailDto>))]

        public IActionResult GetTrails()
        {
            var trails = _trailRepository.GetTrails();
            var trailsDto = new List<TrailDto>();

            foreach(var trail in trails)
            {
                trailsDto.Add(_mapper.Map<TrailDto>(trail));
            }

            return Ok(trailsDto);
        }


        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);

            if(trail == null)
            {
                return NotFound();
            }

            var trailDto = _mapper.Map<TrailDto>(trail);

            return Ok(trailDto);


        }
        /// <summary>
        /// Creates a New Trail
        /// </summary>
        /// <param name="trailCreateDto">National Park DTO</param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail(TrailCreateDto trailCreateDto)
        {
            if(trailCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if(_trailRepository.TrailExists(trailCreateDto.Name))
            {
                ModelState.AddModelError("", "Trail already exists");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailCreateDto);

      

            if(!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error saving {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
        
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);

        }

        /// <summary>
        /// Updates Trail
        /// </summary>
        /// <param name="trailId">Id of Trail</param>
        /// <param name="trailUpdateDto">Trail DTO</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, TrailUpdateDto trailUpdateDto)
        {
            if (trailUpdateDto == null || trailId != trailUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailUpdateDto);



            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error updating {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        /// <summary>
        /// Deletes Trail
        /// </summary>
        /// <param name="trailId">Id of Trail</param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if(!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailObj = _trailRepository.GetTrail(trailId);



            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error deleting {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }

}