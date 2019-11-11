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

        public MainController(IMainService main)
        {
            this.mainService = main;
        }

        // GET: api/Main/players
        [HttpGet]
        public Task<IEnumerable<Player>> Get()
        {
            return this.mainService.GetPlayersAsync();
        }

        // GET: api/Main/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "valueXD";
        }

        // POST: api/Main/players
        [HttpPost("players/add", Name = "PostName")]
        public async Task<ActionResult<Player>> PostName([FromBody] string playerName)
        {
            Player player = new Player(playerName);
            var playerInfo = await this.mainService.AddPlayerAsync(player);

            return Ok(playerInfo);
        }

        // POST: api/Main/players
        [HttpPost("players/alive", Name = "PostAlive")]
        public async Task<ActionResult<IEnumerable<Player>>> PostAlive([FromBody] int playerId)
        {
            return Ok(await this.mainService.GetPlayersAsync());
        }

        // POST: api/Main/players
        [HttpPost("games/request", Name = "GameRequest")]
        public async Task<IActionResult> GameRequest([FromBody] int playerID)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.PlayerId == playerID);

            if (player == null)
            {
                return NotFound();
            }

            return Ok();
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
