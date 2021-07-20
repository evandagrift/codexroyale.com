    using RoyaleTrackerAPI.Models;
    using RoyaleTrackerAPI.Models.RoyaleClasses;
    using RoyaleTrackerClasses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace RoyaleTrackerAPI.Repos
    {
        public class ChestsRepo
        {
            //DB access
            private TRContext context;

            //constructor sets the context to the argumented context
            public ChestsRepo(TRContext c) { context = c; }


            //returns all Chests in DB
            public List<Chest> GetAllChests() { return context.Chests.ToList(); }

            //fetches game mode from DB at given ChestID
            public Chest GetChestByName(string chestName) { return context.Chests.Find(chestName); }

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
                //fetches GameMode from DB using given ID
                Chest chestToUpdate = GetChestByName(chest.Name);

                //if a valid gameMode is fetched from the database it updates it
                if (chestToUpdate != null)
                {
                    chestToUpdate.Url = chest.Url;
                    context.SaveChanges();
                }
            }


            //deletes game mode at with given ID
            public void DeleteChest(string chestName)
            {
                //fetches game mode with ID from DB
                Chest chestToRemove = GetChestByName(chestName);

                //if a valid Chest was fetched it is removed from the context
                if (chestToRemove != null)
                {
                    context.Chests.Remove(chestToRemove);
                    context.SaveChanges();
                }
            }
        }
    }
