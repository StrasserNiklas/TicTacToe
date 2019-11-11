using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public class MainService : IMainService
    {
        private int incrementedId = 1;
        private readonly List<Player> players = new List<Player>() { new Player("felix") { PlayerId = 999 } };

        public Task<Player> AddPlayerAsync(Player player)
        {
            player.PlayerId = this.incrementedId;
            this.incrementedId++;
            this.players.Add(player);
            return Task.FromResult(player);
        }

        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }
    }
}
