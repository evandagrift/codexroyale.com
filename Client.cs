using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI
{
    public class Client
    {
        public static string officialAPIConnectionString = "https://api.clashroyale.com/";

        public HttpClient officialAPI;

        public Client(string token)
        {
            officialAPI = new HttpClient();
            officialAPI.BaseAddress = new Uri(officialAPIConnectionString);
            officialAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
