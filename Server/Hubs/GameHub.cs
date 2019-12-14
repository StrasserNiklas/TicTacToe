using GameLibrary;
using Microsoft.AspNetCore.SignalR;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IMainService mainService;

        public GameHub(IMainService main)
        {
            this.mainService = main;
        }

        public async Task GetPlayers(string requestedPlayerName)
        {
            var allPlayers = await mainService.GetPlayersAsync();

            // select all players except the requested one
            // requested player should not be included in the result
            allPlayers = allPlayers.Where(name => name.PlayerName != requestedPlayerName);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", allPlayers);
        }

        public async Task AddPlayer(string name)
        {
            Player player = new Player(name);
            player.ConnectionId = Context.ConnectionId;
            var addedPlayer = await mainService.AddPlayerAsync(player);
            await base.Clients.Caller.SendAsync("ReturnPlayerInstance", player);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await mainService.GetPlayersAsync());
        }

        public async Task AddGameRequest(GameRequest gameRequest)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.ConnectionId == gameRequest.Enemy.ConnectionId);
            //var player = list.SingleOrDefault(player => player.PlayerId == gameRequest.Enemy.PlayerId); OLD OLD OLD OLD

            if (player != null)
            {
                // wenn der Enemy schon ein request von wem anderen hat, schicken wir Statusnachricht an den Caller

                var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.Enemy == gameRequest.Enemy || request.Enemy == gameRequest.RequestPlayer)
            && (request.RequestPlayer == gameRequest.Enemy || request.RequestPlayer == gameRequest.RequestPlayer));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(gameRequest);//(new GameRequest(gameRequest.Enemy, gameRequest.RequestPlayer));

                    // geht das ? XDDDDDDDDDDDDDDDDDD

                    var task = Task.Run(() =>
                    {
                        var aTimer = new System.Timers.Timer(10000);

                        aTimer.Start();

                        aTimer.Elapsed += async (sender, e) =>
                        {
                            if (!request.Accepted)
                            {
                                await this.mainService.RemoveRequestAsync(request);
                            }
                        };
                    });

                    await base.Clients.Client(player.ConnectionId).SendAsync("GameRequested", gameRequest);
                }
            }


        }

        public async Task DeclineOrAcceptRequest(int id, bool accept)
        {
            var requests = new List<GameRequest>(await this.mainService.GetGameRequestsAsync());
            var existingRequest = requests.SingleOrDefault(request => request.RequestID == id);

            if (existingRequest != null)
            {
                if (!accept)
                {
                    existingRequest.Declined = true;
                    // NÖTIG?
                    await base.Clients.Client(existingRequest.RequestPlayer.ConnectionId).SendAsync("StatusMessage", $"{existingRequest.Enemy.PlayerName} has declined the request.");

                    await this.mainService.RemoveRequestAsync(existingRequest);
                }
                else
                {
                    existingRequest.Accepted = true;
                    // create game here

                    var game = new Game(existingRequest.RequestPlayer, existingRequest.Enemy);
                    
                    await this.mainService.AddGameAsync(game);

                    var gameStatus = new GameStatus(game.CurrentGameStatus, game.CurrentPlayer.ConnectionId, game.CurrentPlayer.Marker, game.GameId);

                    await base.Clients.Clients(existingRequest.RequestPlayer.ConnectionId, existingRequest.Enemy.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
            }
        }

        public async Task UpdateGameStatus(GameStatus update)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());
            var game = games.SingleOrDefault(g => g.GameId == update.GameId);

            if (game != null)
            {
                if (game.PlayerOne.ConnectionId == update.CurrentPlayerId)
                {
                    await this.UpdatePlayerSpecificGameStatus(game, update.UpdatedPosition, game.PlayerOne);
                }
                else if (game.PlayerTwo.ConnectionId == update.CurrentPlayerId)
                {
                    await this.UpdatePlayerSpecificGameStatus(game, update.UpdatedPosition, game.PlayerTwo);
                }
            }
        }

        private async Task UpdatePlayerSpecificGameStatus(Game game, int updatedPosition, Player player)
        {
            if (game.IsMoveValid(updatedPosition, player))
            {
                var gameFinished = game.MakeMove(updatedPosition, player);

                if (gameFinished)
                {
                    await base.Clients.Clients(game.PlayerOne.ConnectionId, game.PlayerTwo.ConnectionId).SendAsync("StatusMessage", game.EndMessage + " New game in 5 seconds");

                    await Task.Delay(4000);

                    game.NewGameSetup();

                    // HIER NOCH LOGIK DASS EIN NEUES SPIEL BEGINTN? TIMEOUT?
                }

                var gameStatus = new GameStatus(game.CurrentGameStatus, game.CurrentPlayer.ConnectionId, game.CurrentPlayer.Marker, game.GameId);
                gameStatus.UpdatedPosition = updatedPosition;

                await base.Clients.Client(game.CurrentPlayer.ConnectionId).SendAsync("GameStatus", gameStatus);

            }
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var allPlayers = await mainService.GetPlayersAsync();
            var disconnectedPlayer = allPlayers.FirstOrDefault(player => player.ConnectionId == id);
            await this.mainService.RemovePlayerAsync(disconnectedPlayer);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", allPlayers.Where(x => x.ConnectionId != id));
        }
    }
}
