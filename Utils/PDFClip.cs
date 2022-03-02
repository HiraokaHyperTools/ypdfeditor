using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yPDFEditor.Delegates;

namespace yPDFEditor.Utils
{
    public class PDFClip : MarshalByRefObject
    {
        public int ProcessId;
        public int SelectionFirst;
        public int SelectionLast;

        public GetPDFDelegate GetPDF;
    }
}
