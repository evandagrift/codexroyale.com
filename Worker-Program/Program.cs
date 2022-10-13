using ClashFeeder.Models;
using ClashFeeder.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ClashFeeder
{
    //to do, make PlayerSnapshots last seen update with added battles
    //sort out old battles to get faster response
    //make there be a full sweep every 3 hours or so to ensure no missed data
    class Program
    {
        static async Task Main(string[] args)
        {


            //config to get connection string
            var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
           
            //db options builder
            var optionsBuilder = new DbContextOptionsBuilder<TRContext>();

            optionsBuilder.UseMySQL(config["ConnectionStrings:DBConnectionString"]);
           



            Client client = new Client(config["ConnectionStrings:BearerToken"]);

            TRContext context = new TRContext(optionsBuilder.Options, client);

            CardsRepo cardsRepo = new CardsRepo(client, context);

            List<Card> cards = cardsRepo.GetAllCards();

            List<Card> cardsOFF = cardsRepo.GetAllOfficialCards().Result;
            ClansRepo clansRepo = new ClansRepo(client, context);

            PlayerSnapshotsRepo PlayerSnapshotsRepo = new PlayerSnapshotsRepo(client, context);
            BattlesRepo battlesRepo = new BattlesRepo(client, context);

            Clan clan;
            List<PlayerSnapshot> watchList = new List<PlayerSnapshot>();

            string clanTag = "#8CYPL8R";
            string randomClanTag = "#9QGPC82Y0";
            int count = 0;
            List<Battle> pBattles;
            //


            //Get Clan
            // Get all battles

            //gets current clan with Tag
            clan = await clansRepo.GetOfficialClan(clanTag);

            //saves clan data to DB
            clansRepo.AddClan(clan);

            //cycles through all PlayerSnapshots in the clan
            clan.MemberList.ForEach(p =>
            {
                //fetches the current PlayerSnapshot battles from the official DB
                pBattles = battlesRepo.GetOfficialPlayerBattles(p.Tag).Result;

                //adds new fetched battles to the DB and gets a count of added lines
                count += battlesRepo.AddBattles(pBattles);
                PlayerSnapshotsRepo.AddPlayerSnapshot(p);
            });

            //give count of battles added to the database
            Console.WriteLine("Initially added " + count + " battles.");
            count = 0;

            //sets date times for comparison
            DateTime now = DateTime.UtcNow;
            DateTime lastFullSave = DateTime.UtcNow;
            DateTime lastClanSearch = DateTime.UtcNow;
            DateTime clanLastUpdated = DateTime.UtcNow;

            //TODO SAVE PlayerSnapshotS W/ FULL SAVE if last seen is after last full save

            //get clan data if 30 sec has elapsed, so we don't call it each loop
            TimeSpan timeSinceClanUpdate = now - clanLastUpdated;
            TimeSpan timeSinceFullSave = now - lastFullSave;
            TimeSpan timeSinceClanSearch = now - lastClanSearch;

            //list to fill with battles to add to DB this is so we don't constantly pass all the battles to the api even though most wont be saved
            List<Battle> battlesToAdd = new List<Battle>();

            while (true)
            {
                now = DateTime.UtcNow;
                //get clan data if 30 sec has elapsed, so we don't call it each loop
                timeSinceClanSearch = now - lastClanSearch;

                //calls the official API every 5 seconds
                if (timeSinceClanSearch.TotalSeconds > 5)
                {
                    lastClanSearch = now;

                    //calls the clan to get clan members and when last seen
                    clan = await clansRepo.GetOfficialClan(clanTag);

                    //if PlayerSnapshot last seen is less than 10 min and not in watched list add to list
                    PlayerSnapshot playerSnapshot;

                    TimeSpan timeSinceLastSeen;

                    //incase the officical API fails to respond properly
                    if (clan != null)
                    {
                        clan.MemberList.ForEach(p =>
                        {

                            //if the PlayerSnapshot isn't already in the watchlist will return null
                            playerSnapshot = watchList.Where(w => w.Tag == p.Tag).FirstOrDefault();

                            //if PlayerSnapshot isn't in the watch list
                            if (playerSnapshot == null)
                            {

                                DateTime playerLastSeen = DateTime.ParseExact(p.LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                                timeSinceLastSeen = now - playerLastSeen;

                                //if PlayerSnapshot last seen in past 15 min
                                if (timeSinceLastSeen.TotalMinutes < 15)
                                {
                                    watchList.Add(p);
                                    Console.WriteLine(p.Name + " was added to the wathchlist");
                                }
                            }
                        });
                    }
                    else { Console.WriteLine("Clan Is Null"); }

                    //SLOWNESS IS COMING FROM ABOVE THIS ^^^^^^^
                    if (watchList.Count > 0)
                    {
                        for (int w = 0; w < watchList.Count; w++)
                        {

                            pBattles = await battlesRepo.GetOfficialPlayerBattles(watchList[w].Tag);

                            if (pBattles != null)
                            {
                                battlesToAdd = new List<Battle>();

                                pBattles.OrderByDescending(b => b.BattleTime);

                                //newest is returned in the list at 0
                                for (int b = 0; b < pBattles.Count; b++)
                                {
                                    DateTime timeOfBattle = DateTime.ParseExact(pBattles[b].BattleTime.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);


                                    //get the difference between last full Save the battleTime
                                    //if the battle is newer than last fullSave it adds
                                    TimeSpan alreadySaved = timeOfBattle - lastFullSave;

                                    if (alreadySaved.Seconds > 0)
                                    {
                                        battlesToAdd.Add(pBattles[b]);
                                    }

                                }


                                if (0 < battlesToAdd.Count)
                                {
                                    count =  battlesRepo.AddBattles(battlesToAdd);

                                    if (count == 0 && battlesToAdd.Count > 0)
                                    {
                                        battlesToAdd = new List<Battle>();
                                    }
                                    else
                                    {
                                        watchList[w].LastSeen = now.ToString("yyyyMMddTHHmmss"); ;
                                        Console.WriteLine(watchList[w].Name + " played " + count + " games.");
                                    }
                                }
                            }
                            else Console.WriteLine("Player Battles is null");
                        }

                        for (int p = 0; p < watchList.Count; p++)
                        {
                            TimeSpan timeOut = now - (DateTime.ParseExact(watchList[p].LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture));

                            if (timeOut.Minutes >= 15)
                            {

                                Console.WriteLine(watchList[p].Name + " was removed from watch list");
                                watchList.Remove(watchList[p]);
                            }
                            //remove if sat too long
                        }
                    }



                    //add recently logged in if not in watch list
                    //remove PlayerSnapshots in watchlist based off battle time
                    //ONLY ADD NEW IF YOU FEED NEW INSTANCES OF ALREADY WATCHED PlayerSnapshotS, PlayerSnapshotS ONLINE FOR LONGER PERIOD MIGHT BE MISSED

                }

                //full save every 2.5 hours in case missed anything
                TimeSpan fullSaveTimer = now - lastFullSave;
                if (fullSaveTimer.Minutes >= 20)
                {
                    //Get Clan
                    // Get all battles

                    //gets current clan with Tag
                    clan = await clansRepo.GetOfficialClan(clanTag);

                    //saves clan data to DB
                    clansRepo.AddClan(clan);

                    //cycles through all PlayerSnapshots in the clan
                    clan.MemberList.ForEach(p =>
                    {
                        //fetches the current PlayerSnapshot battles from the official DB
                        List<Battle> pBattles = battlesRepo.GetOfficialPlayerBattles(p.Tag).Result;
                        PlayerSnapshotsRepo.AddPlayerSnapshot(p);

                        //adds new fetched battles to the DB and gets a count of added lines
                        count += battlesRepo.AddBattles(pBattles);
                    });

                    lastFullSave = DateTime.UtcNow;
                    clanLastUpdated = DateTime.UtcNow;
                    //
                    //ADD Save clan when do full save
                    //

                }
                //cycle through all users in watchlist

                //fetches their battles and sets watched PlayerSnapshot's last update time to the most recent battle time

                //saves their battles newer than their last update time

                // if PlayerSnapshot hasn't played a game in 20 min drop from watched list



                //returns most recent saved





            }

        }

    }
}

/* while (true)
            {
                DateTime now = DateTime.UtcNow;
                List<PlayerSnapshot> watchList = new List<PlayerSnapshot>();
                TimeSpan timeSinceFullSearch = now - lastFullSearch;

                double hrSinceFullUpdate = timeSinceFullSearch.TotalHours;

                // 
                if (hrSinceFullUpdate > 2.5)
                {
                    weAreFunny = clansRepo.GetOfficialClan(clanTag).Result;

                    weAreFunny.MemberList.ForEach(p =>
                    {
                        //convert to int for comparison with PlayerSnapshot
                        //int PlayerSnapshotDayInt = Int32.Parse(p.LastSeen.Substring(0, 8));
                        //only grabbing 4 characters because we only need up to the minute
                        //int PlayerSnapshotTimeInt = Int32.Parse(p.LastSeen.Substring(8, 4));

                        //int daysSinceLoggin = dayInt - PlayerSnapshotDayInt;

                        DateTime lastSeen = DateTime.ParseExact(p.LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                        //get the difference between last time PlayerSnapshot was seen and now
                        TimeSpan ts = now - lastSeen;

                        //if PlayerSnapshot was seen in the past 20 minutes they are added to the watch list if isn't already in list
                        if (ts.TotalMinutes < 30)
                        {
                            if (!watchList.Contains(p)) { watchList.Add(p); }
                        }
                    });
                }


                    watchList.ForEach(p =>
                {
                    List<Battle> pBattles = battlesRepo.GetOfficialPlayerSnapshotBattles(p.Tag).Result;

                    List<Battle> battlesToAdd = new List<Battle>();

                    pBattles.ForEach(b =>
                    {
                        DateTime timeOfBattle = DateTime.ParseExact(p.LastSeen.Substring(0, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                        //get the difference between last time PlayerSnapshot was seen and now
                        TimeSpan alreadySaved = timeOfBattle - lastFullSave;
                        //if (alreadySaved.TotalSeconds > 0)
                        //{
                        //    Console.WriteLine();
                        //}
                        //else Console.WriteLine();
                    });


                    count = battlesRepo.AddBattles(pBattles).Result;
                    if (count > 0)
                        Console.WriteLine(p.Name + " played " + count + " games.");

                    //remove if sat too long
                });



            }
while(true)
{
    var clans = await Repo.GetClanData(clanTag, officialAPI);
    List<PlayerSnapshot> PlayerSnapshots = clans.MemberList;

    PlayerSnapshots.ForEach(p =>
    {
        //20201210T040100.000Z
        //YYYYMMDDTHHMMSS
        string[] dateStringArray = p.LastSeen.Split(".");

        //Console.WriteLine(p.Name + ": " + dateStringArray[0]);
        Console.WriteLine(dateStringArray[1]);
        if(dateStringArray[1] != "000Z")
        {
            Console.WriteLine("Hold Up");
        }    

        //DateTime when = DateTime.ParseExact(p.LastSeen, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
    });
}













/*



PlayerSnapshot elodin = PlayerSnapshots.Where(p => p.Name == "Elodin").FirstOrDefault();
PlayerSnapshot giggidy = PlayerSnapshots.Where(p => p.Name == "Giggidy").FirstOrDefault();

string elodinLastSeen = elodin.LastSeen;
Console.WriteLine("Elodin:" + elodinLastSeen);

string giggidyLastSeen = giggidy.LastSeen;
Console.WriteLine("Giggidy:" + giggidyLastSeen);

while (true)
{ 
    clans = await Repo.GetClanData(clanTag, officialAPI);
    List<PlayerSnapshot> updatedPlayerSnapshots = clans.MemberList.OrderBy(p => p.Tag).ToList();

    for(int i = 0; i < updatedPlayerSnapshots.Count;i++)
    {
        if(updatedPlayerSnapshots[i].Tag == PlayerSnapshots[i].Tag)
        {
            if(updatedPlayerSnapshots[i].LastSeen == PlayerSnapshots.)
        }
    }


    elodin = PlayerSnapshots.Where(p => p.Name == "Elodin").FirstOrDefault();

    if (elodinLastSeen != elodin.LastSeen)
    {
        elodinLastSeen = elodin.LastSeen;
        Console.WriteLine("Elodin:" + elodinLastSeen);
    }

    giggidy = PlayerSnapshots.Where(p => p.Name == "Giggidy").FirstOrDefault();
    if (giggidyLastSeen != giggidy.LastSeen)
    {9999999999
        giggidyLastSeen = giggidy.LastSeen;
        Console.WriteLine("Giggidy:" + giggidyLastSeen);
    }

}
*/
