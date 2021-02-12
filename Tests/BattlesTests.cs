using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class BattlesTests
    {
        TRContext fakeContext;
        BattlesRepo repo;
        List<Battle> battlesToAdd;
        private static string myPlayerTag = "#29PGJURQL";
        private static string randomPlayerTag = "#29PGJURQL";
        HttpClient client;

        private string officialBearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImQ2MjlmZWI0LWU4ZWUtNDAwMi1hNWNhLWY5Y2MxZTYyOGNiMiIsImlhdCI6MTYwNjAzNzc3Nywic3ViIjoiZGV2ZWxvcGVyLzNhNjMxNDdlLWM0MDItNjA0YS1lN2YzLWU0ODc3MDNhOWE2YyIsInNjb3BlcyI6WyJyb3lhbGUiXSwibGltaXRzIjpbeyJ0aWVyIjoiZGV2ZWxvcGVyL3NpbHZlciIsInR5cGUiOiJ0aHJvdHRsaW5nIn0seyJjaWRycyI6WyI3MS4yMzYuMTM4LjE2MSJdLCJ0eXBlIjoiY2xpZW50In1dfQ.upmGRqrABJn6H8cOx0TW5gdwil9_6aZ4vwJHvPqZEfT39Q1RjtNfhEhUKz4SPZYz8sUFgQr8ehlRs1QKBa9-sw";
        private static string officialAPIConnectionString = "https://api.clashroyale.com/";

        //can be called to reset the fake DB
        public void Setup()
        {
            //creates sudo options for the fake context
            var options = new DbContextOptionsBuilder<TRContext>()
                .UseInMemoryDatabase(databaseName: "ClashAPI")
                .Options;
            ///fake context to navigate around Dependancy injection for the controllers
            fakeContext = new TRContext(options);

            //repo for battle handling
            repo = new BattlesRepo(fakeContext);

        }

        //function to get valid battle data from the official Clash Royale API
        public async Task<List<Battle>> GetPlayerBattles()
        {
            //http client to make
            client = new HttpClient();

            //sets the http client base address to the Clash Royale API
            client.BaseAddress = new Uri(officialAPIConnectionString);

            //testing this will require working API Token for the official Clash Royale API
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", officialBearerToken);
            
            //connection string to search a players battle
            string connectionString = "/v1/players/%23" + myPlayerTag.Substring(1) + "/battlelog/";

            //Gets from that address
            var result = await client.GetAsync(connectionString);

            ///if it's a success it will parse response
            if (result.IsSuccessStatusCode)
            {
                //content converted to string once returned and parsed
                var content = await result.Content.ReadAsStringAsync();

                //turns the returned JSON string to a list of Battles
                var battles = JsonConvert.DeserializeObject<List<Battle>>(content);
                
                //returns the fetchd player battles 
                return battles;
            }
            return null;
        }
        
        //tests adding a single battle(as well as GetAllBattles from DB)
        [Test]
        public void AddBattleTest()
        {
            //fresh instance of fakeContext
            Setup();


            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //adds battle to be tested
            repo.AddBattle(playerBattles[0]);

            //GetAllBattles needs to pass for this to pass
            List<Battle> savedBattles = repo.GetAllBattles();

            //call addbattle via repo
            Assert.True(savedBattles.Count > 0);
        }

        [Test]
        public void AddBattlesTest()
        {
            //fresh instance of fakeContext
            Setup();


            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //adds battle to be tested
            repo.AddBattles(playerBattles);

            //GetAllBattles needs to pass for this to pass
            List<Battle> savedBattles = repo.GetAllBattles();


            //call addbattle via repo
            Assert.True(savedBattles.Count > 0);

        }

        [Test]
        public void GetBattleByIdTest()
        {
            //fresh instance of fakeContext
            Setup();

            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //adds battle to be tested
            repo.AddBattles(playerBattles);

            //fetches all battles so a valid Id can be obtained
            List<Battle> allBattles = repo.GetAllBattles();

            //fetches battle by Id from DB
            Battle battle = repo.GetBattleByID(allBattles[0].BattleId);

            //if battle fails to fetch by Id it will return null
            Assert.NotNull(battle);

        }

        [Test]
        public void GetBattleWithIdTest()
        {
            //fresh instance of fakeContext
            Setup();

            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //grabs two valid battles one will be saved before getting
            Battle addedBattle = playerBattles[0];
            Battle notAddedBattle = playerBattles[1];

            //adds battle to test getting an already set Id
            repo.AddBattle(addedBattle);

            //fetches all battles so an initial count can be made
            List<Battle> allBattles = repo.GetAllBattles();

            //fetches added Battles with Id
            addedBattle = repo.GetBattleWithId(addedBattle);

            //Creates notAddedBattle and returns it with an assigned Id
            notAddedBattle = repo.GetBattleWithId(notAddedBattle);

            //test that both have valid Id's set
            Assert.IsTrue(addedBattle.BattleId > 0);
            Assert.IsTrue(notAddedBattle.BattleId > 0);
        }


        [Test]
        public void DeleteBattleTest()
        {
            //fresh instance of fakeContext
            Setup();

            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //adds battle to be tested
            repo.AddBattles(playerBattles);

            //fetches all battles so an initial count can be made
            List<Battle> allBattles = repo.GetAllBattles();
            int battleCount = allBattles.Count;

            //delete battle
            repo.DeleteBattle(allBattles[0].BattleId);

            //fetches all battles and gets an updated count
            allBattles = repo.GetAllBattles();
            int newBattleCount = allBattles.Count;

            //tests to see if a battle was removed
            Assert.AreEqual(newBattleCount, battleCount-1);
        }

        [Test]
        public void UpdateBattleTest()
        {
            //fresh instance of fakeContext
            Setup();

            //fetches battles from the official Clash DB
            List<Battle> playerBattles = GetPlayerBattles().Result;

            //grabs a valid battle to update
            Battle battleToUpdate = repo.GetBattleWithId(playerBattles[0]);

            battleToUpdate.BattleTime = "AAA";

            //updates the battle
            repo.UpdateBattle(battleToUpdate);

            //gets a new instance of the battle to see if updated
            Battle updatedBattle = repo.GetBattleByID(battleToUpdate.BattleId);

            //test if updated
            Assert.AreEqual(updatedBattle.BattleTime, "AAA");
        }
    }
}
