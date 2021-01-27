using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yPDFEditor.Delegates;

namespace yPDFEditor.Utils
{
    public class PDFClip : MarshalByRefObject
    {
        public int PId;
        public int SelFirst;
        public int SelLast;

        public GetPDFDelegate GetPDF;
    }
}
