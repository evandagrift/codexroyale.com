using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameModesController : ControllerBase
    {

        //context to DB and Repo for handling
        private TRContext context;
        private GameModesRepo repo;
        private ILogger<GameModesController> _logger;
        

        //loading in injected dependancies
        public GameModesController(TRContext c, ILogger<GameModesController> logger)
        {
            context = c;
            _logger = logger;
            

            //init the repo with DB context
            repo = new GameModesRepo(context);
        }

        //posts provided gamemode
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] GameMode gameMode)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Posting gamemode {gameMode.Name}");
            repo.AddGameModeIfNew(gameMode);
        }

        //gets all gamemodes in the db
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all gamemodes");
            List<GameMode> gameModes = repo.GetAllGameModes();
            return JsonConvert.SerializeObject(gameModes, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //gets gamemode with given id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting gamemode {id}");
            GameMode gameMode = repo.GetGameModeByID(id);
            return JsonConvert.SerializeObject(gameMode, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/GameModes/{gameModeTag}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING GAMEMODE {id}!");
            repo.DeleteGameMode(id);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/GameModes
        [HttpPut]
        public void Update([FromBody] GameMode gameMode)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING GAMEMODE {gameMode.Name}!");
            repo.UpdateGameMode(gameMode);
        }

    }
}
