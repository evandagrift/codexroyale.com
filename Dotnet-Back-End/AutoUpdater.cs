using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyaleTrackerAPI
{
    public class AutoUpdater
    {
        private Client _client;
        private TRContext _context;
        private PlayerSnapshotRepo repo;
        private List<TrackedPlayer> _trackedPlayers;

        private readonly ILogger _logger;

        public AutoUpdater(Client cl)
        {
            //config to get connection string
            var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

            //db options builder
            var optionsBuilder = new DbContextOptionsBuilder<TRContext>();
            optionsBuilder.UseSqlServer(config["ConnectionStrings:DBConnectionString"]);

            _client = cl;
            //client = new Client(config["ConnectionStrings:BearerToken"]);

            //context
            _context = new TRContext(optionsBuilder.Options, _client);


            _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            //  _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            _logger.Debug("starting auto updater");
        }

        private void RemoveDuplicates()
        {
            _trackedPlayers = _context.TrackedPlayers.ToList();

            List<TrackedPlayer> trackedPlayersWithSameTag;

            _trackedPlayers.ForEach(t =>
                {
                    trackedPlayersWithSameTag = _context.TrackedPlayers.Where(tp => tp.Tag == t.Tag).OrderBy(tp => tp.Priority).ToList();

                    if(trackedPlayersWithSameTag.Count > 1)
                    {
                        TrackedPlayer tempPlayer = trackedPlayersWithSameTag[0];
                        _context.TrackedPlayers.RemoveRange(trackedPlayersWithSameTag);
                        _context.Add(new TrackedPlayer {Tag = tempPlayer.Tag, Priority = tempPlayer.Priority });

                        _context.SaveChanges();
                    }
                });

            _trackedPlayers = _context.TrackedPlayers.ToList();
        }



        private void UpdateHighPriority()
        {
            //besides getting rid of duplicates this will fill the private _trackedPlayerList variable
            RemoveDuplicates();

            if (_trackedPlayers != null)
            {
                if (_trackedPlayers.Any(h => h.Priority == "high"))
                {
                    List<TrackedPlayer> priorityPlayers = _trackedPlayers.Where(p => p.Priority == "high").ToList();
                    priorityPlayers.ForEach(pp =>
                    {
                        repo.SavePlayerFull(pp.Tag);
                        _logger.Debug($"Saving full player {pp.Tag}");
                        pp.Priority = "normal";
                        _context.SaveChanges();
                    });

                }

            }
           
        }

        public void Update()
        {
            // With the options generated above, we can then just construct a new DbContext class


            repo = new PlayerSnapshotRepo(_client, _context);

            DateTime now = DateTime.UtcNow;
            DateTime lastUpdate = now;

            int loopTime = 0;
            while (true)
            {

                //gets the current time to compare time elapsed in the above code
                now = DateTime.UtcNow;


                //gets the time elapsed as an int
                TimeSpan timeElapsed = now - lastUpdate;

                loopTime = ((int)Math.Round(timeElapsed.TotalSeconds));


                if (_trackedPlayers == null) _trackedPlayers = _context.TrackedPlayers.ToList();
                

                    //if it's been less than 15 seconds it sleeps the thread
                    if (loopTime > 15)
                {
                    //saves any new data that has arose
                     _trackedPlayers.ForEach(p =>
                    {
                        UpdateHighPriority();
                        repo.SavePlayerFull(p.Tag);
                    });


                    // sets the lastUpdate time to see how long updating all tracked players tak
                    lastUpdate = now;
                }
            }
        }




    }
}
