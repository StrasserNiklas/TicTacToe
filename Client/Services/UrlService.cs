// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Client.Services
{
    /// <summary>
    /// Contains the connection for the azure service the server is hosted on and a connection for a local host.
    /// </summary>
    public class UrlService
    {
        //public string LobbyAddress => $"https://tictactoefyn.azurewebsites.net/game";
        public string LobbyAddress => $"https://localhost:5001/game";
    }
}
