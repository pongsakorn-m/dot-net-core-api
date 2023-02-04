using DotNetCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config) 
        {
            _config = config;
            var x = _config.GetValue<string>("Secret");
        }
        public static List<User> Users = new List<User>()
        {
            new User() { Username = "pongsakorn_user", Password = "123456", FullName = "Pongsakorn Mukdavannakorn", FailCount = 0 },
            new User() { Username = "mockup_user", Password = "123456", FullName = "Mokcup User", FailCount = 0 }
        };

        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
            int maxLoginFail = _config.GetValue<int>("MaxLoginFail");
            if (model is null || !ModelState.IsValid) 
            { 
                return BadRequest("Please input your username and password.");
            }

            User user = Users.FirstOrDefault(user => user.Username == model.UserName);

            if (user is null)
            {
                return BadRequest("Your username or password is incorrect.");
            }

            if (user.Password != model.Password)
            {
                user.FailCount++;
                if (user.FailCount >= maxLoginFail)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(_config.GetValue<int>("LockAccount"));
                }
                return BadRequest("Your username or password is incorrect.");
            }

            if (user.FailCount >= maxLoginFail && user.LockoutEnd > DateTime.Now)
            {
                TimeSpan ts = (user.LockoutEnd.GetValueOrDefault()) - DateTime.Now;
                return BadRequest(string.Format("Your account is locked. Please try again in {0} minutes.", ts.TotalMinutes));
            }
            var token = this.CreateJwtToken(user);
            return Ok(new LoginResponse
            {
                Token = token
            });
        }

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtConfig:Secret"));
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Username)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires= DateTime.Now.AddMinutes(_config.GetValue<int>("JwtConfig:TokenExpire")),
                SigningCredentials = credentials,
                Issuer = _config.GetValue<string>("JwtConfig:Issuer"),
                Audience = _config.GetValue<string>("JwtConfig:Audience")
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
