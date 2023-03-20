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
    [Route("api/[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private DecksRepo repo;
        private ILogger<DecksController> _logger;
        

        //loading in injected dependancies
        public DecksController(TRContext context, Client client, ILogger<DecksController> logger)
        {
            _context = context;
            _client = client;
            _logger = logger;

            //init the repo with DB context
            repo = new DecksRepo(client, context);
        }

        //gets deck with the given deck id
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public Deck GetDeckWithId([FromBody] Deck deck)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting deck {deck.Id}");
            return repo.GetDeckWithId(deck);
        }

        //fetches all decks in the DB
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all decks");
            List<Deck> decks = repo.GetAllDecks();
            return JsonConvert.SerializeObject(decks, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //gets deck with given id
        [AllowAnonymous]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} getting deck {id}");
            Deck deck = repo.GetDeckByID(id);
            return JsonConvert.SerializeObject(deck, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }


        //deletes deck with given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING DECK {id}");
            repo.DeleteDeck(id);
        }

        //updates the deck with Id provided by Deck argument
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public void Update([FromBody] Deck deck)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING DECK {deck.Id}!"); ;
            repo.UpdateDeck(deck);
        }

    }
}
