using System.Collections.Generic;
using System.Linq;
using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BattlesController : ControllerBase
    {

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private BattlesRepo _repo;
        private  ILogger<BattlesController> _logger;

        //loading in injected dependancies
        public BattlesController(Client c, TRContext ct, ILogger<BattlesController> logger)
        {
            _context = ct;
            _client = c;
            _repo = new BattlesRepo(_client, _context);
            _logger = logger;
        }


        //returns list of battles
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {

            List<Battle> battles = _repo.GetRecentBattles();
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting Most Recently Saved Battles");
            
            return Ok(JsonConvert.SerializeObject(battles, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        //adds the Posted battle to DB if it's new
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Post([FromBody] Battle battle)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING BATTLE!");
            _repo.AddBattle(battle);
            return Ok();
        }

        //adds the list of battles to DB if they are new
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("list")]
        public IActionResult Post([FromBody] List<Battle> battles)
        {

            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} Posting {battles.Count()} battles");
            _repo.AddBattles(battles);
            return Ok();
        }




        [AllowAnonymous]
        [HttpGet("player/{playerTag}")]
        // GET: api/Battles/{user}
        public IActionResult GetBattles(string playerTag)
        {
            
            //returns battle with Id based off given battle, if said battle doesn't exist it is created and returned after assigned Id
            List<Battle> battles = _repo.GetPlayersRecentBattles(playerTag);

            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting Recent Battles from player:{playerTag}");

            //returns list of battles
            return Ok(JsonConvert.SerializeObject(battles, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }


        //returns player with given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("id/{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting battle {id}");
            return JsonConvert.SerializeObject(_repo.GetBattleByID(id), Formatting.Indented, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore });
        }


        //deletes battle at given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING BATTLE {id}!");
            _repo.DeleteBattle(id);
        }

        //updates battle with same Id as argument
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] Battle battle)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING BATTLE {battle.BattleId}!");
            _repo.UpdateBattle(battle);
        }

    }
}
