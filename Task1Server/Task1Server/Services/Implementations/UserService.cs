using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Task1Server.Context;
using Task1Server.DTOs.Users;
using Task1Server.Entities;
using Task1Server.Exceptions;
using Task1Server.Exceptions.UserExceptions;
using Task1Server.Services.Interfaces;

namespace Task1Server.Services.Implementations
{
    public class UserService : UserProto.UserProtoBase, IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserService(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.UserName == request.UserName);
            if(user is null)
            {
                return new SignInResponse { Response = "Username or password incorrect", SignInSuccess = false };
            }
            if (user.Password != request.Password)
            {
                return new SignInResponse { Response = "Username or password incorrect", SignInSuccess = false };

            }
            return new SignInResponse { Response = "you have successfully logged in to your profile", SignInSuccess = true, UserId = user.Id.ToString() };

        }
        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id == userId);
            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }
            return user.Money;

        }

        public async Task MoneyTransactionAsync(MoneyTransactionDTO moneyTransactionDTO)
        {
            var senderUser= await _context.Users.FirstOrDefaultAsync(u=>u.Id == moneyTransactionDTO.SenderId);
            if (senderUser is null) 
            {
                throw new UserNotFoundException("Sender not found");
            }
            if(senderUser.Money < moneyTransactionDTO.TransferAmount)
            {
                throw new UserHasNotEnoughMoneyException("Sender has not enough money");
            }
            var receiverUser = await _context.Users.FirstOrDefaultAsync(u=>u.Id == moneyTransactionDTO.ReceiverId);
            if (receiverUser is null)
            {
                throw new UserNotFoundException("Sender not found");
            }
            senderUser.Money -= moneyTransactionDTO.TransferAmount;
            receiverUser.Money += moneyTransactionDTO.TransferAmount;
            GameTransactions newGameTransaction = new GameTransactions()
            {
                Money = moneyTransactionDTO.TransferAmount,
                SenderId = senderUser.Id,
                ReceiverId = receiverUser.Id,
            };
            _context.GameTransactions.Add(newGameTransaction);

            _context.UpdateRange(senderUser,receiverUser);
           await _context.SaveChangesAsync();
        }

        public async Task PostUserAsync(PostUserDTO postUserDTO)
        {
            var newUser = _mapper.Map<User>(postUserDTO);
            await _context.Users.AddAsync(newUser);
           await _context.SaveChangesAsync();
        }
        public override async Task<UserBalanceResponse> GetUserBalance(UserIdRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == request.UserId);
            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }
            return new UserBalanceResponse() { Balance = user.Money.ToString()};
        }
    }
}
