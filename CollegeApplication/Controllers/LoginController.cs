using CollegeApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "AllowAll")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Please provide valid user name and password.");
            }

            LoginResponseDto response = new() { UserName = model.UserName };
            if(model.UserName == "APK" && model.Password == "Apk@123")
            {
                var keyJWTSecretGoogle = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretGoogle"));
                var keyJWTSecretMicrosoft = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretMicrosoft"));
                var keyJWTSecretLocal = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretLocal"));

                var key = model.Policy.ToLower() == "local" ? keyJWTSecretLocal :
                    model.Policy.ToLower() == "microsoft" ? keyJWTSecretMicrosoft :
                    model.Policy.ToLower() == "google" ? keyJWTSecretGoogle : null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim(ClaimTypes.Role, "Admin")
                    }),

                    Expires = DateTime.Now.AddHours(4),

                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Ok("Invalid user name and password.");
            }
            return Ok(response);
        }
    }
}
