using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {

        //context to DB and Repo for handling
        private TRContext _context;
        private Client _client;
        private CardsRepo _repo;
        private readonly ILogger<CardsController> _logger;

        //loading in injected dependancies
        public CardsController(Client client, TRContext context, ILogger<CardsController> logger)
        {
            _client = client;
            _context = context;
            _logger = logger;

            //init the repo with DB context
            _repo = new CardsRepo(client, context);
            _repo.UpdateCards().Wait();
        }

        [Authorize(Policy = "AdminOnly")]
        // POST api/Cards
        [HttpPost]
        public void Post([FromBody] Card card)
        {
            _logger.LogWarning($"Posting card {card}");

            //adds the recieved card
            _repo.AddCardIfNew(card);
        }

        //[Authorize(Policy = "All")]
        [AllowAnonymous]
        // GET: api/Cards
        [HttpGet]
        public string Get()
        {
            List<Card> cards = _repo.GetAllCards();


            _logger.LogInformation("Getting all Cards");


            return JsonConvert.SerializeObject(cards, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        // GET api/Cards/
        [Authorize(Policy = "AdminOnly")]

        [HttpGet("{id}")]
        public string Get(int id)
        {
            Card card = _repo.GetCardByID(id);

            _logger.LogInformation($"Getting card {id}");

            return JsonConvert.SerializeObject(card, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Cards/{cardID}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarning($"Deleting Card {id}");

            _repo.DeleteCard(id);
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPost("UpdateCards")]
        public IActionResult UpdateCards()
        {
            _logger.LogInformation("Updating cards");

           return Ok(_repo.UpdateCards());
        }



        [Authorize(Policy = "AdminOnly")]
        // Update: api/Cards
        [HttpPut]
        public void Update([FromBody] Card card)
        {

            _logger.LogWarning($"Updating Card {card}");

            _repo.UpdateCard(card);
        }

    }
}
