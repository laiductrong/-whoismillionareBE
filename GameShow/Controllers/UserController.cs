//using GameShow.DTO.User;
//using GameShow.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using System.Security.Claims;
//using System.Text;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;

//namespace GameShow.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        public UserController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        public static User user = new User();
//        [HttpPost("Register")]
//        public ActionResult<User> Register(AddUser addUser)
//        {
//            string pwHash = BCrypt.Net.BCrypt.HashPassword(addUser.PasswordHash);
//            user.UserName = addUser.UserName;
//            user.PasswordHash = pwHash;
//            user.NameDisplay = addUser.NameDisplay;
//            return Ok(user);
//        }
//        [HttpPost("Login")]
//        public ActionResult<User> Login(AddUser addUser)
//        {
//            if(addUser.UserName!=user.UserName)
//            {
//                return BadRequest("Login Fail username");
//            }
//            if(!BCrypt.Net.BCrypt.Verify(addUser.PasswordHash, user.PasswordHash))
//            {
//                return BadRequest("Login Fail pw");
//            }
//            string token = CreateToken(user);
//            return Ok(token);
//        }
//        private string CreateToken(User userToken)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.UserName),
//                new Claim(ClaimTypes.Name, user.UserName),
//            };
//            if(userToken.IsAdmin)
//            {
//                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
//            }
//            else
//            {
//                claims.Add(new Claim(ClaimTypes.Role, "User"));
//            }

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                _configuration.GetSection("AppSettings:Token").Value!));

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            var token = new JwtSecurityToken(
//                    claims: claims,
//                    expires: DateTime.Now.AddDays(1),
//                    signingCredentials: creds
//                );

//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//            return jwt;
//        }
//    }
//}

using GameShow.DTO.User;
using GameShow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using GameShow.Services.UserService;
using GameShow.Migrations;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GameShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        //public static User user = new User();
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceReponse<GetUser>>> Register(AddUser addUser)
        {
            addUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(addUser.PasswordHash);
            return Ok(await _userService.Register(addUser));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceReponse<string>>> Login(AddUser addUser)
        {
            return Ok(await _userService.Login(addUser));
        }
        [HttpDelete("Delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<List<GetUser>>>> DeleteUser(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }
        [HttpGet("Get"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<List<GetUser>>>> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }
        [HttpPut("Update"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceReponse<GetUser>>> UpdateUser(UpdateUser updateUser)
        {
            updateUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.PasswordHash);
            return Ok(await _userService.UpdateUser(updateUser));
        }
    }
}

