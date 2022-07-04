using UnityEngine;
using System.Text;
using System;

namespace AutumnYard.Joust
{
    public sealed class Joust
    {
        public struct Result
        {
            public int PointA { private set; get; }
            public int PointB { private set; get; }
            public bool hasInitiativeA { private set; get; }
        }

        public const int Players = 2;
        public const int Rounds = 5;
        public const int Bouts = 3;

        // Round state
        private Piece[,] _board = new Piece[Players, Bouts];
        private int[] _points = new int[Players];
        private bool _initiativePlayerA;
        private int _currentBout;

        // Game state
        private int _currentRound;
        private Result[,] results = new Result[Rounds, Bouts];

        public Piece[,] Board => _board;
        public int[] Points => _points;
        public bool InitiativePlayerA => _initiativePlayerA;

        public event Action onFinishGame;


        public void SetRound(Player player, int index, Piece to) => _board[(int)player, index] = to;

        private bool IsRoundCorrect()
        {
            foreach (var item in _board)
            {
                if (item == Piece.Empty) return false;
            }
            return true;
        }

        public void PlayRound()
        {
            if (!IsRoundCorrect()) return;

            // Empieza: 0,0,B
            Debug.Log($"Round #x start:");
            PrintOnlyGameState();

            // Round 1
            var result1 = _initiativePlayerA ? CheckPair(_board[0, 0], _board[1, 0]) : CheckPair(_board[1, 0], _board[0, 0]);
            Debug.Log($"  - Result 1: Had A the initiative? {_initiativePlayerA}, let's see: {result1}");
            _points[0] += _initiativePlayerA ? result1.PointsToInitiate : result1.PointsToSecond;
            _points[1] += _initiativePlayerA ? result1.PointsToSecond : result1.PointsToInitiate;
            if (result1.ChangeInitiative) _initiativePlayerA = !_initiativePlayerA;
            // Deberia quedar: 0,1,A
            PrintOnlyGameState();

            // Round 2
            var result2 = _initiativePlayerA ? CheckPair(_board[0, 1], _board[1, 1]) : CheckPair(_board[1, 1], _board[0, 1]);
            Debug.Log($"  - Result 2: Had A the initiative? {_initiativePlayerA}, let's see: {result2}");
            _points[0] += _initiativePlayerA ? result2.PointsToInitiate : result2.PointsToSecond;
            _points[1] += _initiativePlayerA ? result2.PointsToSecond : result2.PointsToInitiate;
            if (result2.ChangeInitiative) _initiativePlayerA = !_initiativePlayerA;
            // Deberia quedar: 2,1,A
            PrintOnlyGameState();

            // Round 3
            var result3 = _initiativePlayerA ? CheckPair(_board[0, 2], _board[1, 2]) : CheckPair(_board[1, 2], _board[0, 2]);
            Debug.Log($"  - Result 3: Had A the initiative? {_initiativePlayerA}, let's see: {result3}");
            _points[0] += _initiativePlayerA ? result3.PointsToInitiate : result3.PointsToSecond;
            _points[1] += _initiativePlayerA ? result3.PointsToSecond : result3.PointsToInitiate;
            if (result3.ChangeInitiative) _initiativePlayerA = !_initiativePlayerA;
            // Deberia quedar: 3,1,B
            PrintOnlyGameState();
        }
        public void NextRound()
        {
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = Piece.Empty;
                }
            }

            _currentRound++;

            if (_currentRound == 5)
            {
                FinishGame();
            }
        }
        private void FinishGame()
        {
            onFinishGame?.Invoke();
        }

        // points iniciative, points receiver, change initiative?
        private (int PointsToInitiate, int PointsToSecond, bool ChangeInitiative) CheckPair(Piece initiative, Piece receiver)
        {
            switch ((initiative, receiver))
            {
                case (Piece.Attack, Piece.Attack): return (1, 1, true);
                case (Piece.Attack, Piece.Parry): return (0, 2, true);
                case (Piece.Attack, Piece.Defense): return (0, 1, false);

                case (Piece.Parry, Piece.Attack): return (2, 0, false);
                case (Piece.Parry, Piece.Parry): return (0, 0, false);
                case (Piece.Parry, Piece.Defense): return (0, 0, true);

                case (Piece.Defense, Piece.Attack): return (1, 0, true);
                case (Piece.Defense, Piece.Parry): return (0, 0, false);
                case (Piece.Defense, Piece.Defense): return (0, 0, true);
                default:
                    break;
            }

            throw new System.ArgumentOutOfRangeException("Trying to CheckPair with wrong Piece value.");
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Board: ");
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                sb.Append("[");
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    sb.Append(_board[i, j]);
                    if (j < 2) sb.Append(", ");
                }
                sb.Append("] ");
            }
            sb.Append($"Points ({_points[0]}, {_points[1]})");
            sb.Append($" - Has player A the Initiative? {_initiativePlayerA}");

            Debug.Log(sb.ToString());
        }
        public void PrintOnlyGameState()
        {
            Debug.Log($"  Points ({_points[0]}, {_points[1]}) - Has player A the Initiative? {_initiativePlayerA}");
        }
    }
}