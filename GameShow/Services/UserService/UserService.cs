using AutoMapper;
using GameShow.Controllers;
using GameShow.Data;
using GameShow.DTO.Question;
using GameShow.DTO.User;
using GameShow.Migrations;
using GameShow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameShow.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _imapper;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public UserService(IMapper mapper, DataContext dataContext, IConfiguration configuration)
        {
            _imapper = mapper;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<ServiceReponse<List<GetUser>>> DeleteUser(int id)
        {
            var serviceReponse = new ServiceReponse<List<GetUser>>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(x=> x.Id == id);
            if(user is null)
            {
                serviceReponse.Data = null;
                serviceReponse.Success = false;
                serviceReponse.Message = "not found email";
            }
            else
            {
                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();
                serviceReponse.Data = (await _dataContext.Users.ToListAsync()).Select(x=> _imapper.Map<GetUser>(x)).ToList();
                serviceReponse.Success = true;
                serviceReponse.Message = "Delete success";
            }
            return serviceReponse;
        }

        public async Task<ServiceReponse<List<GetUser>>> GetUsers()
        {
            var serviceReponse = new ServiceReponse<List<GetUser>>();
            serviceReponse.Data = (await _dataContext.Users.ToListAsync()).Select(x => _imapper.Map<GetUser>(x)).ToList();
            serviceReponse.Success = true;
            serviceReponse.Message = "Get success";
            return serviceReponse;
        }

        public async Task<ServiceReponse<string>> Login(AddUser addUser)
        {
            var serviceReponse = new ServiceReponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.UserName== addUser.UserName);
            if (user == null)
            {
                serviceReponse.Data = null;
                serviceReponse.Success = false;
                serviceReponse.Message = "not found email";
                return serviceReponse;
            }
            if (!BCrypt.Net.BCrypt.Verify(addUser.PasswordHash, user.PasswordHash))
            {
                serviceReponse.Data = null;
                serviceReponse.Success = false;
                serviceReponse.Message = "not found pw";
                return serviceReponse;
                
            }
            serviceReponse.Data = CreateToken(user);
            serviceReponse.Success = true;
            serviceReponse.Message = "login success";
            return serviceReponse;
        }

        public async Task<ServiceReponse<GetUser>> Register(AddUser addUser)
        {
            var serviceReposive = new ServiceReponse<GetUser>();
            var checkUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == addUser.UserName);
            if(checkUser is not null)
            {
                serviceReposive.Success = false;
                serviceReposive.Data = null;
                serviceReposive.Message = "Email is exist";
                return serviceReposive;
            }
            User user = new User();
            user.UserName = addUser.UserName;
            user.NameDisplay = addUser.NameDisplay;
            user.PasswordHash = addUser.PasswordHash;
            try
            {
                await _dataContext.Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                serviceReposive.Success = true;
                serviceReposive.Data = null;
                serviceReposive.Message = "success";
            }
            catch (Exception ex)
            {
                serviceReposive.Success = false;
                serviceReposive.Data = null;
                serviceReposive.Message = "Register fails";
            }
            return serviceReposive;
        }

        public async Task<ServiceReponse<GetUser>> UpdateUser(UpdateUser updateUser)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == updateUser.Id);
            var serviceReposive = new ServiceReponse<GetUser>();
            if(user is not null)
            {
                user.UserName = updateUser.UserName;
                user.PasswordHash = updateUser.PasswordHash;
                user.NameDisplay = updateUser.NameDisplay;
                await _dataContext.SaveChangesAsync();
                serviceReposive.Data = _imapper.Map<GetUser>((await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == updateUser.Id)));
                serviceReposive.Message = "success";
                serviceReposive.Success = true;
            }
            else
            {
                serviceReposive.Data = null;
                serviceReposive.Message = "false";
                serviceReposive.Success = false;
            }
            return serviceReposive;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("NameDisplay", user.NameDisplay)
            };
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
