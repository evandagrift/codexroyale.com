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
        private List<Card> cardsWthUrl;
        //Constructor assigning argumented context
        public CardsRepo(Client c, TRContext ct) 
        { 
            context = ct; 
            client = c; 
            cardsWthUrl = context.Cards.ToList();
            //cardsWthUrl.ForEach(c =>
            //{
            //    AddCardIfNew(c);
            //});
        }
        //Constructor assigning argumented context
        public CardsRepo(TRContext ct) 
        { 
            context = ct;
            cardsWthUrl = context.Cards.ToList();
            //cardsWthUrl.ForEach(c =>
            //{
            //    AddCardIfNew(c);
            //});
        }

        //adds given card to context
        public void AddCardIfNew(Card card)
        {
            if (context.Cards.Find(card.Id) == null)
            {
                context.Cards.Add(card);
                context.SaveChanges();
            }
        }

        //deletes card at given cardID
        public void DeleteCard(int cardID)
        {
            //fetches card with given cardID
            Card cardToDelete = GetCardByID(cardID);

            //if a valid card is fetched from the database that card is removed from the context
            if (cardToDelete != null)
            {
                context.Cards.Remove(cardToDelete);
                context.SaveChanges();
            }
        }

        //returns a list of all cards in DB
        public List<Card> GetAllCards() { return cardsWthUrl; }

        //returns Card from Db with given Card ID
        public Card GetCardByID(int cardId) { return cardsWthUrl.Where( c => c.Id == cardId).FirstOrDefault(); }

        //updates card at given ID
        public void UpdateCard(Card card)
        {
            //fetches card with given ID
            Card cardToUpdate = GetCardByID(card.Id);

            //if a valid card is fetched from the DB that card is Updated
            if (cardToUpdate != null)
            {
                cardToUpdate.Name = card.Name;
                cardToUpdate.Url = cardToUpdate.Url;
                context.SaveChanges();
            }
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
        public List<Card> FillCardUrls(List<Card> cardsToBeFilled)
        {
            cardsToBeFilled.ForEach(card => card = FillCardUrl(card));

            return cardsToBeFilled;
        }
        public Card FillCardUrl(Card cardToBeFilled)
        {
            return cardsWthUrl.Where(c => c.Id == cardToBeFilled.Id).FirstOrDefault();
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


                    //returns the json as a list of Card objects via newtonsoft
                    return JsonConvert.DeserializeObject<List<Card>>(content);
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

            //if successfully returned
            //NEED TO TEST AGAINST 0 in Codex
            if (officialCards != null && codexCards != null)
            {
                List<Card> cardsToAdd = new List<Card>();
                if (officialCards.Count > codexCards.Count)
                {
                    //cycles through all cards in the codex
                    officialCards.ForEach(card =>
                    {
                        if (!codexCards.Contains(card)) { cardsToAdd.Add(card); }
                    });


                }

                if(cardsToAdd.Count > 0)
                {
                    //assigns url variable from IconUrl
                    cardsToAdd = FillCardUrls(cardsToAdd);

                    context.AddRange(cardsToAdd);
                    context.SaveChanges();
                }

            }


            return context.Cards.ToList();
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