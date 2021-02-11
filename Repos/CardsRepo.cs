using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class CardsRepo : ICardsRepo
    {
        //Access to DB
        private TRContext context;

        //Constructor assigning argumented context
        public CardsRepo(TRContext c) { context = c; }

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
            if(cardToDelete != null)
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
            if(cardToUpdate != null)
            {
                cardToUpdate.Name = card.Name;
                cardToUpdate.Url = cardToUpdate.Url;
                context.SaveChanges();
            }
        }
    }
}
