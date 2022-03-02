using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using yPDFEditor.Enums;

namespace yPDFEditor.Utils
{
    public sealed class ThumbSet : IDisposable
    {
        public Bitmap Bitmap { get; set; }
        public ThumbState State { get; set; } = ThumbState.Loaded;

        public static ThumbSet Delayed => new ThumbSet { State = ThumbState.Delayed, };

        public void Dispose()
        {
            Bitmap?.Dispose();
        }
    }
}
