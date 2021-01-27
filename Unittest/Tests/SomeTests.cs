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
                Assert.AreEqual("", HIDGen.Generate(0));
                Assert.AreEqual("B", HIDGen.Generate(1));
                Assert.AreEqual("C", HIDGen.Generate(2));
                Assert.AreEqual("D", HIDGen.Generate(3));

                Assert.AreEqual("Y", HIDGen.Generate(24));
                Assert.AreEqual("Z", HIDGen.Generate(25));
                Assert.AreEqual("AB", HIDGen.Generate(26));
                Assert.AreEqual("BB", HIDGen.Generate(27));
                Assert.AreEqual("CB", HIDGen.Generate(28));

                Assert.AreEqual("YB", HIDGen.Generate(50));
                Assert.AreEqual("ZB", HIDGen.Generate(51));
                Assert.AreEqual("AC", HIDGen.Generate(52));
                Assert.AreEqual("BC", HIDGen.Generate(53));
                Assert.AreEqual("CC", HIDGen.Generate(54));
            }

            {
                var temp = Path.GetTempFileName();
                FileAssert.Exists(temp);
                UtMarkTemp.Add(temp);
                UtMarkTemp.Cleanup();
                FileAssert.DoesNotExist(temp);
            }

            {
                var text = Guid.NewGuid().ToString();
                var redir = new UtRedir(new StringReader(text));
                Assert.AreEqual(text, redir.Wait());
            }

            {
                Assert.AreEqual("\"ABC\"", WinNUt.Quotes("ABC"));
                Assert.AreEqual("\"A B C\"", WinNUt.Quotes("A B C"));
                var text = Guid.NewGuid().ToString();
                Assert.AreEqual($"\"{text}\"", WinNUt.Quotes(text));
            }
        }

        [Test]
        public void PDFInfoTests()
        {
            {
                var pdf = Path.Combine(FilesDir, "title.pdf");
                var it = new PDFInfo(pdf);
                Assert.True(it.Read());
                Assert.AreEqual(1, it.PageCount);
            }

            {
                var pdf = Path.Combine(FilesDir, "ZuABC.pdf");
                var it = new PDFInfo(pdf);
                Assert.True(it.Read());
                Assert.AreEqual(3, it.PageCount);
            }

            {
                var tif = Path.Combine(FilesDir, "ZuABC.tif");
                var it = new PDFInfo(tif);
                Assert.False(it.Read());
                Assert.AreEqual(0, it.PageCount);
            }
        }

        [Test]
        public void CPdftkTests()
        {
            var pdfTitle = Path.Combine(FilesDir, "title.pdf");
            var pdfZuABC = Path.Combine(FilesDir, "ZuABC.pdf");
            {
                var temp = Path.GetTempFileName();

                var pdftk = new CPdftk();
                Assert.True(pdftk.Cat(temp, pdfZuABC));
                Assert.AreEqual(3, GetPdfPageCount(temp));
                File.Delete(temp);
            }
            {
                var temp = Path.GetTempFileName();

                var pdftk = new CPdftk();
                Assert.True(pdftk.Cat2(temp, "1 2 3", pdfZuABC));
                Assert.AreEqual(3, GetPdfPageCount(temp));
                File.Delete(temp);
            }
            {
                var temp = Path.GetTempFileName();

                var pdftk = new CPdftk();
                var pdfs = new Dictionary<string, string>();
                pdfs["A"] = pdfTitle;
                pdfs["B"] = pdfZuABC;
                Assert.True(pdftk.Cat3(temp, "A1 B1 B2 B3", pdfs));
                Assert.AreEqual(4, GetPdfPageCount(temp));
                File.Delete(temp);
            }
        }

        private int GetPdfPageCount(string pdf)
        {
            var it = new PDFInfo(pdf);
            Assert.True(it.Read());
            return it.PageCount;
        }

        [Test]
        public void PDFExploderTests()
        {
            var pdfZuABC = Path.Combine(FilesDir, "ZuABC.pdf");
            var pdfExp = new PDFExploder(pdfZuABC);
            var pp = pdfExp.Extract(1, null, 96, false);
            Assert.NotNull(pp);
            var pic = pp.LoadPic();
            Assert.NotNull(pic);
        }
    }
}
