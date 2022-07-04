using System;
using System.Text;
using UnityEngine;

namespace AutumnYard.Joust
{
    public sealed class Joust
    {
        public struct Score
        {
            public readonly int PointA;
            public readonly int PointB;
            public readonly bool HasInitiativeA;

            public override string ToString()
            {
                return $" ({PointA}, {PointB}) A:{HasInitiativeA}";
            }

            public Score(int pointA, int pointB, bool hasInitiativeA)
            {
                PointA = pointA;
                PointB = pointB;
                HasInitiativeA = hasInitiativeA;
            }
        }
        public struct Result
        {
            public readonly int PointsToInitiate;
            public readonly int PointsToSecond;
            public readonly bool ChangeInitiative;

            public Result(int pointsToInitiate, int pointsToSecond, bool changeInitiative)
            {
                PointsToInitiate = pointsToInitiate;
                PointsToSecond = pointsToSecond;
                ChangeInitiative = changeInitiative;
            }
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
        private Score[,] _results = new Score[Rounds, Bouts];

        public Piece[,] Board => _board;
        public int[] Points => _points;
        public bool InitiativePlayerA => _initiativePlayerA;

        public event Action onFinishGame;


        public void SetRound(Player player, int index, Piece to) => _board[(int)player, index] = to;
        public void SetBout(string Strong)
        {
            _board[0, 0] = Parse(Strong[0]);
            _board[0, 1] = Parse(Strong[1]);
            _board[0, 2] = Parse(Strong[2]);
            _board[1, 0] = Parse(Strong[3]);
            _board[1, 1] = Parse(Strong[4]);
            _board[1, 2] = Parse(Strong[5]);

            Piece Parse(char Char)
            {
                switch (Char)
                {
                    case 'A': return Piece.Attack;
                    case 'P': return Piece.Parry;
                    case 'D': return Piece.Defense;
                    default: return Piece.Empty;
                }
            }
        }

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

            Print_StartRound();

            for (_currentBout = 0; _currentBout < Bouts; _currentBout++)
            {
                CalculateBout();
                SetResult();
            }

            NextRound();

            void CalculateBout()
            {
               Result result = _initiativePlayerA ?
                    CheckPair(_board[0, _currentBout], _board[1, _currentBout])
                    : CheckPair(_board[1, _currentBout], _board[0, _currentBout]);

                _results[_currentRound, _currentBout] = _initiativePlayerA ?
                    new Score(result.PointsToInitiate, result.PointsToSecond, result.ChangeInitiative)
                    : new Score(result.PointsToSecond, result.PointsToInitiate, result.ChangeInitiative);
            }
            void SetResult()
            {
                //Print_Result();
                _points[0] += _results[_currentRound, _currentBout].PointA;
                _points[1] += _results[_currentRound, _currentBout].PointB;
                _initiativePlayerA = _results[_currentRound, _currentBout].HasInitiativeA;
                Print_GameState();
            }
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
        private Result CheckPair(Piece initiative, Piece receiver)
        {
            switch ((initiative, receiver))
            {
                case (Piece.Attack, Piece.Attack): return new Result(1, 1, true);
                case (Piece.Attack, Piece.Parry): return new Result(0, 2, true);
                case (Piece.Attack, Piece.Defense): return new Result(0, 1, false);

                case (Piece.Parry, Piece.Attack): return new Result(2, 0, false);
                case (Piece.Parry, Piece.Parry): return new Result(0, 0, false);
                case (Piece.Parry, Piece.Defense): return new Result(0, 0, true);

                case (Piece.Defense, Piece.Attack): return new Result(1, 0, true);
                case (Piece.Defense, Piece.Parry): return new Result(0, 0, false);
                case (Piece.Defense, Piece.Defense): return new Result(0, 0, true);
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
        public void Print_StartRound()
        {
            Debug.Log($"+ Round #{_currentRound} start!");
            Debug.Log($"  - Initial State: ({_points[0]}, {_points[1]}) A:{_initiativePlayerA}");
        }
        public void Print_Result()
        {
            Debug.Log($"   = #{_currentRound}-{_currentBout}: Result {_results[_currentRound, _currentBout]}");
        }
        public void Print_GameState()
        {
            Debug.Log($"  - #{_currentRound}-{_currentBout}: State:  ({_points[0]}, {_points[1]}) A:{_initiativePlayerA}");
        }
    }
}