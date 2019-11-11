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



        public async Task<IEnumerable<Player>> GetPlayerListAsync()
        {
            var response = await _httpClient.GetAsync("/api/Main");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(json);
            return players;
        }

        public async Task<IEnumerable<Player>> PostPlayerInfoToServerAsync(string playerName)
        {
            string json = JsonConvert.SerializeObject(playerName);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Main/players/add", content);
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();


            // das is zu testzwecken, wollt den ganzen http overhead aus dem returnJson haben und schauen obs dann funktioniert
            string[] test = returnJson.Split(",\"id", StringSplitOptions.None);
            string real = test[0];
            real += "}";


            // es haut ihn hier auf beim deserialisieren von einer liste
            // Wenn man nur ein einzelnes object rausholt macht er jetzt einfach den namen null lol
            // wenn er eine liste bekommt macht er td nur ein object XD
            var playerList = JsonConvert.DeserializeObject<IEnumerable<Player>>(returnJson); //(real);
            return playerList;
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
