using Task1Server.DTOs.Users;

namespace Task1Server.Services.Interfaces
{
    public interface IUserService
    {
        Task PostUserAsync(PostUserDTO postUserDTO);
        Task MoneyTransactionAsync(MoneyTransactionDTO moneyTransactionDTO);
        Task<decimal> GetBalanceAsync(Guid userId);
    }
}
