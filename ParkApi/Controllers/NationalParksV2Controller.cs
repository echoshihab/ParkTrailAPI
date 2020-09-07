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
    [Route("api/v{version:apiVersion}/nationalParks")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkNPAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(IMapper mapper, INationalParkRepository  nationalParkRepository)
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
            var nationalParks = _nationalParkRepository.GetNationalParks().Take(3);
            var nationalParksDto = new List<NationalParkDto>();

            foreach(var park in nationalParks)
            {
                nationalParksDto.Add(_mapper.Map<NationalParkDto>(park));
            }

            return Ok(nationalParksDto);
        }



    }

}