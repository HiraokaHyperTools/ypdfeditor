using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    class PVUt
    {
        internal static bool PGExists(string fp)
        {
            return new FileInfo(fp).Length != 0;
        }
    }

}
