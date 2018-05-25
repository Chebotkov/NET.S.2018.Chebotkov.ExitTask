using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLogic
{
    public class Crossword
    {
        private readonly string[] words;
        private char[,] crossword = new char[100,100];

        /// <summary>
        /// i - word, j involved indices.
        /// </summary>
        private Point[][] involvedIndices;

        public int crosswordSize = 100; 

        private IComparer stringComparer = new Comparer();
        public string[] Words
        {
            get
            {
                return words;
            }
        }

        public Crossword(string[] words)
        {
            this.words = words ?? throw new ArgumentNullException("{0} is null", nameof(words));
            SortWords(words);
            involvedIndices = new Point[words.Length][];
            //crosswordSize = GetMaxSize(words);
            //crossword = new char[crosswordSize, crosswordSize];
            LoadField();
        }

        public char[,] GenerateCrossword()
        {
            int previousIndex = -1;
            int currentIndex = 0;
            int xBoarderIndex = 10;
            int yBoarderIndex = 10;
            Point pointInCrossword = new Point(0, 0);
            bool isHorizontal = true;

            bool isGenerated = false;

            if (words.Length >= 2)
            {
                string currentWord = words[currentIndex];
                string nextWord = words[currentIndex + 1];
                for (int i = 0; i < currentWord.Length; i++)
                {
                    for (int j = 0; j < nextWord.Length; j++)
                    {
                        if (currentWord[i] == nextWord[j])
                        {
                            for (int k = 0; k < currentWord.Length; k++)
                            {
                                crossword[20+j, k+20] = currentWord[k];
                            }
                            for (int k = 0; k < nextWord.Length; k++)
                            {
                                crossword[k+20, 20+i] = nextWord[k];
                            }

                            pointInCrossword.x = 20 + i;
                            pointInCrossword.y = 20;
                            involvedIndices[0] = new Point[words[0].Length];
                            involvedIndices[1] = new Point[words[1].Length];
                            involvedIndices[0][0] = new Point(20 + j, i + 20);
                            involvedIndices[1][0] = new Point(20 + j, i + 20);

                            isGenerated = true;
                            break;
                        }
                    }
                    if(isGenerated)
                    {
                        break;
                    }
                }
                currentIndex = 1;
            }

            while (currentIndex < words.Length- 1)
            {
                string currentWord = words[currentIndex];
                string nextWord = words[currentIndex + 1];
                bool isConcated = false;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    for (int j = 0; j < nextWord.Length; j++)
                    {
                        if (currentWord[i] == nextWord[j])//&& !IsCrossing(currentIndex, i, j))
                        {
                            if (isHorizontal)
                            {
                                for (int k = pointInCrossword.y - j, m = 0 ; k < pointInCrossword.y - j + currentWord.Length; k++, m++)
                                {
                                    crossword[j + pointInCrossword.x, k] = currentWord[m];
                                }
                                
                                pointInCrossword = new Point(pointInCrossword.x + i, pointInCrossword.y + j);
                            }
                            else
                            {
                                for (int k = pointInCrossword.x - j, m =0; k < pointInCrossword.x - j + nextWord.Length; k++, m ++)
                                {
                                    crossword[k, i+ pointInCrossword.y] = nextWord[m];
                                }

                                pointInCrossword = new Point(pointInCrossword.x + i, pointInCrossword.y + j);
                            }

                            /*involvedIndices[currentIndex][0] = new Point(j, i);
                            involvedIndices[currentIndex + 1] = new Point[words[currentIndex + 1].Length];
                            involvedIndices[currentIndex + 1][0] = new Point(j, i);*/
                            isConcated = true;
                            isHorizontal = isHorizontal == true ? false : true;
                        }

                        if (isConcated)
                        {
                            break;
                        }
                    }
                }
                currentIndex++;
            }
            
            return crossword;
        }

        private bool IsCrossing(int wordIndex, int i, int j)
        {
            /*if (involvedIndices[wordIndex] == null)
            {
                return false;
            }*/

            foreach (Point p in involvedIndices[wordIndex])
            {
                if (p.x == i && p.y == j)
                {
                    return true;
                }
            }

            return false;
        }

        private void Redraw(bool isHorizontal, int x, int y, string w1, string w2, int IofE, Point IinC)
        {
            if (isHorizontal)
            {
                if (w2[IofE] > IinC.y)
                {

                }
            }
            else
            {

            }
        }

        /// <summary>
        /// Gets maximum possible size of the crossword.
        /// </summary>
        /// <param name="words">Words in crossword.</param>
        /// <returns>Returns maximum possible size of the crossword.</returns>
        private int GetMaxSize(string[] words)
        {
            int size = 0;
            foreach (string word in words)
            {
                size += word.Length;
            }

            return size / 2;
        }

        private void SortWords(string[] words)
        {
            Array.Sort(words, stringComparer);
        }

        private void LoadField()
        {
            for (int i = 0; i < crosswordSize; i++)
            {
                for (int j = 0; j < crosswordSize; j++)
                {
                    crossword[i, j] = '*';
                }
            }
        }

        private struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
