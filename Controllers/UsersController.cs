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
    public class UsersController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly ICustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private UsersRepo repo;

        //loading in injected dependancies
        public UsersController(ICustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;
            // commented out while testing 
            context = c;

            //init the repo with DB context
            repo = new UsersRepo(context);

        }
        // POST api/<CardController>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void PostUser([FromBody] User user)
        {
            repo.AddUser(user);
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/<NameController>
        [HttpGet]
        public string GetUsers()
        {
            List<User> users = repo.GetAllUsers();

            return JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        [Authorize(Policy = "AdminOnly")]
        // GET api/<NameController>/5
        [HttpGet("{username}", Name = "GetUser")]
        public string GetUser(string username)
        {
            User user = repo.GetUserByUsername(username);
            return JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpDelete("{username}")]
        public void DeleteUser(string username)
        {
            repo.DeleteUser(username);
        }

        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Products/5
        [HttpPut]
        public void UpdateUser([FromBody] User user)
        {
            repo.UpdateUser(user);
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User userCred)
        {
            userCred.Role = "Admin";
            context.Users.Add(userCred);
            context.SaveChanges();

            var token = customAuthenticationManager.Authenticate(userCred.Username, userCred.Password, context);

            if(token == null)
                return Unauthorized();
            

            return Ok(token);
        }
    }
}
