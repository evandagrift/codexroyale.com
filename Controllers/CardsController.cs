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
        private CardsRepo repo;

        //loading in injected dependancies
        public CardsController(CustomAuthenticationManager m, TRContext c)
        {
            customAuthenticationManager = m;

            context = c;

            //init the repo with DB context
            repo = new CardsRepo(context);
        }

        [Authorize(Policy = "AdminOnly")]
        // POST api/Cards
        [HttpPost]
        public void Post([FromBody] Card card)
        {
            //adds the recieved card
            repo.AddCardIfNew(card);
        }

        [AllowAnonymous]
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

        [Authorize(Policy = "AdminOnly")]
        // GET api/Cards/

        [HttpGet("{cardID}", Name = "GetCard")]
        public string Get(int cardID)
        {
            Card card = repo.GetCardByID(cardID);
            return JsonConvert.SerializeObject(card, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        [Authorize(Policy = "AdminOnly")]
        // DELETE: api/Cards/{cardID}
        [HttpDelete("{cardID}")]
        public void Delete(int cardID)
        {
            repo.DeleteCard(cardID);
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
