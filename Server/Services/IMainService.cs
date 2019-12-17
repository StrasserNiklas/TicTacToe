// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Server.Services
{
    using GameLibrary;
    using Server.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest, bool accepted);

        Task<IEnumerable<Player>> GetPlayersNotInGameAsync();

        Task<List<SimpleGameInformation>> GetSimpleGameInformationListAsync();
    }
}
