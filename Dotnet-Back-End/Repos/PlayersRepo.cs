using Newtonsoft.Json;
using NLog;
using NLog.Web;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Models.RoyaleClasses;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class PlayersRepo
    {
        //DB access
        private TRContext _context;
        private Client _client;
        private readonly ILogger _logger;

        //constructor loads in DB Context
        public PlayersRepo(Client c, TRContext ct) { _context = ct; _client = c;
            _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        }

        public void AddPlayer(PlayerSnapshot player)
        {
            // removes Id in case posted with an Id
            player.Id = 0;
            _context.PlayersSnapshots.Add(player);
            _context.SaveChanges();
        }

        //deletes player with given playerTag
        public void DeletePlayer(int playerId)
        {
            //pulls player instance from db w/ given tag
            PlayerSnapshot playerToDelete = GetPlayerById(playerId);

            //if a valid player instance i fetched from the database, it will be removed
            if (playerToDelete != null)
            {
                _context.PlayersSnapshots.Remove(playerToDelete);
                _context.SaveChanges();
            }
        }

        public List<PlayerSnapshot> GetAllPlayers() { return _context.PlayersSnapshots.ToList(); }

        public PlayerSnapshot GetPlayerById(int playerTag) { return _context.PlayersSnapshots.Find(playerTag); }


        public void UpdatePlayer(PlayerSnapshot player)
        {
            //pulls instance of player from DB
            PlayerSnapshot playerToUpdate = GetPlayerById(player.Id);

            //if a valid player is returned it updates all the fields in the fetched class
            if (playerToUpdate != null)
            {
                playerToUpdate.Tag = player.Tag;
                playerToUpdate.Name = player.Name;
                playerToUpdate.BestTrophies = player.BestTrophies;
                playerToUpdate.Trophies = player.Trophies;

                playerToUpdate.CardsDiscovered = player.CardsDiscovered;

                playerToUpdate.ClanTag = player.ClanTag;


                playerToUpdate.ClanCardsCollected = player.ClanCardsCollected;
                playerToUpdate.CurrentDeckId = player.CurrentDeckId;
                playerToUpdate.CurrentFavouriteCardId = player.CurrentFavouriteCardId;
                playerToUpdate.Donations = player.Donations;
                playerToUpdate.DonationsReceived = player.DonationsReceived;
                playerToUpdate.ExpLevel = player.ExpLevel;
                playerToUpdate.LastSeen = player.LastSeen;
                playerToUpdate.StarPoints = player.StarPoints;
                playerToUpdate.Tag = player.Tag;
                playerToUpdate.TeamId = player.TeamId;
                playerToUpdate.TotalDonations = player.TotalDonations;
                playerToUpdate.UpdateTime = player.UpdateTime;
                playerToUpdate.Wins = player.Wins;
                playerToUpdate.Losses = player.Losses;
                _context.SaveChanges();
            }

        }

        public async Task<string> GetLastSeen(string playerTag, string clanTag)
        {
            //handler to fetch clan
            ClansRepo clansHandler = new ClansRepo(_client, _context);
            try
            {

                //fetch clan to get data from
                Clan clan = await clansHandler.GetOfficialClan(clanTag);

                //clan members are a list of players, we grab the player with matching tag
                PlayerSnapshot player = clan.MemberList.Where(p => p.Tag == playerTag).FirstOrDefault();

                //return when last seen, Substringed because returned time has a timezone offset but all players are returned with an offset of 0
                return player.LastSeen.Substring(0, 15);

            }
            catch { return null; }
        }


        //gets player data from the official api via their player tag
        public async Task<PlayerSnapshot> GetOfficialPlayer(string tag)
        {
            //teams handler to get/set teamId
            TeamsRepo temsRepo = new TeamsRepo(_context);
            //decks handler to get set deckId
            DecksRepo decksRepo = new DecksRepo(_client, _context);

            //Needs client so it can autofill new cards if they're missing
            CardsRepo cardsRepo = new CardsRepo(_client, _context);

            ClansRepo clansRepo = new ClansRepo(_client, _context);

            //try in case we get connection errors`
            try
            {

                string connectionString = "/v1/players/%23" + tag.Substring(1);

                var result = await _client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    PlayerSnapshot player = JsonConvert.DeserializeObject<PlayerSnapshot>(content);


                    if (player.Name != "" && player.CurrentFavouriteCard != null)
                    {
                        //gets the players team ID
                        player.TeamId = 0;

                        //assigns current favorite card details
                        player.CurrentFavouriteCardId = player.CurrentFavouriteCard.Id;
                        player.CurrentFavouriteCard = cardsRepo.ConvertCardUrl(player.CurrentFavouriteCard);
                        player.CardsDiscovered = player.Cards.Count;
                        player.Cards.ForEach(c =>
                        {
                            c = cardsRepo.ConvertCardUrl(c);
                        });

                        if (player.Clan != null)
                        {
                            player.ClanTag = player.Clan.Tag;
                            player.LastSeen = await GetLastSeen(player.Tag, player.ClanTag);
                        }


                        player.Deck = decksRepo.GetDeckWithId(player.CurrentDeck);
                        player.CurrentDeckId = player.Deck.Id;

                        //removing this from the provided data so Front end doesn't use currentdeck instead of the dressed Deck with all details
                        player.CurrentDeck = null;


                        //sets the time of this update
                        player.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        TrackedPlayer trackedPlayer = _context.TrackedPlayers.Where(t => t.Tag == player.Tag).FirstOrDefault();


                        //adding battles is done in the auto update thread to handle concurrency errors
                        //adds the player to have their data tracked
                        if (trackedPlayer == null)
                        {
                            _context.TrackedPlayers.Add(new TrackedPlayer { Tag = player.Tag, Priority = "high" });
                            _context.SaveChanges();
                        }


                        return player;
                    }
                    else { return null; }
                }
                else return null;


            }

            catch { return null; }

        }
        public async Task<List<Chest>> GetPlayerChestsAsync(string tag)
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
            string connectionString = "/v1/players/" + tag + "/upcomingchests";
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



        public async Task<PlayerSnapshot> GetFullPlayer(string playerTag)
        {
            TrackedPlayer trackedPlayer = _context.TrackedPlayers.Where(t => t.Tag == playerTag).FirstOrDefault();


            //adding battles is done in the auto update thread to handle concurrency errors
            //adds the player to have their data tracked
            if (trackedPlayer == null)
            {
                _context.TrackedPlayers.Add(new TrackedPlayer { Tag = playerTag, Priority = "high" });
                _context.SaveChanges();
            }


            DecksRepo decksRepo = new DecksRepo(_client, _context);
            BattlesRepo battlesRepo = new BattlesRepo(_client, _context);
            PlayerSnapshot fetchedPlayer = await GetOfficialPlayer(playerTag);

            if (fetchedPlayer != null)
            {
                fetchedPlayer.Battles = battlesRepo.GetRecentBattles(playerTag);

                if (fetchedPlayer.Battles != null)
                {
                    fetchedPlayer.Battles.ForEach(b =>
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


                //gets players Chests r
                List<Chest> playerChests = await GetPlayerChestsAsync("%23" + playerTag.Substring(1));
                if (playerChests.Count > 0)
                {
                    fetchedPlayer.Chests = playerChests;
                }
                return fetchedPlayer;

            }
            else return null;

        }



        public void SavePlayerFull(string playerTag)
        {

            _logger.Debug($"Saving full player {playerTag}");

            BattlesRepo battlesRepo = new BattlesRepo(_client, _context);
            ChestsRepo chestsRepo = new ChestsRepo(_client, _context);
            ClansRepo clansRepo = new ClansRepo(_client, _context);
            DecksRepo decksRepo = new DecksRepo(_client, _context);

            //player fetched from official API
            //still needs to be packaged for front end
            PlayerSnapshot fetchedPlayer = GetOfficialPlayer(playerTag).Result;

            _logger.Debug($"fetched player {fetchedPlayer}");

            //creating the variable outside try so the function has access
            PlayerSnapshot lastSavedPlayer;

            try
            {
                lastSavedPlayer = _context.PlayersSnapshots.Where(p => p.Tag == playerTag).FirstOrDefault();
            }
            catch { lastSavedPlayer = null; }


            //makes sure new player data is recieved from the offical API
            if (fetchedPlayer != null)
            {

                //if there are no instances of this player saved or
                if (lastSavedPlayer == null || fetchedPlayer.Wins != lastSavedPlayer.Wins || fetchedPlayer.Losses != lastSavedPlayer.Losses ||
                    fetchedPlayer.ClanTag != lastSavedPlayer.ClanTag || fetchedPlayer.TotalDonations != lastSavedPlayer.TotalDonations)
                {

                    //if the user's Player's Clan has changed it will Automatically
                    var users = _context.Users.Where(u => u.Tag == playerTag).ToList();

                    if (users.Count() > 0)
                    {
                        users.ForEach(u =>
                        {
                            if (u.ClanTag != fetchedPlayer.ClanTag)
                                u.ClanTag = fetchedPlayer.ClanTag;
                        });
                        _context.SaveChanges();
                    }

                    _logger.Debug($"last saved player");

                    //fetches the current player battles from the official DB
                    List<Battle> pBattles = battlesRepo.GetOfficialPlayerBattles(fetchedPlayer.Tag).Result;
                    
                    
                    
                    _logger.Debug($"got battles");


                    _logger.Debug($"Getting battles in save player:{pBattles}");
                    //adds new fetched battles to the DB and gets a count of added lines
                    battlesRepo.AddBattles(pBattles);

                    _context.PlayersSnapshots.Add(fetchedPlayer);
                    _context.SaveChanges();
                }

                //gets players Chests r
                List<Chest> playerChests = GetPlayerChestsAsync(playerTag).Result;

                _logger.Debug($"fetched player chests {playerChests}");

                if (playerChests.Count > 0)
                {
                    fetchedPlayer.Chests = playerChests;
                }

                if (fetchedPlayer.ClanTag != null)
                {

                    fetchedPlayer.Clan = clansRepo.SaveClanIfNew(fetchedPlayer.ClanTag).Result;
                }
                if (fetchedPlayer.Battles != null)
                {
                    fetchedPlayer.Battles.ForEach(b =>
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
