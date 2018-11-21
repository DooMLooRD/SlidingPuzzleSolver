using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SlidingPuzzleEngine;
using SlidingPuzzleEngine.Solvers;

namespace SlidingPuzzleCommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\DataHandler\Data\";
            if (args.Length < 5)
                Console.WriteLine("Too few arguments");
            else
            {
                PuzzleSolver solver;
                switch (args[0])
                {
                    case "bfs":
                        {
                            solver = new BFSSolver(args[1], path + args[2], path + args[3], path + args[4]);
                            break;

                        }
                    case "dfs":
                        {
                            solver = new DFSSolver(args[1], path + args[2], path + args[3], path + args[4]);
                            break;

                        }
                    case "iddfs":
                    {
                        solver = new IterativeDeepeningDFSSolver(args[1], path + args[2], path + args[3], path + args[4]);
                        break;

                    }
                    case "astr":
                        {
                            if (args[1] == "hamm")
                                solver = new HammingSolver(path + args[2], path + args[3], path + args[4]);
                            else
                                solver = new ManhattanSolver(path + args[2], path + args[3], path + args[4]);
                            break;

                        }
                    default:
                        {
                            solver = new BFSSolver(args[1], path + args[2], path + args[3], path + args[4]);
                            break;
                        }

                }
                solver.Solve();
            }

        }
    }
}
