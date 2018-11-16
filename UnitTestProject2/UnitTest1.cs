using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlidingPuzzleEngine;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            State state = new State(3, 3, new byte[] { 1, 2, 3, 0, 4, 6, 7, 5, 8 }, DirectionEnum.None, 0, null);
            PuzzleSolver solver = new ManhattanSolver(state);
            solver.Solve();
        }
    }
}
