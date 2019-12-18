//-----------------------------------------------------------------------
// <copyright file="TicTacToeTests.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents some test cases for the game.</summary>
//-----------------------------------------------------------------------

namespace Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GameLibrary;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using Server.Services;

    /// <summary>
    /// Represents the Unit Tests for our solution.
    /// </summary>
    /// <seealso cref="Object" />
    public class TicTacToeTests
    {
        /// <summary>
        /// Setup this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tests the game win.
        /// </summary>
        [Test]
        public void TestGameWin()
        {
            Player player1 = new Player("Niklas");
            Player player2 = new Player("Felix");

            Game game = new Game(player1, player2);
            game.NewGameSetup();

            var finishedAfterMove1 = game.MakeMove(0, player1);
            var finishedAfterMove2 = game.MakeMove(4, player2);
            var finishedAfterMove3 = game.MakeMove(8, player1);
            var finishedAfterMove4 = game.MakeMove(6, player2);
            var finishedAfterMove5 = game.MakeMove(2, player1);
            var finishedAfterMove6 = game.MakeMove(3, player2);
            var finishedAfterMove7 = game.MakeMove(5, player1);

            Assert.IsFalse(finishedAfterMove1);
            Assert.IsFalse(finishedAfterMove2);
            Assert.IsFalse(finishedAfterMove3);
            Assert.IsFalse(finishedAfterMove4);
            Assert.IsFalse(finishedAfterMove5);
            Assert.IsFalse(finishedAfterMove6);

            Assert.IsTrue(finishedAfterMove7);
            Assert.AreEqual(1, player1.Wins);
            Assert.AreEqual(0, player2.Wins);
        }

        /// <summary>
        /// Tests the game draw.
        /// </summary>
        [Test]
        public void TestGameDraw()
        {
            Player player1 = new Player("Niklas");
            Player player2 = new Player("Felix");

            Game game = new Game(player1, player2);
            game.NewGameSetup();

            var finishedAfterMove1 = game.MakeMove(4, player1);
            var finishedAfterMove2 = game.MakeMove(1, player2);
            var finishedAfterMove3 = game.MakeMove(2, player1);
            var finishedAfterMove4 = game.MakeMove(6, player2);
            var finishedAfterMove5 = game.MakeMove(3, player1);
            var finishedAfterMove6 = game.MakeMove(5, player2);
            var finishedAfterMove7 = game.MakeMove(8, player1);
            var finishedAfterMove8 = game.MakeMove(0, player2);
            var finishedAfterMove9 = game.MakeMove(7, player1);

            Assert.IsFalse(finishedAfterMove1);
            Assert.IsFalse(finishedAfterMove2);
            Assert.IsFalse(finishedAfterMove3);
            Assert.IsFalse(finishedAfterMove4);
            Assert.IsFalse(finishedAfterMove5);
            Assert.IsFalse(finishedAfterMove6);
            Assert.IsFalse(finishedAfterMove7);
            Assert.IsFalse(finishedAfterMove8);

            Assert.IsTrue(finishedAfterMove9);

            Assert.AreEqual(0, player1.Wins);
            Assert.AreEqual(0, player2.Wins);
        }

        /// <summary>
        /// Tests the service add player method.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Add_Player()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);
            var player = new Player();

            var playerReturned = await service.AddPlayerAsync(player);

            Assert.AreEqual(player, playerReturned);
            var list = service.GetPlayersNotInGameAsync().Result.ToList();
            Assert.Contains(playerReturned, list);
        }

        /// <summary>
        /// Tests the service add game request.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Add_Request()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player("Player1");
            Player player2 = new Player("Player2");

            GameRequest request = new GameRequest(player1, player2);
            await service.AddGameRequestAsync(request);

            var requestList = service.GetGameRequestsAsync().Result.ToList();
            Assert.Contains(request, requestList);
        }

        /// <summary>
        /// Tests the get game requests method.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Get_Requests()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player("Felix");
            Player player2 = new Player("Niklas");
            Player player3 = new Player("Yannick");
            Player player4 = new Player("Emanuel");

            GameRequest request1 = new GameRequest(player1, player2);
            GameRequest request2 = new GameRequest(player3, player4);
            await service.AddGameRequestAsync(request1);
            await service.AddGameRequestAsync(request2);

            var requestList = service.GetGameRequestsAsync().Result.ToList();
            Assert.Contains(request1, requestList);
            Assert.Contains(request2, requestList);
            Assert.AreEqual(2, requestList.Count);
        }

        /// <summary>
        /// Tests the service remove request.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Remove_Request()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player("Player1");
            Player player2 = new Player("Player2");
            var request = new GameRequest(player1, player2);
            var addedRequest = await service.AddGameRequestAsync(request);

            var removedRequest = await service.RemoveRequestAsync(addedRequest, false);

            Assert.AreEqual(addedRequest, removedRequest);
        }

        /// <summary>
        /// Tests the service get players not in game. 
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Get_Players_Not_In_Game()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player();
            Player player2 = new Player();
            Player player3 = new Player();
            Player player4 = new Player();
            Player player5 = new Player();
            Player player6 = new Player();

            player1.ConnectionId = "1";
            player2.ConnectionId = "2";
            player3.ConnectionId = "3";
            player4.ConnectionId = "4";
            player5.ConnectionId = "5";
            player6.ConnectionId = "6";

            await service.AddPlayerAsync(player1);
            await service.AddPlayerAsync(player2);
            await service.AddPlayerAsync(player3);
            await service.AddPlayerAsync(player4);
            await service.AddPlayerAsync(player5);
            await service.AddPlayerAsync(player6);

            Game game1 = new Game(player1, player2);
            await service.AddGameAsync(game1);

            Game game2 = new Game(player3, player5);
            await service.AddGameAsync(game2);

            var playersNotInGame = service.GetPlayersNotInGameAsync().Result.ToList();
            Assert.AreEqual(2, playersNotInGame.Count);
        }

        /// <summary>
        /// Tests the service get games method.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Get_Games()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player();
            Player player2 = new Player();

            Game game1 = new Game(player1, player2);
            Game game2 = new Game(player1, player2);

            await service.AddGameAsync(game1);
            await service.AddGameAsync(game2);

            var gameList = service.GetGamesAsync().Result.ToList();
            Assert.AreEqual(2, gameList.Count);
        }

        /// <summary>
        /// Tests the service get simple game information list.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Get_Simple_Game_Information_List()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player("Yannicka");
            Player player2 = new Player("Felarius");
            Game game1 = new Game(player1, player2);
            await service.AddGameAsync(game1);

            var simpleGameInfo = service.GetSimpleGameInformationListAsync().Result;

            Assert.That(() => simpleGameInfo.First().PlayerOne == player1.PlayerName);
            Assert.That(() => simpleGameInfo.First().PlayerTwo == player2.PlayerName);
        }

        /// <summary>
        /// Tests the service remove game.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Remove_Game()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player();
            Player player2 = new Player();

            Game game1 = new Game(player1, player2);
            Game game2 = new Game(player1, player2);

            await service.AddGameAsync(game1);
            await service.AddGameAsync(game2);

            var gameList = service.GetGamesAsync().Result.ToList();
            Assert.AreEqual(2, gameList.Count);

            await service.RemoveGameAsync(game2);
            gameList = service.GetGamesAsync().Result.ToList();

            Assert.IsFalse(gameList.Contains(game2));
        }

        /// <summary>
        /// Tests the service remove player.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Remove_Player()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player();
            Player player2 = new Player();

            await service.AddPlayerAsync(player1);
            await service.AddPlayerAsync(player2);

            var playerList = service.GetPlayersAsync().Result.ToList();
            Assert.AreEqual(2, playerList.Count);

            await service.RemovePlayerAsync(player2);
            playerList = service.GetPlayersAsync().Result.ToList();

            Assert.IsFalse(playerList.Contains(player2));
        }

        /// <summary>
        /// Tests the service add game.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Add_Game()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);

            Player player1 = new Player();
            Player player2 = new Player();

            Game game1 = new Game(player1, player2);
            await service.AddGameAsync(game1);

            var gameList = service.GetGamesAsync().Result.ToList();
            Assert.Contains(game1, gameList);
        }

        /// <summary>
        /// Tests the service get players.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        [Test]
        public async Task Test_Service_Get_Players()
        {
            var mock = new Mock<ILogger<MainService>>();
            var service = new MainService(mock.Object);
            var player1 = new Player("Felix");
            var player2 = new Player("Niklas");
            var player3 = new Player("Yannick");

            var player1Returned = await service.AddPlayerAsync(player1);
            var player2Returned = await service.AddPlayerAsync(player2);
            var player3Returned = await service.AddPlayerAsync(player3);

            var players = service.GetPlayersAsync().Result.ToList();

            Assert.Contains(player1Returned, players);
            Assert.Contains(player2Returned, players);
            Assert.Contains(player3Returned, players);
            Assert.AreEqual(3, players.Count);
        }
    }   
}