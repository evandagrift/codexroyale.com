using Microsoft.EntityFrameworkCore.Migrations;

namespace RoyaleTrackerAPI.Migrations
{
    public partial class local : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    BattleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "Chests",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chests", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Clans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { 26000043, "Elite Barbarians", "https://api-assets.clashroyale.com/cards/300/C88C5JH_F3lLZj6K-tLcMo5DPjrFmvzIb1R2M6xCfTE.png" },
                    { 26000085, "Electro Giant", "https://api-assets.clashroyale.com/cards/300/_uChZkNHAMq6tPb3v6A49xinOe3CnhjstOhG6OZbPYc.png" },
                    { 26000084, "Electro Spirit", "https://api-assets.clashroyale.com/cards/300/WKd4-IAFsgPpMo7dDi9sujmYjRhOMEWiE07OUJpvD9g.png" },
                    { 26000083, "Mother Witch", "https://api-assets.clashroyale.com/cards/300/fO-Xah8XZkYKaSK9SCp3wnzwxtvIhun9NVY-zzte1Ng.png" },
                    { 26000080, "Skeleton Dragons", "https://api-assets.clashroyale.com/cards/300/qPOtg9uONh47_NLxGhhFc_ww9PlZ6z3Ry507q1NZUXs.png" },
                    { 26000068, "Battle Healer", "https://api-assets.clashroyale.com/cards/300/KdwXcoigS2Kg-cgA7BJJIANbUJG6SNgjetRQ-MegZ08.png" },
                    { 26000067, "Elixir Golem", "https://api-assets.clashroyale.com/cards/300/puhMsZjCIqy21HW3hYxjrk_xt8NIPyFqjRy-BeLKZwo.png" },
                    { 26000064, "Firecracker", "https://api-assets.clashroyale.com/cards/300/c1rL3LO1U2D9-TkeFfAC18gP3AO8ztSwrcHMZplwL2Q.png" },
                    { 26000063, "Electro Dragon", "https://api-assets.clashroyale.com/cards/300/tN9h6lnMNPCNsx0LMFmvpHgznbDZ1fBRkx-C7UfNmfY.png" },
                    { 26000062, "Magic Archer", "https://api-assets.clashroyale.com/cards/300/Avli3W7BxU9HQ2SoLiXnBgGx25FoNXUSFm7OcAk68ek.png" },
                    { 26000061, "Fisherman", "https://api-assets.clashroyale.com/cards/300/U2KZ3g0wyufcuA5P2Xrn3Z3lr1WiJmc5S0IWOZHgizQ.png" },
                    { 26000060, "Goblin Giant", "https://api-assets.clashroyale.com/cards/300/SoW16cY3jXBwaTDvb39DkqiVsoFVaDWbzf5QBYphJrY.png" },
                    { 26000059, "Royal Hogs", "https://api-assets.clashroyale.com/cards/300/ASSQJG_MoVq9e81HZzo4bynMnyLNpNJMfSLb3hqydOw.png" },
                    { 27000000, "Cannon", "https://api-assets.clashroyale.com/cards/300/nZK1y-beLxO5vnlyUhK6-2zH2NzXJwqykcosqQ1cmZ8.png" },
                    { 26000058, "Wall Breakers", "https://api-assets.clashroyale.com/cards/300/_xPphEfC8eEwFNrfU3cMQG9-f5JaLQ31ARCA7l3XtW4.png" },
                    { 26000056, "Skeleton Barrel", "https://api-assets.clashroyale.com/cards/300/vCB4DWCcrGbTkarjcOiVz4aNDx6GWLm0yUepg9E1MGo.png" },
                    { 26000055, "Mega Knight", "https://api-assets.clashroyale.com/cards/300/O2NycChSNhn_UK9nqBXUhhC_lILkiANzPuJjtjoz0CE.png" },
                    { 26000054, "Cannon Cart", "https://api-assets.clashroyale.com/cards/300/aqwxRz8HXzqlMCO4WMXNA1txynjXTsLinknqsgZLbok.png" },
                    { 26000053, "Rascals", "https://api-assets.clashroyale.com/cards/300/KV48DfwVHKx9XCjzBdk3daT_Eb52Me4VgjVO7WctRc4.png" },
                    { 26000052, "Zappies", "https://api-assets.clashroyale.com/cards/300/QZfHRpLRmutZbCr5fpLnTpIp89vLI6NrAwzGZ8tHEc4.png" },
                    { 26000051, "Ram Rider", "https://api-assets.clashroyale.com/cards/300/QaJyerT7f7oMyZ3Fv1glKymtLSvx7YUXisAulxl7zRI.png" },
                    { 26000050, "Royal Ghost", "https://api-assets.clashroyale.com/cards/300/3En2cz0ISQAaMTHY3hj3rTveFN2kJYq-H4VxvdJNvCM.png" },
                    { 26000049, "Bats", "https://api-assets.clashroyale.com/cards/300/EnIcvO21hxiNpoI-zO6MDjLmzwPbq8Z4JPo2OKoVUjU.png" },
                    { 26000048, "Night Witch", "https://api-assets.clashroyale.com/cards/300/NpCrXDEDBBJgNv9QrBAcJmmMFbS7pe3KCY8xJ5VB18A.png" },
                    { 26000047, "Royal Recruits", "https://api-assets.clashroyale.com/cards/300/jcNyYGUiXXNz3kuz8NBkHNKNREQKraXlb_Ts7rhCIdM.png" },
                    { 26000046, "Bandit", "https://api-assets.clashroyale.com/cards/300/QWDdXMKJNpv0go-HYaWQWP6p8uIOHjqn-zX7G0p3DyM.png" },
                    { 26000045, "Executioner", "https://api-assets.clashroyale.com/cards/300/9XL5BP2mqzV8kza6KF8rOxrpCZTyuGLp2l413DTjEoM.png" },
                    { 26000057, "Flying Machine", "https://api-assets.clashroyale.com/cards/300/hzKNE3QwFcrSrDDRuVW3QY_OnrDPijSiIp-PsWgFevE.png" },
                    { 26000044, "Hunter", "https://api-assets.clashroyale.com/cards/300/VNabB1WKnYtYRSG7X_FZfnZjQDHTBs9A96OGMFmecrA.png" },
                    { 27000001, "Goblin Hut", "https://api-assets.clashroyale.com/cards/300/l8ZdzzNLcwB4u7ihGgxNFQOjCT_njFuAhZr7D6PRF7E.png" },
                    { 27000003, "Inferno Tower", "https://api-assets.clashroyale.com/cards/300/GSHY_wrooMMLET6bG_WJB8redtwx66c4i80ipi4gYOM.png" },
                    { 28000016, "Heal Spirit", "https://api-assets.clashroyale.com/cards/300/GITl06sa2nGRLPvboyXbGEv5E3I-wAwn1Eqa5esggbc.png" },
                    { 28000015, "Barbarian Barrel", "https://api-assets.clashroyale.com/cards/300/Gb0G1yNy0i5cIGUHin8uoFWxqntNtRPhY_jeMXg7HnA.png" },
                    { 28000014, "Earthquake", "https://api-assets.clashroyale.com/cards/300/XeQXcrUu59C52DslyZVwCnbi4yamID-WxfVZLShgZmE.png" },
                    { 28000013, "Clone", "https://api-assets.clashroyale.com/cards/300/mHVCet-1TkwWq-pxVIU2ZWY9_2z7Z7wtP25ArEUsP_g.png" },
                    { 28000012, "Tornado", "https://api-assets.clashroyale.com/cards/300/QJB-QK1QJHdw4hjpAwVSyZBozc2ZWAR9pQ-SMUyKaT0.png" },
                    { 28000011, "The Log", "https://api-assets.clashroyale.com/cards/300/_iDwuDLexHPFZ_x4_a0eP-rxCS6vwWgTs6DLauwwoaY.png" },
                    { 28000010, "Graveyard", "https://api-assets.clashroyale.com/cards/300/Icp8BIyyfBTj1ncCJS7mb82SY7TPV-MAE-J2L2R48DI.png" },
                    { 28000009, "Poison", "https://api-assets.clashroyale.com/cards/300/98HDkG2189yOULcVG9jz2QbJKtfuhH21DIrIjkOjxI8.png" },
                    { 28000008, "Zap", "https://api-assets.clashroyale.com/cards/300/7dxh2-yCBy1x44GrBaL29vjqnEEeJXHEAlsi5g6D1eY.png" },
                    { 28000007, "Lightning", "https://api-assets.clashroyale.com/cards/300/fpnESbYqe5GyZmaVVYe-SEu7tE0Kxh_HZyVigzvLjks.png" },
                    { 28000006, "Mirror", "https://api-assets.clashroyale.com/cards/300/wC6Cm9rKLEOk72zTsukVwxewKIoO4ZcMJun54zCPWvA.png" },
                    { 28000005, "Freeze", "https://api-assets.clashroyale.com/cards/300/I1M20_Zs_p_BS1NaNIVQjuMJkYI_1-ePtwYZahn0JXQ.png" },
                    { 27000002, "Mortar", "https://api-assets.clashroyale.com/cards/300/lPOSw6H7YOHq2miSCrf7ZDL3ANjhJdPPDYOTujdNrVE.png" },
                    { 28000004, "Goblin Barrel", "https://api-assets.clashroyale.com/cards/300/CoZdp5PpsTH858l212lAMeJxVJ0zxv9V-f5xC8Bvj5g.png" },
                    { 28000002, "Rage", "https://api-assets.clashroyale.com/cards/300/bGP21OOmcpHMJ5ZA79bHVV2D-NzPtDkvBskCNJb7pg0.png" },
                    { 28000001, "Arrows", "https://api-assets.clashroyale.com/cards/300/Flsoci-Y6y8ZFVi5uRFTmgkPnCmMyMVrU7YmmuPvSBo.png" },
                    { 28000000, "Fireball", "https://api-assets.clashroyale.com/cards/300/lZD9MILQv7O-P3XBr_xOLS5idwuz3_7Ws9G60U36yhc.png" },
                    { 27000013, "Goblin Drill", "https://api-assets.clashroyale.com/cards/300/eN2TKUYbih-26yBi0xy5LVFOA0zDftgDqxxnVfdIg1o.png" },
                    { 27000012, "Goblin Cage", "https://api-assets.clashroyale.com/cards/300/vD24bBgK4rSq7wx5QEbuqChtPMRFviL_ep76GwQw1yA.png" },
                    { 27000010, "Furnace", "https://api-assets.clashroyale.com/cards/300/iqbDiG7yYRIzvCPXdt9zPb3IvMt7F7Gi4wIPnh2x4aI.png" },
                    { 27000009, "Tombstone", "https://api-assets.clashroyale.com/cards/300/LjSfSbwQfkZuRJY4pVxKspZ-a0iM5KAhU8w-a_N5Z7Y.png" },
                    { 27000008, "X-Bow", "https://api-assets.clashroyale.com/cards/300/zVQ9Hme1hlj9Dc6e1ORl9xWwglcSrP7ejow5mAhLUJc.png" },
                    { 27000007, "Elixir Collector", "https://api-assets.clashroyale.com/cards/300/BGLo3Grsp81c72EpxLLk-Sofk3VY56zahnUNOv3JcT0.png" },
                    { 27000006, "Tesla", "https://api-assets.clashroyale.com/cards/300/OiwnGrxFMNiHetYEerE-UZt0L_uYNzFY7qV_CA_OxR4.png" },
                    { 27000005, "Barbarian Hut", "https://api-assets.clashroyale.com/cards/300/ho0nOG2y3Ch86elHHcocQs8Fv_QNe0cFJ2CijsxABZA.png" },
                    { 27000004, "Bomb Tower", "https://api-assets.clashroyale.com/cards/300/rirYRyHPc97emRjoH-c1O8uZCBzPVnToaGuNGusF3TQ.png" },
                    { 28000003, "Rocket", "https://api-assets.clashroyale.com/cards/300/Ie07nQNK9CjhKOa4-arFAewi4EroqaA-86Xo7r5tx94.png" },
                    { 28000017, "Giant Snowball", "https://api-assets.clashroyale.com/cards/300/7MaJLa6hK9WN2_VIshuh5DIDfGwm0wEv98gXtAxLDPs.png" },
                    { 28000018, "Royal Delivery", "https://api-assets.clashroyale.com/cards/300/LPg7AGjGI3_xmi7gLLgGC50yKM1jJ2teWkZfoHJcIZo.png" },
                    { 26000041, "Goblin Gang", "https://api-assets.clashroyale.com/cards/300/NHflxzVAQT4oAz7eDfdueqpictb5vrWezn1nuqFhE4w.png" },
                    { 26000017, "Wizard", "https://api-assets.clashroyale.com/cards/300/Mej7vnv4H_3p_8qPs_N6_GKahy6HDr7pU7i9eTHS84U.png" },
                    { 26000016, "Prince", "https://api-assets.clashroyale.com/cards/300/3JntJV62aY0G1Qh6LIs-ek-0ayeYFY3VItpG7cb9I60.png" },
                    { 26000015, "Baby Dragon", "https://api-assets.clashroyale.com/cards/300/cjC9n4AvEZJ3urkVh-rwBkJ-aRSsydIMqSAV48hAih0.png" },
                    { 26000014, "Musketeer", "https://api-assets.clashroyale.com/cards/300/Tex1C48UTq9FKtAX-3tzG0FJmc9jzncUZG3bb5Vf-Ds.png" },
                    { 26000013, "Bomber", "https://api-assets.clashroyale.com/cards/300/12n1CesxKIcqVYntjxcF36EFA-ONw7Z-DoL0_rQrbdo.png" },
                    { 26000042, "Electro Wizard", "https://api-assets.clashroyale.com/cards/300/RsFaHgB3w6vXsTjXdPr3x8l_GbV9TbOUCvIx07prbrQ.png" },
                    { 26000011, "Valkyrie", "https://api-assets.clashroyale.com/cards/300/0lIoYf3Y_plFTzo95zZL93JVxpfb3MMgFDDhgSDGU9A.png" },
                    { 26000010, "Skeletons", "https://api-assets.clashroyale.com/cards/300/oO7iKMU5m0cdxhYPZA3nWQiAUh2yoGgdThLWB1rVSec.png" },
                    { 26000018, "Mini P.E.K.K.A", "https://api-assets.clashroyale.com/cards/300/Fmltc4j3Ve9vO_xhHHPEO3PRP3SmU2oKp2zkZQHRZT4.png" },
                    { 26000009, "Golem", "https://api-assets.clashroyale.com/cards/300/npdmCnET7jmVjJvjJQkFnNSNnDxYHDBigbvIAloFMds.png" },
                    { 26000007, "Witch", "https://api-assets.clashroyale.com/cards/300/cfwk1vzehVyHC-uloEIH6NOI0hOdofCutR5PyhIgO6w.png" },
                    { 26000006, "Balloon", "https://api-assets.clashroyale.com/cards/300/qBipxLo-3hhCnPrApp2Nn3b2NgrSrvwzWytvREev0CY.png" },
                    { 26000005, "Minions", "https://api-assets.clashroyale.com/cards/300/yHGpoEnmUWPGV_hBbhn-Kk-Bs838OjGzWzJJlQpQKQA.png" },
                    { 26000004, "P.E.K.K.A", "https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png" },
                    { 26000003, "Giant", "https://api-assets.clashroyale.com/cards/300/Axr4ox5_b7edmLsoHxBX3vmgijAIibuF6RImTbqLlXE.png" },
                    { 26000002, "Goblins", "https://api-assets.clashroyale.com/cards/300/X_DQUye_OaS3QN6VC9CPw05Fit7wvSm3XegXIXKP--0.png" },
                    { 26000001, "Archers", "https://api-assets.clashroyale.com/cards/300/W4Hmp8MTSdXANN8KdblbtHwtsbt0o749BbxNqmJYfA8.png" },
                    { 26000000, "Knight", "https://api-assets.clashroyale.com/cards/300/jAj1Q5rclXxU9kVImGqSJxa4wEMfEhvwNQ_4jiGUuqg.png" },
                    { 26000008, "Barbarians", "https://api-assets.clashroyale.com/cards/300/TvJsuu2S4yhyk1jVYUAQwdKOnW4U77KuWWOTPOWnwfI.png" },
                    { 26000019, "Spear Goblins", "https://api-assets.clashroyale.com/cards/300/FSDFotjaXidI4ku_WFpVCTWS1hKGnFh1sxX0lxM43_E.png" },
                    { 26000012, "Skeleton Army", "https://api-assets.clashroyale.com/cards/300/fAOToOi1pRy7svN2xQS6mDkhQw2pj9m_17FauaNqyl4.png" },
                    { 26000021, "Hog Rider", "https://api-assets.clashroyale.com/cards/300/Ubu0oUl8tZkusnkZf8Xv9Vno5IO29Y-jbZ4fhoNJ5oc.png" },
                    { 26000040, "Dart Goblin", "https://api-assets.clashroyale.com/cards/300/BmpK3bqEAviflqHCdxxnfm-_l3pRPJw3qxHkwS55nCY.png" },
                    { 26000020, "Giant Skeleton", "https://api-assets.clashroyale.com/cards/300/0p0gd0XaVRu1Hb1iSG1hTYbz2AN6aEiZnhaAib5O8Z8.png" },
                    { 26000038, "Ice Golem", "https://api-assets.clashroyale.com/cards/300/r05cmpwV1o7i7FHodtZwW3fmjbXCW34IJCsDEV5cZC4.png" },
                    { 26000037, "Inferno Dragon", "https://api-assets.clashroyale.com/cards/300/y5HDbKtTbWG6En6TGWU0xoVIGs1-iQpIP4HC-VM7u8A.png" },
                    { 26000036, "Battle Ram", "https://api-assets.clashroyale.com/cards/300/dyc50V2cplKi4H7pq1B3I36pl_sEH5DQrNHboS_dbbM.png" },
                    { 26000035, "Lumberjack", "https://api-assets.clashroyale.com/cards/300/E6RWrnCuk13xMX5OE1EQtLEKTZQV6B78d00y8PlXt6Q.png" },
                    { 26000034, "Bowler", "https://api-assets.clashroyale.com/cards/300/SU4qFXmbQXWjvASxVI6z9IJuTYolx4A0MKK90sTIE88.png" },
                    { 26000033, "Sparky", "https://api-assets.clashroyale.com/cards/300/2GKMkBrArZXgQxf2ygFjDs4VvGYPbx8F6Lj_68iVhIM.png" },
                    { 26000032, "Miner", "https://api-assets.clashroyale.com/cards/300/Y4yWvdwBCg2FpAZgs8T09Gy34WOwpLZW-ttL52Ae8NE.png" },
                    { 26000039, "Mega Minion", "https://api-assets.clashroyale.com/cards/300/-T_e4YLbuhPBKbYnBwQfXgynNpp5eOIN_0RracYwL9c.png" },
                    { 26000030, "Ice Spirit", "https://api-assets.clashroyale.com/cards/300/lv1budiafU9XmSdrDkk0NYyqASAFYyZ06CPysXKZXlA.png" },
                    { 26000031, "Fire Spirit", "https://api-assets.clashroyale.com/cards/300/16-BqusVvynIgYI8_Jci3LDC-r8AI_xaIYLgXqtlmS8.png" },
                    { 26000023, "Ice Wizard", "https://api-assets.clashroyale.com/cards/300/W3dkw0HTw9n1jB-zbknY2w3wHuyuLxSRIAV5fUT1SEY.png" },
                    { 26000024, "Royal Giant", "https://api-assets.clashroyale.com/cards/300/mnlRaNtmfpQx2e6mp70sLd0ND-pKPF70Cf87_agEKg4.png" },
                    { 26000025, "Guards", "https://api-assets.clashroyale.com/cards/300/1ArKfLJxYo6_NU_S9cAeIrfbXqWH0oULVJXedxBXQlU.png" },
                    { 26000022, "Minion Horde", "https://api-assets.clashroyale.com/cards/300/Wyjq5l0IXHTkX9Rmpap6HaH08MvjbxFp1xBO9a47YSI.png" },
                    { 26000027, "Dark Prince", "https://api-assets.clashroyale.com/cards/300/M7fXlrKXHu2IvpSGpk36kXVstslbR08Bbxcy0jQcln8.png" },
                    { 26000028, "Three Musketeers", "https://api-assets.clashroyale.com/cards/300/_J2GhbkX3vswaFk1wG-dopwiHyNc_YiPhwroiKF3Mek.png" },
                    { 26000029, "Lava Hound", "https://api-assets.clashroyale.com/cards/300/unicRQ975sBY2oLtfgZbAI56ZvaWz7azj-vXTLxc0r8.png" },
                    { 26000026, "Princess", "https://api-assets.clashroyale.com/cards/300/bAwMcqp9EKVIKH3ZLm_m0MqZFSG72zG-vKxpx8aKoVs.png" }
                });

            migrationBuilder.InsertData(
                table: "Chests",
                columns: new[] { "Name", "Url" },
                values: new object[,]
                {
                    { "King's Chest", "https://static.wikia.nocookie.net/clashroyale/images/5/51/Kings_Chest.png" },
                    { "Crown Chest", "https://static.wikia.nocookie.net/clashroyale/images/7/75/CrownChest.png" },
                    { "Fortune Chest", "https://static.wikia.nocookie.net/clashroyale/images/d/de/Fortune_Chest.png" },
                    { "Giant Chest", "https://static.wikia.nocookie.net/clashroyale/images/d/da/Giant_chest.png" },
                    { "Gold Crate", "https://static.wikia.nocookie.net/clashroyale/images/c/cb/GoldCrate.png" },
                    { "Golden Chest", "https://static.wikia.nocookie.net/clashroyale/images/8/8b/GoldenChest.png" },
                    { "Legendary Chest", "https://static.wikia.nocookie.net/clashroyale/images/a/a1/LegendChest.png" },
                    { "Overflowing Gold Crate", "https://static.wikia.nocookie.net/clashroyale/images/7/76/OverflowingGoldCrate.png" },
                    { "Lightning Chest", "https://static.wikia.nocookie.net/clashroyale/images/2/2c/Lightning_Chest.png" },
                    { "Magical Chest", "https://static.wikia.nocookie.net/clashroyale/images/9/93/MagicalChest.png" },
                    { "Mega Lightning Chest", "https://static.wikia.nocookie.net/clashroyale/images/3/3a/MegaLightningChest.png" },
                    { "Plentiful Gold Crate", "https://static.wikia.nocookie.net/clashroyale/images/0/03/PlentifulGoldCrate.png" },
                    { "Silver Chest", "https://static.wikia.nocookie.net/clashroyale/images/0/07/SilverChest.png" },
                    { "Wooden Chest", "https://static.wikia.nocookie.net/clashroyale/images/3/30/WoodenChest.png" },
                    { "Legendary King's Chest", "https://static.wikia.nocookie.net/clashroyale/images/4/42/Legendary_Kings_Chest.png" },
                    { "Epic Chest", "https://https://static.wikia.nocookie.net/clashroyale/images/f/f5/EpicChest.png.wikia.nocookie.net/clashroyale/images/f/f5/EpicChest.png" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Chests");

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
