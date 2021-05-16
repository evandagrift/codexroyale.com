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
    [ApiController, Route("api/[controller]"), Authorize]
    public class UsersController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private Client client;
        private UsersRepo usersRepo;
        private PlayersRepo playersRepo;

        //loading in injected dependancies
        public UsersController(CustomAuthenticationManager m, Client c,  TRContext ct)
        {
            customAuthenticationManager = m;
            client = c;
            context = ct;


            //init the repo with DB context
            usersRepo = new UsersRepo(client,context);
            playersRepo = new PlayersRepo(client, context);


        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult CreateAccount([FromBody] User user)
        {
            user = customAuthenticationManager.CreateAccount(user, usersRepo, context);
            //return result based off of what is null
            if (user != null)
            {
                return Ok(user);
            }
            else return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {

            User fetchedUser = customAuthenticationManager.Login(user, context);

            if (fetchedUser != null)
            {
                return Ok(fetchedUser);
            }
            else return Unauthorized();
        }


        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody] User user)
        //{
        //    User fetchedUser = customAuthenticationManager.Authenticate(user, context);
        //    Console.WriteLine("USER CHECKED");
        //    return Ok(fetchedUser);
        //}

        // POST api/<CardController>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void PostUser([FromBody] User user)
        {
            usersRepo.AddUser(user);
        }

        //[AllowAnonymous]
        [Authorize(Policy = "AdminOnly")]
        // GET: api/<NameController>
        [HttpGet]
        public string GetUsers()
        {
            List<User> users = usersRepo.GetAllUsers();

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
            User user = usersRepo.GetUserByUsername(username);
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
            usersRepo.DeleteUser(username);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void UpdateUser([FromBody] User user)
        {
            usersRepo.UpdateUser(user);
        }

    }
}
