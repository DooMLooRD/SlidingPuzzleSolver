using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public static class DataWriter
    {
        public static void WriteSolutionToFile(InformationDataPack info, string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(info.SizeOfSolvedPuzzle);
                if (info.SizeOfSolvedPuzzle >= 0)
                {
                        outputFile.Write(info.Solution);
                }
                
            }
        }

        public static void WriteInfoToFile(InformationDataPack info, string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(info.SizeOfSolvedPuzzle);
                outputFile.WriteLine(info.StatesVisited);
                outputFile.WriteLine(info.StatesProcessed);
                outputFile.WriteLine(info.DepthSize);
                outputFile.WriteLine(info.Time.ToString("F3"));
            }
        }
    }
}
