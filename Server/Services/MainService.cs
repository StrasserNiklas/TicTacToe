using GameLibrary;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class MainService : IMainService
    {
        private readonly List<Player> players = new List<Player>();

        private readonly List<Game> games = new List<Game>();
        private readonly List<GameRequest> gameRequests = new List<GameRequest>();

        public Task<Player> AddPlayerAsync(Player player)
        {
            var duplicatePlayer = this.players.SingleOrDefault(x => x.PlayerName == player.PlayerName);

            if (duplicatePlayer == null)
            {
                this.players.Add(player);
            }

            return Task.FromResult(player);
        }

        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }

        public Task<IEnumerable<GameRequest>> GetGameRequestsAsync()
        {
            return Task.FromResult<IEnumerable<GameRequest>>(this.gameRequests);
        }

        public Task<IEnumerable<Game>> GetGamesAsync()
        {
            return Task.FromResult<IEnumerable<Game>>(this.games);
        }

        public Task<GameRequest> AddGameRequestAsync(GameRequest gameRequest)
        {
            this.gameRequests.Add(gameRequest);
            return Task.FromResult(gameRequest);
        }

        public Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest)
        {
            this.gameRequests.Remove(gameRequest);
            return Task.FromResult(gameRequest);
        }

        public Task<Player> RemovePlayerAsync(Player player)
        {
            this.players.Remove(player);
            return Task.FromResult(player);
        }

        public Task<Game> AddGameAsync(Game game)
        {
            this.games.Add(game);
            return Task.FromResult(game);
        }
    }
}
