using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface IBattlesRepo
    {
        //Create
        void AddBattle(Battle battles);

        int AddBattles(List<Battle> battles);

        //Read
        List<Battle> GetAllBattles();
        Battle GetBattleByID(int battleID);

        //get/create battle in DB based off battle fetched from official API
        Battle GetBattleWithId(Battle battle);

        //Update
        void UpdateBattle(Battle battle);

        //Delete
        void DeleteBattle(int battleID);
    }
}
