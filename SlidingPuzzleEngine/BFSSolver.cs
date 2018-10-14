﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine
{
    public class BFSSolver
    {
        public long StartTime { get; set; }
        public State StartingState { get; set; }
        public State CurrentState { get; set; }
        public Queue<State> States { get; set; }
        public byte DimensionX { get; set; }
        public byte DimensionY { get; set; }
        public int MaxDepth { get; set; }
        public string InfoPath { get; set; }
        public string SolutionPath { get; set; }
        public List<DirectionEnum> Order { get; set; }

        public BFSSolver(State startingState)
        {
            InfoPath = @"\..\..\test.txt";
            SolutionPath = @"\..\..\test1.txt";
            DimensionX = startingState.DimensionX;
            DimensionY = startingState.DimensionY;

            States = new Queue<State>();
            StartingState = startingState;
            CurrentState = startingState;
            States.Enqueue(StartingState);
        }

        public BFSSolver(string order, string startingStatePath, string solutionPath, string infoPath)
        {
            InfoPath = infoPath;
            SolutionPath = solutionPath;
            StateDataPack data = DataReader.LoadStartingState(startingStatePath);
            DimensionX = data.DimensionX;
            DimensionY = data.DimensionY;
            Order = State.StringToDirectionEnums(order);
            States = new Queue<State>();
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, new List<DirectionEnum>());
            CurrentState = StartingState;
            States.Enqueue(StartingState);
        }

        public void AppendQueueWithChildrens(ref int visited)
        {
            List<DirectionEnum> allowedMoves = CurrentState.GetAllowedMoves(Order);
            for (int i = 0; i < allowedMoves.Count; i++)
            {
                visited++;
                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(allowedMoves[i]), allowedMoves[i], CurrentState.DepthLevel + 1, CurrentState.Path.Append(allowedMoves[i]).ToList());
                States.Enqueue(newPuzzle);
            }
        }

        public void Solve()
        {
            StartTime = DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
            int processed = 0;
            int visited = 1;
            while (States.Count > 0)
            {

                CurrentState = States.Dequeue();
                MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;
                processed++;
                if (CurrentState.IsSolved())
                {
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
                        StatesProcessed = processed,
                        Time = (double)(DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000) - StartTime) / 1000
                    }, InfoPath);
                    Console.WriteLine("Done!");
                    return;
                }

                AppendQueueWithChildrens(ref visited);
            }
        }
    }
}
