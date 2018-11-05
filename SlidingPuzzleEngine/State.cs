using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class State
    {

        #region Property

        /// <summary>
        /// Dimension x of puzzle grid
        /// </summary>
        public byte DimensionX { get; set; }

        /// <summary>
        /// Dimension y of puzzle grid
        /// </summary>
        public byte DimensionY { get; set; }

        /// <summary>
        /// Puzzle grid
        /// </summary>
        public byte[] Grid { get; set; }

        /// <summary>
        /// The last made move
        /// </summary>
        public DirectionEnum LastMove { get; private set; }

        /// <summary>
        /// Depth Level
        /// </summary>
        public int DepthLevel { get; set; }

        /// <summary>
        /// Index of blank space
        /// </summary>
        public int BlankSpaceIndex { get; set; }

        public State Parent { get; set; }

        #endregion

        #region Constructor

        public State(byte dimensionX, byte dimensionY, byte[] grid, DirectionEnum lastMove, int depthLevel, State parent)
        {
            DimensionX = dimensionX;
            DimensionY = dimensionY;
            Grid = grid;
            LastMove = lastMove;
            DepthLevel = depthLevel;
            FindBlankSpace();
            Parent = parent;
        }

        #endregion

        #region Method

        public string GetPath()
        {
            string path=String.Empty;
                
            if (Parent == null)
                return path;
            path +=Parent.GetPath()+ LastMove.ToString()[0];
            return path;


        }
        /// <summary>
        /// Checks if puzzle is solved
        /// </summary>
        /// <returns></returns>
        public bool IsSolved()
        {
            for (int i = 0; i < DimensionY; i++)
            {
                for (int j = 0; j < DimensionX; j++)
                {
                    if (i == DimensionY - 1 && j == DimensionX - 1)
                    {
                        if (Grid[j + i * DimensionX] != 0)
                            return false;
                    }
                    else
                    {
                        if (Grid[j + i * DimensionX] != j + i * DimensionX + 1)
                            return false;
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// Returns X of blank space
        /// </summary>
        /// <returns></returns>
        public int GetBlankSpaceX()
        {
            return BlankSpaceIndex % DimensionX;
        }

        /// <summary>
        /// Returns Y of blank space
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetBlankSpaceY(int x)
        {
            return (BlankSpaceIndex - x) / DimensionX;
        }

        /// <summary>
        /// Returns allowed moves with given order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<DirectionEnum> GetAllowedMoves(List<DirectionEnum> order)
        {
            List<DirectionEnum> moves = GetAllowedMoves();
            List<DirectionEnum> orderedMoves = new List<DirectionEnum>();
            for (int i = 0; i < order.Count; i++)
            {
                foreach (DirectionEnum directionEnum in moves)
                {
                    if (order[i] == directionEnum)
                        orderedMoves.Add(directionEnum);
                }
            }

            return orderedMoves;

        }

        /// <summary>
        /// Returns allowed moves
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<DirectionEnum> GetAllowedMoves()
        {
            List<DirectionEnum> moves = new List<DirectionEnum>(4);
            int blankSpaceX = GetBlankSpaceX();
            int blankSpaceY = GetBlankSpaceY(blankSpaceX);

            if (blankSpaceY > 0 && LastMove != DirectionEnum.Down)
                moves.Add(DirectionEnum.Up);
            if (blankSpaceY < DimensionY - 1 && LastMove != DirectionEnum.Up)
                moves.Add(DirectionEnum.Down);
            if (blankSpaceX > 0 && LastMove != DirectionEnum.Right)
                moves.Add(DirectionEnum.Left);
            if (blankSpaceX < DimensionX - 1 && LastMove != DirectionEnum.Left)
                moves.Add(DirectionEnum.Right);


            return moves;

        }

        /// <summary>
        /// Finds blank space in puzzle grid
        /// </summary>
        public void FindBlankSpace()
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i] == 0)
                    BlankSpaceIndex = i;
            }
        }

        /// <summary>
        /// Returns new puzzle grid with move to given direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public byte[] Move(DirectionEnum direction)
        {
            byte[] grid = (byte[])Grid.Clone();

            if (direction == DirectionEnum.Left)
            {
                Swap(ref grid[BlankSpaceIndex - 1], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Right)
            {
                Swap(ref grid[BlankSpaceIndex + 1], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Up)
            {
                Swap(ref grid[BlankSpaceIndex - DimensionX], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Down)
            {
                Swap(ref grid[BlankSpaceIndex + DimensionX], ref grid[BlankSpaceIndex]);
            }

            return grid;
        }

        /// <summary>
        /// Method that swaps values
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public void Swap(ref byte value1, ref byte value2)
        {
            byte temp = value1;
            value1 = value2;
            value2 = temp;
        }

        #endregion

        #region Static Method

        /// <summary>
        /// Convert String to List of direction enums
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<DirectionEnum> StringToDirectionEnums(string order)
        {
            List<DirectionEnum> directions = new List<DirectionEnum>();
            foreach (char c in order)
            {
                if (c == 'L')
                    directions.Add(DirectionEnum.Left);
                if (c == 'R')
                    directions.Add(DirectionEnum.Right);
                if (c == 'D')
                    directions.Add(DirectionEnum.Down);
                if (c == 'U')
                    directions.Add(DirectionEnum.Up);
            }

            return directions;
        }

        #endregion

    }
}
