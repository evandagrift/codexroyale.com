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
    public class CardsController : ControllerBase
    {
        //Authentication Manager for handling Bearer Token
        private readonly CustomAuthenticationManager customAuthenticationManager;

        //context to DB and Repo for handling
        private TRContext context;
        private Client client;
        private CardsRepo repo;

        //loading in injected dependancies
        public CardsController(CustomAuthenticationManager m, Client c, TRContext ct)
        {
            customAuthenticationManager = m;
            client = c;
            context = ct;

            //init the repo with DB context
            repo = new CardsRepo(client, context);
        }

        [Authorize(Policy = "AdminOnly")]
        // POST api/Cards
        [HttpPost]
        public void Post([FromBody] Card card)
        {
            //adds the recieved card
            repo.AddCardIfNew(card);
        }

        [Authorize(Policy = "All")]
        // GET: api/Cards
        [HttpGet]
        public string Get()
        {
            List<Card> cards = repo.GetAllCards();

            return JsonConvert.SerializeObject(cards, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        // GET api/Cards/
        [Authorize(Policy = "AdminOnly")]

        [HttpGet]
        public string Get([FromHeader] int id)
        {
            Card card = repo.GetCardByID(id);
            return JsonConvert.SerializeObject(card, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Cards/{cardID}
        [HttpDelete]
        public void Delete([FromHeader] int id)
        {
            repo.DeleteCard(id);
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPost("UpdateCards")]
        public void UpdateCards()
        {
            repo.UpdateCards();
        }



        [Authorize(Policy = "AdminOnly")]
        // Update: api/Cards
        [HttpPut]
        public void Update([FromBody] Card card)
        {
            repo.UpdateCard(card);
        }

    }
}
