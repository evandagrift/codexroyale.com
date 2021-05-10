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
            context.Players.Add(player);
            context.SaveChanges();
        }

        //deletes player with given playerTag
        public void DeletePlayer(int playerId)
        {
            //pulls player instance from db w/ given tag
            Player playerToDelete = GetPlayerById(playerId);

            //if a valid player instance i fetched from the database, it will be removed
            if(playerToDelete != null)
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
            if(playerToUpdate != null)
            {
                playerToUpdate.Tag = player.Tag;
                playerToUpdate.Name = player.Name;
                playerToUpdate.BestTrophies = player.BestTrophies;
                playerToUpdate.Trophies = player.Trophies;

                playerToUpdate.CardsDiscovered = player.CardsDiscovered;
                playerToUpdate.CardsInGame = player.CardsInGame;

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
            TeamsRepo teamsHandler = new TeamsRepo(context);
            //decks handler to get set deckId
            DecksRepo decksHandler = new DecksRepo(context);

            //try in case we get connection errors`
            try
            {
                //
                string connectionString = "/v1/players/%23" + tag.Substring(1);

                var result = await client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    Player player = JsonConvert.DeserializeObject<Player>(content);

                    player.TeamId = teamsHandler.GetSetTeamId(player).TeamId;

                    player.Deck = new Deck(player.CurrentDeck);

                    player.CurrentDeckId = decksHandler.GetDeckId(player.Deck);
                    player.CurrentFavouriteCardId = player.CurrentFavouriteCard.Id;
                    player.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                    if (player.Clan != null)
                    {
                        player.ClanTag = player.Clan.Tag;
                        player.LastSeen = await GetLastSeen(player.Tag, player.ClanTag);
                    }
                    player.CardsDiscovered = player.Cards.Count;
                    return player;
                }
            }
            catch { return null; }
            return null;
        }
        public async Task<Player> GetOfficialPlayerWithChests(string tag)
        {
            Player returnPlayer = GetOfficialPlayer(tag).Result;

            //try in case we get connection errors`
            try
            {
                //
                string connectionString = "/v1/players/%23" + tag.Substring(1) + "/upcomingchests";

                var result = await client.officialAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    returnPlayer.Chests = JsonConvert.DeserializeObject<List<Chest>>(content);
                }
            }
            catch { }

            return returnPlayer;
        }

    }
}
