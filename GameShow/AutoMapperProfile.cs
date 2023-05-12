using AutoMapper;
using GameShow.DTO.Question;
using GameShow.DTO.User;
using GameShow.Models;

namespace GameShow
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Question, GetQuestion>();
            CreateMap<User, GetUser>();
        }
    }
}
