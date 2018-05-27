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
        private char[,] crossword = new char[100,100];
        
        private List<Word> words;

        public int crosswordSize = 100; 

        private IComparer stringComparer = new Comparer();

        public Crossword(string[] words)
        {
            string [] newWords = words ?? throw new ArgumentNullException("{0} is null", nameof(words));
            SortWords(newWords);
            AddWords(newWords);
            //crosswordSize = GetMaxSize(words);
            //crossword = new char[crosswordSize, crosswordSize];
            LoadField();
        }

        public char[,] GenerateCrossword()
        {
            int currentIndex = 0;
            bool isHorizontal = true;

            bool isGenerated = false;

            if (words.Count >= 2)
            {
                string currentWord = words[currentIndex].word;
                string nextWord = words[currentIndex + 1].word;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    for (int j = 0; j < nextWord.Length; j++)
                    {
                        if (currentWord[i] == nextWord[j])
                        {
                            Point newWordStart = new Point(20, 20);
                            Point newWordEnd = new Point(20, 20 + currentWord.Length - 1);
                            for (int k = 0; k < currentWord.Length; k++)
                            {
                                crossword[newWordStart.y, newWordStart.x + k] = currentWord[k];
                            }

                            /*
                            for (int k = 0; k < nextWord.Length; k++)
                            {
                                crossword[k+20 - j, 20 + i] = nextWord[k];
                            }*/
                                                        
                            words[currentIndex].SetWordCoordinates(newWordStart, newWordEnd);
                            /*words[currentIndex + 1].SetWordCoordinates(new Point(20 + i, 20 - j), new Point(20 + i, 20 - j + nextWord.Length - 1));

                            Console.WriteLine(words[0].word);
                            Console.WriteLine(words[0].wordStart.x + "; " + words[0].wordStart.y + " ... " + words[0].wordEnd.x + "; " + words[0].wordEnd.y);
                            Console.WriteLine(words[1].word);
                            Console.WriteLine(words[1].wordStart.x + "; " + words[1].wordStart.y + " ... " + words[1].wordEnd.x + "; " + words[1].wordEnd.y);*/

                            isHorizontal = false;
                            isGenerated = true;
                            break;
                        }
                    }
                    if(isGenerated)
                    {
                        break;
                    }
                }
                currentIndex = 0;
            }

            while (currentIndex < 3)// words.Count- 1)
            {
                Console.WriteLine("{0}. {1}", (currentIndex + 1), words[currentIndex+1].word);
                string currentWord = words[currentIndex].word;
                string nextWord = words[currentIndex + 1].word;
                bool isConcated = false;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    for (int j = 0; j < nextWord.Length; j++)
                    {
                        try
                        {
                            if (currentWord[i] == nextWord[j])
                            {
                                if (isHorizontal)
                                {
                                    Point newWordStart = new Point(words[currentIndex].wordStart.x - j, words[currentIndex].wordStart.y + i);
                                    Point newWordEnd = new Point(words[currentIndex].wordStart.x + (nextWord.Length - 1 - j), words[currentIndex].wordStart.y + i);
                                    if (!IsCrossing(newWordStart, newWordEnd, currentIndex, words[currentIndex].word.Length))
                                    {
                                        for (int k = 0; k < nextWord.Length; k++)
                                        {
                                            crossword[newWordStart.y, newWordStart.x + k] = nextWord[k];
                                        }

                                        Console.WriteLine("Horizontal; " +nextWord);
                                        words[currentIndex + 1].SetWordCoordinates(newWordStart, newWordEnd);
                                        Console.WriteLine(words[currentIndex + 1].wordStart.x + "; " + words[currentIndex + 1].wordStart.y + " ... " + words[currentIndex + 1].wordEnd.x + "; " + words[currentIndex + 1].wordEnd.y);
                                        isHorizontal = false;
                                        isConcated = true;
                                        break;
                                    }

                                }
                                else
                                {
                                    Point newWordStart = new Point(words[currentIndex].wordStart.x + i, words[currentIndex].wordStart.y - i);
                                    Point newWordEnd = new Point(words[currentIndex].wordStart.x + i, words[currentIndex].wordStart.y + nextWord.Length - 1 - i);

                                    if (!IsCrossing(newWordStart, newWordEnd, currentIndex, words[currentIndex].word.Length))
                                    {
                                        for (int k = 0; k < nextWord.Length; k++)
                                        {
                                            crossword[newWordStart.y + k, newWordStart.x ] = nextWord[k];
                                        }

                                        Console.WriteLine("Vertical; " + nextWord);
                                        words[currentIndex + 1].SetWordCoordinates(newWordStart, newWordEnd);
                                        Console.WriteLine(words[currentIndex + 1].wordStart.x + "; " + words[currentIndex + 1].wordStart.y + " ... " + words[currentIndex + 1].wordEnd.x + "; " + words[currentIndex + 1].wordEnd.y);
                                        isHorizontal = true;
                                        isConcated = true;
                                        break;
                                    }

                                }

                            }
                        }
                        catch(IndexOutOfRangeException)
                        {
                            Console.WriteLine(words[currentIndex + 1].word);
                            isConcated = true;
                            break;
                        }

                    }
                    if (isConcated)
                    {
                        break;
                    }
                } 
                currentIndex++;
                
                if (!isConcated)
                {
                    if (currentIndex + 1 < words.Count)
                    {
                        Word word = words[currentIndex];
                        words[currentIndex] = words[currentIndex + 1];
                        words[currentIndex + 1] = word;
                        currentIndex--;
                    }
                    Console.WriteLine("!");
                }
            }
            
            return crossword;
        }

        private bool IsCrossing(Point startPoint, Point endPoint, int currentIndex, int wordLength)
        {
            int i = 0;
            foreach (Word word in words)
            {
                if (i == currentIndex)
                {
                    i++;
                    continue;
                }

                for (int k = 0; k < wordLength; k++)
                {
                    //instead of checking: is word horizontal or vertical.
                    int x = startPoint.x + (endPoint.x - startPoint.x) + k;
                    int y = startPoint.y + (endPoint.y - startPoint.y) + k;
                    //

                    if (word.wordStart.x <= x && word.wordEnd.x >= x && word.wordStart.y <= y && word.wordEnd.y >= y)
                    {
                        return true;
                    }
                }

                i++;
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

        /// <summary>
        /// Sorts words.
        /// </summary>
        /// <param name="words"></param>
        private void SortWords(string[] words)
        {
            Array.Sort(words, stringComparer);
        }

        /// <summary>
        /// Creates new crossword field.
        /// </summary>
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
            public readonly int x;
            public readonly int y;
            
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private class Word
        {
            public string word;
            public Point wordStart;
            public Point wordEnd;    

            public Word(string word)
            {
                this.word = word;
                wordStart = new Point();
                wordEnd = new Point();
            }

            public void SetWordCoordinates(Point start, Point end)
            {
                wordStart = start;
                wordEnd = end;
            }
        }

        private void AddWords(string[] words)
        {
            this.words = new List<Word>();
            foreach (string word in words)
            {
                this.words.Add(new Word(word));
            }
        }
    }
}
