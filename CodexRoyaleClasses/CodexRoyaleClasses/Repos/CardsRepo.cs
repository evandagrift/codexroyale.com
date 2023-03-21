using CodexRoyaleClasses.Models;
using Newtonsoft.Json;

namespace CodexRoyaleClasses.Repos
{
    public class CardsRepo
    {
        //Access to DB
        private TRContext _context;
        private Client _client;
        public List<Card> _cards;

        //Constructor assigning argumented context
        public CardsRepo(Client client, TRContext context)
        {
            _context = context;
            _client = client;
            _cards = _context.Cards.ToList();
        }

        //I recieve card URL as Card.IconUrls["medium"] from the official API
        //Must Convert to a string for saving in DB
        public Card ConvertCardUrl(Card cardToConvert)
        {

            cardToConvert.Url = cardToConvert.IconUrls["medium"];
            return cardToConvert;
        }

        //retrieves all cards in the game from official API
        //Returns as a list of Card objects parsed via Newtonsoft
        public async Task<List<Card>> GetAllOfficialCards()
        {
            try
            {
                //connection string for official cards
                string connectionString = "cards";

                //gets the cards in a response message variable
                var result = await _client.officialAPI.GetAsync(connectionString);

                //If the api call was successful
                if (result.IsSuccessStatusCode)
                {
                    //puts the returned content to a string (of Json)
                    var content = await result.Content.ReadAsStringAsync();

                    // trim "{"items":" and the  "}"   from the end from the json call to make it consumable via Newtonsoft 
                    //10 for the begginning being removed, 2 for the last two characters
                    content = content.Substring(9, (content.Length - (9 + 1)));

                    List<Card> cardsToReturn = JsonConvert.DeserializeObject<List<Card>>(content);

                    cardsToReturn.ForEach(c =>
                    {
                        c = ConvertCardUrl(c);
                    });

                    //returns the json as a list of Card objects via newtonsoft
                    return cardsToReturn;
                }


            }
            catch { }
            return null;

        }


        public async Task<List<Card>> UpdateCards()
        {
            //test against official
            //add any that aren't in DB
            List<Card> officialCards = await GetAllOfficialCards();
            List<Card> codexCards = _context.Cards.ToList();
            List<Card> cardsToAdd = new List<Card>();
            if (officialCards != null)
            {
                if (codexCards.Count == 0)
                {
                    cardsToAdd = officialCards;
                }
                else if (codexCards.Count != officialCards.Count && officialCards != null && codexCards != null)
                {
                    AddCardsIfNew(officialCards);

                }
            }
            _cards = _context.Cards.ToList();
            return _cards;
        }

        //adds given card to context
        public void AddCardIfNew(Card card)
        {
            //adds card to db if currently unsaved
            if (_cards.Any(c => c.Id == card.Id))
            {
                _context.Cards.Add(card);
                _context.SaveChanges();
            }
        }

        //checks a list of cards to see if any are not included in the database and adds them
        public void AddCardsIfNew(List<Card> card)
        {
            card.ForEach(c => { AddCardIfNew(c); });

        }


        //deletes card at given cardID
        public bool DeleteCard(int cardID)
        {
            //if a valid card is fetched from the database that card is removed from the context
            if (_cards.Any(c => c.Id == cardID))
            {
                //fetches card with given cardID
                Card cardToDelete = GetCardByID(cardID);

                //removes that card and saves changes
                _context.Cards.Remove(cardToDelete);
                _context.SaveChanges();

                //true if deleted
                return true;
            }
            else { return false; }
        }

        //returns a list of all cards in DB
        public List<Card> GetAllCards() { return _cards; }

        //returns Card from Db with given Card ID
        public Card GetCardByID(int cardId)
        {
            if (_cards.Any(c => c.Id == cardId))
            {
                return _cards.Where(c => c.Id == cardId).FirstOrDefault();
            }
            else return null;
        }

        public async void UpdateCardEventStatus()
        {
            //test against official
            //add any that aren't in DB
            List<Card> officialCards = GetAllOfficialCards().Result;
            List<Card> codexCards = _context.Cards.ToList();

            if (officialCards != null && codexCards != null)
            {
                codexCards.ForEach(c => {

                    int matchCardId = officialCards.FindIndex(o => c.Id == o.Id);
                    if (matchCardId == -1)
                    {
                        c.EventCard = true;
                    }
                    else
                    {
                        c.EventCard = false;
                    }

                    _context.Cards.Update(c);
                });
                _context.SaveChanges();
            }
        }


        //commented out until future testing

        ////updates card at given ID
        ////public void UpdateCard(Card card)
        ////{
        ////    if this card exists in the databases
        ////    if (_cards.Any(c => c.Id == card.Id))
        ////    {

        ////        fetches card with given ID
        ////        Card cardToUpdate = GetCardByID(card.Id);

        ////        updates fields
        ////        cardToUpdate.Name = card.Name;
        ////        cardToUpdate.Url = card.Url;

        ////        saves changes to DB
        ////        _context.SaveChanges();
        ////    }
        ////}



    }

}


//}
/*
        public CardsHandler()
        {

        }
        public async Task<List<Card>> GetAllCards(HttpClient client)
        {
            string connectString;
            if (client.BaseAddress.OriginalString == "http://localhost:52003/api/")
            {
                connectString = "Cards";
            }
            else connectString = "/v1/cards?";

            var result = await client.GetAsync(connectString);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                //Official API has different format for Cards than mine
                if (content.StartsWith("{\"items\":"))
                {
                    content = content.Substring(9, content.Length - 10);
                }
                return JsonConvert.DeserializeObject<List<Card>>(content);
            }
            return null;
        }
*/


/*
public List<Card> FillCardUrls(List<Card> cardsToBeFilled)
{
    for(int i = 0; i<cardsToBeFilled;i++)
    {
        cardsToBeFilled[i] = cardsWthUrl
    }

    return cardsToBeFilled;
}
public Card FillCardUrl(Card cardToBeFilled)
{
    return cardsWthUrl.Where(c => c.Id == cardToBeFilled.Id).FirstOrDefault();
}



public Deck FillDeckUrls(Deck deckToBeFilled)
{
    deckToBeFilled.Card1 = FillCardUrl(deckToBeFilled.Card1);
    deckToBeFilled.Card2 = FillCardUrl(deckToBeFilled.Card2);
    deckToBeFilled.Card3 = FillCardUrl(deckToBeFilled.Card3);
    deckToBeFilled.Card4 = FillCardUrl(deckToBeFilled.Card4);
    deckToBeFilled.Card5 = FillCardUrl(deckToBeFilled.Card5);
    deckToBeFilled.Card6 = FillCardUrl(deckToBeFilled.Card6);
    deckToBeFilled.Card7 = FillCardUrl(deckToBeFilled.Card7);
    deckToBeFilled.Card8 = FillCardUrl(deckToBeFilled.Card8);

    return deckToBeFilled;
}
*/
