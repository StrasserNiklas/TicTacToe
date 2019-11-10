using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public class MainService : IMainService
    {
        private int incrementedId = 0;
        private readonly List<Player> players = new List<Player>();

        public Task<Player> AddPlayer(Player player)
        {
            player.PlayerId = this.incrementedId;
            this.incrementedId++;
            this.players.Add(player);
            return Task.FromResult(player);
        }

        public Task<IEnumerable<Player>> GetPlayers()
        {
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }
    }
}
