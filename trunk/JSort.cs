using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace yPDFEditor {
    public partial class JSort : Form {
        public JSort() {
            InitializeComponent();
        }

        private void JSort_Load(object sender, EventArgs e) {

        }

        public int[] Indices = new int[0];

        public void Set(BindingList<TvPict> picts) {
            int cx = picts.Count;
            {
                int PW = 45;
                int PH = 60;
                int M = 5, L = 10;
                StringFormat sf = new StringFormat();
                sf.LineAlignment = sf.Alignment = StringAlignment.Center;
                List<int> pis = new List<int>();
                for (int x = 0; x < cx; x++) {
                    int pi = 1 + ((x < cx / 2) ? 2 * x : 2 * cx - x * 2 - 1);
                    pis.Add(pi);
                }
                bW1.Tag = pis.ToArray();
                for (int x = 0; x < cx; x++) {
                    Bitmap pic = new Bitmap(PW, PH);
                    using (Graphics cv = Graphics.FromImage(pic)) {
                        cv.FillPolygon(Brushes.White, new Point[] { 
                            new Point(M,PH-M),
                            new Point(M,M+L),
                            new Point(M+L,M),
                            new Point(PW-M,M),
                            new Point(PW-M,PH-M),
                        });
                        cv.DrawPolygon(Pens.Black, new Point[] { 
                            new Point(M,PH-M),
                            new Point(M,M+L),
                            new Point(M+L,M+L),
                            new Point(M+L,M),
                            new Point(PW-M,M),
                            new Point(PW-M,PH-M),
                        });
                        cv.DrawLine(Pens.Black, M, M + L, M + L, M);
                        String s = "" + pis[x];
                        cv.DrawString(s, Font, Brushes.Black, Rectangle.FromLTRB(0, M, PW, PH), sf);
                    }
                    PictureBox pb = new PictureBox();
                    pb.Image = pic;
                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb.Parent = flpW1;
                    pb.Show();
                }
            }
        }

        private void bW1_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Indices = (int[])bW1.Tag;
            Close();
        }
    }
}