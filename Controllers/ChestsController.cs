using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ChestsController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private ChestsRepo repo;

        //loading in injected dependancies
        public ChestsController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new ChestsRepo(context);
        }

        // POST api/Chests
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Chest chest)
        {
            context.Chests.Add(chest);
            context.SaveChanges();
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/Chests
        [HttpGet]
        public string Get()
        {
            List<Chest> Chests = repo.GetAllChests();

            return JsonConvert.SerializeObject(Chests, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Chests/ChestTag
        [HttpGet("{chestName}", Name = "GetChest")]
        public string Get(string chestName)
        {
            Chest chest = repo.GetChestByName(chestName);
            return JsonConvert.SerializeObject(chest, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Chests/{ChestTag}
        [HttpDelete("{chestName}")]
        public void Delete(string chestName)
        {
            repo.DeleteChest(chestName);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Chests
        [HttpPut]
        public void Update([FromBody] Chest chest)
        {
            repo.UpdateChest(chest);
        }

    }
}
