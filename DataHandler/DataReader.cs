using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public static class DataReader
    {

        public static StateDataPack LoadStartingState(string path)
        {
            Console.WriteLine(path);
            using (StreamReader sr = new StreamReader(path))
            {

                String[] line = sr.ReadLine()?.Split(' ', '\r', '\n');
                byte y = byte.Parse(line[0]);

                byte x = byte.Parse(line[1]);

                List<string> gridString = new List<string>();
                for (int i = 0; i < y; i++)
                {
                    gridString.AddRange(sr.ReadLine()?.Split(' ','\r','\n'));
                }

                byte[] grid = new Byte[x * y];
                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        grid[j + i * x] = byte.Parse(gridString[j + i * x]);
                    }
                }
                return new StateDataPack() { DimensionX = x, DimensionY = y, Grid = grid };
            }

        }
    }
}
