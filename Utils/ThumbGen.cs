using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace yPDFEditor.Utils
{
    public class ThumbGen
    {
        private TvPict parent;

        private SortedDictionary<string, Bitmap> dict = new SortedDictionary<string, Bitmap>();

        internal ThumbGen(TvPict parent)
        {
            this.parent = parent;
            this.parent.PictureChanged += new EventHandler(parent_PictureChanged);
        }

        ~ThumbGen()
        {
            this.parent.PictureChanged -= new EventHandler(parent_PictureChanged);
        }

        void parent_PictureChanged(object sender, EventArgs e)
        {
            foreach (Bitmap pic in dict.Values) pic.Dispose();
            dict.Clear();
        }

        public Bitmap GetThumbnail(Size size)
        {
            if (size.IsEmpty)
                size = parent.Picture.Size;
            return GetThumbnail(size.Width, size.Height);
        }

        public Bitmap GetThumbnail(int cx, int cy)
        {
            String key = cx + "," + cy;
            Bitmap th;
            if (dict.TryGetValue(key, out th))
            {
                return th;
            }
            else
            {
                Rectangle rc = FitRect3.Fit(new Rectangle(0, 0, cx, cy), parent.Picture.Size);
                Bitmap pic = (Bitmap)parent.pic.GetThumbnailImage(rc.Width, rc.Height, (Image.GetThumbnailImageAbort)delegate { return false; }, IntPtr.Zero);
                return dict[key] = pic;
            }
        }
    }
}
