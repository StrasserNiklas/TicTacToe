using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IMainService
    {
        Task<IEnumerable<Player>> GetPlayers();
        Task<Player> AddPlayer(Player player);
    }

    public class MainService : IMainService
    {
        private readonly List<Player> players = new List<Player>();

        public Task<Player> AddPlayer(Player player)
        {
            this.players.Add(player);
            return Task.FromResult(player);
        }

        public Task<IEnumerable<Player>> GetPlayers()
        {
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }
    }
}
