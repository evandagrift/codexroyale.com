using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ChestsController : ControllerBase
    {
        private TRContext context;
        private ChestsRepo chestsRepo;
        private Client client;
        private ILogger<ChestsController> _logger;

        public ChestsController(Client c, TRContext ct, ILogger<ChestsController> logger)
        {
            context = ct;
            client = c;
            chestsRepo = new ChestsRepo(client, context);
            _logger = logger;
        }

        // POST api/Chests
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Chest chest)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING CHEST {chest.Name}!");
            //makes sure this chest doesn't already exist in the database
            if (!context.Chests.Any(c => c.Name == chest.Name))
            {
                context.Chests.Add(chest);
                context.SaveChanges();
            }
        }

        //retuns all saved chests in the DB
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all Chests");
            List<Chest> Chests = chestsRepo.GetAllChests();
            return JsonConvert.SerializeObject(Chests, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //Gets chest with provided name
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{chestName}")]
        public string Get(string chestName)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting chest {chestName}");
            Chest chest = chestsRepo.GetChestByName(chestName);
            return JsonConvert.SerializeObject(chest, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Chests/{ChestName}
        [HttpDelete("{chestName}")]
        public void Delete(string chestName)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING CHEST {chestName}!");
            chestsRepo.DeleteChest(chestName);
        }



        [Authorize(Policy = "AdminOnly")]
        // Update: api/Chests
        [HttpPut]
        public void Update([FromBody] Chest chest)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING CHEST {chest.Name}!");
            chestsRepo.UpdateChest(chest);
        }

    }
}
