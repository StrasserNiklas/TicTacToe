using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary;
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

        // POST: api/Main/players/add
        [HttpPost("players/add", Name = "PostName")]
        public async Task<ActionResult<Player>> PostName([FromBody] string playerName)
        {
            Player player = new Player(playerName);
            var playerInfo = await this.mainService.AddPlayerAsync(player);

            return Ok(playerInfo);
        }

        // POST: api/Main/players/alive
        [HttpGet("players/{id}", Name = "GetAlive")]
        public async Task<ActionResult<IEnumerable<Player>>> GetAlive(int playerId)//([FromBody] int playerId)
        {
            var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.EnemyId == playerId));

            //hier weiter logic

            return Ok(await this.mainService.GetPlayersAsync());
        }

        // POST: api/Main/games/request
        [HttpPost("games/request", Name = "GameRequest")]
        public async Task<IActionResult> GameRequest([FromBody] GameRequest data)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.PlayerId == data.EnemyId);

            if (player != null)
            {
            //    var existingRequestFromRequestingPlayer = this.mainService.RequestedGames.SingleOrDefault(request => (request.EnemyId == data.EnemyId || request.EnemyId == data.RequestPlayerId)
            //&& (request.RequestPlayerId == data.EnemyId || request.RequestPlayerId == data.RequestPlayerId));

                var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.EnemyId == data.EnemyId || request.EnemyId == data.RequestPlayerId)
            && (request.RequestPlayerId == data.EnemyId || request.RequestPlayerId == data.RequestPlayerId));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(new GameRequest(data.EnemyId, data.RequestPlayerId));
                }

                return Ok();
            }

            return NotFound();
        }

        // POST: api/Main/games/request
        [HttpPost("games/status", Name = "CheckForGameStatus")]
        public async Task<ActionResult<GameStatusResponse>> CheckForGameStatus([FromBody] GameRequest data)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());

            var game = games.SingleOrDefault(game => (game.PlayerOne.PlayerId == data.EnemyId || game.PlayerOne.PlayerId == data.RequestPlayerId)
            && (game.PlayerTwo.PlayerId == data.EnemyId || game.PlayerTwo.PlayerId == data.RequestPlayerId));

            if (games.Contains(game))
            {
                var status = new GameStatusResponse(game.CurrentGameStatus, game.CurrentPlayer.PlayerId);
                return Ok(status);
            }

            return NotFound();
        }

        // PUT: api/Main/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //// GET: api/Main/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "valueXD";
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
