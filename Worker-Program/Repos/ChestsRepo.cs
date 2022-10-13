using Newtonsoft.Json;
using ClashFeeder.Models;
using ClashFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashFeeder.Repos
{
    public class ChestsRepo
    {
        //DB access
        private TRContext context;
        private Client client;

        //constructor sets the context to the argumented context
        public ChestsRepo(Client c, TRContext ct) { context = ct; client = c; }
     

        //returns all Chests in DB
        public List<Chest> GetAllChests() { return context.Chests.ToList(); }

        //fetches game mode from DB at given ChestID
        public Chest GetChestByName(string chestName) { return context.Chests.Find(chestName); }

        //takes a raw list of chests fetched from official DB and fills them with usable Urls
        public List<Chest> FillChestUrls(List<Chest> chestsToBeFilled)
        {
            List<Chest> urlChests = GetAllChests();

            for (int c = 0; c < chestsToBeFilled.Count; c++)
            {
                urlChests.ForEach(u =>
                {
                    if (chestsToBeFilled[c].Name == u.Name) { chestsToBeFilled[c].Url = u.Url; }
                });
            }

            return chestsToBeFilled;
        }


        //updates chest with given name
        public void UpdateChest(Chest chest)
        {
            //checks if this chest exists
            if (context.Chests.Any(c => c.Name == chest.Name))
            {
                //fetches GameMode from DB using given ID
                Chest chestToUpdate = GetChestByName(chest.Name);

                //updates and saves chest
                chestToUpdate.Url = chest.Url;
                context.SaveChanges();
            }
        }


        //deletes game mode at with given ID
        public void DeleteChest(string chestName)
        {
            //checks if this chest exists
            if (context.Chests.Any(c => c.Name == chestName))
            {
                //fetches game mode with ID from DB
                Chest chestToRemove = GetChestByName(chestName);

                //deletes the fetched deck
                context.Chests.Remove(chestToRemove);
                context.SaveChanges();
            }
        }
    }
}

