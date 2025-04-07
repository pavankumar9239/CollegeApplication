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

            byte[] key = null;
            var issuer = string.Empty;
            var audience = string.Empty;

            if(model.Policy.ToLower() == "local")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretLocal"));
                issuer = _configuration.GetValue<string>("LocalIssuer");
                audience = _configuration.GetValue<string>("LocalAudience");
            }
            else if(model.Policy.ToLower() == "microsoft")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretMicrosoft"));
                issuer = _configuration.GetValue<string>("MicrosoftIssuer");
                audience = _configuration.GetValue<string>("MicrosoftAudience");
            }
            else if(model.Policy.ToLower() == "google")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretGoogle"));
                issuer = _configuration.GetValue<string>("GoogleIssuer");
                audience = _configuration.GetValue<string>("GoogleAudience");
            }

            LoginResponseDto response = new() { UserName = model.UserName };

            if(model.UserName == "APK" && model.Password == "Apk@123")
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = issuer,

                    Audience = audience,

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
