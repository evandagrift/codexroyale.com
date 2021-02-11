using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class PlayersRepo : IPlayersRepo
    {
        //DB access
        private TRContext context;
        
        //constructor loads in DB Context
        public PlayersRepo(TRContext c) { context = c; }

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
    }
}
