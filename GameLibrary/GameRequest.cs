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


        /// <summary>
        /// A value indicating if a game request has been accepted.
        /// </summary>
        public bool Accepted { get; set; }


        /// <summary>
        /// A value indicating if a game has been declined.
        /// </summary>
        public bool Declined { get; set; }

        /// <summary>
        /// The id of the request if chosen randomly and can´t be set afterwards.
        /// </summary>
        public int RequestID { get; set; }

        /// <summary>
        /// The player that has sent/started the request
        /// </summary>
        public Player RequestPlayer { get; set; }

        /// <summary>
        /// The player the request is sent to.
        /// </summary>
        public Player Enemy { get; set; }

    }
}
