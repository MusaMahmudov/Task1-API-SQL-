using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Task1Server.Context;
using Task1Server.DTOs;
using Task1Server.DTOs.GameDTOs;
using Task1Server.DTOs.Users;
using Task1Server.Entities;
using Task1Server.Exceptions.GameExceptions;
using Task1Server.Exceptions.UserExceptions;
using Task1Server.Services.Interfaces;

namespace Task1Server.Services.Implementations
{
    public class GameService : Game.GameBase,IGameService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GameService(AppDbContext context,IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }
        public override async Task<GameResult> DetermineResult(GameRequest request, ServerCallContext context)
        {
            var game = await _context.MatchHistories.Include(mh=>mh.PlayerOne).Include(mh=>mh.PlayerTwo).FirstOrDefaultAsync(mh=>mh.Id.ToString() == request.GameId);
            if(game is null)
            {
                return new GameResult() { Result = "Game not found" };
            }
            User userOne = null;

            if(game.PlayerOne is not null)
            {
                userOne = await _context.Users.FirstOrDefaultAsync(u => u.Id == game.PlayerOneId);
            }
            User userTwo = null;
            if (game.PlayerTwo is not null)
            {
                userTwo = await _context.Users.FirstOrDefaultAsync(u => u.Id == game.PlayerTwoId);
            }
            

            if (game.PlayerOneMove is not null && game.PlayerTwoMove is not null)
            {
               
                    if (game.PlayerOneMove == game.PlayerTwoMove)
                    {
                        game.Result = "Drawn";
                        _context.MatchHistories.Update(game);
                        await _context.SaveChangesAsync();
                        return new GameResult() { Result = "Drawn" };

                    }
                    else if ((game.PlayerOneMove.ToUpper().Trim() == "Н" && game.PlayerTwoMove.ToUpper().Trim() == "К") || (game.PlayerOneMove.ToUpper().Trim() == "K" && game.PlayerTwoMove.ToUpper().Trim() == "Б") || (game.PlayerOneMove.ToUpper().Trim() == "Б" && game.PlayerTwoMove.ToUpper().Trim() == "Н"))
                    {
                        if (userTwo is null)
                        return new GameResult() { Result = "User not found" };

                    game.Result = $"{userTwo.UserName} is a winner";
                        var moneyTransactionDTO = new MoneyTransactionDTO() { ReceiverId = userTwo.Id, SenderId = userOne.Id, TransferAmount = game.Bet };
                        await _userService.MoneyTransactionAsync(moneyTransactionDTO);
                        _context.MatchHistories.Update(game);
                        await _context.SaveChangesAsync();
                        return new GameResult() { Result = $"{userTwo.UserName} is winner" };


                    }
                    else if ((game.PlayerOneMove.ToUpper().Trim() == "Н" && game.PlayerTwoMove.ToUpper().Trim() == "Б") || (game.PlayerOneMove.ToUpper().Trim() == "K" && game.PlayerTwoMove.ToUpper().Trim() == "Н") || (game.PlayerOneMove.ToUpper().Trim() == "Б" && game.PlayerTwoMove.ToUpper().Trim() == "К"))
                    {
                        if (userTwo is null)
                            throw new UserNotFoundException("User not found");
                        game.Result = $"{userOne.UserName} is a winner";
                        var moneyTransactionDTO = new MoneyTransactionDTO() { ReceiverId = userOne.Id, SenderId = userTwo.Id, TransferAmount = game.Bet };
                        await _userService.MoneyTransactionAsync(moneyTransactionDTO);
                        _context.MatchHistories.Update(game);
                        await _context.SaveChangesAsync();
                        return new GameResult() { Result = $"{userOne.UserName} is winner" };

                    }
                    else
                    {
                        _context.MatchHistories.Update(game);
                        await _context.SaveChangesAsync();
                        return new GameResult() { Result = "Error" };

                    }
                }
                else
                {
                    _context.MatchHistories.Update(game);
                    await _context.SaveChangesAsync();
                    return new GameResult() { Result = "Wait for the opponent's move" };
                }
            


        }
        public override async Task<Response> MakeMove(MoveRequest request, ServerCallContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == request.PlayerId);
            if (user is null)
            {
                return new Response() { Response_ = "User not found" };
            }
            var game = await _context.MatchHistories.Include(g=>g.PlayerOne).Include(g => g.PlayerTwo).FirstOrDefaultAsync(mh=>mh.Id.ToString() == request.GameId);
            if(game is null)
            {
                return new Response() { Response_ = "Game not found" };
            }
            if(game.PlayerOneId == user.Id)
            {
                game.PlayerOneMove = request.Move.ToUpper().Trim();
             
            
            }
            else
            {

                game.PlayerTwoMove = request.Move.ToUpper().Trim();
               
            }
            _context.MatchHistories.Update(game);
            await _context.SaveChangesAsync();
            return new Response() { Response_ = "You move is accepted " };
        }
        public override async Task<ConnectToGameResponse> ConnectToGame(ConnectToGameRequest request, ServerCallContext context)
        {
            var game = await _context.MatchHistories.Include(mh => mh.PlayerOne).Include(mh => mh.PlayerTwo).FirstOrDefaultAsync(g => g.Id.ToString() == request.GameId);
            if (game is null)
            {
                return new ConnectToGameResponse
                {
                    Response = "Game bot found",
                    IsSuccess = false
                    
                };
            }
            if (game.Result is not null)
            {
                return new ConnectToGameResponse
                {
                    Response = "Game is full",
                    IsSuccess = false
                };
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == request.UserId);
            if (user is null)
            {
                return new ConnectToGameResponse
                {
                    Response = "User is full",
                    IsSuccess = false
                };
            }
            if (user.Money < game.Bet)
            {
                return new ConnectToGameResponse
                {
                    Response = "Not enough money on balance",
                    IsSuccess = false
                };
            }
            
            if (game.PlayerOne is null)
            {
                game.PlayerOne = user;
            }
            else if (game.PlayerTwo is null)
            {
                if(game.PlayerOneId == user.Id)
                {
                    return new ConnectToGameResponse
                    {
                        Response = "You are already connected to this game",
                        IsSuccess = false
                    };
                }
                game.PlayerTwo = user;
            }
            else
            {
                return new ConnectToGameResponse
                {
                    Response = "Game is full",
                    IsSuccess = false
                };
            }
            _context.MatchHistories.Update(game);
            await _context.SaveChangesAsync();
            return new ConnectToGameResponse
            {
                Response =  "Make move" ,
                IsSuccess = true
            };
        }
        public override async Task<MatchResponse> GetGamesToPlay(MatchRequest request, ServerCallContext context)
        {
            var gamesToPlay = await _context.MatchHistories.AsNoTracking().Where(mh => mh.Result == null && mh.PlayerTwo == null).Include(mh => mh.PlayerOne).Include(mh => mh.PlayerTwo).ToListAsync();
            var matches = new RepeatedField<GetGamesToPlay>();
            int gameNumber = 1;
            foreach (var game in gamesToPlay)
            {
                GetGamesToPlay newGameToPlay;
                if (game.PlayerOne is not null)
                {

                    newGameToPlay = new GetGamesToPlay() { PlayerOne = game.PlayerOne?.UserName, Bet = game.Bet.ToString(),GameNumber = gameNumber,GameId = game.Id.ToString() };
                }
                else
                {
                    newGameToPlay = new GetGamesToPlay() { Bet = game.Bet.ToString(), GameNumber = gameNumber, GameId = game.Id.ToString() };

                }
                gameNumber++;

                matches.Add(newGameToPlay);
            }
            return new MatchResponse
            {

                Games = { matches }
            };
        }


        public async Task PostGameAsync(PostGameDTO postGameDTO)
        {
            var newGame = _mapper.Map<MatchHistory>(postGameDTO);
            _context.MatchHistories.Add(newGame);
            await _context.SaveChangesAsync();

        }
    }
        //public async Task ConnectToTheGameAsync(Guid gameId,Guid userId)
        //{
            
        //    var game = await _context.MatchHistories.FirstOrDefaultAsync(g=>g.Id == gameId);
        //    if(game is null)
        //    {
        //        throw new GameNotFoundException("Game not found");
        //    }
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        //    if(user is null)
        //    {
        //        throw new UserNotFoundException("User not found");
        //    }
        //    if (game.PlayerOne is null)
        //    {
        //        game.PlayerOne = user;
        //    }
        //    else if (game.PlayerTwo is null)
        //    {
        //        game.PlayerTwo = user;
        //    }
        //    else
        //    {
        //        throw new GameIsFullException("This game is full");
        //    }
        //    _context.MatchHistories.Update(game);
        //   await _context.SaveChangesAsync();
        //    await Task.FromResult(new ResponseDTO("You joined the game"));

        //}
      
    
}
