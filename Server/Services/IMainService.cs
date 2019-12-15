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
        Task<IEnumerable<GameRequest>> GetGameRequestsAsync();
        Task<GameRequest> AddGameRequestAsync(GameRequest gameRequest);
        Task<IEnumerable<Game>> GetGamesAsync();
        Task<Game> AddGameAsync(Game game);
        Task<Game> RemoveGameAsync(Game game);
        Task<IEnumerable<Player>> GetPlayersAsync();
        Task<Player> AddPlayerAsync(Player player);
        Task<Player> RemovePlayerAsync(Player player);
        Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest);
        Task<IEnumerable<Player>> GetPlayersNotInGameAsync();
        Task<IEnumerable<SimpleGameInformation>> GetSimpleGameInformationListAsync();
    }
}
