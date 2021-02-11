using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface IPlayersRepo
    {
        //Create
        void AddPlayer(Player player);

        //Read
        List<Player> GetAllPlayers();
        Player GetPlayerById(int playerId);

        //Update
        void UpdatePlayer(Player player);

        //Delete
        void DeletePlayer(int playerId);
    }
}
