using CodexRoyaleUpdater;
using CodexRoyaleUpdater.Handlers;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Handlers
{
    public class GeneralHandler
    {
        TRContext context;
        Client client;
        public GeneralHandler(Client c, TRContext ct)
        {
            client = c;
            context = ct;
        }


        public User SaveAllByUser(User user)
        {
            //check if the inserted tag is correct, and if so. get clan tag as well
            PlayersHandler playersHandler = new PlayersHandler(client, context);
            BattlesHandler battlesHandler = new BattlesHandler(client, context);
            ClansHandler clansHandler = new ClansHandler(client, context);
            BattlesRepo battleRepo = new BattlesRepo(context);

            try
            {
                Player player = playersHandler.GetOfficialPlayer(user.Tag).Result;
                if (player != null)
                {
                    context.Players.Add(player);

                    List<Battle> pBattles;
                    //TODO:get save user and their battles to DB
                    //fetches the current player battles from the official DB
                    pBattles = battlesHandler.GetOfficialPlayerBattles(player.Tag).Result;


                    //adds new fetched battles to the DB and gets a count of added lines
                    battleRepo.AddBattles(pBattles);

                    if (player.ClanTag != "")
                    {
                        //TODO:Save Clan to DB
                        //gets current clan with Tag
                        Clan clan = clansHandler.GetOfficialClan(player.ClanTag).Result;

                        //saves clan data to DB
                        context.Clans.Add(clan);
                        user.ClanTag = player.ClanTag;
                    }
                    context.SaveChanges();
                }
                else
                {

                    user.Tag = null;
                    user.ClanTag = null;
                }
            }
            catch { return null; }

            return user;
        }


        public User SaveNew(User user)
        {
            //check if the inserted tag is correct, and if so. get clan tag as well
            PlayersHandler playersHandler = new PlayersHandler(client, context);
            BattlesHandler battlesHandler = new BattlesHandler(client, context);
            ClansHandler clansHandler = new ClansHandler(client, context);
            BattlesRepo battleRepo = new BattlesRepo(context);

            try
            {
                Player player = playersHandler.GetOfficialPlayer(user.Tag).Result;
                if (player != null)
                {
                    //get newest player
                    //if it's been more than 24 hours save player/clan
                    Player fetchedPlayer = context.Players.Where(p => p.Tag == player.Tag).FirstOrDefault();
                    
                    if(fetchedPlayer.Wins != player.Wins ||fetchedPlayer.Losses != player.Losses || fetchedPlayer.ClanTag != player.ClanTag)
                    {

                        context.Players.Add(player);

                        List<Battle> pBattles;
                        //TODO:get save user and their battles to DB
                        //fetches the current player battles from the official DB
                        pBattles = battlesHandler.GetOfficialPlayerBattles(player.Tag).Result;


                        //adds new fetched battles to the DB and gets a count of added lines
                        battleRepo.AddBattles(pBattles);

                    }
                    if (user.ClanTag != player.ClanTag || user.Tag != player.Tag)
                    {
                        user = context.Users.Find(user.Username);
                        user.Tag = player.Tag;
                        user.ClanTag = player.ClanTag;
                        context.SaveChanges();
                    }


                }
            }
            catch { return null; }

            return user;
        }
    }
}