using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PlayersController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private PlayersRepo playersRepo;
        private Client client;

        //loading in injected dependancies
        public PlayersController(CustomAuthenticationManager m, Client c, TRContext ct)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = ct;
            client = c;
            //init the repo with DB context
            playersRepo = new PlayersRepo(client, context);
        }

        // POST api/Players
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Player player)
        {
            playersRepo.AddPlayer(player);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/<NameController>
        [HttpGet]
        public string Get()
        {
            List<Player> players = playersRepo.GetAllPlayers();

            return JsonConvert.SerializeObject(players, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Players/5
        [HttpGet("{playerId}", Name = "Get")]
        public string Get(int playerId)
        {
            Player player = playersRepo.GetPlayerById(playerId);
            return JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        [Authorize(Policy = "All")]
        //[AllowAnonymous]
        [HttpPost("update")]
        public async Task<Player> GetUpdatePlayer([FromBody] User user)
        {
            Player returnPlayer = await playersRepo.UpdateGetPlayerWithChestsBattles(user);


            if (returnPlayer != null)
            {

                return returnPlayer;
            }
            else
            {
                return null; }
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpDelete("{playerId}")]
        public void Delete(int playerId)
        {
            playersRepo.DeletePlayer(playerId);
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpPut]
        public void Update([FromBody] Player Player)
        {
            playersRepo.UpdatePlayer(Player);
        }


    }
}
