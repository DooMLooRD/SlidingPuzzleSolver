using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace SlidingPuzzleEngine
{
    public class HammingSolver : PuzzleSolver
    {
        /// <summary>
        /// Queue for heuristic states
        /// </summary>
        private FastPriorityQueue<State> States { get; set; }
        public HammingSolver(State startingState) : base(startingState)
        {
        }

        public HammingSolver(string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            States = new FastPriorityQueue<State>(100_000_000);
            States.Enqueue(StartingState, 0);
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
        }

        protected override State GetFromStates()
        {
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
            for (int i = 0; i < board.Length - 1; i++)
            {
                distance += board[i] != i + 1 ? 1 : 0;
            }

            return distance;
        }
    }
}
