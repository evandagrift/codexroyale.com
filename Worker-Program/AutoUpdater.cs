using ClashFeeder;
using ClashFeeder.Models;
using ClashFeeder.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClashFeeder
{
    public class AutoUpdater
    {
        private Client _client;
        private TRContext _context;
        private PlayerSnapshotsRepo repo;
        private List<TrackedPlayer> _trackedPlayerSnapshots;

        public AutoUpdater(Client cl)
        {
            //("/var/www/codexupdater/appsettings.json", true, true)
            //config to get connection string
            var config = new ConfigurationBuilder().AddJsonFile("/var/www/codexupdater/appsettings.json", true, true).Build();

            //db options builder
            var optionsBuilder = new DbContextOptionsBuilder<TRContext>();
            optionsBuilder.UseMySQL(config["ConnectionStrings:DBConnectionString"]);

            _client = cl;
            //client = new Client(config["ConnectionStrings:BearerToken"]);

            //context
            _context = new TRContext(optionsBuilder.Options, _client);


        }

        private void RemoveDuplicates()
        {
            _trackedPlayerSnapshots = _context.TrackedPlayerSnapshots.ToList();

            List<TrackedPlayer> trackedPlayerSnapshotsWithSameTag;

            _trackedPlayerSnapshots.ForEach(t =>
                {
                    trackedPlayerSnapshotsWithSameTag = _context.TrackedPlayerSnapshots.Where(tp => tp.Tag == t.Tag).OrderBy(tp => tp.Priority).ToList();

                    if(trackedPlayerSnapshotsWithSameTag.Count > 1)
                    {
                        TrackedPlayer tempPlayerSnapshot = trackedPlayerSnapshotsWithSameTag[0];
                        _context.TrackedPlayerSnapshots.RemoveRange(trackedPlayerSnapshotsWithSameTag);
                        _context.Add(new TrackedPlayer {Tag = tempPlayerSnapshot.Tag, Priority = tempPlayerSnapshot.Priority });

                        _context.SaveChanges();
                    }
                });

            _trackedPlayerSnapshots = _context.TrackedPlayerSnapshots.ToList();
        }



        private void UpdateHighPriority()
        {
            //besides getting rid of duplicates this will fill the private _trackedPlayerSnapshotList variable
            RemoveDuplicates();

            if (_trackedPlayerSnapshots != null)
            {
                if (_trackedPlayerSnapshots.Any(h => h.Priority == "high"))
                {
                    List<TrackedPlayer> priorityPlayerSnapshots = _trackedPlayerSnapshots.Where(p => p.Priority == "high").ToList();
                    priorityPlayerSnapshots.ForEach(pp =>
                    {
                        repo.SavePlayerSnapshotFull(pp.Tag);
                        pp.Priority = "normal";
                        _context.SaveChanges();
                    });

                }

            }
           
        }

        public void Update()
        {
            // With the options generated above, we can then just construct a new DbContext class


            repo = new PlayerSnapshotsRepo(_client, _context);

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


                if (_trackedPlayerSnapshots == null) _trackedPlayerSnapshots = _context.TrackedPlayerSnapshots.ToList();
                

                    //if it's been less than 15 seconds it sleeps the thread
                    if (loopTime > 15)
                {
                    //saves any new data that has arose
                     _trackedPlayerSnapshots.ForEach(p =>
                    {
                        UpdateHighPriority();
                        repo.SavePlayerSnapshotFull(p.Tag);
                    });


                    // sets the lastUpdate time to see how long updating all tracked PlayerSnapshots tak
                    lastUpdate = now;
                }
            }
        }




    }
}
