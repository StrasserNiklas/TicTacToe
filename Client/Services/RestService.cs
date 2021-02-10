using GameLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class RestService : IRestService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly UrlService urlService = new UrlService();

        public async Task<ApiResponse> Login(string userName, string hashedPassword)
        {
            var json = JsonConvert.SerializeObject(new User() { Id= 0, Username = userName, Password = hashedPassword });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(urlService.ApiAddress + "/users/login", data);
            //var response = await httpClient.PostAsync("https://localhost:44384/api/users/login", data);


            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

                return apiResponse;
            }

            return new ApiResponse(false, "", 0);
        }

        public async Task<ApiResponse> AddUser(string userName, string hashedPassword)
        {
            var json = JsonConvert.SerializeObject(new User() { Username = userName, Password = hashedPassword });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(urlService.ApiAddress + "/users/add", data);
            //var response = await httpClient.PostAsync("https://localhost:44384/api/users/add", data);


            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);

                return apiResponse;
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new ApiResponse(false, "Name is already used.", 0);
            }

            return new ApiResponse(false, "Try again with a different name.", 0);
        }

        public async Task<List<PlayerData>> GetLeaderboardData()
        {
            var response = await httpClient.GetAsync(urlService.ApiAddress + "/wins");

            var responseString = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<PlayerData>>(responseString);

            return data;
        }
    }

    public interface IRestService
    {
        Task<ApiResponse> Login(string userName, string hashedPassword);

        Task<ApiResponse> AddUser(string userName, string hashedPassword);

        Task<List<PlayerData>> GetLeaderboardData();
    }
}
