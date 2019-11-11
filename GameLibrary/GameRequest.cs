using Newtonsoft.Json;
using System;

namespace GameLibrary
{
    public class GameRequest
    {
        [JsonConstructor]
        public GameRequest(int enemyId, int requestPlayerId)
        {
            this.EnemyId = enemyId; 
            this.RequestPlayerId = requestPlayerId;
        }

        public GameRequest()
        {
        }

        public int RequestPlayerId { get; set; }

        public int EnemyId { get; set; }

    }
}
