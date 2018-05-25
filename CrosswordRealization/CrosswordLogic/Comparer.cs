using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordLogic
{
    public class Comparer : IComparer
    {
        public Comparer()
        {

        }

        public int Compare(object x, object y)
        {
            string a = (string)x;
            string b = (string)y;

            if (a.Length > b.Length)
            {
                return -1;
            }
            else
            {
                return a.Length == b.Length ? 0 : 1; 
            }
        }
    }
}
