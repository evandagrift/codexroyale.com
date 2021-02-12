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
    class DecksTests
    {
        TRContext fakeContext;
        DecksRepo repo;
        public DecksTests()
        {
            //creates sudo options for the fake context
            var options = new DbContextOptionsBuilder<TRContext>()
                .UseInMemoryDatabase(databaseName: "ClashAPI")
                .Options;
            fakeContext = new TRContext(options);

            //seeds some data into fake context for testing
            fakeContext.Decks.Add(new Deck() { Card1Id = 001 });

            fakeContext.Decks.Add(new Deck() { Card1Id = 002 });

            fakeContext.Decks.Add(new Deck() {Card1Id = 003 });
            fakeContext.SaveChanges();
            repo = new DecksRepo(fakeContext);
        }

        [Test]
        public void GetAllDecksTest()
        {
            //fetches all decks
            List<Deck> decks = repo.GetAllDecks();

            //if it successfully fetches all decks from DB the count will include the seeded data
            Assert.True(decks.Count > 0);
        }
        [Test]
        public void GetDeckTest()
        {
            //get decks from list for valid Id
            List<Deck> decks = repo.GetAllDecks();
            
            //gets a deck with a valid Id
            Deck deck = repo.GetDeckByID(decks[0].Id);

            //compares the fetched deck with the original on we got the Id from
            Assert.AreEqual(decks[0].Card1Id, deck.Card1Id);
        }
        [Test]
        public void GetDeckWithIDTest()
        {
            //creates a new deck
            Deck deck = new Deck() { Card1Id = 1, Card2Id = 1, Card3Id = 1, Card4Id = 1, Card5Id = 1, Card6Id = 1, Card7Id = 1, Card8Id = 1 };

            //get/creates that deck with Id
            Deck deckWithId = repo.GetDeckWithId(deck);

            //makes sure that it infact has an Id
            Assert.True(deckWithId.Id > 0);
        }
        [Test]
        public void GetDeckIDTest()
        {
            //creates a deck to get the Id for
            Deck deck = new Deck() { Card1Id = 1, Card2Id = 1, Card3Id = 1, Card4Id = 1, Card5Id = 1, Card6Id = 1, Card7Id = 1, Card8Id = 1 };

            //depending on whether or not getDeckWithId was run this will either create a new instance of this deck in the DB and return it's Id or return the Id of already existing deck
            int deckId = repo.GetDeckId(deck);

            //if it succeeded the Id will be over 0
            Assert.True(deckId > 0);
        }


        [Test]
        public void DeleteDeckTest()
        {
            //deletes deck with Id 2
            repo.DeleteDeck(2);
            //saves this delete to the DB
            fakeContext.SaveChanges();

            //tries to fetch the deleted deck
            Deck deletedDeck = repo.GetDeckByID(2);

            //if the deck was deleted than it will fail to fetch and register null
            Assert.IsNull(deletedDeck);
        }

        [Test]
        public void UpdateDeckTest()
        {
            //get decks from list for valid Id
            List<Deck> decks = repo.GetAllDecks();

            //gets a deck with a valid Id
            Deck decktoUpdate = decks[0];

            //updates that deck reference
            decktoUpdate.Card1Id = 999;

            //since the updated deck is a reference to one in the DB we just need to save the changes
            fakeContext.SaveChanges();

            //gets the updated deck from the DB
            Deck updatedDeck = repo.GetDeckByID(decks[0].Id);

            //test if deck updated
            Assert.AreEqual(updatedDeck.Card1Id, 999);
        }


    }
}
