using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    class HIDGen
    {
        internal static string Generate(int num)
        {
            string s = "";
            while (num != 0)
            {
                int x = num % 26;
                s += (char)('A' + (char)x);
                num /= 26;
            }
            return s;
        }
    }
}
