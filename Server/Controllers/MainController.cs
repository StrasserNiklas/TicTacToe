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
        public async Task<ActionResult<PlayerServerStatus>> GetAlive(int id)//([FromBody] int playerId)
        {
            var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => request.RequestPlayer.PlayerId == id || request.Enemy.PlayerId == id);

            var list = new List<Player>(await this.mainService.GetPlayersAsync());

            var status = new PlayerServerStatus(list);

            if (existingRequest != null && existingRequest.Declined == false)
            {
                status.RequestingPlayer = existingRequest.RequestPlayer;
                status.RequestID = existingRequest.RequestID;
            }
            // if a request has been declined, remove it and notify the player if he was the one requesting it
            else if (existingRequest != null && existingRequest.Declined)
            {
                var player = list.SingleOrDefault(i => i.PlayerId == id);

                if (player != null && player.PlayerId == existingRequest.RequestPlayer.PlayerId)
                {
                    await this.mainService.RemoveRequestAsync(existingRequest);

                    status.StatusMessage = $"{existingRequest.Enemy.PlayerName} has declined.";
                }

                //now notify player pls :)))
            }

            return Ok(status);
        }

        // POST: api/Main/games/request
        [HttpPost("games/request", Name = "GameRequest")]
        public async Task<IActionResult> GameRequest([FromBody] GameRequest data)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.PlayerId == data.Enemy.PlayerId);

            if (player != null)
            {
            //    var existingRequestFromRequestingPlayer = this.mainService.RequestedGames.SingleOrDefault(request => (request.EnemyId == data.EnemyId || request.EnemyId == data.RequestPlayerId)
            //&& (request.RequestPlayerId == data.EnemyId || request.RequestPlayerId == data.RequestPlayerId));

                var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => (request.Enemy == data.Enemy || request.Enemy == data.RequestPlayer)
            && (request.RequestPlayer == data.Enemy || request.RequestPlayer == data.RequestPlayer));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(new GameRequest(data.Enemy, data.RequestPlayer));
                }

                return Ok();
            }

            return NotFound();
        }

        //this should be a Get based on a game id
        // POST: api/Main/games/request
        [HttpPost("games/status", Name = "CheckForGameStatus")]
        public async Task<ActionResult<GameStatusResponse>> CheckForGameStatus([FromBody] GameRequest data)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());

            var game = games.SingleOrDefault(game => (game.PlayerOne.PlayerId == data.Enemy.PlayerId || game.PlayerOne.PlayerId == data.RequestPlayer.PlayerId)
            && (game.PlayerTwo.PlayerId == data.Enemy.PlayerId || game.PlayerTwo.PlayerId == data.RequestPlayer.PlayerId));

            if (games.Contains(game))
            {
                var status = new GameStatusResponse(game.CurrentGameStatus, game.CurrentPlayer.PlayerId);
                return Ok(status);
            }

            return NotFound();
        }

        // PUT: api/Main/games/request
        [HttpPut("games/request/{id}", Name = "DeclineRequest")]
        public async Task<IActionResult> DeclineRequest(int id, [FromBody] bool accept)
        {
            var existingRequest = new List<GameRequest>(await this.mainService.GetGameRequestsAsync()).SingleOrDefault(request => request.RequestID == id );

            if (existingRequest != null)
            {
                if (!accept)
                {
                    existingRequest.Declined = true;
                    //await this.mainService.RemoveRequestAsync(existingRequest);
                    return Ok();
                }
                else
                {
                    existingRequest.Accepted = true;
                    return Ok();
                }
            }

            return NotFound();
        }

        // PUT: api/Main/games/request
        //[HttpPut("games/request/{id}")]
        //public void Put(int id, [FromBody] string value)
        //{

        //}

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
