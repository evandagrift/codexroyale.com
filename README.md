<h1 display="flex" align="center"> Codex Royale REST API </h1>

<p display="flex" align="center"><img src="images/clash-logo.png"  alt="Clash Banner" width='60%' height="auto" /></p>

*   [Overview](#overview)
*   [Setup](#setup)
*   [Dependancies](#build-dependancies)
*   [EndPoints](#end-points)
    *   [Users](#users)
    *   [battles](#battles)
    *   [Cards](#cards)
    *   [Chests](#chests)
    *   [Clans](#clans)
    *   [Decks](#decks)
    *   [Game Modes](#game-modes)
    *   [Players](#players)
    *   [Teams](#teams)
*   [Contact](#Contact)

# Overview
#### Codex Royale API is a REST API built in Asp.Net Core 3.1. This program calls the [Clash Royale API](https://developer.clashroyale.com) and repackages the recieved data into [more practical classes](https://github.com/evandagrift/clash-royale-classes) using [Newtonsoft](https://www.newtonsoft.com/json). Consumed data is saved to a Database using [EF Core](https://docs.microsoft.com/en-us/ef/core/). This API also services [codexroyale.com](www.codexroyale.com) \**still in development\**

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
7. Run the program


# Build Dependancies 
You will need to install all the below packages to be able to build the project<br />
<br />
[Newtonsoft.Json (12.0.3)](https://www.newtonsoft.com/json)<br />
<br />
[Microsoft.EntityFrameworkCore (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.Design (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.Tools (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)<br />
<br />
the two dependancies below this are for using SqlServer, this can be connected to any DB though with the correct dependancies installed<br />
<br />
[Microsoft.EntityFrameworkCore.SqlServer (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)
<br />
[Microsoft.EntityFrameworkCore.SqlServer.Design (3.1.9)](https://docs.microsoft.com/en-us/ef/core/)<br />
<br />
[Microsoft.AspNetCore.Cors (2.2.0)](https://www.nuget.org/packages/Microsoft.AspNetCore.Cors/)<br />
<br />
[Microsoft.AspNetCore.Authentication (2.2.0)](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)
<br />
[Microsoft.AspNetCore.Authentication.JwtBearer (3.1.9)](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)<br />
<br />
[BCrypt.Net-Next (4.0.2)](https://github.com/BcryptNet/bcrypt.net/)

# End Points
ASP.Net Core uses the [MVC Pattern](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?WT.mc_id=dotnet-35129-website&view=aspnetcore-3.1) so all the end points can be found in the Controllers folder. These files can be identified by end-point-name-Controller.cs and can be called at http://localhost:52003/api/EndPoint

# add data about dotnet authentication Bearer token


# ADD A NAVIGATION SYSTEM SO Contact is easy to find below everything

## Users 

* ### Post://Users/Signup 
###### [Public] (Post w/ User JSON in Body)
If there is no user with this username or email, the user's password is encrypted and then the user is saved into the database, and a user token is generated. The server then returns the User with relevant fields filled, and a token included for access. 
<br />
<br />

* ### Post://Users/Login 
###### [Public] (Post with User JSON in Body)
If the username and password is correct it returns the user with relevant data including the user token for access to the API
<br />
<br />

* ### Get://Users 
###### [AdminOnly] (Get)
Returns all User saved in the databse
<br />
<br />

* ### Post://Users 
###### [AdminOnly] (Post with user JSON in Body)
Saves the given user to the database
<br />
<br />

* ### Get://Users/username 
###### [AdminOnly] (Get with username in header)
Returns user with given username
<br />
<br />


* ### Delete://Users/username 
###### [AdminOnly] (Delete with username in header)
Returns user with given username
<br />
<br />

* ### Put://Users 
###### [AdminOnly] (Put with User JSON in Body)
Updates user with given username
<br />
<br />




## Battles 

* ### Post://Battles 
###### [AdminOnly] (Post w/ JSON in body)
Adds the battle to the database if it is new.

* ### Post://Battles 
###### [AdminOnly] (Post w/ JSON List of battles in body)
Adds all new battles to the database

* ### Get://Battles/{User} 
###### [All] (Get w/ JSON User in Header)
Returns a list of all battles played by the given user

* ### Get://Battles 
###### [AdminOnly] (Get)
Returns all battles saved within the Database

* ### Get://Battles/id 
###### [AdminOnly] (Get w/ id in header)
Returns the battle with given id

* ### Delete://Battles/id 
###### [AdminOnly] (Delete w/ id in header)
Deletes battle with given id

* ### Put://api/Battles/{Battle} 
###### [AdminOnly] (Put w/ Battle JSON in body)
Updates the battle in the database with the given data


## Cards 

* ### Post://api/Cards 
###### [Admin Only] (Post w/ JSON in Body)
Adds the card to the database if it is not currently in the database

* ### Get://api/Cards 
###### [All] (Get)
Returns all cards in the database

* ### Get://api/Cards/id 
###### [All] (Get w/ id in header)
Returns card with given id

* ### Delete://api/Cards/id 
###### [Admin Only] (Delete w/ id in header)
Deletes card with given id

* ### Post://api/Cards/UpdateCards |
###### [Admin Only] (Post)
Calls the cards from offical database and adds new cards to the database

* ### Put://api/Cards 
###### [Admin Only] (Put with Card in body)
Updates the given Card


## Chests 

* ### Post://api/Chests
###### [Admin Only] (Post with the Chest JSON in body)
Adds given Chest item to the database

* ### Get://api/Chests
###### [Admin Only] (Get)
Gets all chests saved in the Database

* ### Get://api/Chests/chest-name
###### [Admin Only] (Get with chest name in header)
Gets chest details on the chest saved in the database with that name

* ### Delete://api/Chests/chest-name
###### [Admin Only] (Delete with chest name in header)
Deletes the chest with the given name from the database

* ### Put://api/Chests
###### [Admin Only] (Put with chest JSON in body)
Updates the given chest





## Clans

* ### Post://api/Clans
###### [Admin Only] (Post with the clan JSON in body)
Adds given clan instance to the database

* ### Get://api/Clans
###### [Admin Only] (Get)
Gets all clan data saved in the Database

* ### Get://api/Clans/id
###### [Admin Only] (Get with id in header)
Gets clan data at given id

* ### Delete://api/Clans/id
###### [Admin Only] (Delete with id in header)
Deletes the clan save at given id from the database

* ### Put://api/Clans
###### [Admin Only] (Put with clan JSON in body)
Updates the given clan data save


## Decks

* ### Post://api/Decks
###### [Admin Only] (Post with the Deck JSON in body)
Takes deck data, organizes it and checks if it is in the database, if it is not it adds it. The deck is then returned with an assigned id

* ### Get://api/Decks
###### [Admin Only] (Get)
Gets all decks saved in the Database

* ### Get://api/Decks/id
###### [Admin Only] (Get with id in header)
Gets deck with given id

* ### Delete://api/Decks/id
###### [Admin Only] (Delete with id in header)
Deletes the deck at given id from the database

* ### Put://api/Decks
###### [Admin Only] (Put with clan JSON in body)
Updates the given deck




## Game Modes

* ### Post://api/GameModes
###### [Admin Only] (Post with the game mode JSON in body)
Adds given gamemode if it is not already saved

* ### Get://api/GameModes
###### [Admin Only] (Get)
Gets all game modes saved in the Database

* ### Get://api/GameModes/id
###### [Admin Only] (Get with id in header)
Gets game mode with given id

* ### Delete://api/GameModes/id
###### [Admin Only] (Delete with id in header)
Deletes the game mode at given id from the database

* ### Put://api/GameModes
###### [Admin Only] (Put with game mode JSON in body)
Updates the given game mode


## Players

* ### Post://api/Players
###### [Admin Only] (Post with the player JSON in body)
saves given player data 

* ### Get://api/Players
###### [Admin Only] (Get)
Gets all saved player data in the Database

* ### Get://api/Players/id
###### [Admin Only] (Get with id in header)
Gets player data with given id

* ### Post://api/Players/update
###### [Admin Only] (Get with user JSON in body)
Packages all player data and saves it to the database if new, then returns filled player data

* ### Delete://api/Players/id
###### [Admin Only] (Delete with id in header)
Deletes the player data at given id from the database

* ### Put://api/Players
###### [Admin Only] (Put with player JSON in body)
Updates the given save of player data



## Teams

* ### Post://api/Teams
###### [Admin Only] (Post with the Team JSON in body)
Creates a team with the player combination if nonexistent, then returns the team with an assigned id

* ### Get://api/Teams
###### [Admin Only] (Get)
Gets all teams in the database

* ### Get://api/Teams/id
###### [Admin Only] (Get with id in header)
Gets team with given id

* ### Delete://api/Teams/id
###### [Admin Only] (Delete with id in header)
Deletes the team at given id from the database

* ### Put://api/Teams
###### [Admin Only] (Put with player JSON in body)
Updates the given team

# Contact
Hi, if you have any questions feel free to email me at <a href = "mailto: Evandagrift@gmail.com">Evandagrift@gmail.com</a> I'm currently looking for my first job as a software developer. Any feedback would be greatly appreciated :)

[clash-logo]:https://uc09cdbb3b2643b7064228146b69.previews.dropboxusercontent.com/p/thumb/ABMP6UdWBTH37pdZewBaJ7FywNLGhdKXUOVOxOSen902cGr-b01gnUUbTc81ZwKb7CpNcL_T9sdP_jVPN0fsdajS0BPUefVjl7gZtVPBfNIDFa8zAj66Fh4ExDaNKQHk3J7KS0111Evph892MpySuhmigW0puuKuDGmPtT3fUqZGWfsJRDkoBOxQA8ZQiL2f4NC72a2oNwwuh21lFNxY9RpB4Yp0t3T6iVw0LbqLIlT383277nXxyrwb-FJVVFTH0gtq60Xk1CMmsW3Om5D-CFj6hIVhISqGdezPPJW1RAuljNr6Xu43_oilpnPJPTL9UgsUISk4jEva1Cl95ToUqZlWpS5rTF433YReGD7yoS9O3Exl1nL0fNrdpTumeLu6BZ5rmRCuYEHdim_oFNGxsnCZFhdLa8i3R4PCnp7y9UaU_WIERgLzheXfCRG4uk4ceoZNXGqzQhLqOqWCTVU_iAWs_x8PQ0tRI7uVX-bPdTLnHosQz1llf79YUQPyFhgT04GQeBzw1_Gf6wLn3pYRN21pr6kOW85mfpMhGWlTWx4g8A/p.png
