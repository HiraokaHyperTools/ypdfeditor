using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using yPDFEditor.Utils;

namespace Unittest.Tests
{
    public class SomeTests
    {
        [Test]
        public void Tests()
        {
            {
                var temp = Path.GetTempFileName();
                FileAssert.Exists(temp);
                UtMarkTemp.Add(temp);
                UtMarkTemp.Cleanup();
                FileAssert.DoesNotExist(temp);
            }
        }
    }
}
