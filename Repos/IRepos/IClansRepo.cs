using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface IClansRepo
    {
        //Create
        void AddClan(Clan clan);

        //Read
        Clan GetClanById(int id);
        List<Clan> GetAllClans();

        //Update
        void UpdateClan(Clan clan);

        //Delete
        void DeleteClan(int id);
    }
} 
