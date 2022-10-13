using ClashFeeder.Models;
using ClashFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashFeeder.Repos
{
    public class DecksRepo
    {
        //DB Access
        private TRContext _context;
        private Client _client;
        private CardsRepo _cardsRepo;

        //constructor assigning argumented context
        public DecksRepo(Client client, TRContext context) 
        { 
            _context = context;
            _client = client;
        }


        public Deck GetDeckWithId(Deck deck)
        {
            CardsRepo cardsRepo = new CardsRepo(_client, _context);


            //sorts cards in deck highest to lowest so any combo will register the same
            deck.SortCards();

            //once the deck has an Id this will be set
            Deck deckToReturn = null;

            //if there are decks in the DB
            if (_context.Decks.Count() > 0)
            {
                //finds deck with given cards and sets the returnDeck to the deck from the DB w/ Id
                deckToReturn = _context.Decks.Where(d => d.Card1Id == deck.Card1Id && d.Card2Id == deck.Card2Id &&
            d.Card3Id == deck.Card3Id && d.Card4Id == deck.Card4Id && d.Card5Id == deck.Card5Id &&
            d.Card6Id == deck.Card6Id && d.Card7Id == deck.Card7Id && d.Card8Id == deck.Card8Id).FirstOrDefault();
            }

            //if this deck isn't in the DB
            if (deckToReturn == null)
            {
                //sets id to 0 so EF Core will auto assign Id
                deck.Id = 0;

                //check if all the cards in the deck are saved in DB
                //if not gets all cards and sends them to add new one via decksRepo

                //gets a list of all the cards that are both in the deck and saved in the database
                //if this list isn't 8 cards then at least one card in the deck is not saved in the cards section of the database

                List<Card> cardsInDeckAndSaved = _context.Cards.Where(c => c.Id == deck.Card1Id || c.Id == deck.Card2Id || c.Id == deck.Card3Id || c.Id == deck.Card4Id || c.Id == deck.Card5Id || c.Id == deck.Card6Id || c.Id == deck.Card7Id || c.Id == deck.Card8Id).ToList();
                

                //TODO make this more efficient... it's bad
                if(cardsInDeckAndSaved.Count != 8)
                {
                    List<Card> allCards = cardsRepo.GetAllOfficialCards().Result;
                    cardsRepo.AddCardsIfNew(allCards);
                }


                //this deck is added and will be assigned an Id
                _context.Decks.Add(deck);
                //saves that deck to the DB
                _context.SaveChanges();

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
            CardsRepo carsdRepo = new CardsRepo(_client, _context);

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
                _context.Decks.Remove(deckToRemove);
                _context.SaveChanges();
            }
        }

        //returns all decks in the DB
        public List<Deck> GetAllDecks() { return _context.Decks.ToList(); }

        //returns Deck with given ID from DB
        public Deck GetDeckByID(int deckId)
        {
            _cardsRepo = new CardsRepo(_client, _context);
            if (_context.Decks.Any(d => d.Id == deckId))
            {
                Deck returnDeck = _context.Decks.Find(deckId);
                returnDeck.Card1 = _cardsRepo.GetCardByID(returnDeck.Card1Id);
                returnDeck.Card2 = _cardsRepo.GetCardByID(returnDeck.Card2Id);
                returnDeck.Card3 = _cardsRepo.GetCardByID(returnDeck.Card3Id);
                returnDeck.Card4 = _cardsRepo.GetCardByID(returnDeck.Card4Id);
                returnDeck.Card5 = _cardsRepo.GetCardByID(returnDeck.Card5Id);
                returnDeck.Card6 = _cardsRepo.GetCardByID(returnDeck.Card6Id);
                returnDeck.Card7 = _cardsRepo.GetCardByID(returnDeck.Card7Id);
                returnDeck.Card8 = _cardsRepo.GetCardByID(returnDeck.Card8Id);
                return returnDeck;
            }
            else return null;
        }

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
                _context.SaveChanges();
            }
        }

    }
}
