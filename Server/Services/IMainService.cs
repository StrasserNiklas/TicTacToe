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
}
