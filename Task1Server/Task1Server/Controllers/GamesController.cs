using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task1Server.DTOs.GameDTOs;
using Task1Server.Services.Interfaces;

namespace Task1Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateGame(PostGameDTO postGameDTO)
        {
           await _gameService.PostGameAsync(postGameDTO);
            return Ok("Game is created");
        }
    }
}
