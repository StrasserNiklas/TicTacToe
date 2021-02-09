using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class AuthenticationEventArgs : EventArgs
    {
        public AuthenticationEventArgs(int id, string playerName)
        {
            this.Id = id;
            this.PlayerName = playerName;
        }
        public int Id { get;  }

        public string PlayerName { get;  }
    }
}
