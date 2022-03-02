using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using yPDFEditor.Utils;
using yPDFEditor.Enums;

namespace yPDFEditor
{
    public partial class PreViewer : UserControl
    {
        public PreViewer()
        {
            InitializeComponent();
        }

        private class Recalc
        {
            internal SizeF sizePic = SizeF.Empty;
            internal SizeF sizeDisp = SizeF.Empty;
        }

        private ThumbSet pict = null;
        private Recalc recalc = null;

        public Func<ThumbSet> ThumbSetProvider { get; set; }

        public ThumbSet Pict
        {
            get => pict;
            set
            {
                if (pict != value)
                {
                    pict = value;
                    recalc = null;

                    Invalidate();

                    PictChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler PictChanged;

        private Recalc GetRecalc()
        {
            if (pict == null)
            {
                return recalc = new Recalc { };
            }
            using (Graphics cv = CreateGraphics())
            {
                var picture = ResolvePicture();

                float rx = cv.DpiX;
                float ry = cv.DpiY;
                float px = picture.HorizontalResolution;
                float py = picture.VerticalResolution;
                var sizePic = new SizeF(
                    rx / px * picture.Width,
                    ry / py * picture.Height
                    );
                SizeF sizeDisp = SizeF.Empty;
                FitCnf fc = fitcnf;
                switch (fc.SizeSpec)
                {
                    case SizeSpec.ResoRate:
                        {
                            sizeDisp = new SizeF(
                                sizePic.Width * fc.Rate,
                                sizePic.Height * fc.Rate
                            );
                            break;
                        }
                    case SizeSpec.FW:
                        {
                            Size client = ClientSize;
                            if (client.Width < sizePic.Width)
                            {
                                // 画面より大きい→縮小
                                sizeDisp = new SizeF(client.Width, sizePic.Height / sizePic.Width * client.Width);
                            }
                            else
                            {
                                // 同等・小さい→そのまま
                                sizeDisp = sizePic;
                            }
                            break;
                        }
                    case SizeSpec.FWH:
                        sizeDisp = FitRect3.Fit(ClientRectangle, picture.Size).Size;
                        break;
                }

                AutoScrollMinSize = Size.Round(sizeDisp);

                return recalc = new Recalc
                {
                    sizeDisp = sizeDisp,
                    sizePic = sizePic,
                };
            }
        }

        private Bitmap ResolvePicture()
        {
            var bitmap = pict?.Bitmap;
            if (bitmap == null)
            {
                var pict = ThumbSetProvider?.Invoke();
                if (pict != null)
                {
                    bitmap = pict.Bitmap;
                }
                if (!ReferenceEquals(pict, Pict))
                {
                    Pict = pict;
                }
            }
            return bitmap;
        }

        public float ActualRate
        {
            get
            {
                var recalc = GetRecalc();

                if (recalc.sizePic.IsEmpty)
                {
                    return 1;
                }
                return recalc.sizeDisp.Width / recalc.sizePic.Width;
            }
        }

        private void PreViewer_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        }

        private void PreViewer_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            if (DesignMode)
            {
                Rectangle rc = ClientRectangle;
                rc.Inflate(-1, -1);
                using (Pen pen = new Pen(this.ForeColor))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    cv.DrawRectangle(pen, rc);
                }
                rc.Inflate(-1, -1);
                using (HatchBrush br = new HatchBrush(HatchStyle.Wave, this.ForeColor, Color.Transparent))
                {
                    cv.FillRectangle(br, rc);
                }

                return;
            }

            var picture = ResolvePicture();

            if (picture != null)
            {
                var recalc = GetRecalc();

                cv.InterpolationMode = InterpolationMode.High;

                cv.DrawImage(picture, RUt.Centerize(this.ClientSize, new Rectangle(AutoScrollPosition, Size.Round(recalc.sizeDisp))), new Rectangle(Point.Empty, picture.Size), GraphicsUnit.Pixel);

                cv.InterpolationMode = InterpolationMode.Default;
            }
        }

        class RUt
        {
            internal static Rectangle Centerize(Size clientSize, Rectangle rcDest)
            {
                if (rcDest.Width < clientSize.Width)
                {
                    rcDest.Offset((clientSize.Width - rcDest.Width) / 2, 0);
                }
                if (rcDest.Height < clientSize.Height)
                {
                    rcDest.Offset(0, (clientSize.Height - rcDest.Height) / 2);
                }
                return rcDest;
            }
        }

        FitCnf fitcnf = new FitCnf(SizeSpec.FWH);

        public FitCnf FitCnf
        {
            get { return fitcnf; }
            set
            {
                if (fitcnf != value)
                {
                    fitcnf = value;

                    recalc = null;
                    Invalidate();

                    FitCnfChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler FitCnfChanged;

        private void PreViewer_Resize(object sender, EventArgs e)
        {
            recalc = null;
            Invalidate();
        }
    }

}
