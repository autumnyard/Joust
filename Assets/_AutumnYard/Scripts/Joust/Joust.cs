using System;
using System.Text;
using UnityEngine;

namespace AutumnYard.Joust
{
    public sealed class Joust
    {
        public enum Phase { Locked, Playing, FinishedRound, Finished }
        public struct Score
        {
            public readonly int PointA;
            public readonly int PointB;
            public readonly bool HasInitiativeA;

            public override string ToString()
            {
                return $" ({PointA}, {PointB}) A:{HasInitiativeA}";
            }

            public Score(Result result, bool initiativeA)
            {
                if (initiativeA)
                {
                    PointA = result.PointsToInitiate;
                    PointB = result.PointsToSecond;
                    HasInitiativeA = result.ChangeInitiative;
                }
                else
                {
                    PointA = result.PointsToSecond;
                    PointB = result.PointsToInitiate;
                    HasInitiativeA = result.ChangeInitiative;
                }
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
        private Phase _currentPhase;
        private int _currentRound;
        private Score[,] _results = new Score[Rounds, Bouts];

        public Phase CurrentPhase => _currentPhase;
        public Piece[,] Board => _board;
        public int[] Points => _points;
        public bool InitiativePlayerA => _initiativePlayerA;

        public event Action onFinishBout;
        public event Action onFinishRound;
        public event Action onFinishGame;

        public void Clear()
        {
            _board = new Piece[Players, Bouts];
            _points = new int[Players];
            _initiativePlayerA = true;
            _currentBout = 0;

            _currentPhase = Phase.Locked;
            _currentRound = 0;
            _results = new Score[Rounds, Bouts];
        }

        // Step 1:
        public void SetRound(Player player, int index, Piece to)
        {
            if (_currentPhase != Phase.Locked) return;

            _board[(int)player, index] = to;
        }
        public void SetBout(string Strong)
        {
            if (_currentPhase != Phase.Locked) return;

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

        // Step 2:
        public void TryChangeModeToPlay()
        {
            if (_currentPhase != Phase.Locked) return;

            foreach (var item in _board)
            {
                if (item == Piece.Empty) return;
            }

            _currentPhase = Phase.Playing;
        }

        // Step 3: Play the bouts until finished
        public void PlayBout()
        {
            if (_currentPhase != Phase.Playing) return;

            CalculateBout();
            SetResult();
            onFinishBout?.Invoke();

            _currentBout++;
            if (_currentBout >= Bouts)
            {
                _currentPhase = Phase.FinishedRound;
            }
        }

        // Step 4: Finish this round and begin the next one, unless game is finished
        public void FinishRound()
        {
            if (_currentPhase != Phase.FinishedRound) return;

            onFinishRound?.Invoke();
            _board = new Piece[Players, Bouts];
            _currentRound++;

            if (_currentRound >= Rounds)
            {
                FinishGame();
            }
        }

        public void FinishGame()
        {
            _currentPhase = Phase.Finished;
            onFinishGame?.Invoke();
        }


        #region Game Logic

        private void CalculateBout()
        {
            Result result = _initiativePlayerA ?
                 CheckPair(_board[(int)Player.A, _currentBout], _board[(int)Player.B, _currentBout])
                 : CheckPair(_board[(int)Player.B, _currentBout], _board[(int)Player.A, _currentBout]);

            _results[_currentRound, _currentBout] = new Score(result, _initiativePlayerA);
        }
        private void SetResult()
        {
            _points[0] += _results[_currentRound, _currentBout].PointA;
            _points[1] += _results[_currentRound, _currentBout].PointB;
            _initiativePlayerA = _results[_currentRound, _currentBout].HasInitiativeA;
            Print_GameState();
        }
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
        
        #endregion // Game Logic

        #region Printing

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

        #endregion // Printing

    }
}