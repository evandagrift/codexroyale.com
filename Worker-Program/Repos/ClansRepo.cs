using Newtonsoft.Json;
using ClashFeeder.Models;
using ClashFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClashFeeder.Repos
{
    public class ClansRepo
    {
        //DB Access
        private TRContext context;
        private Client client;

        //assigns argumented context
        public ClansRepo(Client c,  TRContext ct) { context = ct; client = c; }

        //adds given clan to context
        public Clan AddClan(Clan clan)
        {
            //sanitizes the Id incase it was entered with one
            clan.Id = 0;
            context.Clans.Add(clan);
            context.SaveChanges();
            return clan;
        }

        //gets clan data from the official api via their clan tag
        public async Task<Clan> GetOfficialClan(string tag)
        {
            string officialConnectionString = "/v1/clans/%23";

            if (tag != null)
            {
                try
                {
                    //connection string for clan in offical API
                    string connectionString = officialConnectionString + tag.Substring(1);

                    //fetches clan data
                    var result = await client.officialAPI.GetAsync(connectionString);


                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();

                        //deseriealizes json into Clan object
                        Clan clan = JsonConvert.DeserializeObject<Clan>(content);

                        //sets location code to a format that the DB can consume
                        clan.LocationCode = (clan.Location["isCountry"] == "false") ? "International" : clan.Location["countryCode"];

                        //update time in same format as official API
                        clan.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        return clan;
                    }
                }
                catch { return null; }
            }
            return null;
        }
        public async Task<Clan> GetSiteClan(string clanTag)
        {
            Clan returnClan = await SaveClanIfNew(clanTag);
            returnClan.MemberList = GetClanPlayerSnapshots(clanTag).Result;

            return returnClan;
        }
        public async Task<Clan> SaveClanIfNew(string clanTag)
        {
            Clan clan = await GetOfficialClan(clanTag);
            if (clan != null)
            {
                //gets the last saved line in the Clan DB for this particuar Clan
                Clan lastLoggedClan = context.Clans.Where(c => c.Tag == clanTag).OrderByDescending(c => c.UpdateTime).FirstOrDefault();

                //if this clan has been saved before it makes sure the data is new
                //otherwise it will save it by default
                if (lastLoggedClan != null)
                {
                    if (clan.BadgeId != lastLoggedClan.BadgeId || clan.ClanChestLevel != lastLoggedClan.ClanChestLevel || clan.ClanChestStatus != lastLoggedClan.ClanChestStatus || clan.ClanScore != lastLoggedClan.ClanScore ||
                        clan.ClanWarTrophies != lastLoggedClan.ClanWarTrophies || clan.Description != lastLoggedClan.Description || clan.DonationsPerWeek != lastLoggedClan.DonationsPerWeek || clan.LocationCode != lastLoggedClan.LocationCode ||
                        clan.Members != lastLoggedClan.Members || clan.Name != lastLoggedClan.Name || clan.RequiredTrophies != lastLoggedClan.RequiredTrophies || clan.Type != lastLoggedClan.Type)
                    {
                        //if the clan has changed it adds it to DB and returns the new
                        lastLoggedClan = AddClan(clan);
                    }

                }//if there are no instances of this clan saved it goes ahead and saves it then assigns to return variable
                else { lastLoggedClan = AddClan(clan); }

                return lastLoggedClan;
            }
            else return null;
        }

        public async Task<List<PlayerSnapshot>> GetClanPlayerSnapshots(string clanTag)
        {
            string officialConnectionString = "/v1/clans/%23";

            try
            {
                //connection string for clan in offical API
                string connectionString = officialConnectionString + clanTag.Substring(1) + "/members";

                //fetches clan data
                var result = await client.officialAPI.GetAsync(connectionString);


                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    //deseriealizes json into Clan object
                    List<PlayerSnapshot> PlayerSnapshots = JsonConvert.DeserializeObject<List<PlayerSnapshot>>(content.Substring(9, content.Length - 34));

                    return PlayerSnapshots;
                }
            }
            catch { return null; }

            return null;
        }

        //Deletes clan with given clanTag
        public void DeleteClan(int id)
        {
            //fetches clan with given clan tag
            Clan clanToDelete = context.Clans.Find(id);

            //if a valid clan is fetched from the database it removes it from the context
            if (clanToDelete != null)
            {
                context.Clans.Remove(clanToDelete);
                context.SaveChanges();
            }
        }

        //Returns a List of all Clans in DB
        public List<Clan> GetAllClans() { return context.Clans.ToList(); }

        //gets clan with given clanTag
        public Clan GetClanById(int id) { return context.Clans.Find(id); }

        //updates clan at given clantag
        public void UpdateClan(Clan clan)
        {
            //fetches clan at given Tag
            Clan clanToUpdate = context.Clans.Find(clan.Id);

            //if a valid clan is fetched it updates the fields to those of the argumented clan
            if (clanToUpdate != null)
            {
                clanToUpdate.Name = clan.Name;
                clanToUpdate.Type = clan.Type;
                clanToUpdate.Description = clan.Description;
                clanToUpdate.BadgeId = clan.BadgeId;
                clanToUpdate.LocationCode = clan.LocationCode;
                clanToUpdate.RequiredTrophies = clan.RequiredTrophies;
                clanToUpdate.DonationsPerWeek = clan.DonationsPerWeek;
                clanToUpdate.ClanChestStatus = clan.ClanChestStatus;
                clanToUpdate.ClanChestLevel = clan.ClanChestLevel;
                clanToUpdate.ClanScore = clan.ClanScore;
                clanToUpdate.ClanWarTrophies = clan.ClanWarTrophies;
                clanToUpdate.Members = clan.Members;
                clanToUpdate.UpdateTime = clan.UpdateTime;
                context.SaveChanges();
            }
        }
    }
}
