using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Dtos.Request;
using ReminderToEmail.Dtos.Response;
using ReminderToEmail.Helper;
using ReminderToEmail.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReminderToEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AccountController> logger;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork unitOfWork, ILogger<AccountController> logger,IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.configuration = configuration;
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var model = await unitOfWork.userRepository.GetAll();
            if(model!=null)
            {
                return Ok(model.Select(x=>new UserDto()
                {
                    Id=x.Id,
                    Email=x.email
                }));
            }
            return NotFound();
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var exist = await unitOfWork.userRepository.GetByEmail(model.Email);

            if(exist!=null)
            {
                return BadRequest("This email is exist");
            }


            var new_user = new User()
            {
                email = model.Email,
                password= new PasswordHash(model.Password).ToArray()
            };
            var result = await unitOfWork.userRepository.Add(new_user);

            await unitOfWork.saveAsync();

            return Ok("Successfully");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var exist = await unitOfWork.userRepository.GetByEmail(model.Email);

            if(exist == null)
            {
                return NotFound("This user not found");
            }

            var verify = new PasswordHash(exist.password).Verify(model.Password);

            if(verify)
            {
                var token = CreateToken(exist.Id);
                return Ok(new {Message="Success",Token=token});
            }
            return BadRequest("Failed");
        }
        private string CreateToken(Guid Id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,Id.ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                expires: DateTime.UtcNow.AddHours(10),
                claims: claims,
                signingCredentials: creds
            ); ;

            return tokenHandler.WriteToken(token);
        }
    }
}
