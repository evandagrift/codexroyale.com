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
        public CardsRepo(Client c, TRContext ct) { context = ct; client = c; }

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
        public List<Card> GetAllCards() { return context.Cards.ToList(); }

        //returns Card from Db with given Card ID
        public Card GetCardByID(int cardID) { return context.Cards.Find(cardID); }

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


                    cardsToAdd.ForEach(card => card.Url = card.IconUrls["medium"]);
                }

                if(cardsToAdd.Count > 0)
                {
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