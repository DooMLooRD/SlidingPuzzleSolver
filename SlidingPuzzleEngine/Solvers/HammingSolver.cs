using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class HammingSolver : PuzzleSolver
    {
        /// <summary>
        /// Queue for heuristic states
        /// </summary>
        public C5.IntervalHeap<Tuple<State, int>> States { get; set; }
        public HammingSolver(State startingState) : base(startingState)
        {
        }

        public HammingSolver(string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            States = new C5.IntervalHeap<Tuple<State, int>>(
                Comparer<Tuple<State, int>>.Create((t1, t2) =>
                    t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 0)) { new Tuple<State, int>(StartingState, 0) };
        }

        protected override bool CanMove()
        {
            return true;
        }

        protected override List<DirectionEnum> GetAllMoves()
        {
            return CurrentState.GetAllowedMoves();
        }

        protected override void AddToStates(State newPuzzle)
        {
            States.Add(new Tuple<State, int>(newPuzzle, HeuristicFunction(newPuzzle)));
        }

        protected override State GetFromStates()
        {
            return States.DeleteMin().Item1;
        }

        protected override int StatesCount()
        {
            return States.Count;
        }
        private int HeuristicFunction(State state)
        {
            byte[] board = state.Grid;
            int distance = state.DepthLevel;

            for (int i = 0; i < DimensionY; i++)
            {
                for (int j = 0; j < DimensionX; j++)
                {
                    if (i == DimensionY - 1 && j == DimensionX - 1)
                    {
                        if (board[j + i * DimensionX] != 0)
                            distance++;
                    }
                    else
                    {
                        if (board[j + i * DimensionX] != j + i * DimensionX + 1)
                            distance++;
                    }

                }
            }

            return distance;
        }
    }
}
