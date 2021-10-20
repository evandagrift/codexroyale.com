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
        //context to DB and Repo for handling
        private TRContext _context;
        private TeamsRepo _repo;

        //loading in injected dependancies
        public TeamsController(TRContext context)
        {
            _context = context;

            //init the repo with DB context
            _repo = new TeamsRepo(context);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        // POST: api/Teams
        public Team GetSetTeamId([FromBody] Team team)
        {
            Team returnTeam = _repo.GetSetTeamId(team);
            return returnTeam;
        }


        [Authorize(Policy = "AdminOnly")]
        // GET: api/Teams
        [HttpGet]
        public string Get()
        {
            List<Team> teams = _repo.GetAllTeams();

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
            Team team = _repo.GetTeamById(id);
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
            _repo.DeleteTeam(teamid);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Teams
        [HttpPut]
        public void Update([FromBody] Team team)
        {
            _repo.UpdateTeam(team);
        }
    }
}
