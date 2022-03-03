using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unittest.Utils
{
    internal static class TempUtil
    {
        public static string Get(string suffix)
        {
            return Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                suffix
            );
        }
    }
}
