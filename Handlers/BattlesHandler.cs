using CodexRoyaleUpdater.Handlers.Interfaces;
using Newtonsoft.Json;
using RoyaleTrackerAPI;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public class BattlesHandler : IBattlesHandler
    {

        //provides HTTP access for both codex API and the official API
        Client client;
        TRContext context;

        //connection sting for the Codex API Controller that is being handled\
        private string officialConnectionString = "/v1/players/%23";


        //passes in client for use
        public BattlesHandler(Client c, TRContext ct)
        {
            client = c;
            context = ct;
        }


        public async Task<List<Battle>> GetOfficialPlayerBattles(string tag)
        {
            //connection string to fetch player battles with given Tag
            string connectionString =  officialConnectionString + tag.Substring(1) + "/battlelog/";

            //calls the official API
            var result = await client.officialAPI.GetAsync(connectionString);

            //if the call is a success it returns the List of Battles
                if (result.IsSuccessStatusCode)
                {
                //content to json string once recieved and parsed
                    var content = await result.Content.ReadAsStringAsync();

                //deserielizes the json to list of battles
                    var battles = JsonConvert.DeserializeObject<List<Battle>>(content);
                    
                    //cleans up the time string, the official API includes a non functioning TimeZone offset to their datetime string
                    battles.ForEach(b =>
                    {
                        b.BattleTime = b.BattleTime.Substring(0, 15);
                    });

                //returns fetched list of battles
                    return battles;
                }
                return null;
        }


    }
}
