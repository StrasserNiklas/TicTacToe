using Newtonsoft.Json;

namespace GameLibrary
{
    public class GameStatus
    {
        [JsonConstructor]
        public GameStatus(int[] indexedGame, int currentPlayerId)
        {
            this.IndexedGame = indexedGame;
            this.CurrentPlayerId = currentPlayerId;
        }

        public GameStatus()
        {
        }

        public int UpdatedPosition { get; set; } = -1;

        public int GameId { get; set; }
        public int[] IndexedGame { get; set; }

        public int CurrentPlayerId { get; set; }
    }
}
