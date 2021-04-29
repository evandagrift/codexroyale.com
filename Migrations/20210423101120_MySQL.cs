using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace RoyaleTrackerAPI.Migrations
{
    public partial class MySQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    BattleId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BattleTime = table.Column<string>(nullable: true),
                    Team1Name = table.Column<string>(nullable: true),
                    Team1Id = table.Column<int>(nullable: false),
                    Team1Win = table.Column<bool>(nullable: false),
                    Team1StartingTrophies = table.Column<int>(nullable: false),
                    Team1TrophyChange = table.Column<int>(nullable: false),
                    Team1DeckAId = table.Column<int>(nullable: false),
                    Team1DeckBId = table.Column<int>(nullable: false),
                    Team1Crowns = table.Column<int>(nullable: false),
                    Team1KingTowerHp = table.Column<int>(nullable: false),
                    Team1PrincessTowerHpA = table.Column<int>(nullable: false),
                    Team1PrincessTowerHpB = table.Column<int>(nullable: false),
                    Team2Name = table.Column<string>(nullable: true),
                    Team2Id = table.Column<int>(nullable: false),
                    Team2Win = table.Column<bool>(nullable: false),
                    Team2StartingTrophies = table.Column<int>(nullable: false),
                    Team2TrophyChange = table.Column<int>(nullable: false),
                    Team2DeckAId = table.Column<int>(nullable: false),
                    Team2DeckBId = table.Column<int>(nullable: false),
                    Team2Crowns = table.Column<int>(nullable: false),
                    Team2KingTowerHp = table.Column<int>(nullable: false),
                    Team2PrincessTowerHpA = table.Column<int>(nullable: false),
                    Team2PrincessTowerHpB = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    DeckSelection = table.Column<string>(nullable: true),
                    IsLadderTournament = table.Column<bool>(nullable: false),
                    GameModeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.BattleId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Tag = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BadgeId = table.Column<int>(nullable: false),
                    LocationCode = table.Column<string>(nullable: true),
                    RequiredTrophies = table.Column<int>(nullable: false),
                    DonationsPerWeek = table.Column<int>(nullable: false),
                    ClanChestStatus = table.Column<string>(nullable: true),
                    ClanChestLevel = table.Column<int>(nullable: false),
                    ClanScore = table.Column<int>(nullable: false),
                    ClanWarTrophies = table.Column<int>(nullable: false),
                    Members = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Card1Id = table.Column<int>(nullable: false),
                    Card2Id = table.Column<int>(nullable: false),
                    Card3Id = table.Column<int>(nullable: false),
                    Card4Id = table.Column<int>(nullable: false),
                    Card5Id = table.Column<int>(nullable: false),
                    Card6Id = table.Column<int>(nullable: false),
                    Card7Id = table.Column<int>(nullable: false),
                    Card8Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Tag = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<string>(nullable: true),
                    ClanTag = table.Column<string>(nullable: true),
                    CurrentFavouriteCardId = table.Column<int>(nullable: false),
                    CurrentDeckId = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    LastSeen = table.Column<string>(nullable: true),
                    ExpLevel = table.Column<int>(nullable: false),
                    Trophies = table.Column<int>(nullable: false),
                    BestTrophies = table.Column<int>(nullable: false),
                    StarPoints = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    Donations = table.Column<int>(nullable: false),
                    DonationsReceived = table.Column<int>(nullable: false),
                    TotalDonations = table.Column<int>(nullable: false),
                    CardsDiscovered = table.Column<int>(nullable: false),
                    ClanCardsCollected = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: true),
                    TwoVTwo = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Name2 = table.Column<string>(nullable: true),
                    Tag2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    ClanTag = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Clans");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "GameModes");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
