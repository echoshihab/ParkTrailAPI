using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkConsumer.Models;
using ParkConsumer.Repository.IRepository;

namespace ParkConsumer.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParksController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if(id == null)
            {
                return View(obj);
            }
            obj = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            var data = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            return Json(new { data });
        }

    }
}