using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    public class TvPict : IDisposable
    {
        internal Bitmap pic = null;
        private PDFExploder pdfExp;
        private int index;

        public TvPict(PDFExploder pdfExp, int index)
        {
            this.thumbGen = new ThumbGen(this);
            this.pdfExp = pdfExp;
            this.index = index;
        }

        public Bitmap Picture
        {
            get
            {
                if (pic == null)
                {
                    PagePic pp = pdfExp.Extract(1 + index, null, 96, false);
                    pic = pp.LoadPic();
                }
                return pic;
            }
            set
            {
                if (PictureChanging != null) PictureChanging(this, new EventArgs());

                this.pic = value;

                if (PictureChanged != null) PictureChanged(this, new EventArgs());
            }
        }

        public void Rotate(bool fr)
        {
            throw new NotSupportedException();
        }

        public event EventHandler PictureChanging, PictureChanged;

        ThumbGen thumbGen;

        public ThumbGen DefTumbGen
        {
            get
            {
                return thumbGen;
            }
        }

        #region IDisposable メンバ

        public void Dispose()
        {
            pic.Dispose();
        }

        #endregion

        public TvPict Clone()
        {
            TvPict o = (TvPict)MemberwiseClone();
            o.pic = (Bitmap)pic.Clone();
            return o;
        }

        public void Relocate(PDFExploder pdfexp, int i)
        {
            this.pdfExp = pdfexp;
            this.index = i;
        }
    }
}
