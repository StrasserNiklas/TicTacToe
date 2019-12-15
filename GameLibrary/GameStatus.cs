using Newtonsoft.Json;

namespace GameLibrary
{
    public class GameStatus
    {
        [JsonConstructor]
        public GameStatus(int[] indexedGame, string currentPlayerId, int currentPlayerMarker, int gameID, int winsPlayer1, int winsPlayer2)
        {
            this.IndexedGame = indexedGame;
            this.CurrentPlayerId = currentPlayerId;
            this.CurrentPlayerMarker = currentPlayerMarker;
            this.GameId = gameID;
            this.UpdatedPosition = -1;
            this.WinsPlayer1 = winsPlayer1;
            this.WinsPlayer2 = winsPlayer2;
        }

        public GameStatus()
        {
            this.UpdatedPosition = -1;
            this.WinsPlayer1 = 0;
            this.WinsPlayer2 = 0;
        }

        public int UpdatedPosition { get; set; }

        public int GameId { get; set; }
        public int[] IndexedGame { get; set; }

        public string CurrentPlayerId { get; set; }

        public int CurrentPlayerMarker { get; set; }

        public bool IsNewGame { get; set; }
        public int WinsPlayer1 { get; set; }
        public int WinsPlayer2 { get; set; }
    }
}
