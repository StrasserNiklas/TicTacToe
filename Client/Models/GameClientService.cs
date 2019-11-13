using GameLibrary;
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
        private readonly HttpClient _httpClient;
        //private readonly ILogger _logger;
        public GameClientService(HttpClient httpClient)//, ILogger<BooksClientService> logger)
        {
            _httpClient = httpClient;
            //_logger = logger;
        }

        public async Task PostGameRequest(GameRequest data)
        {
            string json = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Main/games/request", content);
        }

        public async Task DeclineOrAcceptGameRequest(int gameRequestId, bool accept)
        {
            string json = JsonConvert.SerializeObject(accept);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Main/games/request/{gameRequestId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<PlayerServerStatus> GetPlayerListAndPostAliveAsync(int playerId)
        {
            var response = await _httpClient.GetAsync($"/api/Main/players/{playerId}");
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<PlayerServerStatus>(returnJson);
            return players;
        }

        public async Task<Player> PostPlayerInfoToServerAsync(string playerName)
        {
            string json = JsonConvert.SerializeObject(playerName);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Main/players/add", content);
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var player = JsonConvert.DeserializeObject<Player>(returnJson);
            return player;
        }
    }
}
