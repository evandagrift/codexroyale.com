using System.Net.Http.Headers;

namespace CodexRoyaleClasses
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
