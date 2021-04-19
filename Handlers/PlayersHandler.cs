using Newtonsoft.Json;
using RoyaleTrackerAPI;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public class PlayersHandler : IPlayersHandler
    {
        //client class has access to both APIs
        Client client;
        TRContext context;

        //connection string for players in the API
        private string connectionString = "Players";

        //Constructor adds reference to HTTP Clients
        public PlayersHandler(Client c, TRContext ct)
        {
            client = c;
            context = ct;
        }

        //Player Last seen is fetched from a Clan Api Call
        public async Task<string> GetLastSeen(string playerTag, string clanTag)
        {
            //handler to fetch clan
            ClansHandler clansHandler = new ClansHandler(client,context);

            //fetch clan to get data from
            Clan clan = await clansHandler.GetOfficialClan(clanTag);

            //clan members are a list of players, we grab the player with matching tag
            Player player = clan.MemberList.Where(p => p.Tag == playerTag).FirstOrDefault();

            //return when last seen, Substringed because returned time has a timezone offset but all players are returned with an offset of 0
            return player.LastSeen.Substring(0,15);
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


    }
}
