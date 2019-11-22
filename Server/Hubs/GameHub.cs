using GameLibrary;
using Microsoft.AspNetCore.SignalR;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IMainService mainService;

        public GameHub(IMainService main)
        {
            this.mainService = main;
        }

        public async Task GetPlayers(string requestedPlayer)
        {
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await mainService.GetPlayersAsync());
        }

        public async Task AddPlayer(string name)
        {
            Player player = new Player(name);
            var addedPlayer = await mainService.AddPlayerAsync(player);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await mainService.GetPlayersAsync());
        }
    }
}
