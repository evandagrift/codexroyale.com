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
    public class DecksController : ControllerBase
    {

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private DecksRepo repo;

        //loading in injected dependancies
        public DecksController(TRContext context, Client client)
        {
            _context = context;
            _client = client;

            //init the repo with DB context
            repo = new DecksRepo(client, context);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        // POST api/Decks
        public Deck GetDeckWithId([FromBody] Deck deck)
        {
            //gets deck with the given deck id
            Deck returnDeck = repo.GetDeckWithId(deck);

            //returns the deck to the end user
            return returnDeck;
        }

        [Authorize(Policy = "AdminOnly")]
        // GET: api/Decks
        [HttpGet]
        public string Get()
        {
            //fetches all decks in the DB
            List<Deck> decks = repo.GetAllDecks();

            //returns the list to the end user
            return JsonConvert.SerializeObject(decks, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }
        [AllowAnonymous]
        // GET api/Decks/id
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //gets deck by Id
            Deck deck = repo.GetDeckByID(id);

            //returns fetched deck
            return JsonConvert.SerializeObject(deck, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }



        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Decks/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //deletes deck at given Id
            repo.DeleteDeck(id);
        }

        [Authorize(Policy = "AdminOnly")]
        // Update: api/Decks
        [HttpPut]
        public void Update([FromBody] Deck deck)
        {
            //updates the deck with given Id
            repo.UpdateDeck(deck);
        }

    }
}
