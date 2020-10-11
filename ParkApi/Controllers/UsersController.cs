using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkApi.Models;
using ParkApi.Models.Repository.IRepository;

namespace ParkApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;


        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepository.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepository.isUniqueUser(model.UserName);
            if(!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });

            }

            var user = _userRepository.Register(model.UserName, model.Password);

            if(user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }
    }

}