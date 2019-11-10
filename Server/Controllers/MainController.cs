using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMainService mainService;
        private int incrementedId;

        public MainController(IMainService main)
        {
            this.mainService = main;
            this.incrementedId = 0;
        }


        // GET: api/Main
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Main/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "valueXD";
        }

        // POST: api/Main/players
        [HttpPost("players/add", Name = "Post")]
        public async Task<ActionResult<List<Player>>> Post([FromBody] string playerName)
        {
            Player player = new Player(playerName, this.incrementedId);
            await this.mainService.AddPlayer(player);


            this.incrementedId++;

            return Ok(this.mainService.GetPlayers());
        }

        // PUT: api/Main/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
