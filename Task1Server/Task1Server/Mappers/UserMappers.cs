using AutoMapper;
using Task1Server.DTOs.Users;
using Task1Server.Entities;

namespace Task1Server.Mappers
{
    public class UserMappers : Profile
    {
       public UserMappers() 
        {
            CreateMap<User, PostUserDTO>().ReverseMap();
        }

    }
}
