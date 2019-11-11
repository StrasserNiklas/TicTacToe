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

        //public async Task<IEnumerable<PlayerWildcard>> GetBooksAsync()
        //{
        //    //_logger.LogDebug("GetBooksAsync called");
        //    var response = await _httpClient.GetAsync("/api/Books");
        //    response.EnsureSuccessStatusCode();
        //    var json = await response.Content.ReadAsStringAsync();
        //    var books = JsonConvert.DeserializeObject<IEnumerable<PlayerWildcard>>(json);
        //    return books;
        //}


        public async Task PostGameRequest(int playerId)
        {
            string json = JsonConvert.SerializeObject(playerId);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Main/games/request", content);
        }

        public async Task<IEnumerable<Player>> PostAliveAndGetPlayerListAsync(int playerId)
        {
            string json = JsonConvert.SerializeObject(playerId);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Main/players/alive", content);
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(returnJson);
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

        //public async Task<PlayerWildcard> AddBookAsync(PlayerWildcard book)
        //{
        //    string json = JsonConvert.SerializeObject(book);
        //    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync("/api/Books", content);
        //    response.EnsureSuccessStatusCode();
        //    var returnJson = await response.Content.ReadAsStringAsync();
        //    var updatedBook = JsonConvert.DeserializeObject<PlayerWildcard>(returnJson);
        //    return updatedBook;
        //}
    }
}
