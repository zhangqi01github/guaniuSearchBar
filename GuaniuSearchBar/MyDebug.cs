using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuaniuSearchBar
{
    public static class MyDebug
    {
        public static void Print(string str)
        {
            Debug.Print(str);
        }

        public static void Print(string format, params object[] args)
        {
            Debug.Print(format, args);
        }
    }
}
