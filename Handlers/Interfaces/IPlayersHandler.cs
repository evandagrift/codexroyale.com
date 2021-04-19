using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public interface IPlayersHandler
    {
        //Read
        public Task<Player> GetOfficialPlayer(string tag);
    }
}
