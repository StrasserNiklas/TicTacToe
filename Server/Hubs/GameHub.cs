//-----------------------------------------------------------------------
// <copyright file="ActionVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents </summary>
//-----------------------------------------------------------------------

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
            //await base.Clients.All.SendAsync("ReceivePlayersAsync", allPlayers);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
        }

        public async Task AddPlayer(string nameForNewPlayer)
        {
            Player newPlayer = new Player(nameForNewPlayer) { ConnectionId = Context.ConnectionId };

            var allPlayers = await mainService.GetPlayersAsync();
            bool playerExists = false;

            foreach (var player in allPlayers)
            {
                if (player.PlayerName == nameForNewPlayer)
                {
                    playerExists = true;
                    break;
                }
            }

            if (playerExists)
            {
                await base.Clients.Caller.SendAsync("DuplicateName");
                return;
            }

            await mainService.AddPlayerAsync(newPlayer);
            await base.Clients.Caller.SendAsync("ReturnPlayerInstance", newPlayer);


            await base.Clients.Caller.SendAsync("ReceiveGames", await this.mainService.GetSimpleGameInformationListAsync());
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
            //await base.Clients.All.SendAsync("ReceivePlayersAsync", await mainService.GetPlayersAsync());
        }

        public async Task AddGameRequest(GameRequest gameRequest)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.ConnectionId == gameRequest.Enemy.ConnectionId);
            //var player = list.SingleOrDefault(player => player.PlayerId == gameRequest.Enemy.PlayerId); OLD OLD OLD OLD

            if (player != null)
            {
                // wenn der Enemy schon ein request von wem anderen hat, schicken wir Statusnachricht an den Caller

                var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.Enemy == gameRequest.Enemy || request.Enemy == gameRequest.RequestingPlayer)
            && (request.RequestingPlayer == gameRequest.Enemy || request.RequestingPlayer == gameRequest.RequestingPlayer));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(gameRequest);//(new GameRequest(gameRequest.Enemy, gameRequest.RequestPlayer));

                    // geht das ? XDDDDDDDDDDDDDDDDDD

                    var task = Task.Run(() =>
                    {
                        var aTimer = new System.Timers.Timer(9000) { AutoReset = false } ;

                        aTimer.Start();

                        aTimer.Elapsed += async (sender, e) =>
                        {
                            if (!request.Accepted)
                            {
                                await this.mainService.RemoveRequestAsync(request, false);
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
                    //existingRequest.Declined = true;
                    await base.Clients.Client(existingRequest.RequestingPlayer.ConnectionId).SendAsync("StatusMessage", $"{existingRequest.Enemy.PlayerName} has declined the request.");
                    await this.mainService.RemoveRequestAsync(existingRequest, false);
                }
                else
                {
                    //existingRequest.Accepted = true;
                    // create game here

                    var game = new Game(existingRequest.RequestingPlayer, existingRequest.Enemy);
                    await this.mainService.RemoveRequestAsync(existingRequest, true);

                    await this.mainService.AddGameAsync(game);

                    
                    await base.Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());

                    var simpleGameInfo = await this.mainService.GetSimpleGameInformationListAsync();
                    await base.Clients.All.SendAsync("ReceiveGames", simpleGameInfo);

                    var gameStatus = CreateNewGameStatus(game, true);

                    await base.Clients.Clients(existingRequest.RequestingPlayer.ConnectionId, existingRequest.Enemy.ConnectionId).SendAsync("GameStatus", gameStatus);
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

        public async Task ReturnToLobby(string id, string enemyId)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());

            foreach (var game in games)
            {
                if (game.PlayerOne.ConnectionId == id || game.PlayerTwo.ConnectionId == id)
                {
                    await this.mainService.RemoveGameAsync(game);

                    //await base.Clients.Client(enemyId).SendAsync("StatusMessage", "Enemy left the game, please return to lobby.");
                    await base.Clients.Client(enemyId).SendAsync("EnemyLeftGame");
                    

                    var simpleGameInfo = await this.mainService.GetSimpleGameInformationListAsync();
                    await base.Clients.All.SendAsync("ReceiveGames", simpleGameInfo);
                    await base.Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
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
                    var oldGameStatus = CreateNewGameStatus(game, false, updatedPosition);

                    await base.Clients.Client(game.CurrentPlayer.ConnectionId).SendAsync("GameStatus", oldGameStatus);
                    await Task.Delay(5000);

                    game.NewGameSetup();
                    var gameStatus = CreateNewGameStatus(game, true, updatedPosition);

                    await base.Clients.Clients(game.PlayerOne.ConnectionId, game.PlayerTwo.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
                else
                {
                    var gameStatus = CreateNewGameStatus(game, false, updatedPosition);

                    await base.Clients.Client(game.CurrentPlayer.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
            }
        }

        private GameStatus CreateNewGameStatus(Game game, bool isNewGame, int updatedPosition = -1)
        {
            var gameStatus = new GameStatus(game.CurrentGameStatus, game.CurrentPlayer.ConnectionId, game.CurrentPlayer.Marker, game.GameId, game.PlayerOne.Wins, game.PlayerTwo.Wins)
            {
                UpdatedPosition = updatedPosition
            };

            if (isNewGame)
            {
                gameStatus.IsNewGame = true;
            }

            return gameStatus;
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

            var games = new List<Game>(await this.mainService.GetGamesAsync());

            foreach (var game in games)
            {
                if (game.PlayerOne.ConnectionId == id || game.PlayerTwo.ConnectionId == id)
                {
                    await this.mainService.RemoveGameAsync(game);
                }
            }

            await this.mainService.RemovePlayerAsync(disconnectedPlayer);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
        }
    }
}
