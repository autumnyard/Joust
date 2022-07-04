using UnityEngine;
using NaughtyAttributes;

namespace AutumnYard.Joust
{
    public sealed class GameDirector : MonoBehaviour
    {
        [SerializeField] private GameDisplay display;
        private Joust _game;

        private void Awake()
        {
            NewGame();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        [ContextMenu("New Game")]
        public void NewGame()
        {
            _game = new Joust();

            display.Display(in _game);
            _game.Print();
        }


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        [ContextMenu("Test: Set Game 1")]
        public void Test_Game1Set()
        {
            _game.SetRound(Player.A, 0, Piece.Attack);
            _game.SetRound(Player.A, 1, Piece.Parry);
            _game.SetRound(Player.A, 2, Piece.Defense);
            _game.SetRound(Player.B, 0, Piece.Defense);
            _game.SetRound(Player.B, 1, Piece.Attack);
            _game.SetRound(Player.B, 2, Piece.Attack);

            display.Display(in _game);
            _game.Print();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        [ContextMenu("Test: Play Game 1")]
        public void Test_Game1Play()
        {
            _game.PlayRound();

            display.Display(in _game);
            _game.Print();
        }

        [ContextMenu("Print Game")]
        public void PrintGame() => _game.Print();
    }
}