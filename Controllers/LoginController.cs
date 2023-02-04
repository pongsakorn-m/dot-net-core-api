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
        public static List<User> Users = new List<User>()
        {
            new User() { Username = "pongsakorn_user", Password = "123456", FullName = "Pongsakorn Mukdavannakorn", FailCount = 0 },
            new User() { Username = "mockup_user", Password = "123456", FullName = "Mokcup User", FailCount = 0 }
        };

        public static int MaxFailCount = 3;

        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
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
                if (user.FailCount >= MaxFailCount)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(1);
                }
                return BadRequest("Your username or password is incorrect.");
            }

            if (user.FailCount >= MaxFailCount && user.LockoutEnd > DateTime.Now)
            {
                TimeSpan ts = (user.LockoutEnd.GetValueOrDefault()) - DateTime.Now;
                return BadRequest(string.Format("Your account is locked. Please try again in {0} seconds.", ts.TotalSeconds));
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
            var key = Encoding.UTF8.GetBytes("ShoppingFoodSecret");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FullName)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires= DateTime.Now.AddMinutes(10),
                SigningCredentials = credentials,
                Issuer = "https://localhost:5000/",
                Audience = "https://localhost:5000/"
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
