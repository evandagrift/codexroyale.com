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
    class CardsTests
    {
        TRContext fakeContext;
        CardsRepo repo;

        public CardsTests()
        {
            //creates sudo options for the fake context
            var options = new DbContextOptionsBuilder<TRContext>()
                .UseInMemoryDatabase(databaseName: "ClashAPI")
                .Options;
            fakeContext = new TRContext(options);

            fakeContext.Cards.Add(new Card() { Id = 001, Name = "test1" });
            fakeContext.Cards.Add(new Card() { Id = 002, Name = "test2" });
            fakeContext.Cards.Add(new Card() { Id = 003, Name = "test3" });
            fakeContext.Cards.Add(new Card() { Id = 004, Name = "test4" });
            fakeContext.SaveChanges();

            repo = new CardsRepo(fakeContext);
        }

        //test get all
        [Test]
        public void GetAllTest()
        {
            List<Card> cards = repo.GetAllCards();
            Assert.IsNotNull(cards);
        }

        [Test]
        public void GetCardTest()
        {
            Card card = repo.GetCardByID(001);
            Assert.IsTrue(card.Name == "test1");
        }

        [Test]
        public void AddCardTest()
        {
            Card card = new Card() { Id = 005, Name = "test5" };
            repo.AddCardIfNew(card);
            fakeContext.SaveChanges();

            Assert.AreEqual(repo.GetCardByID(005).Name, "test5");
        }

        [Test]
        public void DeleteCardTest()
        {
            Assert.IsNotNull(repo.GetCardByID(002));

            repo.DeleteCard(002);
            fakeContext.SaveChanges();

            Assert.IsNull(repo.GetCardByID(002));
        }

        [Test]
        public void UpdateCardTest()
        {
            Card card = repo.GetCardByID(003);
            card.Name = "UPDATED";
            fakeContext.SaveChanges();

            Assert.AreEqual("UPDATED", repo.GetCardByID(003).Name);
        }




}
}
