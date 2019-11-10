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

        public async Task<IEnumerable<PlayerWildcard>> GetBooksAsync()
        {
            //_logger.LogDebug("GetBooksAsync called");
            var response = await _httpClient.GetAsync("/api/Books");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<IEnumerable<PlayerWildcard>>(json);
            return books;
        }

        public async Task<PlayerWildcard> AddBookAsync(PlayerWildcard book)
        {
            string json = JsonConvert.SerializeObject(book);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Books", content);
            response.EnsureSuccessStatusCode();
            var returnJson = await response.Content.ReadAsStringAsync();
            var updatedBook = JsonConvert.DeserializeObject<PlayerWildcard>(returnJson);
            return updatedBook;
        }
    }
}
