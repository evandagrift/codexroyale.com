using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos.IRepos
{
    interface IGameModesRepo
    {
        //Create
        void AddGameModeIfNew(GameMode GameMode);

        //Read
        List<GameMode> GetAllGameModes();
        GameMode GetGameModeByID(int GameModeID);

        //Update
        void UpdateGameMode(GameMode GameMode);

        //Delete
        void DeleteGameMode(int GameModeID);
    }
}