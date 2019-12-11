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
                //    var existingRequestFromRequestingPlayer = this.mainService.RequestedGames.SingleOrDefault(request => (request.EnemyId == data.EnemyId || request.EnemyId == data.RequestPlayerId)
                //&& (request.RequestPlayerId == data.EnemyId || request.RequestPlayerId == data.RequestPlayerId));

                var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.Enemy == gameRequest.Enemy || request.Enemy == gameRequest.RequestPlayer)
            && (request.RequestPlayer == gameRequest.Enemy || request.RequestPlayer == gameRequest.RequestPlayer));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(new GameRequest(gameRequest.Enemy, gameRequest.RequestPlayer));

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
            var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => request.RequestID == id);

            if (existingRequest != null)
            {
                if (!accept)
                {
                    existingRequest.Declined = true;
                }
                else
                {
                    existingRequest.Accepted = true;
                    // create game here

                }
            }
        }

        public async Task UpdateGameStatus(int id, GameStatus update)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());
            var game = games.SingleOrDefault(g => g.GameId == id);

            if (game != null)
            {
                if (game.PlayerOne.PlayerId == update.CurrentPlayerId)
                {
                    if (update.UpdatedPosition > 0 && update.UpdatedPosition < 9)
                    {
                        if (game.CurrentGameStatus[update.UpdatedPosition] == 0)
                        {
                            game.CurrentGameStatus[update.UpdatedPosition] = game.CurrentPlayer.Marker;
                        }
                    }

                    game.CurrentPlayer = game.PlayerTwo;
                }
                else if (game.PlayerTwo.PlayerId == update.CurrentPlayerId)
                {
                    if (update.UpdatedPosition > 0 && update.UpdatedPosition < 9)
                    {
                        if (game.CurrentGameStatus[update.UpdatedPosition] == 0)
                        {
                            game.CurrentGameStatus[update.UpdatedPosition] = game.CurrentPlayer.Marker;
                        }
                    }

                    game.CurrentPlayer = game.PlayerOne;
                }
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
            var test = await mainService.GetPlayersAsync();
        }
    }
}
