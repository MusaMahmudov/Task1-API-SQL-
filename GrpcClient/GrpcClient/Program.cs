using Grpc.Net.Client;
using GrpcClient;
using System.Threading.Channels;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7108");

            var userClient = new UserProto.UserProtoClient(channel);
            Console.WriteLine("Write UserName and Password");
            string userName = Console.ReadLine();
            string password = Console.ReadLine();   
            SignInResponse signInResponse =await userClient.SignInAsync(new SignInRequest() { Password = password,UserName = userName });
            while (!signInResponse.SignInSuccess)
            {
                Console.WriteLine(signInResponse.Response);
                Console.WriteLine("Write UserName and Password");

                userName = Console.ReadLine();
                 password = Console.ReadLine();
                signInResponse = await userClient.SignInAsync(new SignInRequest() { Password = password, UserName = userName });
            }
            Console.WriteLine(signInResponse.Response);
            string userId = signInResponse.UserId;
            int choice;
            Console.WriteLine("For getting list of games press 1");
            Console.WriteLine("To show balance press 2");
            Console.WriteLine("To exit  press 3");



            while (int.TryParse(Console.ReadLine(), out choice)) 
            {
                var gameClient = new Game.GameClient(channel);
                switch (choice)
                {
                    case 1:
                        var reply = await gameClient.GetGamesToPlayAsync(new MatchRequest() { MatchRequest_ = "Get games" });
                        if(reply.Games.Count == 0)
                        {
                            Console.WriteLine("No games available");
                            break;
                        }
                        foreach (var game in reply.Games)
                        {
                            if (game.HasPlayerOne)
                            {
                                Console.WriteLine($"GameNumber:{game.GameNumber} - Bet:{game.Bet} - Player:{game.PlayerOne} - ");
                            }
                            else
                            {
                                Console.WriteLine($"GameNumber:{game.GameNumber}  - Bet:{game.Bet} - No players");
                            }
                        }
                        int gameNumber;
                        bool gameIdFound = false;
                        string gameId = string.Empty;
                        while (!gameIdFound)
                        {
                            Console.WriteLine("Choose game by Game Number");

                            while (!int.TryParse(Console.ReadLine(), out gameNumber))
                            {
                                Console.WriteLine("Choose game by Game Number");
                            }
                            
                            var game  = reply.Games.FirstOrDefault(g => g.GameNumber == gameNumber);
                            if(game is not  null)
                            {
                                gameId = game.GameId;
                                gameIdFound = true;

                            }
                          
                        }
                   
                        
                        
                        var responseConnect = await gameClient.ConnectToGameAsync(new ConnectToGameRequest() { UserId = userId, GameId = gameId });

                        Console.WriteLine(responseConnect.Response);
                        if (!responseConnect.IsSuccess) 
                        {
                            break;
                        }

                        Console.WriteLine("Введите Н или К или Б");
                        string move = Console.ReadLine();
                        while (move != "Н" && move != "Б" && move != "К")
                        {
                            Console.WriteLine("Введите Н или К или Б");
                            move = Console.ReadLine();
                        }
                        var result = gameClient.MakeMove(new MoveRequest() { PlayerId = userId, GameId = gameId, Move = move });

                        Console.WriteLine($"{result.Response_}");
                        var gameResult = await gameClient.DetermineResultAsync(new GameRequest() { GameId = gameId });
                        Console.WriteLine(gameResult.Result);


                        break;
                    case 2:
                        var response = await userClient.GetUserBalanceAsync(new UserIdRequest() { UserId = userId });
                        Console.WriteLine(response.Balance);
                        break;
                    
                    case 3:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;
                       
                }

            }
          

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
      
    }
}