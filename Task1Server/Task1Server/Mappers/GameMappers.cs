using AutoMapper;
using Task1Server.DTOs.GameDTOs;
using Task1Server.Entities;

namespace Task1Server.Mappers
{
    public class GameMappers : Profile
    {
        public GameMappers() 
        {
            CreateMap<PostGameDTO, MatchHistory>().ReverseMap();
            CreateMap<MatchHistory,GetGamesToPlayDTO>().ForMember(gg=>gg.PlayerOne,x=>x.MapFrom(mh=>mh.PlayerOne.UserName)).ReverseMap();
        }
    }
}
