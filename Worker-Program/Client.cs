using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace ClashFeeder
{
    public class Client
    {
        public static string officialAPIConnectionString = "https://api.clashroyale.com/v1/";

        public HttpClient officialAPI;

        public Client(string officialToken)
        {
            officialAPI = new HttpClient();
            officialAPI.BaseAddress = new Uri(officialAPIConnectionString);
            officialAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", officialToken);
        }
    }
}
