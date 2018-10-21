using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, new List<DirectionEnum>());
            CurrentState = StartingState;
        }

        #endregion

        #region Method

        /// <summary>
        /// Method that appends priority queue with new states from allowed moves for current state.
        /// </summary>
        private void AppendWithChildren()
        {
            if (!CanMove())
                return;

            List<DirectionEnum> allowedMoves = GetAllMoves();
            foreach (var move in allowedMoves)
            {
                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(move), move, CurrentState.DepthLevel + 1, CurrentState.Path.Append(move).ToList());
                AddToStates(newPuzzle);
            }
        }



        /// <summary>
        /// Solve puzzle with selected function
        /// </summary>
        public void Solve()
        {
            StartTime = DateTime.Now;
            //States visited
            int visited = 0;

            while (StatesCount() > 0)
            {

                CurrentState = GetFromStates();

                MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;
                visited++;

                //Check if state is solved, if its solved Write info to the file
                if (CurrentState.IsSolved())
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan span = endTime - StartTime;
                    string path = null;
                    foreach (DirectionEnum directionEnum in CurrentState.Path)
                    {
                        path += directionEnum.ToString()[0];
                    }

                    DataWriter.WriteSolutionToFile(new InformationDataPack()
                    {
                        SizeOfSolvedPuzzle = CurrentState.Path.Count,
                        Solution = path,
                    }, SolutionPath);

                    DataWriter.WriteInfoToFile(new InformationDataPack()
                    {
                        DepthSize = MaxDepth,
                        SizeOfSolvedPuzzle = CurrentState.Path.Count,
                        StatesVisited = visited,
                        StatesProcessed = visited + StatesCount(),
                        Time = span.TotalMilliseconds
                    }, InfoPath);

                    Console.WriteLine("Done!");
                    return;
                }

                AppendWithChildren();
            }
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
