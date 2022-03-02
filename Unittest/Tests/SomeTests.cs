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
        private string FilesDir => Path.Combine(TestContext.CurrentContext.TestDirectory, "files");

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
