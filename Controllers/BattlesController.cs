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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BattlesController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private Client client;
        private BattlesRepo repo;

        //loading in injected dependancies
        public BattlesController(CustomAuthenticationManager m, Client c, TRContext ct)
        {
            customAuthenticationManager = m;
            context = ct;
            client = c;
            repo = new BattlesRepo(client, context);
        }


        // POST api/Battles
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        // POST: api/Battles/addbattle
        public IActionResult Post([FromBody] Battle battle)
        {
            //adds the Posted battle to DB if it's new
            repo.AddBattle(battle);
            return Ok();
        }

        // POST api/Battles
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("list")]
        public IActionResult Post([FromBody] List<Battle> battles)
        {
            //adds the list of battles to DB if they are new
            repo.AddBattles(battles);
            return Ok();
        }

        [Authorize(Policy = "All")]
        [HttpGet("player/{playerTag}")]
        // GET: api/Battles/{user}
        public string GetBattles(string playerTag)
        {
            //returns battle with Id based off given battle, if said battle doesn't exist it is created and returned after assigned Id
            List<Battle> battles = repo.GetAllBattles(playerTag);
            return JsonConvert.SerializeObject(battles, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [AllowAnonymous]
        // GET: api/Battles
        [HttpGet]
        public string Get()
        {
            List<Battle> battles = repo.GetRecentBattles();

            //returns list of battles
            return JsonConvert.SerializeObject(battles, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Battles/{id}
        [HttpGet("id/{id}")]
        public string Get(int id)
        {
            //returns player with given Id
            return JsonConvert.SerializeObject(repo.GetBattleByID(id), Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Battles/{battleID}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //deletes battle at given /Id
            repo.DeleteBattle(id);
        }
        
        [Authorize(Policy = "AdminOnly")]
        // PUT: api/Battles
        [HttpPut]
        public void Update([FromBody] Battle battle)
        {
            //updates battle with same Id as argument
            repo.UpdateBattle(battle);
        }

    }
}
