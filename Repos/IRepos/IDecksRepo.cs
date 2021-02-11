using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface IDecksRepo
    {
        //Read
        Deck GetDeckByID(int deckID);
        List<Deck> GetAllDecks();

        
        //Gets the Id for a given deck
        //and adds the decl if it doesn't exist in the DB
        int GetDeckId(Deck deck);

        //returns a deck with set Id and adds the deck to the DB if it doesn't exist
        Deck GetDeckWithId(Deck deck);

        //Update
        void UpdateDeck(Deck deck);

        //Delete
        void DeleteDeck(int deckID);
    }
}
