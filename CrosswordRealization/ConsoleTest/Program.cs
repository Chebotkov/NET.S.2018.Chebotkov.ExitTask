using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrosswordLogic;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words = { "ушиб", "закрытый", "шина", "открытый", "перелом", "подвывих", "вывих"};

            Crossword crosswordClass = new Crossword(words);

            char[,] crossword = crosswordClass.GenerateCrossword();
            for (int i = 0; i < crosswordClass.crosswordSize; i++)
            {
                for (int j = 0; j < crosswordClass.crosswordSize; j++)
                {
                    Console.Write(crossword[i,j]);
                }
                Console.WriteLine();
            }
        }
    }
}
