using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Repos;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ClansController : ControllerBase
    {
        
        //context to DB and Repo for handling
        private TRContext context;
        private Client client;
        private ClansRepo repo;
        private ILogger<ClansController> _logger;
        

        //loading in injected dependancies
        public ClansController(Client c, TRContext ct, ILogger<ClansController> logger)
        {
            context = ct;
            client = c;
            _logger = logger;

            //init the repo with DB context
            repo = new ClansRepo(client, context);
        }

        //Posts snapshot of clan data
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Clan clan)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING CLAN SNAPSHOT {clan.Tag}!");
            repo.AddClan(clan);
        }

        //getting all saved clan snapshots
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string Get()
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all Clan Snapshots");
            List<Clan> clans = repo.GetAllClans();
            return JsonConvert.SerializeObject(clans, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //gets specific clan snapshot with given id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("id/{id}")]
        public string Get(int id)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting Clan Snapshot w/ id:{id}");
            Clan clan = repo.GetClanById(id);
            return JsonConvert.SerializeObject(clan, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }
         
        //Gets current clan data and saves it if it's new
        [AllowAnonymous]
        [HttpGet("{tag}")]
        public string Get(string tag)
        {
            Clan clan = repo.GetSiteClan(tag).Result;
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting {tag}'s clan {clan.Tag}'s current data");
            return JsonConvert.SerializeObject(clan, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        } 

        //Deleting clan snapshot at given Id-
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING CLAN SNAPSHOT {id}!");
            repo.DeleteClan(id);
        }

        //updates clan snapshot at id provided in clan class
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] Clan clan)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING CLAN SNAPSHOT {clan.Id}");
            repo.UpdateClan(clan);
        }

    }
}
