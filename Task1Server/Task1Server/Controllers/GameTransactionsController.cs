using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task1Server.DTOs.Users;
using Task1Server.Services.Interfaces;

namespace Task1Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameTransactionsController : ControllerBase
    {
        private readonly IUserService _userService;
        public GameTransactionsController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> MoneyTrasactions(MoneyTransactionDTO moneyTransactionDTO)
        {
           await _userService.MoneyTransactionAsync(moneyTransactionDTO);
            return Ok("Success");
        }
    }
}
