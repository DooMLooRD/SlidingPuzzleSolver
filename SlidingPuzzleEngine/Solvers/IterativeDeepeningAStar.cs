using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine.Solvers
{
    public class IterativeDeepeningAStar : PuzzleSolver
    {
        public int ExploredCounter { get; set; }
        public Stack<State> States { get; set; }
        public IterativeDeepeningAStar(State startingState) : base(startingState)
        {
        }

        public IterativeDeepeningAStar(string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            States = new Stack<State>();

        }

        public override void Solve()
        {

            States.Push(StartingState);
            int bound = HeuristicFunction(StartingState);
            bool isSolved = false;
            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (isSolved != true)
            {
                var result = Search(bound);
                if (result.Item1)
                {
                    timer.Stop();
                    SolvedState = CurrentState;
                    Console.WriteLine("Done!");
                    isSolved = true;
                    continue;
                }

                if (result.Item2 == Int32.MaxValue)
                {
                    break;
                }

                bound = result.Item2;
            }

            if (timer.IsRunning)
                timer.Stop();

            DataWriter.WriteSolutionToFile(new InformationDataPack
            {
                SizeOfSolvedPuzzle = SolvedState?.DepthLevel ?? -1,
                Solution = SolvedState?.GetPath() ?? "",
            }, SolutionPath);

            DataWriter.WriteInfoToFile(new InformationDataPack
            {
                DepthSize = MaxDepth,
                SizeOfSolvedPuzzle = SolvedState?.DepthLevel ?? -1,
                StatesVisited = Visited,
                StatesProcessed = Visited,
                Time = timer.Elapsed.TotalMilliseconds
            }, InfoPath);
        }

        private (bool, int) Search(int bound)
        {
            CurrentState = States.Peek();
            State currentForChildren = CurrentState;
            ExploredCounter++;
            MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;
            int heuristicFunction = HeuristicFunction(CurrentState);
            if (heuristicFunction > bound)
                return (false, heuristicFunction);
            if (CurrentState.IsSolved())
                return (true, heuristicFunction);
            int min = Int32.MaxValue;
            List<DirectionEnum> moves = CurrentState.GetAllowedMoves();
            foreach (var move in moves)
            {
                State state = new State(DimensionX, DimensionY, currentForChildren.Move(move), move, currentForChildren.DepthLevel + 1, currentForChildren);
                States.Push(state);
                Visited++;
                var result = Search(bound);
                if (result.Item1)
                {
                    return (true, heuristicFunction);
                }

                if (result.Item2 < min)
                    min = result.Item2;
                States.Pop();
            }

            return (false, min);
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
            States.Push(newPuzzle);
        }

        protected override State GetFromStates()
        {
            return States.Pop();
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

    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        public EqualityComparer(Func<T, T, bool> cmp)
        {
            this.cmp = cmp;
        }
        public bool Equals(T x, T y)
        {
            return cmp(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public Func<T, T, bool> cmp { get; set; }
    }
}
