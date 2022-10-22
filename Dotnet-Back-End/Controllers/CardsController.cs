using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using Microsoft.Extensions.Logging;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoyaleTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private TRContext _context;
        private Client _client;
        private CardsRepo _repo;
        private ILogger<CardsController> _logger;

        public CardsController(Client client, TRContext context, ILogger<CardsController> logger)
        {
            _client = client;
            _context = context;
            _logger = logger;

            //init the repo with DB context
            _repo = new CardsRepo(client, context);
        }

        //adds the recieved card
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public void Post([FromBody] Card card)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} POSTING CARD {card.Id}!");
            _repo.AddCardIfNew(card);
        }

        //Gets all cards in DB
        [AllowAnonymous]
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting all Cards");
            List<Card> cards = _repo.GetAllCards();
            return JsonConvert.SerializeObject(cards, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //Gets card w/ provided Id
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} Getting card {id}");
            Card card = _repo.GetCardByID(id);
            return JsonConvert.SerializeObject(card, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        //Delete card with given Id
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} DELETING CARD {id}!");
            _repo.DeleteCard(id);
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPost("UpdateCards")]
        public IActionResult UpdateCards()
        {
            _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} Auto updating cards");
           return Ok(_repo.UpdateCards());
        }


        /* update commented out until properly tested */

        //Updates card to given details at provided card id from the Card class
        //[Authorize(Policy = "AdminOnly")]
        //[HttpPut]
        //public void Update([FromBody] Card card)
        //{

        //    _logger.LogWarning($"{Request.HttpContext.Connection.RemoteIpAddress} UPDATING CARD {card.Id}!");
        //    _repo.UpdateCard(card);
        //}

    }
}
