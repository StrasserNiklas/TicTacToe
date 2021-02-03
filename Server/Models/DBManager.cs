using Dapper;
using GameLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class DBManager
    {
        public void UpdatePlayerWins(int userId)
        {
            using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            var query = db.Query<PlayerData>($"SELECT wins FROM Scores WHERE userID = {userId}").ToList();

            if (query.Count() == 0)
            {
                db.Execute("INSERT INTO Scores (userID, wins) VALUES (@userId, 1)", new { userId });
            }
            else
            {
                var newWins = query.First().Wins + 1;
                db.Execute($"UPDATE Scores SET wins = {newWins} WHERE userID = {userId}");
            }
        }

        public List<PlayerData> GetLeaderBoardData()
        {
            using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            var query = db.Query<PlayerData>($"SELECT u.username, s.wins FROM Users u INNER JOIN Scores s ON u.Id = s.userID").ToList();

            return query;
        }

        //public ApiResponse AddNewPlayer(string username, string passwordHash)
        //{
        //    using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

        //    if (db.State == ConnectionState.Closed)
        //    {
        //        db.Open();
        //    }

        //    var query = db.Query<User>($"select * from Users where username='{username}'");

        //    if (query.Count() >= 1)
        //    {
        //        return new ApiResponse(true, "", 0);
        //    }

        //    var id = db.QuerySingle<int>("Insert into Users (username, password) OUTPUT INSERTED.Id VALUES (@username, @passwordHash)", new { username, passwordHash });

        //    return new ApiResponse(false, "", id);
        //}

        //public ApiResponse CheckLogin(string username, string passwordHash)
        //{
        //    using IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ticData"].ConnectionString);

        //    if (db.State == ConnectionState.Closed)
        //    {
        //        db.Open();
        //    }

        //    var query = db.Query<User>($"select * from Users where username='{username}'").ToList();

        //    if (query.Count() == 0)
        //    {
        //        return new ApiResponse(true, "", 0);
        //    }

        //    if (query.Count() == 1)
        //    {
        //        if (query.First().Password != passwordHash)
        //        {
        //            return new ApiResponse(true, "", 0);
        //        }
        //    }

        //    return new ApiResponse(false, "", query.First().Id); 
        //}


    }
}
