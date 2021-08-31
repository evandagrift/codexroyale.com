using Newtonsoft.Json;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class CardsRepo
    {
        //Access to DB
        private TRContext context;
        private Client client;
        //Constructor assigning argumented context
        public CardsRepo(Client c, TRContext ct)
        {
            context = ct;
            client = c;
        }
        //Constructor assigning argumented context
        public CardsRepo(TRContext ct)
        {
            context = ct;
        }
        //Constructor assigning argumented context
        public CardsRepo(Client cl)
        {
            client = cl;
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
                var result = await client.officialAPI.GetAsync(connectionString);

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
                return null;


            }
            catch { return null; }

        }


        public async Task<List<Card>> UpdateCards()
        {
            //test against official
            //add any that aren't in DB
            List<Card> officialCards = await GetAllOfficialCards();
            List<Card> codexCards = context.Cards.ToList();
            List<Card> cardsToAdd = new List<Card>();

            if (codexCards.Count == 0)
            {
                cardsToAdd = officialCards;
            }
            else if (codexCards.Count != officialCards.Count && officialCards != null && codexCards != null)
            {
                AddCardsIfNew(officialCards);

            }
            return context.Cards.ToList(); ;
        }

        public async Task<List<Card>> UpdateGetCards()
        {
            await UpdateCards();
            return context.Cards.ToList();
        }

        //adds given card to context
        public void AddCardIfNew(Card card)
        {
            //adds card to db if currently unsaved
            if (!context.Cards.Any(c => c.Id == card.Id))
            {
                context.Cards.Add(card);
                context.SaveChanges();
            }
        }      

        //checks a list of cards to see if any are not included in the database and adds them
        public void AddCardsIfNew(List<Card> card)
        {
            card.ForEach(c =>
            {
                if (!context.Cards.Any(c2 => c2.Id == c.Id))
                {
                    context.Cards.Add(c);
                    context.SaveChanges();
                }
            });
    
        }



        //deletes card at given cardID
        public void DeleteCard(int cardID)
        {
            //if a valid card is fetched from the database that card is removed from the context
            if (context.Cards.Any(c => c.Id == cardID))
            {
                //fetches card with given cardID
                Card cardToDelete = GetCardByID(cardID);

                //removes that card and saves changes
                context.Cards.Remove(cardToDelete);
                context.SaveChanges();
            }
        }

        //returns a list of all cards in DB
        public List<Card> GetAllCards() { return context.Cards.ToList(); }

        //returns Card from Db with given Card ID
        public Card GetCardByID(int cardId) { return context.Cards.Where(c => c.Id == cardId).FirstOrDefault(); }

        //updates card at given ID
        public void UpdateCard(Card card)
        {
            //if this card exists in the databases
            if (context.Cards.Any(c => c.Id == card.Id))
            {

                //fetches card with given ID
                Card cardToUpdate = GetCardByID(card.Id);

                //updates fields
                cardToUpdate.Name = card.Name;
                cardToUpdate.Url = card.Url;

                //saves changes to DB
                context.SaveChanges();
            }
        }



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
