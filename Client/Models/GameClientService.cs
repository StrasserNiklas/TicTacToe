using GameLibrary;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class GameClientService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        public GameClientService(HttpClient httpClient, ILogger<GameClientService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task PostGameRequest(GameRequest data)
        {
            string json = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Main/games/request", content);
        }

        public async Task DeclineOrAcceptGameRequest(int gameRequestId, bool accept)
        {
            string json = JsonConvert.SerializeObject(accept);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/Main/games/request/{gameRequestId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<PlayerServerStatus> GetPlayerListAndPostAliveAsync(int playerId)
        {
            var response = await httpClient.GetAsync($"/api/Main/players/{playerId}");
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<PlayerServerStatus>(returnJson);
            return players;
        }

        public async Task<Player> PostPlayerInfoToServerAsync(string playerName)
        {
            string json = JsonConvert.SerializeObject(playerName);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Main/players/add", content);
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var player = JsonConvert.DeserializeObject<Player>(returnJson);
            return player;
        }

        public async Task<GameStatus> GetGameStatusAsync(int gameId)
        {
            var response = await httpClient.GetAsync($"/Main/games/{gameId}");
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var status = JsonConvert.DeserializeObject<GameStatus>(returnJson);
            return status;
        }

        public async Task UpdateGameStatusAsync(GameStatus status)
        {
            var json = JsonConvert.SerializeObject(status);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PutAsync($"/api/Main/games{status.GameId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
