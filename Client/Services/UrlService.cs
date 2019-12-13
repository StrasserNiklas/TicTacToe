using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Services
{
    public class UrlService
    {
        
            
            
        private const string BaseUri = "https://tictactoefyn.azurewebsites.net/api/main";

        public string LobbyAddress => $"https://tictactoefyn.azurewebsites.net/game";
        
        // lol
        //private const string BaseUri = "https://localhost:44384/api/main";
        //public string LobbyAddress => $"https://localhost:44384/game";
        //public string GroupAddress => $"{BaseUri}/groupchat";
    }
}
