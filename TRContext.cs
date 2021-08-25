using Microsoft.EntityFrameworkCore;
using RoyaleTrackerAPI.Models.RoyaleClasses;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Models
{
    public class TRContext : DbContext
    {

        private Client client;

        //DB Context using EF Core
        public TRContext(DbContextOptions<TRContext> options, Client c) : base(options) { client = c; }


        //Each Table is Generated off the Classes in DBSets
        public DbSet<Player> Players { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chest> Chests { get; set; }


        //seeds in needed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CardsRepo cardsRepo = new CardsRepo(client);
            List<Card> allCards = cardsRepo.GetAllOfficialCards().Result;

            //this.Cards.AddRange(allCards);

            //if there are no cards in the database all current cards in the game are seeded in
            allCards.ForEach(c =>
            {
                modelBuilder.Entity<Card>().HasData(c);
            });


            //all available chests are seeded in with a url to their image
            //this list is used to assign URLs to fetched player chests
            modelBuilder.Entity<Chest>().HasData(
                new Chest { Name = "Epic Chest", Url = "https://https://static.wikia.nocookie.net/clashroyale/images/f/f5/EpicChest.png.wikia.nocookie.net/clashroyale/images/f/f5/EpicChest.png" },
                new Chest { Name = "Crown Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/7/75/CrownChest.png" },
                new Chest { Name = "Fortune Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/d/de/Fortune_Chest.png" },
                new Chest { Name = "Giant Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/d/da/Giant_chest.png" },
                new Chest { Name = "Gold Crate", Url = "https://static.wikia.nocookie.net/clashroyale/images/c/cb/GoldCrate.png" },
                new Chest { Name = "Golden Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/8/8b/GoldenChest.png" },
                new Chest { Name = "King's Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/5/51/Kings_Chest.png" },
                new Chest { Name = "Legendary Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/a/a1/LegendChest.png" },
                new Chest { Name = "Legendary King's Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/4/42/Legendary_Kings_Chest.png" },
                new Chest { Name = "Lightning Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/2/2c/Lightning_Chest.png" },
                new Chest { Name = "Magical Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/9/93/MagicalChest.png" },
                new Chest { Name = "Mega Lightning Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/3/3a/MegaLightningChest.png" },
                new Chest { Name = "Overflowing Gold Crate", Url = "https://static.wikia.nocookie.net/clashroyale/images/7/76/OverflowingGoldCrate.png" },
                new Chest { Name = "Plentiful Gold Crate", Url = "https://static.wikia.nocookie.net/clashroyale/images/0/03/PlentifulGoldCrate.png" },
                new Chest { Name = "Silver Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/0/07/SilverChest.png" },
                new Chest { Name = "Wooden Chest", Url = "https://static.wikia.nocookie.net/clashroyale/images/3/30/WoodenChest.png" });
            Console.WriteLine();

        }

    }
}