using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yPDFEditor.Enums;

namespace yPDFEditor.Utils
{
    public struct TvInsertMark : IComparable<TvInsertMark>
    {
        int i;
        TvInsertMarkLocation loc;

        public TvInsertMark(int i)
        {
            this.i = i;
            this.loc = TvInsertMarkLocation.None;
        }

        public TvInsertMark(int i, TvInsertMarkLocation loc)
        {
            this.i = i;
            this.loc = loc;
        }

        public int Item { get { return i; } }
        public TvInsertMarkLocation Location { get { return loc; } }

        public override bool Equals(object obj) { return (obj is TvInsertMark) ? CompareTo((TvInsertMark)obj) == 0 : base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(TvInsertMark x, TvInsertMark y) { return x.CompareTo(y) == 0; }
        public static bool operator !=(TvInsertMark x, TvInsertMark y) { return x.CompareTo(y) != 0; }

        #region IComparable<InsertMark> メンバ

        public int CompareTo(TvInsertMark other)
        {
            int t;
            t = i.CompareTo(other.i); if (t != 0) return t;
            t = loc.CompareTo(other.loc); if (t != 0) return t;
            return 0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Item={0}, Location={1}", i, loc);
        }
    }
}
