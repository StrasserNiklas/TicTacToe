using Newtonsoft.Json;

namespace GameLibrary
{
    public class GameStatusResponse
    {
        [JsonConstructor]
        public GameStatusResponse(int[] indexedGame, int currentPlayerId)
        {
            this.IndexedGame = indexedGame;
            this.CurrentPlayerId = currentPlayerId;
        }

        public GameStatusResponse()
        {
        }

        public int[] IndexedGame { get; set; }

        public int CurrentPlayerId { get; set; }
    }
}
