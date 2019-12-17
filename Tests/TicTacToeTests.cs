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
    using GameLibrary;
    using NUnit.Framework;
    using Server.Models;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

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
    }
}