using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.BusinessLogic.Login;
using WebApi.Models.Token;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly LoginBus loginBus = null;
        public LoginController(IConfiguration config)
        {
            _configuration = config;
            if(loginBus is null)
            {
                loginBus = new LoginBus();
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            var userInfo = await loginBus.GetUser(user);

            if (userInfo != null)
            {
                var userName = Convert.ToString(userInfo.Rows[0]["UserName"]);
                var fullName = Convert.ToString(userInfo.Rows[0]["FullName"]);
                var employeeID = Convert.ToString(userInfo.Rows[0]["EmployeeID"]);
                var email = Convert.ToString(userInfo.Rows[0]["Email"]);
                var role = Convert.ToString(userInfo.Rows[0]["Roles"]);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("FullName", fullName),
                    new Claim("UserId", employeeID),
                    new Claim("UserName", userName),
                    new Claim("Email", email),
                    new Claim(ClaimTypes.Role, role)
                };
                var accessToken = this.CreateToken(claims.ToList());
                var refreshToken = this.GenerateRefreshToken();
                var dateTimeNow = DateTime.Now;
                return Ok(new 
                { 
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken), 
                    RefreshToken = refreshToken, 
                    //ExpirationTime = dateTimeNow.AddDays(1).Subtract(dateTimeNow).TotalSeconds 
                });
            }
            return NotFound("User not found");
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var dateTimeNow = DateTime.Now;
            var accessToken = new JwtSecurityToken
                (
                _configuration["Jwt:Issuser"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );

            return accessToken;
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        [HttpPost]
        [Route("refreshtoken")]
        public IActionResult RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var userId = principal.Claims.First(x => x.Type == "UserId").Value;


            //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            //#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
            //#pragma warning restore CS8602 // Dereference of a possibly null reference.
            //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //var user = await _userManager.FindByNameAsync(username);      // get uername from databae

            //if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            //{
            //    return BadRequest("Invalid access token or refresh token");
            //}

            var newAccessToken = this.CreateToken(principal.Claims.ToList());
            var newRefreshToken = this.GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //await _userManager.UpdateAsync(user);                                // update user token to database

            return new ObjectResult(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost]
        [Route("getuser")]
        public IActionResult GetUser(Token tokenModel)
        {
            var token = tokenModel.token;
            if (token is null)
            {
                return BadRequest("Invalid client request");
            }

            //string accessToken = tokenModel.AccessToken;
            //string refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var userName = principal.Claims.First(x => x.Type == "UserName").Value;
            var fullName = principal.Claims.First(x => x.Type == "FullName").Value;
            var employeeID = principal.Claims.First(x => x.Type == "UserId").Value;

            //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            //#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
            //#pragma warning restore CS8602 // Dereference of a possibly null reference.
            //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //var user = await _userManager.FindByNameAsync(username);      // get uername from databae

            //if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            //{
            //    return BadRequest("Invalid access token or refresh token");
            //}

            var newAccessToken = this.CreateToken(principal.Claims.ToList());
            var newRefreshToken = this.GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //await _userManager.UpdateAsync(user);                                // update user token to database

            return new ObjectResult(new
            {
                UserName = userName,
                FullName = fullName,
                EmployeeID = employeeID,
            });
        }

    }

    public class Token
    {
        public string token { get; set; }
    }
}
