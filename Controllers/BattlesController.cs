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
    public class BattlesController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private BattlesRepo repo;

        //loading in injected dependancies
        public BattlesController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            context = c;
            repo = new BattlesRepo(context);
        }

        // POST api/Battles
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("addbattles")]
        public int Post([FromBody] List<Battle> battles)
        {
            //adds the list of battles to DB
            return repo.AddBattles(battles);
        }

        // POST api/Battles
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("addbattle")]
        // POST: api/Battles/addbattle
        public void Post([FromBody] Battle battle)
        {
            //adds the Posted battle to DB
            repo.AddBattle(battle);
        }

        [Authorize(Policy = "All")]

        [HttpPost("getbattlesbyuser")]
        // POST: api/Battles/getbattlebyuser
        public List<Battle> GetBattle([FromBody] User user)
        {
            //returns battle with Id based off given battle, if said battle doesn't exist it is created and returned after assigned Id
            return repo.GetAllBattles(user);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/Battles
        [HttpGet]
        public string Get()
        {
            //returns all battles in the game via Json
            return JsonConvert.SerializeObject(repo.GetAllBattles(), Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Battles/battleTID
        [HttpGet("{battleID}", Name = "getbattlebyid")]
        public string Get(int battleID)
        {
            //returns player with given Id
            return JsonConvert.SerializeObject(repo.GetBattleByID(battleID), Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Battles/{battleID}
        [HttpDelete("{battleID}")]
        public void Delete(int battleID)
        {
            //deletes battle at given /Id
            repo.DeleteBattle(battleID);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Battles
        [HttpPut]
        public void Update([FromBody] Battle battle)
        {
            //updates battle with same Id as argument
            repo.UpdateBattle(battle);
        }

    }
}
