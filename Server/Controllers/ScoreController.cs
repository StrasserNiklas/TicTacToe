using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GameLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{

    [ApiController]
    public class ScoreController : ControllerBase
    {
        [HttpGet]
        [Route("api/wins")]
        public ActionResult<PlayerData> GetLeaderboard()
        {
            var dbManager = new DBManager();

            return Ok(dbManager.GetLeaderBoardData());
        }
    }
}
