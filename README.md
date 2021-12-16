 <h1 display="flex" align="center"> Codex Royale REST API </h1>

<p display="flex" align="center"><img src="images/clash-logo.png"  alt="Clash Banner" width='60%' height="auto" /></p>



*   [Overview](#overview)
*   [Setup](#setup)
*   [Dependancies](#build-dependancies)
*   [EndPoints](#end-points)
    *   [Users](#users)
    *   [Battles](#battles)
    *   [Cards](#cards)
    *   [Chests](#chests)
    *   [Clans](#clans)
    *   [Decks](#decks)
    *   [Game Modes](#game-modes)
    *   [Players](#players)
    *   [Teams](#teams)
*   [Contact](#Contact)



# Overview
#### Codex Royale API is a REST API built in Asp.Net Core 3.1. This program calls the [Clash Royale API](https://developer.clashroyale.com) and repackages the recieved data into [more practical classes](https://github.com/evandagrift/clash-royale-classes) using [Newtonsoft](https://www.newtonsoft.com/json). Consumed data is saved to a Database using [EF Core](https://docs.microsoft.com/en-us/ef/core/). This API services [codexroyale.com](www.codexroyale.com) as well as automatically saves all new battles from users that have been previously searched



# Setup
1. Get a bearer token for the [Clash Royale API](https://developer.clashroyale.com) connected to the IP you will be using
2. Clone this repository
3. <a href="#dependancies">Install the dependancies in Visual Studio Package Manager</a>
4. Edit [appsettings.json](appsettings.json)
* Change the BearerToken to the token given to you by Clash Royale API
* _Change the ConnectionString to your connection string if you are using a different database system_
5. Setup EF Tools
* Open Terminal/Cli routed to the project file
* run `dotnet tool install --global dotnet-ef` in the terminal to install ef core tools
* close the terminal
6. Build the database
* Reopen the terminal as you did previously
* Create a migration for the database (Local SqlServer is chosen be default). Run `dotnet ef migrations add migration-name` in the terminal
* Build the database for the project `dotnet ef database update`
7. Run the program, neccesary data will automatically be seeded if your Clash Royale bearer token is valid


# Build Dependancies 
You will need to install all the below packages to be able to build the project
<br />
[Microsoft.EntityFrameworkCore (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.Design (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.Tools (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)<br />
<br />
[Microsoft.AspNetCore.Cors (2.2.0)](https://www.nuget.org/packages/Microsoft.AspNetCore.Cors/)<br />
<br />
[Microsoft.AspNetCore.Authentication (2.2.0)](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)
<br />
[Microsoft.AspNetCore.Authentication.JwtBearer (3.1.9)](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)<br />
<br />
[Newtonsoft.Json (12.0.3)](https://www.newtonsoft.com/json)<br />
<br />
[SendGrid (9.24.2)](https://www.nuget.org/packages/Sendgrid)
<br />
<br />
[BCrypt.Net-Next (4.0.2)](https://github.com/BcryptNet/bcrypt.net/)
<br />
<br />
the two dependancies below this are for using SqlServer, this can be connected to any DB though with the correct dependancies installed.
<br />
[Microsoft.EntityFrameworkCore.SqlServer (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.SqlServer.Design (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)<br />
<br />



# End Points
ASP.Net Core uses the [MVC Pattern](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?WT.mc_id=dotnet-35129-website&view=aspnetcore-3.1) so all the end points can be found in the /Controllers folder. These files can be identified by end-point-name-Controller.cs and can be called at `http://localhost:52003/api/EndPoint`. This API uses [AspNet Core Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0) to handle authentication. There are three level of authorization Anonymous [AllowAnonymous], Any user with a valid user token [All], and Admin users [Admin Only]. If there are no users within the database, the first user created will be given admin privledges. 



## Users 
Account to access deeper API functions, and log into [codexroyale.com](www.codexroyale.com)
#### User JSON format
`{
    "username": "username",
    "password": null,
    "email": "user-email",
    "tag": "#null-if-given-invalid-tag",
    "clanTag": "#clantag-if-user-have-valid-tag",
    "role": "user-role",
    "token": "user-Token"
}`

* ### POST://Users/Signup 
###### [AllowAnonymous] (POST w/ User JSON in Body)
If there is no user with this username or email, the user's password is encrypted and then the user is saved into the database, and a user token is generated. The server then returns the User with relevant fields filled, and a token included for access. If signup fails returns 401

#### POST from body JSON login format
`{ 
"username": "username", 
"password": "password", 
"email": "user-email", 
"tag": "#user-Clash-Royale-Tag"
}`

* ### POST://Users/Login 
###### [AllowAnonymous] (POST with User JSON in Body)
**Only Include Username and Password**, if the username and password are correct, returns the user with relevant data including the user token for access to the API

* ### GET://Users 
###### [AdminOnly] (GET)
Returns all Users saved in the databse

* ### POST://Users 
###### [AdminOnly] (POST with user JSON in Body)
Saves the given user to the database. Note this will not encrypt passwords so users won't be able to login or be assigned a token through this end-point. See Users/Signup if you are trying to create a valid user.

* ### GET://Users/username 
###### [AdminOnly] (GET)
Returns user with given username

* ### DELETE://Users/username 
###### [AdminOnly] (DELETE)
Deletes user with given Username

* ### PUT://Users 
###### [AdminOnly] (PUT with User JSON in Body)
Updates user with given username



## Battles 
battles are added if new, battles are returned from the viewpoint of a player, but via Teams I am able to avoid duplicate battles and have conistent battle formatting between 1v1 and 2v2
#### Battle JSON Object format
`{
  "BattleId": 1,
  "BattleTime": "20210821T213000",
  "Team1Name": "Elodin",
  "Team1Id": 1,
  "Team1Win": false,
  "Team1StartingTrophies": 5674,
  "Team1TrophyChange": 0,
  "Team1DeckAId": 2,
  "Team1DeckBId": 0,
  "Team1Crowns": 0,
  "Team1KingTowerHp": 0,
  "Team1PrincessTowerHpA": -1,
  "Team1PrincessTowerHpB": -1,
  "Team2Name": "xume",
  "Team2Id": 2,
  "Team2Win": true,
  "Team2StartingTrophies": 5517,
  "Team2TrophyChange": 0,
  "Team2DeckAId": 3,
  "Team2DeckBId": 0,
  "Team2Crowns": 3,
  "Team2KingTowerHp": 5832,
  "Team2PrincessTowerHpA": 3237,
  "Team2PrincessTowerHpB": 2183,
  "Type": "riverRacePvP",
  "DeckSelection": "collection",
  "IsLadderTournament": false,
  "GameModeId": 72000268
}`

* ### POST://Battles 
###### [AdminOnly] (POST w/ JSON in body)
Adds the battle to the database if it is new. **BattleId is automatically generated so POST without an assigned ID**

* ### POST://Battles/list
###### [AdminOnly] (POST w/ JSON List of battles in body)
Adds all new battles to the database

* ### GET://Battles 
###### [AdminOnly] (GET)
Returns all battles saved within the Database

* ### GET://Battles/player/{playerTag} 
###### [All] (GET)
Returns a list of all saved battles played by the given user. Note To send a user tag over the browser you will need to replace the # with the UTF-8 code %23. I.E. #player-tag would become %23player-tag

* ### GET://Battles/id/battle-id
###### [AdminOnly] (GET)
Returns the battle with given id

* ### DELETE://Battles/id 
###### [AdminOnly] (DELETE)
Deletes battle with given id

* ### PUT://api/Battles
###### [AdminOnly] (PUT w/ Battle JSON in body)
Updates the battle in the database with the given data



## Cards 
#### Card JSON format
`{
  "Id": 26000000,
  "Name": "Knight",
  "Url": "https://api-assets.clashroyale.com/cards/300/jAj1Q5rclXxU9kVImGqSJxa4wEMfEhvwNQ_4jiGUuqg.png"
}`


* ### POST://api/Cards 
###### [Admin Only] (POST w/ card JSON in Body)
Adds the card to the database if it is not currently in the database

* ### GET://api/Cards 
###### [All] (GET)
Returns all cards in the database

* ### GET://api/Cards/id 
###### [All] (GET)
Returns card with given id

* ### DELETE://api/Cards/id 
###### [Admin Only] (DELETE)
Deletes card with given id

* ### POST://api/Cards/UpdateCards
###### [Admin Only] (POST)
POST with no body, Calls the cards from the clash royale database and adds new cards to the database

* ### PUT://api/Cards 
###### [Admin Only] (PUT with Card in body)
Updates the given Card



## Chests 
Chests are seeded in with usable URL, if more chests are added to the game this will need to be updated.
#### Chest JSON Format
`{
    "Name": "Crown Chest",
    "Url": "https://static.wikia.nocookie.net/clashroyale/images/7/75/CrownChest.png"
}`


* ### POST://api/Chests
###### [Admin Only] (POST with the Chest JSON in body)
Adds given Chest to the database

* ### GET://api/Chests
###### [Admin Only] (GET)
Gets all chests saved in the Database

* ### GET://api/Chests/chest-name
###### [Admin Only] (GET with chest name in header)
Gets chest details on the chest saved in the database with that name

* ### DELETE://api/Chests/chest-name
###### [Admin Only] (DELETE with chest name in header)
Deletes the chest with the given name from the database

* ### PUT://api/Chests
###### [Admin Only] (PUT with chest JSON in body)
Updates the given chest


## Clans

#### Clan JSON format
`{
    "Id": 1,
    "Tag": "#8CYPL8R",
    "UpdateTime": "20210824T121500",
    "Name": "We are Funny?",
    "Type": "open",
    "Description": "Welcome! We are an active clan that focuses on donations/participation. discord.gg/VRK4eVg over 10 days inactive booted.",
    "BadgeId": 16000153,
    "LocationCode": "International",
    "RequiredTrophies": 4300,
    "DonationsPerWeek": 1070,
    "ClanChestStatus": "inactive",
    "ClanChestLevel": 1,
    "ClanScore": 49138,
    "ClanWarTrophies": 1655,
    "Members": 34
  }`

* ### POST://api/Clans
###### [Admin Only] (POST with the clan JSON in body)
Adds given clan instance to the database **POST without Id, the database will automatically assign a Primary Key/Id**

* ### GET://api/Clans
###### [Admin Only] (GET)
Gets all clan data saved in the Database


* ### GET://api/Clans/id
###### [Admin Only] (GET with id in header)
Gets clan data at given id

* ### DELETE://api/Clans/id
###### [Admin Only] (DELETE with id in header)
Deletes the clan at given id from the database

* ### PUT://api/Clans
###### [Admin Only] (PUT with clan JSON in body)
Updates the given clan data save



## Decks
decks are typically automatically added when new ones are encountered in the proccess of saving battles
#### Deck JSON format
`{
  "Id": 36,
  "Card1Id": 77777777,
  "Card2Id": 999999,
  "Card3Id": 999999,
  "Card4Id": 999999,
  "Card5Id": 999999,
  "Card6Id": 27000012,
  "Card7Id": 27000013,
  "Card8Id": 28000013
  }`

* ### POST://api/Decks
###### [Admin Only] (POST with the Deck JSON in body)
Takes deck data, organizes it and checks if it is in the database, if it is not it adds it. The deck is then returned with an assigned id. **Do not post with an Id, id is auto assigned**

* ### GET://api/Decks
###### [Admin Only] (GET)
Gets all decks saved in the Database

* ### GET://api/Decks/id
###### [Admin Only] (GET with id in header)
Gets deck with given id

* ### DELETE://api/Decks/id
###### [Admin Only] (DELETE with id in header)
Deletes the deck at given id from the database

* ### PUT://api/Decks
###### [Admin Only] (PUT with clan JSON in body)
Updates the given deck



## Game Modes

game modes are typically automatically added when new ones are encountered in the proccess of saving battles

#### GameMode JSON format
`{
    "Id": 72000006,
    "Name": "Ladder"
  }`

* ### POST://api/GameModes
###### [Admin Only] (POST with the game mode JSON in body)
Adds given gamemode if it is not already saved

* ### GET://api/GameModes
###### [Admin Only] (GET)
Gets all game modes saved in the Database

* ### GET://api/GameModes/id
###### [Admin Only] (GET)
Gets game mode with given id

* ### DELETE://api/GameModes/id
###### [Admin Only] (DELETE)
Deletes the game mode at given id from the database

* ### PUT://api/GameModes
###### [Admin Only] (PUT with game mode JSON in body)
Updates the given game mode


## Players

#### Player JSON format
`{
  "Id": 1,
  "Tag": "#29PGJURQL",
  "TeamId": 1,
  "Name": "It's been changed!",
  "UpdateTime": "20210824T121501",
  "ClanTag": "#8CYPL8R",
  "CurrentFavouriteCardId": 26000020,
  "CurrentDeckId": 1,
  "Role": "coLeader",
  "LastSeen": "20210824T074916",
  "ExpLevel": 13,
  "Trophies": 5721,
  "BestTrophies": 5750,
  "StarPoints": 4272,
  "Wins": 9671,
  "Losses": 10413,
  "Donations": 106,
  "DonationsReceived": 160,
  "TotalDonations": 152367,
  "CardsDiscovered": 103,
  "ClanCardsCollected": 348242
}`

* ### POST://api/Players
###### [Admin Only] (POST with the player JSON in body)
saves given player data 

* ### GET://api/Players
###### [Admin Only] (GET)
Gets all saved player data in the Database

* ### GET://api/Players/id
###### [Admin Only] (GET)
Gets player data with given id

* ### GET://api/Players/update/playerTag
###### [Admin Only] (GET)
Packages all player data and saves it to the database if new, then returns current filled player data, upcoming chests in rotation, and recent battles.

* ### DELETE://api/Players/id
###### [Admin Only] (DELETE with id in header)
Deletes the player data at given id from the database

* ### PUT://api/Players
###### [Admin Only] (PUT with player JSON in body)
Updates the given save of player data



## Teams
teams are generated for each unique combination of player(s) they are typically automatically added when new ones are encountered in the proccess of saving battles
#### Team JSON format
`{
    "TeamId": 1,
    "TeamName": "Elodin",
    "TwoVTwo": false,
    "Name": "Elodin",
    "Tag": "#29PGJURQL"
  }`


* ### POST://api/Teams
###### [Admin Only] (POST with Team JSON in body)
Creates a team with the player combination if nonexistent, then returns the team with an assigned id

* ### GET://api/Teams
###### [Admin Only] (GET)
Gets all teams in the database

* ### GET://api/Teams/id
###### [Admin Only] (GET)
Gets team with given id

* ### DELETE://api/Teams/id
###### [Admin Only] (DELETE)
Deletes the team at given id from the database

* ### PUT://api/Teams
###### [Admin Only] (PUT with player JSON in body)
Updates the given team



# Contact
<p>If you have any questions feel free to email me at <a href = "mailto: Evandagrift@gmail.com">Evandagrift@gmail.com</a></p>
<p>I'm currently looking for my first job as a software developer. Any feedback would be greatly appreciated 😃</p>

[clash-logo]:https://uc09cdbb3b2643b7064228146b69.previews.dropboxusercontent.com/p/thumb/ABMP6UdWBTH37pdZewBaJ7FywNLGhdKXUOVOxOSen902cGr-b01gnUUbTc81ZwKb7CpNcL_T9sdP_jVPN0fsdajS0BPUefVjl7gZtVPBfNIDFa8zAj66Fh4ExDaNKQHk3J7KS0111Evph892MpySuhmigW0puuKuDGmPtT3fUqZGWfsJRDkoBOxQA8ZQiL2f4NC72a2oNwwuh21lFNxY9RpB4Yp0t3T6iVw0LbqLIlT383277nXxyrwb-FJVVFTH0gtq60Xk1CMmsW3Om5D-CFj6hIVhISqGdezPPJW1RAuljNr6Xu43_oilpnPJPTL9UgsUISk4jEva1Cl95ToUqZlWpS5rTF433YReGD7yoS9O3Exl1nL0fNrdpTumeLu6BZ5rmRCuYEHdim_oFNGxsnCZFhdLa8i3R4PCnp7y9UaU_WIERgLzheXfCRG4uk4ceoZNXGqzQhLqOqWCTVU_iAWs_x8PQ0tRI7uVX-bPdTLnHosQz1llf79YUQPyFhgT04GQeBzw1_Gf6wLn3pYRN21pr6kOW85mfpMhGWlTWx4g8A/p.png
