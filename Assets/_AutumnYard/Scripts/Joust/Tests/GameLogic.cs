using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AutumnYard.Joust.Tests
{
    public class GameLogic
    {
        [Test]
        public void ExampleGame1()
        {
            // Use the Assert class to test conditions
            var game = new Joust();
            game.Print();

            game.SetRound(Player.A, 0, Piece.Attack);
            game.SetRound(Player.A, 1, Piece.Parry);
            game.SetRound(Player.A, 2, Piece.Defense);
            game.SetRound(Player.B, 0, Piece.Defense);
            game.SetRound(Player.B, 1, Piece.Attack);
            game.SetRound(Player.B, 2, Piece.Attack);
            game.Print();

            game.PlayRound();
            game.Print();

            int[] expected = new int[2] { 3, 1 };
            Assert.AreEqual(game.Points, expected);
            Assert.AreEqual(game.InitiativePlayerA, false);

        }

    }
}
