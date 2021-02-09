using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class AuthenticationEventArgs : EventArgs
    {
        public AuthenticationEventArgs(int id, string playerName, string token)
        {
            this.Id = id;
            this.PlayerName = playerName;
            this.Token = token;
        }
        public int Id { get; }

        public string PlayerName { get; }

        public string Token { get; }
    }
}
