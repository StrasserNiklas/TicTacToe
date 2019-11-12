using Newtonsoft.Json;
using System;

namespace GameLibrary
{
    public class GameRequest
    {
        [JsonConstructor]
        public GameRequest(Player enemy, Player requestPlayer)
        {
            this.Enemy = enemy; 
            this.RequestPlayer = requestPlayer;

            Random r = new Random();

            this.RequestID = r.Next(999, 1234567) + r.Next(999, 1234567);
        }

        public GameRequest()
        {
        }

        public bool Declined { get; set; }

        public int RequestID { get; }

        public Player RequestPlayer { get; set; }

        public Player Enemy { get; set; }

    }
}
