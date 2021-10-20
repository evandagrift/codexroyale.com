using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private PlayersRepo _playersRepo;
        private ChestsRepo _chestsRepo;
        private Client _client;

        //loading in injected dependancies
        public PlayersController(Client c, TRContext ct)
        {
            _context = ct;
            _client = c;
            //init the repo with DB context
            _playersRepo = new PlayersRepo(_client, _context);
            _chestsRepo = new ChestsRepo(_client, _context);
        }

        // POST api/Players
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] PlayerSnapshot player)
        {
            _playersRepo.AddPlayer(player);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/players
        [HttpGet]
        public string Get()
        {
            List<PlayerSnapshot> players = _playersRepo.GetAllPlayers();

            return JsonConvert.SerializeObject(players, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Players/id
        [HttpGet("{id}")]
        public string Get(int id)
        {
            PlayerSnapshot player = _playersRepo.GetPlayerById(id);
            return JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        //[Authorize(Policy = "All")]
        [AllowAnonymous]
        [HttpPost("full/{playerTag}")]
        public async Task<string> GetUpdatePlayer(string playerTag)
        {
            //get the users's player data w/ their chests in rotation as well as battles
            PlayerSnapshot returnPlayer = await _playersRepo.GetFullPlayer(playerTag);
            //
            return JsonConvert.SerializeObject(returnPlayer, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        //[Authorize(Policy = "All")]
        [AllowAnonymous]
        [HttpPost("official/{playerTag}")]
        public async Task<string> GetOfficialPlayer(string playerTag)
        {
            //get the users's player data w/ their chests in rotation as well as battles
            PlayerSnapshot returnPlayer = await _playersRepo.GetOfficialPlayer(playerTag);

            //gets players Chests r
            List<Chest> playerChests = await _playersRepo.GetPlayerChestsAsync(playerTag);

            if (playerChests.Count > 0)
            {
                returnPlayer.Chests = playerChests;
            }

            return JsonConvert.SerializeObject(returnPlayer, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        //[Authorize(Policy = "All")]
        [AllowAnonymous]
        [HttpPost("Chests/{playerTag}")]
        public async Task<string> GetPlayerChests(string playerTag)
        {
            //gets players upcoming Chests
            List<Chest> playerChests = await _playersRepo.GetPlayerChestsAsync(playerTag);


            return JsonConvert.SerializeObject(playerChests, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Players/id
        [HttpDelete("{id}")]
        public void Delete([FromHeader]int id)
        {
            _playersRepo.DeletePlayer(id);
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Players/id
        [HttpPut]
        public void Update([FromBody] PlayerSnapshot Player)
        {
            _playersRepo.UpdatePlayer(Player);
        }


    }
}
