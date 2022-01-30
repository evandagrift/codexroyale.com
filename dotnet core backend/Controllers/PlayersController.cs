using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Models.RoyaleClasses;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private PlayersRepo _playersRepo;
        private ChestsRepo _chestsRepo;
        private ILogger<PlayersController> _logger;

        //loading in injected dependancies
        public PlayersController(Client c, TRContext ct, ILogger<PlayersController> logger)
        {
            _context = ct;
            _client = c;
            //init the repo with DB context
            _playersRepo = new PlayersRepo(_client, _context);
            _chestsRepo = new ChestsRepo(_client, _context);
            _logger = logger;
        }

        // POST api/Players
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] PlayerSnapshot player)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING PLAYER SNAPSHOT FROM PLAYER {player.Tag}");
            _playersRepo.AddPlayer(player);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/players
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting all player snapshots");
            List<PlayerSnapshot> players = _playersRepo.GetAllPlayers();

            return JsonConvert.SerializeObject(players, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        //getting player snapshot at given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("id/{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting playersnapshot with Id:{id}");
            PlayerSnapshot player = _playersRepo.GetPlayerById(id);
            return JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //gets current player data with given tag
        [AllowAnonymous]
        [HttpGet("{playerTag}")]
        public async Task<IActionResult> GetOfficialPlayer(string playerTag)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting {playerTag}'s current player data");
            PlayerSnapshot returnPlayer = await _playersRepo.GetOfficialPlayer(playerTag);

            return Ok(JsonConvert.SerializeObject(returnPlayer, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }
        
        //gets the players upcoming chests that they will unlock
        [AllowAnonymous]
        [HttpGet("chests/{playerTag}")]
        public async Task<IActionResult> GetPlayerChests(string playerTag)
        {

            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting {playerTag}'s upcoming chests");
            //gets players upcoming Chests
            List<Chest> playerChests = await _playersRepo.GetPlayerChestsAsync(playerTag);

            return Ok(JsonConvert.SerializeObject(playerChests, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }

        //Deleteing player snapshot with given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete([FromHeader] int id)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING PLAYER SNAPSHOT WITH Id:{id}");
            _playersRepo.DeletePlayer(id);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] PlayerSnapshot player)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING PLAYER SNAPSHOT WITH Id:{player.Id}");
            _playersRepo.UpdatePlayer(player);
        }


    }
}