using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace SlidingPuzzleEngine
{
    public class ManhattanSolver : PuzzleSolver
    {
        /// <summary>
        /// Queue for heuristic states
        /// </summary>
        //public C5.IntervalHeap<(State, int)> States { get; set; }
        private FastPriorityQueue<State> States { get; set; }
        public ManhattanSolver(State startingState) : base(startingState)
        {
            //  States = new C5.IntervalHeap<(State, int)>(
            //    Comparer<(State, int)>.Create((t1, t2) =>
            //        t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 0)) { new ValueTuple<State, int>(StartingState, 0) };
            States = new FastPriorityQueue<State>(100_000_000);
            States.Enqueue(StartingState, 0);
        }

        public ManhattanSolver(string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            //States = new C5.IntervalHeap<(State, int)>(
            //    Comparer<(State, int)>.Create((t1, t2) =>
            //     t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 0)) { (StartingState, 0) };
            States = new FastPriorityQueue<State>(100_000_000);
            States.Enqueue(StartingState,0);
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
            States.Enqueue(newPuzzle, HeuristicFunction(newPuzzle));
            //States.Add((newPuzzle, HeuristicFunction(newPuzzle)));
        }

        protected override State GetFromStates()
        {
            //return States.DeleteMin().Item1;
            return States.Dequeue();
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
                    int value = board[j + i * DimensionX];
                    if (value != 0)
                    {              
                        int x = (value - 1) % DimensionX;
                        int y = (value - 1 - x) / DimensionX;
                        distance += Math.Abs(j - x) + Math.Abs(i - y);
                    }

                }
            }

            return distance;
        }


    }
}
