using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    public class PagePic
    {
        MemoryStream os;

        public PagePic(MemoryStream os)
        {
            this.os = os;
        }

        public Bitmap LoadPic()
        {
            return new Bitmap(os);
        }
    }
}
