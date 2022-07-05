using UnityEngine;
using NaughtyAttributes;
using System;

namespace AutumnYard.Joust
{
    public sealed class GameDirector : MonoBehaviour
    {
        private static GameDirector _instance;
        public static GameDirector Instance => _instance;

        [SerializeField] private GameDisplay display;
        private Joust _game;

        private void Awake()
        {
            _instance = this;
            NewGame();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        [ContextMenu("New Game")]
        public void NewGame()
        {
            _game = new Joust();
            _game.onFinishBout += () => Debug.Log(" --- Finished Bout");
            _game.onFinishRound += () => Debug.Log(" --- Finished Round");
            _game.onFinishGame += () => Debug.Log(" --- Finished Game");

            display.Display(in _game);
            _game.Print();
        }

        public void TogglePiece(Player player, int bout)
        {
            var nextPiece = _game.Board[(int)player, bout];
            nextPiece++;

            if ((int)nextPiece >= System.Enum.GetValues(typeof(Piece)).Length)
            {
                nextPiece = 0;
            }

            _game.SetRound(player, bout, nextPiece);
            display.Display(in _game);
        }
        public void Button_LockAndPlay()
        {
            //Debug.Log("Button_LockAndPlay");
            _game.TryChangeModeToPlay();
            display.Display(in _game);
        }
        public void Button_NextBout()
        {
            //Debug.Log("Button_NextBout");
            _game.PlayBout();
            display.Display(in _game);
        }
        public void Button_FinishRound()
        {
            //Debug.Log("Button_FinishRound");
            _game.FinishRound();
            display.Display(in _game);
        }


        #region Testing

        [ContextMenu("Print Game")]
        public void PrintGame() => _game.Print();

        public void OnGUI() => GUILayout.Label($"{_game.CurrentPhase}");

        #endregion // Testing

    }
}