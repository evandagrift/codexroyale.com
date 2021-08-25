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
    public class TeamsController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private TeamsRepo repo;

        //loading in injected dependancies
        public TeamsController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new TeamsRepo(context);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        // POST: api/Teams
        public Team GetSetTeamId([FromBody] Team team)
        {
            Team returnTeam = repo.GetSetTeamId(team);
            return returnTeam;
        }


        [Authorize(Policy = "AdminOnly")]
        // GET: api/Teams
        [HttpGet]
        public string Get()
        {
            List<Team> teams = repo.GetAllTeams();

            return JsonConvert.SerializeObject(teams, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/Teams/id
        [HttpGet("{id}")]
        public string Get(int id)
        {
            Team team = repo.GetTeamById(id);
            return JsonConvert.SerializeObject(team, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Teams/{teamTag}
        [HttpDelete("{teamid}")]
        public void Delete(int teamid)
        {
            repo.DeleteTeam(teamid);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Teams
        [HttpPut]
        public void Update([FromBody] Team team)
        {
            repo.UpdateTeam(team);
        }
    }
}
