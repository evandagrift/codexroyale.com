using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameModesController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private GameModesRepo repo;

        //loading in injected dependancies
        public GameModesController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new GameModesRepo(context);
        }

        // POST api/GameModes
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] GameMode gameMode)
        {
            repo.AddGameModeIfNew(gameMode);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/GameModes
        [HttpGet]
        public string Get()
        {
            List<GameMode> gameModes = repo.GetAllGameModes();

            return JsonConvert.SerializeObject(gameModes, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/GameModes/gameModeTag
        [HttpGet("{gameModeID}", Name = "GetGameMode")]
        public string Get(int gameModeID)
        {
            GameMode gameMode = repo.GetGameModeByID(gameModeID);
            return JsonConvert.SerializeObject(gameMode, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/GameModes/{gameModeTag}
        [HttpDelete("{gameModeID}")]
        public void Delete(int gameModeID)
        {
            repo.DeleteGameMode(gameModeID);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/GameModes
        [HttpPut]
        public void Update([FromBody] GameMode gameMode)
        {
            repo.UpdateGameMode(gameMode);
        }

    }
}
