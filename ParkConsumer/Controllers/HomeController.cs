using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkConsumer.Models;
using ParkConsumer.Models.ViewModel;
using ParkConsumer.Repository.IRepository;

namespace ParkConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly IAccountRepository _accountRepository;
        public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepository, 
            ITrailRepository trailRepository, IAccountRepository accountRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM parksAndTrails = new IndexVM()
            {
                NationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                Trails = await _trailRepository.GetAllAsync(SD.TrailAPIPath),
            };
           return View(parksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            if(objUser.Token == null)
            {
                return View();
            }
            HttpContext.Session.SetString("JWToken", objUser.Token);
            return RedirectToAction("~/Home/Index");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accountRepository.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }
            return RedirectToAction("~/Home/Login");

        }

        public async Task<IActionResult> LogoutAsync()
        {
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("~/Home/Index");

        }


    }
}
