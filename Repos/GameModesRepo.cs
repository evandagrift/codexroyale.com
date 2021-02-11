using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos.IRepos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class GameModesRepo : IGameModesRepo
    {
        //DB access
        private TRContext context;
        
        //constructor sets the context to the argumented context
        public GameModesRepo(TRContext c) { context = c; }

        //adds given game mode to the context
        public void AddGameModeIfNew(GameMode gameMode) 
        { 
            if(context.GameModes.Find(gameMode.Id) == null)
            { 
                context.Add(gameMode);
                context.SaveChanges();
            }
        }


        //returns all gameModes in DB
        public List<GameMode> GetAllGameModes() { return context.GameModes.ToList(); }

        //fetches game mode from DB at given gameModeID
        public GameMode GetGameModeByID(int gameModeID) { return context.GameModes.Find(gameModeID); }

        //updates gameMode at given ID
        public void UpdateGameMode(GameMode gameMode)
        {
            //fetches GameMode from DB using given ID
            GameMode gameModeToUpdate = GetGameModeByID(gameMode.Id);
            
            //if a valid gameMode is fetched from the database it updates it
            if(gameModeToUpdate != null)
            {
                gameModeToUpdate.Id = gameMode.Id;
                gameModeToUpdate.Name = gameMode.Name;
                context.SaveChanges();
            }
        }
        //deletes game mode at with given ID
        public void DeleteGameMode(int gameModeID)
        {
            //fetches game mode with ID from DB
            GameMode gameModeToRemove = GetGameModeByID(gameModeID);

            //if a valid gameMode was fetched it is removed from the context
            if (gameModeToRemove != null)
            {
                context.GameModes.Remove(gameModeToRemove);
                context.SaveChanges();
            }
        }
    }
}
