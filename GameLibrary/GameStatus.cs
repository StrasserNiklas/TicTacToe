using Newtonsoft.Json;

namespace GameLibrary
{
    public class GameStatus
    {
        [JsonConstructor]
        public GameStatus(int[] indexedGame, string currentPlayerId, int currentPlayerMarker, int gameID)
        {
            this.IndexedGame = indexedGame;
            this.CurrentPlayerId = currentPlayerId;
            this.CurrentPlayerMarker = currentPlayerMarker;
            this.GameId = gameID;
        }

        public GameStatus()
        {
        }

        public int UpdatedPosition { get; set; } = -1;

        public int GameId { get; set; }
        public int[] IndexedGame { get; set; }

        public string CurrentPlayerId { get; set; }

        public int CurrentPlayerMarker { get; set; }
    }
}
