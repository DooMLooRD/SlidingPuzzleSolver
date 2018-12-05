using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine
{
    public abstract class PuzzleSolver
    {
        #region Property

        /// <summary>
        /// The time that solve algorithm starts
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Starting state loaded from file
        /// </summary>
        public State StartingState { get; set; }

        /// <summary>
        /// State that is currently processing
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Dimension X loaded from file
        /// </summary>
        public byte DimensionX { get; set; }

        /// <summary>
        /// Dimension Y loaded from file
        /// </summary>
        public byte DimensionY { get; set; }

        /// <summary>
        /// Max depth achieved by solver
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Path to save Info file
        /// </summary>
        public string InfoPath { get; set; }

        /// <summary>
        /// Path to save soultion file
        /// </summary>
        public string SolutionPath { get; set; }
        public int Visited { get; set; }

        public Dictionary<string, int> Explored { get; set; }

        public State SolvedState { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for View?? 
        /// </summary>
        /// <param name="startingState"></param>
        protected PuzzleSolver(State startingState)
        {
            InfoPath = @"\..\..\test.txt";
            SolutionPath = @"\..\..\test1.txt";
            DimensionX = startingState.DimensionX;
            DimensionY = startingState.DimensionY;

            StartingState = startingState;
            CurrentState = startingState;

        }

        /// <summary>
        /// Constructor for Command Line with paths and function params 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="startingStatePath"></param>
        /// <param name="solutionPath"></param>
        /// <param name="infoPath"></param>
        protected PuzzleSolver(string startingStatePath, string solutionPath, string infoPath)
        {
            StateDataPack data = DataReader.LoadStartingState(startingStatePath);
            SolutionPath = solutionPath;
            InfoPath = infoPath;
            DimensionX = data.DimensionX;
            DimensionY = data.DimensionY;
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, null);
            CurrentState = StartingState;
            Explored = new Dictionary<string, int>();
            Visited = 1;
        }

        #endregion

        #region Method

        /// <summary>
        /// Method that appends priority queue with new states from allowed moves for current state.
        /// </summary>
        protected void AppendWithChildren()
        {
            if (!CanMove())
                return;

            List<DirectionEnum> allowedMoves = GetAllMoves();
            foreach (var move in allowedMoves)
            {

                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(move), move, CurrentState.DepthLevel + 1, CurrentState);
                if (!Explored.ContainsKey(newPuzzle.ToString()) ||
                    Explored[newPuzzle.ToString()] > newPuzzle.DepthLevel)
                {
                    AddToStates(newPuzzle);
                    Visited++;
                }
            }
        }



        /// <summary>
        /// Solve puzzle with selected function
        /// </summary>
        public virtual void Solve()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //States visited

            while (StatesCount() > 0)
            {

                CurrentState = GetFromStates();

                if (!Explored.ContainsKey(CurrentState.ToString()))
                {
                    Explored.Add(CurrentState.ToString(), CurrentState.DepthLevel);
                }
                else if (CurrentState.DepthLevel < Explored[CurrentState.ToString()])
                {
                    Explored[CurrentState.ToString()] = CurrentState.DepthLevel;
                }

                MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;

                //Check if state is solved, if its solved Write info to the file
                if (CurrentState.IsSolved())
                {

                    timer.Stop();
                    SolvedState = CurrentState;
                    Console.WriteLine("Done!");
                    break;
                }

                AppendWithChildren();
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
                StatesProcessed = Explored.Count,
                Time = timer.Elapsed.TotalMilliseconds
            }, InfoPath);
        }


        #endregion

        #region Abstract Methods

        protected abstract bool CanMove();
        protected abstract List<DirectionEnum> GetAllMoves();
        protected abstract void AddToStates(State newPuzzle);
        protected abstract State GetFromStates();
        protected abstract int StatesCount();

        #endregion
    }

}
