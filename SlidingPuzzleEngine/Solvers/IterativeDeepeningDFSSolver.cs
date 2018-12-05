using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine.Solvers
{
    public class IterativeDeepeningDFSSolver : PuzzleSolver
    {
        private int _explored;
        private int _depthLevel;

        public Stack<State> States { get; set; }
        public List<DirectionEnum> Order { get; set; }

        public IterativeDeepeningDFSSolver(State startingState) : base(startingState)
        {
        }

        public IterativeDeepeningDFSSolver(string order, string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            Order = State.StringToDirectionEnums(order);
            Order.Reverse();
            States = new Stack<State>();
        }

        public State Solve(State root)
        {
            States.Clear();
            Explored.Clear();
            States.Push(root);
            Visited++;
            while (StatesCount() > 0)
            {

                CurrentState = GetFromStates();

                if (!Explored.ContainsKey(CurrentState.ToString()))
                {
                    Explored.Add(CurrentState.ToString(), CurrentState.DepthLevel);
                    _explored++;
                }
                else if (CurrentState.DepthLevel < Explored[CurrentState.ToString()])
                {
                    Explored[CurrentState.ToString()] = CurrentState.DepthLevel;
                }

                MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;

                //Check if state is solved, if its solved Write info to the file
                if (CurrentState.IsSolved())
                {
                    return CurrentState;
                }

                AppendWithChildren();
            }

            return null;
        }
        public override void Solve()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < 20; i++)
            {
                _depthLevel = i;
                State solution= Solve(StartingState);
                if (solution != null)
                {
                    timer.Stop();

                    DataWriter.WriteSolutionToFile(new InformationDataPack()
                    {
                        SizeOfSolvedPuzzle = CurrentState.DepthLevel,
                        Solution = CurrentState.GetPath(),
                    }, SolutionPath);

                    DataWriter.WriteInfoToFile(new InformationDataPack()
                    {
                        DepthSize = MaxDepth,
                        SizeOfSolvedPuzzle = CurrentState.DepthLevel,
                        StatesVisited = Visited,
                        StatesProcessed = _explored,
                        Time = timer.Elapsed.TotalMilliseconds
                    }, InfoPath);

                    Console.WriteLine("Done!");
                    return;
                }                   
            }
        }

        protected override bool CanMove()
        {
            return CurrentState.DepthLevel < _depthLevel;
        }

        protected override List<DirectionEnum> GetAllMoves()
        {
            return CurrentState.GetAllowedMoves(Order);
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
    }
}
