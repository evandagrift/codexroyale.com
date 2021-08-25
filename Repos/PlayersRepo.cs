using Newtonsoft.Json;
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
        private TRContext context;
        private Client client;

        //constructor loads in DB Context
        public PlayersRepo(Client c, TRContext ct) { context = ct; client = c; }

        //Adds given Player
        public void AddPlayer(Player player)
        {
            // removes Id in case posted with an Id
            player.Id = 0;
            context.Players.Add(player);
            context.SaveChanges();
        }

        //deletes player with given playerTag
        public void DeletePlayer(int playerId)
        {
            //pulls player instance from db w/ given tag
            Player playerToDelete = GetPlayerById(playerId);

            //if a valid player instance i fetched from the database, it will be removed
            if (playerToDelete != null)
            {
                context.Players.Remove(playerToDelete);
                context.SaveChanges();
            }
        }

        //Returns a List of all players in DB
        public List<Player> GetAllPlayers() { return context.Players.ToList(); }

        //returns player from DB with given Tag
        public Player GetPlayerById(int playerTag) { return context.Players.Find(playerTag); }

        //updates Player
        public void UpdatePlayer(Player player)
        {
            //pulls instance of player from DB
            Player playerToUpdate = GetPlayerById(player.Id);

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
                context.SaveChanges();
            }

        }

        //Player Last seen is fetched from a Clan Api Call
        public async Task<string> GetLastSeen(string playerTag, string clanTag)
        {
            //handler to fetch clan
            ClansRepo clansHandler = new ClansRepo(client, context);

            //fetch clan to get data from
            Clan clan = await clansHandler.GetOfficialClan(clanTag);

            //clan members are a list of players, we grab the player with matching tag
            Player player = clan.MemberList.Where(p => p.Tag == playerTag).FirstOrDefault();

            //return when last seen, Substringed because returned time has a timezone offset but all players are returned with an offset of 0
            return player.LastSeen.Substring(0, 15);
        }


        //gets player data from the official api via their player tag
        public async Task<Player> GetOfficialPlayer(string tag)
        {
            //teams handler to get/set teamId
            TeamsRepo temsRepo = new TeamsRepo(context);
            //decks handler to get set deckId
            DecksRepo decksRepo = new DecksRepo(context);

            //Needs client so it can autofill new cards if they're missing
            CardsRepo cardsRepo = new CardsRepo(client,context);

            ClansRepo clansRepo = new ClansRepo(client, context);

            //try in case we get connection errors`
            try
            {

                string connectionString = "/v1/players/%23" + tag.Substring(1);

                var result = await client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    Player player = JsonConvert.DeserializeObject<Player>(content);

                    
                    if(player.Name != "")
                    { 
                        //gets the players team ID
                        player.TeamId = temsRepo.GetSetTeamId(player).TeamId;

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
                            player.Clan = await clansRepo.SaveClansNewest(player.ClanTag);
                        }


                        player.Deck = decksRepo.GetDeckWithId(player.CurrentDeck);
                        player.CurrentDeckId = player.Deck.Id;

                        //removing this from the provided data so Front end doesn't use currentdeck instead of the dressed Deck with all details
                        player.CurrentDeck = null;


                        //sets the time of this update
                        player.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        return player;
                    }
                    else{ return null; }
                }
                else return null;

            }
                
            catch { return null; }

        }
        public async Task<List<Chest>> GetPlayerChestsAsync(string tag)
        {

            string connectionString = "/v1/players/%23" + tag.Substring(1) + "/upcomingchests";
            ChestsRepo chestsRepo = new ChestsRepo(client, context);
            //try in case we get connection errors`
            try
            {
                var result = await client.officialAPI.GetAsync(connectionString);

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

        public async Task<Player> UpdateGetPlayerWithChestsBattles(User user)
        {

            BattlesRepo battlesRepo = new BattlesRepo(client, context);
            ChestsRepo chestsRepo = new ChestsRepo(client, context);

            //player fetched from official API
            //still needs to be packaged for front end
            Player fetchedPlayer = await GetOfficialPlayer(user.Tag);


            //creating the variable outside try so the function has access
            Player lastSavedPlayer;

            try
            {
                lastSavedPlayer = context.Players.Where(p => p.Tag == user.Tag).FirstOrDefault();
            }
            catch { lastSavedPlayer = null; }



            //makes sure new player data is recieved from the offical API
            if (fetchedPlayer != null)
            {
                //fetches the current player battles from the official DB
                List<Battle> pBattles = await battlesRepo.GetOfficialPlayerBattles(fetchedPlayer.Tag); ;


                //if there are no instances of this player saved or
                if (lastSavedPlayer == null || fetchedPlayer.Wins != lastSavedPlayer.Wins || fetchedPlayer.Losses != lastSavedPlayer.Losses ||
                    fetchedPlayer.ClanTag != lastSavedPlayer.ClanTag)
                {

                    if (user.ClanTag != fetchedPlayer.ClanTag || user.Tag != fetchedPlayer.Tag)
                    {
                        //if the user's Player's Clan has changed it will Automatically
                        user = context.Users.Find(user.Username);
                        user.ClanTag = fetchedPlayer.ClanTag;
                        context.SaveChanges();
                    }

                    //adds new fetched battles to the DB and gets a count of added lines
                    battlesRepo.AddBattles(pBattles);

                    context.Players.Add(fetchedPlayer);
                    context.SaveChanges();
                }
                else { fetchedPlayer = lastSavedPlayer; }

                //gets players Chests r
                List<Chest> playerChests = await GetPlayerChestsAsync(user.Tag);

                fetchedPlayer.Chests = playerChests;

                fetchedPlayer.Battles = battlesRepo.GetRecentBattles(user);
            }//if 
            else { return null;  }

            return fetchedPlayer;
        }
    }
}
