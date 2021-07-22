using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class DecksRepo
    {
        //DB Access
        private TRContext context;

        //constructor assigning argumented context
        public DecksRepo(TRContext c) { context = c; }


        public Deck GetDeckWithId(Deck deck)
        {
            CardsRepo cardsRepo = new CardsRepo(context);


            //sorts cards in deck highest to lowest so any combo will register the same
            deck.SortCards();

            //once the deck has an Id this will be set
            Deck deckToReturn = null;

            //if there are decks in the DB
            if (context.Decks.Count() > 0)
            {
                //finds deck with given cards and sets the returnDeck to the deck from the DB w/ Id
                deckToReturn = context.Decks.Where(d => d.Card1Id == deck.Card1Id && d.Card2Id == deck.Card2Id &&
            d.Card3Id == deck.Card3Id && d.Card4Id == deck.Card4Id && d.Card5Id == deck.Card5Id &&
            d.Card6Id == deck.Card6Id && d.Card7Id == deck.Card7Id && d.Card8Id == deck.Card8Id).FirstOrDefault();
            }

            //if this deck isn't in the DB
            if (deckToReturn == null)
            {
                //this deck is added and will be assigned an Id
                context.Decks.Add(deck);
                //saves that deck to the DB
                context.SaveChanges();

                deckToReturn = deck;

                //fetches the just saved deck
                //deckToReturn = context.Decks.Where(d => d.Card1Id == deck.Card1Id && d.Card2Id == deck.Card2Id &&
                //d.Card3Id == deck.Card3Id && d.Card4Id == deck.Card4Id && d.Card5Id == deck.Card5Id &&
                //d.Card6Id == deck.Card6Id && d.Card7Id == deck.Card7Id && d.Card8Id == deck.Card8Id).FirstOrDefault();
            }

            return FillDeckUrls(deckToReturn);
        }

        public Deck FillDeckUrls(Deck deck)
        {
            CardsRepo carsdRepo = new CardsRepo(context);

            deck.Card1 = carsdRepo.GetCardByID(deck.Card1Id);
            deck.Card2 = carsdRepo.GetCardByID(deck.Card2Id);
            deck.Card3 = carsdRepo.GetCardByID(deck.Card3Id);
            deck.Card4 = carsdRepo.GetCardByID(deck.Card4Id);
            deck.Card5 = carsdRepo.GetCardByID(deck.Card5Id);
            deck.Card6 = carsdRepo.GetCardByID(deck.Card6Id);
            deck.Card7 = carsdRepo.GetCardByID(deck.Card7Id);
            deck.Card8 = carsdRepo.GetCardByID(deck.Card8Id);

            return deck;
        }

        public Deck GetDeckWithId(List<Card> listCards)
        {
            return GetDeckWithId(new Deck(listCards));
        }

        public int GetDeckId(Deck deck)
        {
            //fetches/creates the deck with given cards
            Deck temp = GetDeckWithId(deck);

            //returns that deck's Id
            return temp.Id;
        }

        //deletes deck with given ID
        public void DeleteDeck(int deckID)
        {
            //fetches deck with given ID
            Deck deckToRemove = GetDeckByID(deckID);

            //if a valid deck is fetched from DB it removes it from context
            if (deckToRemove != null)
            {
                context.Decks.Remove(deckToRemove);
                context.SaveChanges();
            }
        }

        //returns all decks in the DB
        public List<Deck> GetAllDecks() { return context.Decks.ToList(); }

        //returns Deck with given ID from DB
        public Deck GetDeckByID(int deckID) { return context.Decks.Find(deckID); }

        //updates deck at given ID with argumented Deck Fields
        public void UpdateDeck(Deck deck)
        {
            //fetches deck at given DeckID
            Deck deckToUpdate = GetDeckByID(deck.Id);

            //if a valid deck is fetched it updates all of the fields of that deck
            if (deckToUpdate != null)
            {
                deckToUpdate.Card1Id = deck.Card1Id;
                deckToUpdate.Card2Id = deck.Card2Id;
                deckToUpdate.Card3Id = deck.Card3Id;
                deckToUpdate.Card4Id = deck.Card4Id;
                deckToUpdate.Card5Id = deck.Card5Id;
                deckToUpdate.Card6Id = deck.Card6Id;
                deckToUpdate.Card7Id = deck.Card7Id;
                deckToUpdate.Card8Id = deck.Card8Id;

                //because the deck is a reference to one in the db, changes can just be saved after being made
                context.SaveChanges();
            }
        }

    }
}
