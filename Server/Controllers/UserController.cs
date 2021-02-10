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
using Server.Authentication;

namespace Server.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public UserController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        [Route("api/users/login")]
        public ActionResult<ApiResponse> CheckLogin([FromBody] User user)
        {
            using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            //test
            //var username = user.Username.ToString() + "test";
            //var passwordHash = user.Password.ToString() + "test";
            //var id = db.QuerySingle<int>("Insert into Users (username, password) OUTPUT INSERTED.Id VALUES (@username, @passwordHash)", new { username, passwordHash });

            var query = db.Query<User>($"select * from Users where username='{user.Username}'").ToList();

            if (query.Count() == 0)
            {
                return NotFound();
            }

            if (query.Count() == 1)
            {
                if (query.First().Password != user.Password)
                {
                    return BadRequest();
                }
            }

            var token = this.jwtAuthenticationManager.Authenticate(user.Username, user.Password);

            return Ok(new ApiResponse(true, "", query.First().Id, token));
            //return Ok(new ApiResponse(true, "", 0, token));
        }

        [HttpPost]
        [Route("api/users/add")]
        public ActionResult<ApiResponse> AddPlayer([FromBody] User user)
        {
            using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            var query = db.Query<User>($"select * from Users where username='{user.Username}'");

            if (query.Count() >= 1)
            {
                return BadRequest();
            }

            string username = user.Username;
            string passwordHash = user.Password;

            var id = db.QuerySingle<int>("Insert into Users (username, password) OUTPUT INSERTED.Id VALUES (@username, @passwordHash)", new { username, passwordHash });

            var token = this.jwtAuthenticationManager.Authenticate(user.Username, user.Password);

            return Ok(new ApiResponse(true, "", id, token));
            //return Ok(new ApiResponse(true, "", 0, token));
        }
    }
}
