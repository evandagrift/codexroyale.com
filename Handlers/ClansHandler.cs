using CodexRoyaleUpdater.Handlers.Interfaces;
using Newtonsoft.Json;
using RoyaleTrackerAPI;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater
{
    public class ClansHandler : IClansHandler
    {
        //client class has access to both APIs
        Client client;
        TRContext context;

        //Constructor adds reference to HTTP Clients
        public ClansHandler(Client c, TRContext ct)
        {
            client = c;
            context = ct;
        }

        //gets clan data from the official api via their clan tag
        public async Task<Clan> GetOfficialClan(string tag)
        {
            string officialConnectionString = "/v1/clans/%23";

            if (tag != null)
            {
                try
                {
                    //connection string for clan in offical API
                    string connectionString = officialConnectionString + tag.Substring(1);

                    //fetches clan data
                    var result = await client.officialAPI.GetAsync(connectionString);

                    
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();

                        //deseriealizes json into Clan object
                        Clan clan = JsonConvert.DeserializeObject<Clan>(content);

                        //sets location code to a format that the DB can consume
                        clan.LocationCode = (clan.Location["isCountry"] == "false")?  "International" : clan.Location["countryCode"];

                        //update time in same format as official API
                        clan.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        return clan;
                    }
                }
                catch { return null; }
            }
            return null;
        }
    }
}
