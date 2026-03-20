using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Repos;
using CodexRoyaleClassesCore3.Models.Email;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class UsersController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private CustomAuthenticationManager _customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private UsersRepo _usersRepo;
        private PlayerSnapshotRepo _playerSnapshotRepo;
        private EmailSender _emailSender;
        private ILogger<UsersController> _logger;
        

        //loading in injected dependancies
        public UsersController(CustomAuthenticationManager customAuthenticationManager, Client client, TRContext ct, EmailSender emailSender, ILogger<UsersController> logger)
        {
            _customAuthenticationManager = customAuthenticationManager;
            _client = client;
            _context = ct;
            _emailSender = emailSender;
            _logger = logger;
            
            //init the repo with DB context
            _usersRepo = new UsersRepo(_client, _context);
            _playerSnapshotRepo = new PlayerSnapshotRepo(_client, _context);
        }

        //creates an account and sends email confirmation
        [AllowAnonymous]
        [HttpPost("Signup")]
        public IActionResult CreateAccount([FromBody] User user)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Signing up with username {user.Username}");
            //tries to create account, if created returns true
            string signupResult = _customAuthenticationManager.CreateAccount(user, _emailSender, _usersRepo, _context);
            return Ok(signupResult);
        }

        //logs in user returning user with bearer token if credentials are correct
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} logging in user {user.Username}");
            //if the login is correct it will return the user sanatized with their bearer token
            User fetchedUser = _customAuthenticationManager.Login(user, _context);
            if (fetchedUser != null) { return Ok(fetchedUser); }
                else return Unauthorized();
        }

        //gets all users in the DB
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string GetUsers()
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} getting all saved users in db!");
            List<User> users = _usersRepo.GetAllUsers(); 
            return JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //saves provided user to the db
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void PostUser([FromBody] User user)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING USER {user.Username}!");
            _usersRepo.AddUser(user);
        }
        
        //gets user with provided username
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{username}")]
        public string GetUser(string username)
        {
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting user {username}");
            User user = _usersRepo.GetUserByUsername(username);
            return JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //deletes user with given username
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{username}")]
        public void DeleteUser(string username)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING USER {username}!");
            _usersRepo.DeleteUser(username);
        }

        //Updates user with given username
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            //_logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING USER {user.Username}!");
            _usersRepo.UpdateUser(user);
            return Ok();
        }

        //verifies email verification code
        [AllowAnonymous]
        [HttpPost("VerifyAccount/{verificationCode}")]
        public IActionResult VerifyAccount(string verificationCode)
        {
            //if the verification code is a valid one it sets the connected user to being verified.
            //it then returns a sanitized instance of the user to the client
            User returnUser = _customAuthenticationManager.VerifyUser(verificationCode, _usersRepo, _context);
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} verified their account {returnUser.Username}");
            return Ok(returnUser);
        }

        //Sends password reset email
        [AllowAnonymous]
        [HttpPost("ResetPassword/{email}")]
        public IActionResult ResetPassword(string email)
        {
            //if the verification code is a valid one it sets the connect user to being verified.
            //it then returns a sanitized instance of the user to the client
            string username = _customAuthenticationManager.SendPasswordReset(email, _usersRepo, _context, _emailSender);
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} sending password reset for {username}");
            return Ok(username);
        }

        //verifies email verification code
        [AllowAnonymous]
        [HttpPost("ResetPassword/Authenticate")]
        public IActionResult ResetPassword([FromBody] User user)
        {
            //if the verification code is a valid one it sets the connect user to being verified.
            //it then returns a sanitized instance of the user to the client'
            User returnUser = _customAuthenticationManager.ResetUserPassword(user.Password, user.PasswordResetCode, _usersRepo, _context);
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} verefied email for {returnUser.Username}");
            return Ok(returnUser);
        }

        //updates given users data
        [Authorize(Policy = "All")]
        [HttpPost("Update")]
        public IActionResult UpdatePlayerSetting([FromBody] JObject recievedJson)
        {
            User user = recievedJson["user"].ToObject<User>();
            //_logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} updating {user.Username} user settings");
            string newPassword = recievedJson["newPassword"].ToString();
            return Ok(_customAuthenticationManager.UpdateUserSetting(user, newPassword, _context, _client));
        }
    }
}
