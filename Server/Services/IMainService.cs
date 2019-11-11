using GameLibrary;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IMainService
    {
        public List<GameRequest> RequestedGames { get; }
        public List<Game> Games { get;}
        Task<IEnumerable<Player>> GetPlayersAsync();
        Task<Player> AddPlayerAsync(Player player);
    }
}
