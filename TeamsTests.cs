using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    class TeamsTests
    {
        TRContext fakeContext;
        TeamsRepo repo;

        public TeamsTests()
        {
            //creates sudo options for the fake context
            var options = new DbContextOptionsBuilder<TRContext>()
                .UseInMemoryDatabase(databaseName: "ClashAPI")
                .Options;
            fakeContext = new TRContext(options);

            fakeContext.Teams.Add(new Team() { Name = "test1" });
            fakeContext.Teams.Add(new Team() { Name = "test2" });
            fakeContext.Teams.Add(new Team() { Name = "test3" });
            fakeContext.Teams.Add(new Team() { Name = "test4" });
            fakeContext.SaveChanges();

            repo = new TeamsRepo(fakeContext);
        }

        //test get all
        [Test]
        public void GetAllTest()
        {
            List<Team> teams = repo.GetAllTeams();
            Assert.IsNotNull(teams);
        }

        [Test]
        public void GetTeamTest()
        {
            List<Team> teams = repo.GetAllTeams();
            Team Team = repo.GetTeamById(teams[0].TeamId);
            Assert.AreEqual(teams[0].Name, Team.Name);
        }

        [Test]
        public void GetSetTeamIdTest()
        {
            //testing via Team and Player
            //both of these also run GetSetTeamId(List<TeamMember>)
            Team team = new Team() { Name = "AAA", Tag = "ABAB", Name2 = "BBB", Tag2 = "BABA" };
            Team fetchedViaTeam = repo.GetSetTeamId(team);

            Player player = new Player() { Name = "CCC", Tag = "DDDD" };
            Team fetchedViaPlayer = repo.GetSetTeamId(player);

            //if either fail to fetch it will return null
            Assert.NotNull(fetchedViaPlayer);
            Assert.NotNull(fetchedViaTeam);

        }

        [Test]
        public void DeleteTeamTest()
        {
            Assert.IsNotNull(repo.GetTeamById(002));

            repo.DeleteTeam(002);
            fakeContext.SaveChanges();

            Assert.IsNull(repo.GetTeamById(002));
        }

        [Test]
        public void UpdateTeamTest()
        {
            Team team = repo.GetTeamById(003);
            team.Name = "UPDATED";
            fakeContext.SaveChanges();

            Assert.AreEqual("UPDATED", repo.GetTeamById(003).Name);
        }



    }
}
