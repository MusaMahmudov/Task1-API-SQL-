using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task1Server.Context;
using Task1Server.DTOs.Users;
using Task1Server.Services.Interfaces;

namespace Task1Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;
        public UsersController(IUserService userService,AppDbContext context)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(PostUserDTO postUserDTO)
        {
           await _userService.PostUserAsync(postUserDTO);
            return Ok("User created");
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}
