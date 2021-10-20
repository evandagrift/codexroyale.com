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
using RoyaleTrackerAPI.Models.Email;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class UsersController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private CustomAuthenticationManager _customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private UsersRepo _usersRepo;
        private PlayersRepo _playersRepo;
        private EmailSender _emailSender;

        //loading in injected dependancies
        public UsersController(CustomAuthenticationManager customAuthenticationManager, Client client, TRContext ct, EmailSender emailSender)
        {
            _customAuthenticationManager = customAuthenticationManager;
            _client = client;
            _context = ct;
            _emailSender = emailSender;

            //init the repo with DB context
            _usersRepo = new UsersRepo(_client, _context);
            _playersRepo = new PlayersRepo(_client, _context);
        }

        [Authorize(Policy = "All")]
        [HttpPost("Update")]
        public IActionResult UpdatePlayerSetting([FromBody] JObject recievedJson)
        {
            Console.WriteLine();
           User user = recievedJson["user"].ToObject<User>();
           string newPassword = recievedJson["newPassword"].ToString();
            return Ok(_customAuthenticationManager.UpdateUserSetting(user, newPassword, _context,_client));
        }


        [AllowAnonymous]
        [HttpPost("Signup")]
        public IActionResult CreateAccount([FromBody] User user)
        {
            //tries to create account, if created returns true
            string signupResult = _customAuthenticationManager.CreateAccount(user, _emailSender, _usersRepo, _context);

            return Ok(signupResult);
        }



        //verifies email verification code
        [AllowAnonymous]
        [HttpPost("VerifyAccount/{verificationCode}")]
        public IActionResult VerifyAccount(string verificationCode)
        {
            //if the verification code is a valid one it sets the connect user to being verified.
            //it then returns a sanitized instance of the user to the client
            return Ok(_customAuthenticationManager.VerifyUser(verificationCode, _usersRepo, _context));
        }



        //verifies email verification code
        [AllowAnonymous]
        [HttpPost("ResetPassword/{email}")]
        public IActionResult ResetPassword(string email)
        {
            //if the verification code is a valid one it sets the connect user to being verified.
            //it then returns a sanitized instance of the user to the client
            return Ok(_customAuthenticationManager.SendPasswordReset(email, _usersRepo, _context, _emailSender));
        }



        //verifies email verification code
        [AllowAnonymous]
        [HttpPost("ResetPassword/Authenticate")]
        public IActionResult ResetPassword([FromBody] User user)
        {
            //if the verification code is a valid one it sets the connect user to being verified.
            //it then returns a sanitized instance of the user to the client
            return Ok(_customAuthenticationManager.ResetUserPassword(user.Password, user.PasswordResetCode, _usersRepo, _context));
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {

            User fetchedUser = _customAuthenticationManager.Login(user, _context);

            if (fetchedUser != null)
            {
                return Ok(fetchedUser);
            }
            else return Unauthorized();
        }



        // POST api/Users
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void PostUser([FromBody] User user)
        {
            _usersRepo.AddUser(user);
        }



        [Authorize(Policy = "AdminOnly")]
        // GET: api/Users
        [HttpGet]
        public string GetUsers()
        {
            List<User> users = _usersRepo.GetAllUsers();

            return JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }



        [Authorize(Policy = "AdminOnly")]
        // GET api/Users/username
        [HttpGet("{username}")]
        public string GetUser(string username)
        {
            User user = _usersRepo.GetUserByUsername(username);
            return JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }



        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Users/username
        [HttpDelete("{username}")]
        public void DeleteUser(string username)
        {
            _usersRepo.DeleteUser(username);
        }




        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            _usersRepo.UpdateUser(user);
            return Ok();
        }

    }



}
