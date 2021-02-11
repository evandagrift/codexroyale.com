using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface ICardsRepo
    {
        //Create
        void AddCardIfNew(Card card);
        
        //Read
        List<Card> GetAllCards();
        Card GetCardByID(int cardID);

        //Update
        void UpdateCard(Card card);

        //Delete
        void DeleteCard(int cardID);
    }
}
