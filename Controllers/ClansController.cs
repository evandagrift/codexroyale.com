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
    public class ClansController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private ClansRepo repo;

        //loading in injected dependancies
        public ClansController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new ClansRepo(context);
        }
        // POST api/Clans
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Clan clan)
        {
            repo.AddClan(clan);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/Clans
        [HttpGet]
        public string Get()
        {
            List<Clan> clans = repo.GetAllClans();

            return JsonConvert.SerializeObject(clans, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Clans/clanTag
        [HttpGet("{id}", Name = "GetClan")]
        public string Get(int id)
        {
            Clan clan = repo.GetClanById(id);
            return JsonConvert.SerializeObject(clan, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Clans/{clanTag}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            repo.DeleteClan(id);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Clans
        [HttpPut]
        public void Update([FromBody] Clan clan)
        {
            repo.UpdateClan(clan);
        }

    }
}
