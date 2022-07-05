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
            var game = new Joust();

            // Empieza: 0,0,B
            game.SetRound(Player.A, 0, Piece.Attack);
            game.SetRound(Player.B, 0, Piece.Defense);
            // Deberia quedar: 0,1,A
            game.SetRound(Player.A, 1, Piece.Parry);
            game.SetRound(Player.B, 1, Piece.Attack);
            // Deberia quedar: 2,1,A
            game.SetRound(Player.A, 2, Piece.Defense);
            game.SetRound(Player.B, 2, Piece.Attack);
            // Deberia quedar: 3,1,B

            game.TryChangeModeToPlay();
            game.PlayBout();
            game.PlayBout();
            game.PlayBout();
            game.FinishRound();

            Assert.AreEqual(new int[] { 3, 1 }, game.Points);
            Assert.AreEqual(false, game.InitiativePlayerA);
        }
        [Test]
        public void ExampleGame1_SetWithString()
        {
            // Use the Assert class to test conditions
            var game = new Joust();
            game.SetBout("APDDAA");
            game.TryChangeModeToPlay();
            game.PlayBout();
            game.PlayBout();
            game.PlayBout();
            game.FinishRound();

            int[] expected = new int[2] { 3, 1 };
            Assert.AreEqual(expected, game.Points);
            Assert.AreEqual(false, game.InitiativePlayerA);
        }

    }
}
