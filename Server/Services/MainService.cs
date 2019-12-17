//-----------------------------------------------------------------------
// <copyright file="ActionVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents </summary>
//-----------------------------------------------------------------------

namespace Server.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameLibrary;
    using Microsoft.Extensions.Logging;
    using Server.Models;
    
    public class MainService : IMainService
    {
        private readonly List<Player> players = new List<Player>();

        private readonly List<Game> games = new List<Game>();//{ new Game(new Player("nikalallala"), new Player("felixixixixix")) };

        private readonly List<GameRequest> gameRequests = new List<GameRequest>();

        private readonly ILogger<MainService> logger;

        public MainService(ILogger<MainService> logger)
        {
            this.logger = logger;
        }

        public Task<Player> AddPlayerAsync(Player player)
        {
            this.logger.LogInformation("[AddPlayerAsync] ConnectionId: {0}, PlayerName: {1}", new object[] { player.ConnectionId, player.PlayerName });

            this.players.Add(player);

            return Task.FromResult(player);
        }

        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            this.logger.LogInformation("[GetPlayersAsync]");
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }

        public Task<IEnumerable<GameRequest>> GetGameRequestsAsync()
        {
            this.logger.LogInformation("[GetGameRequestsAsync]");
            return Task.FromResult<IEnumerable<GameRequest>>(this.gameRequests);
        }

        public Task<IEnumerable<Game>> GetGamesAsync()
        {
            this.logger.LogInformation("[GetGamesAsync]");
            return Task.FromResult<IEnumerable<Game>>(this.games);
        }

        public Task<GameRequest> AddGameRequestAsync(GameRequest gameRequest)
        {
            this.logger.LogInformation("[AddGameRequestAsync] Player {0} requests a game with player {1}", new object[] { gameRequest.RequestingPlayer.PlayerName, gameRequest.Enemy.PlayerName });
            this.gameRequests.Add(gameRequest);
            return Task.FromResult(gameRequest);
        }

        public Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest, bool accepted)
        {
            this.logger.LogInformation($"[RemoveRequestAsync] Game request from player {0} to player {1} has been accepted: {accepted.ToString()}", new object[] { gameRequest.RequestingPlayer.PlayerName, gameRequest.Enemy.PlayerName });
            this.gameRequests.Remove(gameRequest);
            return Task.FromResult(gameRequest);
        }

        public Task<Player> RemovePlayerAsync(Player player)
        {
            this.logger.LogInformation("[RemovePlayerAsync] ConnectionId: {0}, PlayerName: {1}", new object[] { player.ConnectionId, player.PlayerName });
            this.players.Remove(player);
            return Task.FromResult(player);
        }

        public Task<Game> AddGameAsync(Game game)
        {
            this.logger.LogInformation("[AddGameAsync] PlayerOneName: {0}, PlayerTwoName: {1}", new object[] { game.PlayerOne.PlayerName, game.PlayerTwo.PlayerName });
            this.games.Add(game);
            return Task.FromResult(game);
        }

        public Task<Game> RemoveGameAsync(Game game)
        {
            this.logger.LogInformation("[RemoveGameAsync] PlayerOneName: {0}, PlayerTwoName: {1}", new object[] { game.PlayerOne.PlayerName, game.PlayerTwo.PlayerName });
            this.games.Remove(game);
            return Task.FromResult(game);
        }

        public Task<IEnumerable<Player>> GetPlayersNotInGameAsync()
        {
            this.logger.LogInformation("[GetPlayersNotInGameAsync]");

            List<Player> playerList = new List<Player>();

            if (this.games.Count == 0)
            {
                return Task.FromResult<IEnumerable<Player>>(this.players);
            }

            foreach (var game in this.games)
            {
                foreach (var player in this.players)
                {
                    if (game.PlayerOne.ConnectionId != player.ConnectionId && game.PlayerTwo.ConnectionId != player.ConnectionId)
                    {
                        playerList.Add(player);
                    }
                }
            }

            return Task.FromResult<IEnumerable<Player>>(playerList);
        }

        public Task<List<SimpleGameInformation>> GetSimpleGameInformationListAsync()
        {
            this.logger.LogInformation("[GetSimpleGameInformationListAsync]");

            List<SimpleGameInformation> simpleGameInformation = new List<SimpleGameInformation>();

            foreach (var item in this.games)
            {
                simpleGameInformation.Add(new SimpleGameInformation(item.PlayerOne.PlayerName, item.PlayerTwo.PlayerName));
            }

            return Task.FromResult(simpleGameInformation);
        }
    }
}
