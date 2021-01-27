using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yPDFEditor.Enums
{
    [Flags]
    public enum TvInsertMarkLocation
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
    }
}
