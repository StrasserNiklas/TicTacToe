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
    public class RestService
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
                var id = JsonConvert.DeserializeObject<int>(responseString);

                return new ApiResponse(true, "", id);
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
                var id = JsonConvert.DeserializeObject<int>(responseString);

                return new ApiResponse(true, "", id);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new ApiResponse(false, "Name is already used.", 0);
            }

            return new ApiResponse(false, "Try again with a different name.", 0);
        }
    }
}
