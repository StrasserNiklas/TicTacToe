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

        public async Task GetPlayers(string requestedPlayerName)
        {
            var allPlayers = await mainService.GetPlayersAsync();
            // select all players except the requested one
            // requested player should not be included in the result
            allPlayers = allPlayers.Where(name => name.PlayerName != requestedPlayerName);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", allPlayers);
        }

        public async Task AddPlayer(string name)
        {
            Player player = new Player(name);
            player.ConnectionId = Context.ConnectionId;
            var addedPlayer = await mainService.AddPlayerAsync(player);
            await base.Clients.All.SendAsync("ReceivePlayersAsync", await mainService.GetPlayersAsync());
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var allPlayers = await mainService.GetPlayersAsync();
            var disconnectedPlayer = allPlayers.FirstOrDefault(player => player.ConnectionId == id);
            await this.mainService.RemovePlayerAsync(disconnectedPlayer);
            var test = await mainService.GetPlayersAsync();
        }
    }
}
