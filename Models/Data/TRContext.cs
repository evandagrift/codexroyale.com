using Microsoft.EntityFrameworkCore;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Models
{
    public class TRContext : DbContext
    {
        //DB Context using EF Core
        public TRContext(DbContextOptions<TRContext> options) : base(options) { }

        //Each Table is Generated off the Classes in DBSets
        public DbSet<Player> Players { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
    }
}