using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    class PlayersTests
    {
        TRContext fakeContext;
        PlayersRepo repo;

        public PlayersTests()
        {
            //creates sudo options for the fake context
            var options = new DbContextOptionsBuilder<TRContext>()
                .UseInMemoryDatabase(databaseName: "ClashAPI")
                .Options;
            fakeContext = new TRContext(options);

            //seeds some player data into fake context
            fakeContext.Players.Add(new Player() { Tag = "tag1", Name = "name1" });
            fakeContext.Players.Add(new Player() { Tag = "tag2", Name = "name2" });
            fakeContext.Players.Add(new Player() { Tag = "tag3", Name = "name3" });

            //saves those seeded players
            fakeContext.SaveChanges();

            //repo for testing
            repo = new PlayersRepo(fakeContext);

        }
        [Test]
        public void GetAllPlayersTest()
        {
            //tries to get all players from DB
            List<Player> players = repo.GetAllPlayers();

            //if succeeded there will be seeded players in DB
            Assert.True(players.Count > 0);
        }


        [Test]
        public void GetPlayerTest()
        {
            //gets  players to grab a valid Id
            List<Player> players = repo.GetAllPlayers();

            //fetches player at that Id
            Player player = repo.GetPlayerById(players[0].Id);

            //if player failed to fetch it will be null
            Assert.IsNotNull(players);
        }
        
        [Test]
        public void AddPlayerTest()
        {
            //gets  players to get count before add
            List<Player> players = repo.GetAllPlayers();
            int countBefore = players.Count;

            //creates new player to add
            Player player = new Player() { Tag = "tag4", Name = "name4" };

            //adds said player and saves
            repo.AddPlayer(player);
            //gets updated list of all players
            players = repo.GetAllPlayers();

            int newCount = players.Count;
            Assert.AreEqual(newCount, countBefore + 1);
        }

        [Test]
        public void DeletePlayerTest()
        {
            //gets  players to get count before delete
            List<Player> players = repo.GetAllPlayers();
            int countBefore = players.Count;

            //tries to delete the last player added
            repo.DeletePlayer(players[players.Count-1].Id);



        }
        [Test]
        public void UpdatePlayerTest()
        {
            Player player = repo.GetPlayerById(3);
            player.Name = "UPDATED";

            string name = repo.GetPlayerById(3).Name;

            Assert.AreEqual(name, "UPDATED");

        }

    }
}
