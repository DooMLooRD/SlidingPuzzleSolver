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
            //args = InitArgs();

            string path = "";// @"..\..\..\DataHandler\Data\";
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
                            else if (args[1] == "manh")
                                solver = new ManhattanSolver(path + args[2], path + args[3], path + args[4]);
                            else
                                solver = new IterativeDeepeningAStar(path + args[2], path + args[3], path + args[4]);
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

        static string[] InitArgs()
        {
            return new[]
            {
                "astr",
                "idastr",
                "4x3_01_00001.txt",
                "4x3_01_00001_astr_idastr_sol.txt",
                "4x3_01_00001_astr_idastr_stats.txt"
            };
            //return new[]
            //{
            //    "astr",
            //    "hamm",
            //    "4x3_01_00001.txt",
            //    "4x3_01_00001_astr_hamm_sol.txt",
            //    "4x3_01_00001_astr_hamm_stats.txt"
            //};
            //return new[]
            //{
            //    "astr",
            //    "manh",
            //    "4x3_01_00001.txt",
            //    "4x3_01_00001_astr_manh_sol.txt",
            //    "4x3_01_00001_astr_manh_stats.txt"
            //};
            //return new[]
            //{
            //    "bfs",
            //    "RDUL",
            //    "4x3_01_00001.txt",
            //    "4x3_01_00001_bfs_rdul_sol.txt",
            //    "4x3_01_00001_bfs_rdul_stats.txt"
            //};
            //return new[]
            //{
            //    "dfs",
            //    "RDUL",
            //    "4x3_01_00001.txt",
            //    "4x3_01_00001_dfs_rdul_sol.txt",
            //    "4x3_01_00001_dfs_rdul_stats.txt"
            //};
        }
    }
}
