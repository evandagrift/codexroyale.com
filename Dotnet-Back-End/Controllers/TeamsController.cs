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
    public class TeamsController : ControllerBase
    {
        //context to DB and Repo for handling
        private TRContext _context;
        private TeamsRepo _repo;
        private ILogger<TeamsController> _logger;
        

        //loading in injected dependancies
        public TeamsController(TRContext context, ILogger<TeamsController> logger)
        {
            _context = context;
            _logger = logger;
            

            //init the repo with DB context
            _repo = new TeamsRepo(context);
        }

        //Gets the provided team with an assigned Id
        //if the team doesn't exist already it creates it before returning
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public Team GetSetTeamId([FromBody] Team team)
        {
            Team returnTeam = _repo.GetSetTeamId(team);
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} getting team with an assigned Id {returnTeam.TeamId}!");
            return returnTeam;
        }


        //gets the players upcoming chests that they will unlock
        [AllowAnonymous]
        [HttpGet("playertag/{id}")]
        public async Task<IActionResult> GetTeamPlayerOneTag(int id)
        {
            return Ok(JsonConvert.SerializeObject(_repo.GetTeamById(id).Tag, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }


        //gets all team saved in the db
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string Get()
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all saved teams");
            List<Team> teams = _repo.GetAllTeams();
            return JsonConvert.SerializeObject(teams, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

        }

        //gets team with provided Id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting Team with Id {id}");
            Team team = _repo.GetTeamById(id);
            return JsonConvert.SerializeObject(team, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Teams/{teamTag}
        [HttpDelete("{teamid}")]
        public void Delete(int id)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING TEAM {id}!");
            _repo.DeleteTeam(id);
        }

        //Updates the team with the given TeamId
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] Team team)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING TEAM {team.TeamId}!");
            _repo.UpdateTeam(team);
        }
    }
}
