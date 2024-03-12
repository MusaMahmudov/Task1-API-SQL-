using Task1Server.DTOs.GameDTOs;

namespace Task1Server.Services.Interfaces
{
    public interface IGameService
    {
        Task PostGameAsync(PostGameDTO postGameDTO);
    }
}
