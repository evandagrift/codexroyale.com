using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface ICardsHandler
    {
        //Read
        public Task<List<Card>> GetAllOfficialCards();
    }
}
