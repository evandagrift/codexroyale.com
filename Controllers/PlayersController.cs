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
        private PlayersRepo repo;

        //loading in injected dependancies
        public PlayersController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new PlayersRepo(context);
        }

        // POST api/Players
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Player player)
        {
            repo.AddPlayer(player);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/<NameController>
        [HttpGet]
        public string Get()
        {
            List<Player> players = repo.GetAllPlayers();

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
            Player player = repo.GetPlayerById(playerId);
            return JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpDelete("{playerId}")]
        public void Delete(int playerId)
        {
            repo.DeletePlayer(playerId);
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpPut]
        public void Update([FromBody] Player Player)
        {
            repo.UpdatePlayer(Player);
        }


    }
}
