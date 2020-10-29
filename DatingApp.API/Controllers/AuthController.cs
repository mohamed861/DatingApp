using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;

        public AuthController(IAuthRepository _authRepository, IConfiguration _configuration)
        {
            authRepository = _authRepository;
            configuration = _configuration;
        }   

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userRegisterDto)
        {
            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);
            userRegisterDto.UserName = userRegisterDto.UserName.ToLower();    
            if(await authRepository.UserExists(userRegisterDto.UserName))
                return BadRequest("The user name used before.");

            var userToCreate= new User()
            {
                UserName=userRegisterDto.UserName
            };

            var createdUser = authRepository.Register(userToCreate, userRegisterDto.Password);
            return StatusCode(201);
            //return CreatedAtRoute()
            //return Created("",user);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDTO userLoginDTO)
        {
            var userFromRepo = await authRepository.Login(userLoginDTO.UserName.ToLower(), userLoginDTO.Password);
            if(userFromRepo == null)
                return Unauthorized();
            
            var issuer= "http://localhost:5000";
            var audience= "http://localhost:4200";

            var claims=new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };

            var key= new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            
            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(200),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}