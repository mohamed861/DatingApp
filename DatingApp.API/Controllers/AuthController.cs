using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository _authRepository)
        {
            authRepository = _authRepository;
        }   

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDto)
        {
            if(await authRepository.UserExists(userRegisterDto.UserName.ToLower()))
                return BadRequest("The user name used before.");

            var userToCreate= new User()
            {
                UserName=userRegisterDto.UserName
            };

            var user = authRepository.Register(userToCreate, userRegisterDto.Password);
            
            return StatusCode(201);
            //return CreatedAtRoute()
            //return Created("",user);
        }
    }
}