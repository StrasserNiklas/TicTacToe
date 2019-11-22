using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Services
{
    public class UrlService
    {
        private const string BaseUri = "https://localhost:5001/api/main";
        public string ChatAddress => $"https://localhost:5001/test";
        public string GroupAddress => $"{BaseUri}/groupchat";
    }
}
