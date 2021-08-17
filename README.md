<h1 display="flex" align="center"> Codex Royale REST API </h1>

<p display="flex" align="center"><img src="images/clash-logo.png"  alt="Clash Banner" width='60%' height="auto" /></p>

### Codex Royale API is a REST API built in Asp.Net Core 3.1. This program calls the [Clash Royale API](https://developer.clashroyale.com) and repackages the recieved data into [more practical classes](https://github.com/evandagrift/clash-royale-classes) using [Newtonsoft](https://www.newtonsoft.com/json). Consumed data is saved to a Database using [EF Core](https://docs.microsoft.com/en-us/ef/core/). This API also services [codexroyale.com](www.codexroyale.com) \**still in development\**

## Setup </span>
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

## <a id="dependancies"> Build Dependancies </a>
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

## Endpoints
ASP.Net Core uses the [MVC Pattern](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?WT.mc_id=dotnet-35129-website&view=aspnetcore-3.1)
<h3>an endpoint</h3>


### contact, etc


[clash-logo]:https://uc09cdbb3b2643b7064228146b69.previews.dropboxusercontent.com/p/thumb/ABMP6UdWBTH37pdZewBaJ7FywNLGhdKXUOVOxOSen902cGr-b01gnUUbTc81ZwKb7CpNcL_T9sdP_jVPN0fsdajS0BPUefVjl7gZtVPBfNIDFa8zAj66Fh4ExDaNKQHk3J7KS0111Evph892MpySuhmigW0puuKuDGmPtT3fUqZGWfsJRDkoBOxQA8ZQiL2f4NC72a2oNwwuh21lFNxY9RpB4Yp0t3T6iVw0LbqLIlT383277nXxyrwb-FJVVFTH0gtq60Xk1CMmsW3Om5D-CFj6hIVhISqGdezPPJW1RAuljNr6Xu43_oilpnPJPTL9UgsUISk4jEva1Cl95ToUqZlWpS5rTF433YReGD7yoS9O3Exl1nL0fNrdpTumeLu6BZ5rmRCuYEHdim_oFNGxsnCZFhdLa8i3R4PCnp7y9UaU_WIERgLzheXfCRG4uk4ceoZNXGqzQhLqOqWCTVU_iAWs_x8PQ0tRI7uVX-bPdTLnHosQz1llf79YUQPyFhgT04GQeBzw1_Gf6wLn3pYRN21pr6kOW85mfpMhGWlTWx4g8A/p.png
