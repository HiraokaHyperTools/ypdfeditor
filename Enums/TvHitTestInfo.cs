using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yPDFEditor.Enums
{
    public class TvHitTestInfo
    {
        int i;
        TvHitTestLocation location;

        public TvHitTestInfo(int i, TvHitTestLocation location)
        {
            this.i = i;
            this.location = location;
        }

        public int Item { get { return i; } }
        public TvHitTestLocation Location { get { return location; } }

        public override string ToString()
        {
            return string.Format("Item={0}, Location={1}", i, location);
        }
    }
}
