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

        Bitmap pic = null;

        public Bitmap Pic
        {
            get { return pic; }
            set
            {
                if (pic != value)
                {
                    pic = value;

                    Recalc();
                    Invalidate();

                    if (PicChanged != null)
                        PicChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler PicChanged;

        private void Recalc()
        {
            if (pic == null)
            {
                sizeDisp = Size.Empty;
                sizePic = SizeF.Empty;
                return;
            }
            using (Graphics cv = CreateGraphics())
            {
                float rx = cv.DpiX;
                float ry = cv.DpiY;
                float px = pic.HorizontalResolution;
                float py = pic.VerticalResolution;
                sizePic = new SizeF(
                    rx / px * pic.Width,
                    ry / py * pic.Height
                    );
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
                        sizeDisp = FitRect3.Fit(ClientRectangle, pic.Size).Size;
                        break;
                }

                AutoScrollMinSize = Size.Round(sizeDisp);
            }
        }

        SizeF sizePic = SizeF.Empty;

        public float ActualRate
        {
            get
            {
                if (sizePic.IsEmpty)
                    return 1;
                return sizeDisp.Width / sizePic.Width;
            }
        }

        SizeF sizeDisp = SizeF.Empty;

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

            if (pic != null)
            {
                cv.InterpolationMode = InterpolationMode.High;

                cv.DrawImage(pic, RUt.Centerize(this.ClientSize, new Rectangle(AutoScrollPosition, Size.Round(sizeDisp))), new Rectangle(Point.Empty, pic.Size), GraphicsUnit.Pixel);

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

                    Recalc();
                    Invalidate();

                    if (FitCnfChanged != null)
                        FitCnfChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler FitCnfChanged;

        private void PreViewer_Resize(object sender, EventArgs e)
        {
            Recalc();
            Invalidate();
        }
    }

}
