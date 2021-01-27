using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yPDFEditor.Enums;

namespace yPDFEditor.Utils
{
    public struct FitCnf : IComparable<FitCnf>
    {
        float rate;
        SizeSpec sizeSpec;

        public FitCnf(float rate)
        {
            this.rate = rate;
            this.sizeSpec = SizeSpec.ResoRate;
        }

        public FitCnf(SizeSpec spec)
        {
            this.rate = 1;
            this.sizeSpec = spec;
        }

        public float Rate { get { return rate; } }
        public SizeSpec SizeSpec { get { return sizeSpec; } }

        public override bool Equals(object obj) { return (obj is FitCnf) ? CompareTo((FitCnf)obj) == 0 : base.Equals(obj); }
        public override int GetHashCode() { return Convert.ToInt32(sizeSpec) ^ BitConverter.ToInt32(BitConverter.GetBytes(rate), 0); }

        public static bool operator ==(FitCnf x, FitCnf y) { return x.CompareTo(y) == 0; }
        public static bool operator !=(FitCnf x, FitCnf y) { return x.CompareTo(y) != 0; }

        #region IComparable<FitCnf> メンバ

        public int CompareTo(FitCnf other)
        {
            int t;
            t = sizeSpec.CompareTo(other.sizeSpec); if (t != 0) return t;
            t = rate.CompareTo(other.rate); if (t != 0) return t;
            return t;
        }

        #endregion
    }
}
