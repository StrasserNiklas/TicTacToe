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

        /// <summary>
        /// E.g. Player x has declined your game.
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// If there is a request, its Id is set here.
        /// </summary>
        public int RequestID { get; set; }

        /// <summary>
        /// Is set to default (null) if there has been no request.
        /// </summary>
        public Player RequestingPlayer { get; set; }

        /// <summary>
        /// The list of online players.
        /// </summary>
        public List<Player> Players { get; set; }
    }
}
