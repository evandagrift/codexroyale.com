using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase

    {

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private PlayerSnapshotRepo _playerSnapshotRepo;
        private ChestsRepo _chestsRepo;
        private ILogger<PlayersController> _logger;

        //loading in injected dependancies
        public PlayersController(Client c, TRContext ct, ILogger<PlayersController> logger)
        {
            _context = ct;
            _client = c;
            //init the repo with DB context
            _playerSnapshotRepo = new PlayerSnapshotRepo(_client, _context);
            _chestsRepo = new ChestsRepo(_client, _context);
            _logger = logger;
        }


        //gets current player data with given tag
        [AllowAnonymous]
        [HttpGet("{playerTag}")]
        public async Task<IActionResult> GetOfficialPlayer(string playerTag)
        {
            _logger.LogDebug($"{Request.HttpContext.Connection.RemoteIpAddress} getting {playerTag}'s current player data");
            PlayerSnapshot returnPlayer = await _playerSnapshotRepo.GetOfficialPlayer(playerTag);

            _logger.LogInformation($"returnPlayer:{returnPlayer.Name}");

            return Ok(JsonConvert.SerializeObject(returnPlayer, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        //gets the players upcoming chests that they will unlock
        [AllowAnonymous]
        [HttpGet("{playerTag}/chests")]
        public async Task<IActionResult> GetPlayerChests(string playerTag)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting {playerTag}'s upcoming chests");
            //gets players upcoming Chests
            List<Chest> playerChests = await _playerSnapshotRepo.GetPlayerChestsAsync(playerTag);

            return Ok(JsonConvert.SerializeObject(playerChests, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        //gets the players upcoming chests that they will unlock
        [AllowAnonymous]
        [HttpGet("{playerTag}/decks")]
        public IActionResult GetPlayerTopDeks(string playerTag)
        {
            //gets players upcoming Chests
            List<Deck> playerTopDecks = _playerSnapshotRepo.GetPlayerTopDecksAsync(playerTag);
            ////_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting {playerTag}'s Top Decks");

            return Ok(JsonConvert.SerializeObject(playerTopDecks, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }


        // POST api/Players
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] PlayerSnapshot player)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING PLAYER SNAPSHOT FROM PLAYER {player.Tag}");
            _playerSnapshotRepo.AddPlayer(player);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/players
        [HttpGet]
        public string Get()
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting all player snapshots");
            List<PlayerSnapshot> players = _playerSnapshotRepo.GetAllPlayers();

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
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting playersnapshot with Id:{id}");
            PlayerSnapshot player = _playerSnapshotRepo.GetPlayerById(id);
            return JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }


        //Deleteing player snapshot with given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete([FromHeader] int id)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING PLAYER SNAPSHOT WITH Id:{id}");
            _playerSnapshotRepo.DeletePlayer(id);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] PlayerSnapshot player)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING PLAYER SNAPSHOT WITH Id:{player.Id}");
            _playerSnapshotRepo.UpdatePlayer(player);
        }


    }
}