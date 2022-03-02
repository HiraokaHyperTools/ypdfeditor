using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yPDFEditor.Enums;

namespace yPDFEditor.Utils
{
    public struct TvInsertMark : IComparable<TvInsertMark>
    {
        public TvInsertMark(int index)
        {
            Index = index;
            Location = TvInsertMarkLocation.None;
        }

        public TvInsertMark(int index, TvInsertMarkLocation location)
        {
            Index = index;
            Location = location;
        }

        public int Index { get; private set; }
        public TvInsertMarkLocation Location { get; private set; }

        public override bool Equals(object obj) { return (obj is TvInsertMark) ? CompareTo((TvInsertMark)obj) == 0 : base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(TvInsertMark x, TvInsertMark y) { return x.CompareTo(y) == 0; }
        public static bool operator !=(TvInsertMark x, TvInsertMark y) { return x.CompareTo(y) != 0; }

        #region IComparable<InsertMark> メンバ

        public int CompareTo(TvInsertMark other)
        {
            int t;
            t = Index.CompareTo(other.Index); if (t != 0) return t;
            t = Location.CompareTo(other.Location); if (t != 0) return t;
            return 0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Index={0}, Location={1}", Index, Location);
        }
    }
}
