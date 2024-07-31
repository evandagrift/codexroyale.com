using Newtonsoft.Json;
using ClashFeeder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashFeeder.Repos
{
    public class PlayerSnapshotsRepo
    {
        //DB access
        private TRContext _context;
        private Client _client;

        //constructor loads in DB Context
        public PlayerSnapshotsRepo(Client c, TRContext ct) { _context = ct; _client = c; }

        public void AddPlayerSnapshot(PlayerSnapshot playerSnapshot)
        {
            // removes Id in case posted with an Id
            playerSnapshot.Id = 0;
            _context.PlayerSnapshots.Add(playerSnapshot);
            _context.SaveChanges();
        }

        //deletes PlayerSnapshot with given PlayerSnapshotTag
        public void DeletePlayerSnapshot(int PlayerSnapshotId)
        {
            //pulls PlayerSnapshot instance from db w/ given tag
            PlayerSnapshot PlayerSnapshotToDelete = GetPlayerSnapshotById(PlayerSnapshotId);

            //if a valid PlayerSnapshot instance i fetched from the database, it will be removed
            if (PlayerSnapshotToDelete != null)
            {
                _context.PlayerSnapshots.Remove(PlayerSnapshotToDelete);
                _context.SaveChanges();
            }
        }

        public List<PlayerSnapshot> GetAllPlayerSnapshots() { return _context.PlayerSnapshots.ToList(); }

        public PlayerSnapshot GetPlayerSnapshotById(int PlayerSnapshotTag) { return _context.PlayerSnapshots.Find(PlayerSnapshotTag); }


        public void UpdatePlayerSnapshot(PlayerSnapshot PlayerSnapshot)
        {
            //pulls instance of PlayerSnapshot from DB
            PlayerSnapshot PlayerSnapshotToUpdate = GetPlayerSnapshotById(PlayerSnapshot.Id);

            //if a valid PlayerSnapshot is returned it updates all the fields in the fetched class
            if (PlayerSnapshotToUpdate != null)
            {
                PlayerSnapshotToUpdate.Tag = PlayerSnapshot.Tag;
                PlayerSnapshotToUpdate.Name = PlayerSnapshot.Name;
                PlayerSnapshotToUpdate.BestTrophies = PlayerSnapshot.BestTrophies;
                PlayerSnapshotToUpdate.Trophies = PlayerSnapshot.Trophies;

                PlayerSnapshotToUpdate.CardsDiscovered = PlayerSnapshot.CardsDiscovered;

                PlayerSnapshotToUpdate.ClanTag = PlayerSnapshot.ClanTag;


                PlayerSnapshotToUpdate.ClanCardsCollected = PlayerSnapshot.ClanCardsCollected;
                PlayerSnapshotToUpdate.CurrentDeckId = PlayerSnapshot.CurrentDeckId;
                PlayerSnapshotToUpdate.CurrentFavouriteCardId = PlayerSnapshot.CurrentFavouriteCardId;
                PlayerSnapshotToUpdate.Donations = PlayerSnapshot.Donations;
                PlayerSnapshotToUpdate.DonationsReceived = PlayerSnapshot.DonationsReceived;
                PlayerSnapshotToUpdate.ExpLevel = PlayerSnapshot.ExpLevel;
                PlayerSnapshotToUpdate.LastSeen = PlayerSnapshot.LastSeen;
                PlayerSnapshotToUpdate.StarPoints = PlayerSnapshot.StarPoints;
                PlayerSnapshotToUpdate.Tag = PlayerSnapshot.Tag;
                PlayerSnapshotToUpdate.TeamId = PlayerSnapshot.TeamId;
                PlayerSnapshotToUpdate.TotalDonations = PlayerSnapshot.TotalDonations;
                PlayerSnapshotToUpdate.UpdateTime = PlayerSnapshot.UpdateTime;
                PlayerSnapshotToUpdate.Wins = PlayerSnapshot.Wins;
                PlayerSnapshotToUpdate.Losses = PlayerSnapshot.Losses;
                _context.SaveChanges();
            }

        }

        public async Task<string> GetLastSeen(string PlayerSnapshotTag, string clanTag)
        {
            ClansRepo clanRepo = new ClansRepo(_client, _context);

            try
            {

                //fetch clan to get data from
                Clan clan = await clanRepo.GetOfficialClan(clanTag);

                //clan members are a list of PlayerSnapshots, we grab the PlayerSnapshot with matching tag
                PlayerSnapshot PlayerSnapshot = clan.MemberList.Where(p => p.Tag == PlayerSnapshotTag).FirstOrDefault();

                //return when last seen, Substringed because returned time has a timezone offset but all PlayerSnapshots are returned with an offset of 0
                return PlayerSnapshot.LastSeen.Substring(0, 15);

            }
            catch { return null; }
        }


        //gets PlayerSnapshot data from the official api via their PlayerSnapshot tag
        public async Task<PlayerSnapshot> GetOfficialPlayerSnapshot(string tag)
        {
            TeamsRepo temsRepo = new TeamsRepo(_context);
            DecksRepo decksRepo = new DecksRepo(_client, _context);
            CardsRepo cardsRepo = new CardsRepo(_client, _context);

            ClansRepo clansRepo = new ClansRepo(_client, _context);

            //try in case we get connection errors`
            try
            {

                string connectionString = "/v1/PlayerSnapshots/%23" + tag.Substring(1);

                var result = await _client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    PlayerSnapshot PlayerSnapshot = JsonConvert.DeserializeObject<PlayerSnapshot>(content);


                    if (PlayerSnapshot.Name != "" && PlayerSnapshot.CurrentFavouriteCard != null)
                    {
                        PlayerSnapshot.TeamId = 0;

                        //assigns current favorite card details
                        PlayerSnapshot.CurrentFavouriteCardId = PlayerSnapshot.CurrentFavouriteCard.Id;
                        PlayerSnapshot.CurrentFavouriteCard.SetUrls();
                        PlayerSnapshot.CardsDiscovered = PlayerSnapshot.Cards.Count;
                        PlayerSnapshot.Cards.ForEach(c =>
                        {
                            c.SetUrls();
                        });

                        if (PlayerSnapshot.Clan != null)
                        {
                            PlayerSnapshot.ClanTag = PlayerSnapshot.Clan.Tag;
                            PlayerSnapshot.LastSeen = await GetLastSeen(PlayerSnapshot.Tag, PlayerSnapshot.ClanTag);
                        }


                        PlayerSnapshot.Deck = decksRepo.GetDeckWithId(PlayerSnapshot.CurrentDeck);
                        PlayerSnapshot.CurrentDeckId = PlayerSnapshot.Deck.Id;

                        //removing this from the provided data so Front end doesn't use currentdeck instead of the dressed Deck with all details
                        PlayerSnapshot.CurrentDeck = null;


                        //sets the time of this update
                        PlayerSnapshot.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        TrackedPlayer trackedPlayerSnapshot = _context.TrackedPlayerSnapshots.Where(t => t.Tag == PlayerSnapshot.Tag).FirstOrDefault();


                        //adding battles is done in the auto update thread to handle concurrency errors
                        //adds the PlayerSnapshot to have their data tracked
                        if (trackedPlayerSnapshot == null)
                        {
                            _context.TrackedPlayerSnapshots.Add(new TrackedPlayer { Tag = PlayerSnapshot.Tag, Priority = "high" });
                            _context.SaveChanges();
                        }


                        return PlayerSnapshot;
                    }
                    else { return null; }
                }
                else return null;


            }

            catch { return null; }

        }
        public async Task<List<Chest>> GetPlayerSnapshotChestsAsync(string tag)
        {
            if (tag != "" && tag.Length > 3)
            {

                if (tag[0] == '#')
                {
                    tag = "%23" + tag.Substring(1);
                }
                else 
                    if(tag.Substring(0,3) == "%23")
                    {

                    }
                else
                {
                    return null;
                }



            }
            string connectionString = "/v1/PlayerSnapshots/" + tag + "/upcomingchests";
            ChestsRepo chestsRepo = new ChestsRepo(_client, _context);
            //try in case we get connection errors`
            try
            {
                var result = await _client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();

                    List<Chest> chests = JsonConvert.DeserializeObject<List<Chest>>(content.Substring(9, content.Length - 10));

                    //fills returned chests with urls
                    return chestsRepo.FillChestUrls(chests);
                }
                else return null;
            }

            catch { return null; }

        }



        public async Task<PlayerSnapshot> GetFullPlayerSnapshot(string PlayerSnapshotTag)
        {
            TrackedPlayer trackedPlayerSnapshot = _context.TrackedPlayerSnapshots.Where(t => t.Tag == PlayerSnapshotTag).FirstOrDefault();


            //adding battles is done in the auto update thread to handle concurrency errors
            //adds the PlayerSnapshot to have their data tracked
            if (trackedPlayerSnapshot == null)
            {
                _context.TrackedPlayerSnapshots.Add(new TrackedPlayer { Tag = PlayerSnapshotTag, Priority = "high" });
                _context.SaveChanges();
            }


            DecksRepo decksRepo = new DecksRepo(_client, _context);
            BattlesRepo battlesRepo = new BattlesRepo(_client, _context);
            PlayerSnapshot fetchedPlayerSnapshot = await GetOfficialPlayerSnapshot(PlayerSnapshotTag);

            if (fetchedPlayerSnapshot != null)
            {
                fetchedPlayerSnapshot.Battles = battlesRepo.GetRecentBattles(PlayerSnapshotTag);

                if (fetchedPlayerSnapshot.Battles != null)
                {
                    fetchedPlayerSnapshot.Battles.ForEach(b =>
                    {
                        b.Team1DeckA = decksRepo.GetDeckByID(b.Team1DeckAId);
                        b.Team2DeckA = decksRepo.GetDeckByID(b.Team2DeckAId);
                        if (b.Team1DeckBId != 0)
                        {
                            b.Team1DeckB = decksRepo.GetDeckByID(b.Team1DeckBId);
                            b.Team2DeckB = decksRepo.GetDeckByID(b.Team2DeckBId);

                        }
                    });
                }


                //gets PlayerSnapshots Chests r
                List<Chest> PlayerSnapshotChests = await GetPlayerSnapshotChestsAsync("%23" + PlayerSnapshotTag.Substring(1));
                if (PlayerSnapshotChests.Count > 0)
                {
                    fetchedPlayerSnapshot.Chests = PlayerSnapshotChests;
                }
                return fetchedPlayerSnapshot;

            }
            else return null;

        }



        public void SavePlayerSnapshotFull(string PlayerSnapshotTag)
        {
            BattlesRepo battlesRepo = new BattlesRepo(_client, _context);
            ChestsRepo chestsRepo = new ChestsRepo(_client, _context);
            ClansRepo clansRepo = new ClansRepo(_client, _context);
            DecksRepo decksRepo = new DecksRepo(_client, _context);

            //PlayerSnapshot fetched from official API
            //still needs to be packaged for front end
            PlayerSnapshot fetchedPlayerSnapshot = GetOfficialPlayerSnapshot(PlayerSnapshotTag).Result;


            //creating the variable outside try so the function has access
            PlayerSnapshot lastSavedPlayerSnapshot;

            try
            {
                lastSavedPlayerSnapshot = _context.PlayerSnapshots.Where(p => p.Tag == PlayerSnapshotTag).FirstOrDefault();
            }
            catch { lastSavedPlayerSnapshot = null; }


            //makes sure new PlayerSnapshot data is recieved from the offical API
            if (fetchedPlayerSnapshot != null)
            {

                //if there are no instances of this PlayerSnapshot saved or
                if (lastSavedPlayerSnapshot == null || fetchedPlayerSnapshot.Wins != lastSavedPlayerSnapshot.Wins || fetchedPlayerSnapshot.Losses != lastSavedPlayerSnapshot.Losses ||
                    fetchedPlayerSnapshot.ClanTag != lastSavedPlayerSnapshot.ClanTag || fetchedPlayerSnapshot.TotalDonations != lastSavedPlayerSnapshot.TotalDonations)
                {

                    //fetches the current PlayerSnapshot battles from the official DB
                    List<Battle> pBattles = battlesRepo.GetOfficialPlayerBattles(fetchedPlayerSnapshot.Tag).Result;
                   
                    //adds new fetched battles to the DB and gets a count of added lines
                    battlesRepo.AddBattles(pBattles);

                    _context.PlayerSnapshots.Add(fetchedPlayerSnapshot);
                    _context.SaveChanges();
                }

                //gets PlayerSnapshots Chests r
                List<Chest> PlayerSnapshotChests = GetPlayerSnapshotChestsAsync(PlayerSnapshotTag).Result;

                if (PlayerSnapshotChests.Count > 0)
                {
                    fetchedPlayerSnapshot.Chests = PlayerSnapshotChests;
                }

                if (fetchedPlayerSnapshot.ClanTag != null)
                {

                    fetchedPlayerSnapshot.Clan = clansRepo.SaveClanIfNew(fetchedPlayerSnapshot.ClanTag).Result;
                }
                if (fetchedPlayerSnapshot.Battles != null)
                {
                    fetchedPlayerSnapshot.Battles.ForEach(b =>
                    {
                        b.Team1DeckA = decksRepo.GetDeckByID(b.Team1DeckAId);
                        b.Team2DeckA = decksRepo.GetDeckByID(b.Team2DeckAId);

                        if (b.Team1DeckBId != 0)
                        {
                            b.Team1DeckB = decksRepo.GetDeckByID(b.Team1DeckBId);
                            b.Team2DeckB = decksRepo.GetDeckByID(b.Team2DeckBId);
                        }
                    });

                }

            }
        }
    }
}
