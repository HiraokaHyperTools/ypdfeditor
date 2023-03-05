using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using yPDFEditor.Tests.Utils;
using yPDFEditor.Utils;

namespace yPDFEditor.Tests.Tests
{
    public class PdfEditTest
    {
        private string FilesDir => Path.Combine(TestContext.CurrentContext.TestDirectory, "files");

        private readonly ObservableCollection<ThumbSet> picts;
        private readonly PdfEdit pdfEdit;

        public PdfEditTest()
        {
            picts = new ObservableCollection<ThumbSet>();

            pdfEdit = new PdfEdit()
            {
                Picts = picts,
                UpdateTitle = () => { },
            };
        }

        [Test]
        [TestCase("title.pdf", 1)]
        [TestCase("ZuABC.pdf", 3)]
        public void Load(string name, int numPages)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));
            Assert.That(pdfEdit.PageCount, Is.EqualTo(numPages));
            Assert.That(pdfEdit.Picts.Count, Is.EqualTo(numPages));
        }

        [Test]
        [TestCase("title.pdf", 1)]
        [TestCase("ZuABC.pdf", 3)]
        public void LoadAgain(string name, int numPages)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));
            Assert.That(pdfEdit.PageCount, Is.EqualTo(numPages));
            Assert.That(pdfEdit.Picts.Count, Is.EqualTo(numPages));
            var newPdf = TempUtil.Get(name + ".Save.pdf");
            pdfEdit.SaveTo(newPdf);

            pdfEdit.Close();

            pdfEdit.OpenFile(newPdf);
            Assert.That(pdfEdit.PageCount, Is.EqualTo(numPages));
            Assert.That(pdfEdit.Picts.Count, Is.EqualTo(numPages));
        }

        [Test]
        [TestCase("title.pdf")]
        [TestCase("ZuABC.pdf")]
        public void RotateLeft(string name)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));

            var newPdf = TempUtil.Get(name + $".RotateLeft.pdf");
            pdfEdit.RotatePages(0, pdfEdit.PageCount - 1, true);
            pdfEdit.SaveTo(newPdf);
        }

        [Test]
        [TestCase("title.pdf")]
        [TestCase("ZuABC.pdf")]
        public void RotateRight(string name)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));

            var newPdf = TempUtil.Get(name + $".RotateRight.pdf");
            pdfEdit.RotatePages(0, pdfEdit.PageCount - 1, false);
            pdfEdit.SaveTo(newPdf);
        }

        [Test]
        [TestCase("ZuABC.pdf")]
        public void DeletePage(string name)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));

            var newPdf = TempUtil.Get(name + $".DeletePage.pdf");
            pdfEdit.DeletePages(0, 0);
            pdfEdit.SaveTo(newPdf);
        }

        [Test]
        [TestCase("title.pdf", "ZuABC.pdf")]
        [TestCase("ZuABC.pdf", "title.pdf")]
        public void AppendPage(string pdf1, string pdf2)
        {
            var newPdf = TempUtil.Get(pdf1 + $".AppendPage.pdf");
            File.Copy(Path.Combine(FilesDir, pdf1), newPdf, true);
            pdfEdit.OpenFile(newPdf);
            pdfEdit.AppendPages(Path.Combine(FilesDir, pdf2));
            pdfEdit.SaveTo(newPdf);
        }

        private void PrepNewFile(string newPdf)
        {
            if (File.Exists(newPdf))
            {
                File.Delete(newPdf);
            }

            using (File.Create(newPdf))
            {

            }
        }

        [Test]
        [TestCase("SH", FileAttributes.System | FileAttributes.Hidden, "ZuABC.pdf")]
        [TestCase("S", FileAttributes.System, "ZuABC.pdf")]
        [TestCase("H", FileAttributes.Hidden, "ZuABC.pdf")]
        [TestCase("N", FileAttributes.Normal, "ZuABC.pdf")]
        public void SaveToAttr(string mark, FileAttributes atts, string name)
        {
            pdfEdit.OpenFile(Path.Combine(FilesDir, name));

            var newPdf = TempUtil.Get(name + $".{mark}.pdf");
            PrepNewFile(newPdf);
            File.SetAttributes(newPdf, atts);
            pdfEdit.SaveTo(newPdf);
        }
    }
}
