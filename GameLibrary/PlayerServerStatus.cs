using System.Collections.Generic;

namespace GameLibrary
{
    public class PlayerServerStatus
    {
        public PlayerServerStatus(List<Player> players)
        {
            this.Players = players;
        }

        public PlayerServerStatus()
        {
        }

        public string StatusMessage { get; set; }

        public int RequestID { get; set; }

        public Player RequestingPlayer { get; set; }

        public List<Player> Players { get; set; }
    }
}
