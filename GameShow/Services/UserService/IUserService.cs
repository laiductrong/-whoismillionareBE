using GameShow.Controllers;
using GameShow.DTO.User;

namespace GameShow.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceReponse<GetUser>> Register(AddUser addUser);
        Task<ServiceReponse<string>> Login(AddUser addUser);
        Task<ServiceReponse<List<GetUser>>> GetUsers();
        Task<ServiceReponse<List<GetUser>>> DeleteUser(int id);
        Task<ServiceReponse<GetUser>> UpdateUser(UpdateUser updateUser);
    }
}
