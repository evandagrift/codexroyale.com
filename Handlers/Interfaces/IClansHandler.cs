using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface IClansHandler
    {
        //Read
        public Task<Clan> GetOfficialClan(string tag);
    }
}

