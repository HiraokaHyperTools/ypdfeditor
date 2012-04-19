using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace yPDFEditor {
    public partial class ThumbView : UserControl {
        public ThumbView() {
            InitializeComponent();
        }

        private void ThumbView_Load(object sender, EventArgs e) {
            DoubleBuffered = true;
        }

        [NonSerialized()]
        BindingList<TvPict> picts = null;

        public BindingList<TvPict> Picts {
            get {
                return picts;
            }
            set {
                if (this.picts != null) picts.ListChanged -= new ListChangedEventHandler(Picts_ListChanged);

                this.picts = value;

                if (this.picts != null) picts.ListChanged += new ListChangedEventHandler(Picts_ListChanged);
            }
        }

        void Picts_ListChanged(object sender, ListChangedEventArgs e) {
            switch (e.ListChangedType) {
                case ListChangedType.ItemAdded:
                    if (e.NewIndex <= SelLast) {
                        iSel++;
                        iSel2++;

                        LayoutClient();

                        if (SelChanged != null)
                            SelChanged(this, e);
                    }
                    else {
                        LayoutClient();
                    }
                    break;

                case ListChangedType.ItemDeleted: {
                        bool isUp = false;

                        if (e.NewIndex < SelFirst || SelFirst == picts.Count) {
                            if (iSel < iSel2) iSel--; else iSel2--;
                            isUp = true;
                        }
                        if (e.NewIndex < SelLast || SelLast == picts.Count) {
                            if (iSel < iSel2) iSel2--; else iSel--;
                            isUp = true;
                        }

                        bool itsMe = e.NewIndex == SelFirst || e.NewIndex == SelLast;

                        LayoutClient();

                        if (isUp || itsMe)
                            if (SelChanged != null)
                                SelChanged(this, e);

                        break;
                    }

                case ListChangedType.ItemMoved:
                case ListChangedType.Reset:
                    LayoutClient();
                    if (SelChanged != null)
                        SelChanged(this, e);
                    break;
                case ListChangedType.ItemChanged:
                    if (SelChanged != null)
                        SelChanged(this, e);
                    Invalidate();
                    break;
            }
        }

        Size MaxBox { get { return new Size(200, 280); } }

        Rectangle RectPic {
            get {
                Size size = MaxBox;
                return Rectangle.FromLTRB(15, 15, size.Width - 15, size.Height - 20);
            }
        }

        public bool IsSelected(int i) {
            if (0 <= i && SelFirst <= i && i <= SelLast)
                return true;
            return false;
        }

        class Lay {
            public int i;
            public Rectangle rcBound, rcPic;

            public Lay(int i, Rectangle rcBound, Rectangle rcPic) {
                this.i = i;
                this.rcBound = rcBound;
                this.rcPic = rcPic;
            }
        }
        List<Lay> lays = new List<Lay>();
        int cxTile = 0, cyTile = 0;

        [Browsable(false)]
        public int RowCount { get { return cyTile; } }

        [Browsable(false)]
        public int ColumnCount { get { return cxTile; } }

        void LayoutClient() {
            int cnt = (picts == null) ? 0 : picts.Count;
            this.cxTile = Math.Max(1, ClientSize.Width / MaxBox.Width);
            this.cyTile = Math.Max(1, (cnt + cxTile - 1) / (cxTile));

            lays.Clear();

            if (cnt != 0) {
                for (int y = 0, i = 0; y < cyTile || i < cnt; y++) {
                    for (int x = 0; x < cxTile && i < cnt; x++, i++) {
                        Rectangle rc1 = new Rectangle(new Point(MaxBox.Width * x, MaxBox.Height * y), MaxBox);
                        Rectangle rc2 = RectPic;
                        rc2.Offset(rc1.Location);
                        lays.Add(new Lay(i, rc1, rc2));
                    }
                }

                if (iSel < 0 && lays.Count != 0)
                    SSel = 0;

                AutoScrollMinSize = new Size(MaxBox.Width, MaxBox.Height * cyTile);
            }
            else {
                if (iSel >= 0)
                    SSel = -1;

                AutoScrollMinSize = Size.Empty;
            }

            Invalidate();
        }

        TvInsertMark iMark = new TvInsertMark(-1);

        [Browsable(false)]
        public TvInsertMark InsertMark {
            get { return iMark; }
            set {
                if (iMark != value) {
                    iMark = value;

                    Invalidate();
                }
            }
        }

        private void ThumbView_Paint(object sender, PaintEventArgs e) {
            Graphics cv = e.Graphics;

            if (DesignMode) {
                Rectangle rc = ClientRectangle;
                rc.Inflate(-1, -1);
                using (Pen pen = new Pen(this.ForeColor)) {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    cv.DrawRectangle(pen, rc);
                }
                rc.Inflate(-1, -1);
                using (HatchBrush br = new HatchBrush(HatchStyle.Divot, this.ForeColor, Color.Transparent)) {
                    cv.FillRectangle(br, rc);
                }

                return;
            }

            foreach (Lay lay in lays) {
                Rectangle rc1 = lay.rcBound;
                rc1.Offset(AutoScrollPosition);
                if (e.ClipRectangle.IntersectsWith(rc1)) {
                    int i = lay.i;
                    Bitmap pic = picts[i].DefTumbGen.GetThumbnail(RectPic.Size);
                    Rectangle rc2 = lay.rcPic;
                    rc2.Offset(AutoScrollPosition);
                    cv.DrawImage(pic, FitRect3.Fit(rc2, pic.Size));
                    cv.DrawRectangle(Pens.Black, rc2);

                    {
                        Rectangle rcBorder = rc2;
                        Pen p = Pens.Transparent;
                        switch (GetSelLv(i)) {
                            case SelLv.Sel: p = pSel; break;
                            case SelLv.SelFoc: p = pSelFoc; break;
                        }
                        rcBorder.Inflate(1, 1);
                        if (p != Pens.Transparent)
                            for (int w = 0; w < 3; w++) {
                                rcBorder.Inflate(1, 1);
                                cv.DrawRectangle(p, rcBorder);
                            }
                    }

                    if (iMark.Item == lay.i) {
                        int w = 8, hw = w / 2;
                        Rectangle rc3 = rc1;
                        rc3.Inflate(-1, -1);
                        if (0 != (iMark.Location & TvInsertMarkLocation.Right)) {
                            Point[] pts = new Point[] { 
                                new Point(rc3.Right -1,      rc3.Top),
                                new Point(rc3.Right -1 - w,  rc3.Top),
                                new Point(rc3.Right -1 - hw, rc3.Top       +hw),
                                new Point(rc3.Right -1 - hw, rc3.Bottom -1 -hw),
                                new Point(rc3.Right -1 - w,  rc3.Bottom -1),
                                new Point(rc3.Right -1,      rc3.Bottom -1),
                                new Point(rc3.Right -1,      rc3.Top),
                            };
                            cv.FillPolygon(Brushes.Blue, pts);
                        }
                        if (0 != (iMark.Location & TvInsertMarkLocation.Left)) {
                            Point[] pts = new Point[] { 
                                new Point(rc3.X,      rc3.Top),
                                new Point(rc3.X + w,  rc3.Top),
                                new Point(rc3.X + hw, rc3.Top       +hw),
                                new Point(rc3.X + hw, rc3.Bottom -1 -hw),
                                new Point(rc3.X + w,  rc3.Bottom -1),
                                new Point(rc3.X,      rc3.Bottom -1),
                                new Point(rc3.X,      rc3.Top),
                            };
                            cv.FillPolygon(Brushes.Blue, pts);
                        }
                        if (0 != (iMark.Location & TvInsertMarkLocation.Top)) {
                            Point[] pts = new Point[] { 
                                new Point(rc3.Left,          rc3.Top),
                                new Point(rc3.Left,          rc3.Top + w),
                                new Point(rc3.Left + hw,     rc3.Top + hw),
                                new Point(rc3.Right -1 - hw, rc3.Top + hw),
                                new Point(rc3.Right -1,      rc3.Top + w),
                                new Point(rc3.Right -1,      rc3.Top),
                                new Point(rc3.X,             rc3.Top),
                            };
                            cv.FillPolygon(Brushes.Blue, pts);
                        }
                        if (0 != (iMark.Location & TvInsertMarkLocation.Bottom)) {
                            Point[] pts = new Point[] { 
                                new Point(rc3.Left,          rc3.Bottom -1),
                                new Point(rc3.Left,          rc3.Bottom -1 - w),
                                new Point(rc3.Left + hw,     rc3.Bottom -1 - hw),
                                new Point(rc3.Right -1 - hw, rc3.Bottom -1 - hw),
                                new Point(rc3.Right -1,      rc3.Bottom -1 - w),
                                new Point(rc3.Right -1,      rc3.Bottom -1),
                                new Point(rc3.X,             rc3.Bottom -1),
                            };
                            cv.FillPolygon(Brushes.Blue, pts);
                        }
                    }
                }
            }

            if (fSelDrag) {
                cv.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(Color.Blue, 1.5f)) {
                    pen.DashStyle = DashStyle.Dash;

                    Rectangle rc = RectDrag;
                    rc.Offset(AutoScrollPosition);
                    cv.DrawRectangle(pen, rc);
                }

                cv.SmoothingMode = SmoothingMode.Default;
            }
        }

        // AutoScrollPosition加算済み
        Rectangle RectDrag {
            get {
                return Rectangle.FromLTRB(
                    Math.Min(pt0.X, pt1.X),
                    Math.Min(pt0.Y, pt1.Y),
                    Math.Max(pt0.X, pt1.X),
                    Math.Max(pt0.Y, pt1.Y)
                    ) ;
            }
        }

        Pen pSelFoc = new Pen(Color.FromArgb(50, 50, 250 ));
        Pen pSel = new Pen(Color.FromArgb(150, 150, 200));

        private void ThumbView_Resize(object sender, EventArgs e) {
            LayoutClient();
        }

        int iSel = -1, iSel2 = -1;

        public event EventHandler SelChanged;

        [Browsable(false)]
        public int SSel {
            get {
                return iSel;
            }
            set {
                int newSel = Math.Min(lays.Count - 1, Math.Max(0, value));
                if (newSel == iSel2 && newSel == iSel)
                    return;

                iSel = iSel2 = newSel;
                MakeVisible(iSel2);
                Invalidate();

                if (SelChanged != null)
                    SelChanged(this, new EventArgs());
            }
        }

        [Browsable(false)]
        public int SelFirst { get { return Math.Min(iSel, iSel2); } }

        [Browsable(false)]
        public int SelLast { get { return Math.Max(iSel, iSel2); } }

        [Browsable(false)]
        public int SelCount {
            get {
                if (SelFirst < 0)
                    return 0;
                return SelLast - SelFirst + 1;
            }
        }

        [Browsable(false)]
        public int Sel2 {
            get {
                return iSel2;
            }
            set {
                int newSel = Math.Min(lays.Count - 1, Math.Max(0, value));
                if (newSel == iSel2)
                    return;

                iSel2 = newSel;
                MakeVisible(iSel2);
                Invalidate();

                if (SelChanged != null)
                    SelChanged(this, new EventArgs());
            }
        }

        enum SelLv {
            None, Sel, SelFoc,
        }

        SelLv GetSelLv(int i) {
            if (i == iSel2)
                return SelLv.SelFoc;
            if (iSel <= iSel2 && iSel <= i && i <= iSel2)
                return SelLv.Sel;
            if (iSel2 <= iSel && iSel2 <= i && i <= iSel)
                return SelLv.Sel;

            return SelLv.None;
        }

        private void MakeVisible(int iSel2) {
            foreach (Lay lay in lays) {
                if (lay.i == iSel2) {
                    Point pos = Point.Empty - new Size(AutoScrollPosition);
                    Rectangle rc = new Rectangle(pos, ClientSize);

                    if (lay.rcBound.X < rc.X) {
                        // ←へ
                        pos.X = lay.rcBound.X;
                    }
                    else if (rc.Right < lay.rcBound.Right) {
                        // →へ
                        pos.X = lay.rcBound.Right - rc.Width;
                    }

                    if (lay.rcBound.Y < rc.Y) {
                        // ↑へ
                        pos.Y = lay.rcBound.Y;
                    }
                    else if (rc.Bottom < lay.rcBound.Bottom) {
                        // ↓へ
                        pos.Y = lay.rcBound.Bottom - rc.Height;
                    }

                    AutoScrollPosition = (pos);
                    break;
                }
            }
        }

        private void ThumbView_KeyDown(object sender, KeyEventArgs e) {
            if (e.Alt || e.Control) {

            }
            else if (e.Shift) {
                switch (e.KeyCode) {
                    case Keys.Home:
                        Sel2 = 0;
                        break;
                    case Keys.End:
                        Sel2 = int.MaxValue;
                        break;
                    case Keys.Left:
                        Sel2--;
                        break;
                    case Keys.Right:
                        Sel2++;
                        break;
                    case Keys.Up:
                        Sel2 -= cxTile;
                        break;
                    case Keys.Down:
                        Sel2 += cxTile;
                        break;
                }
            }
            else {
                switch (e.KeyCode) {
                    case Keys.Home:
                        SSel = 0;
                        break;
                    case Keys.End:
                        SSel = int.MaxValue;
                        break;
                    case Keys.Left:
                        SSel = Sel2 - 1;
                        break;
                    case Keys.Right:
                        SSel = Sel2 + 1;
                        break;
                    case Keys.Up:
                        SSel = Sel2 - cxTile;
                        break;
                    case Keys.Down:
                        SSel = Sel2 + cxTile;
                        break;
                }
            }
        }

        private void ThumbView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.Alt || e.Control) {

            }
            else {
                switch (e.KeyCode) {
                    case Keys.Home:
                    case Keys.End:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        e.IsInputKey = true;
                        break;
                }
            }
        }

        bool fSelDrag = false;

        // AutoScrollPosition加算済み
        Point pt0 = Point.Empty, pt1 = Point.Empty;

        int fPictDragChance = -1;

        bool fSkipUp = true;

        private void ThumbView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                Point pt = e.Location - new Size(AutoScrollPosition);
                int i = HitTestPic(pt.X, pt.Y);
                if (0 != (ModifierKeys & (Keys.Alt | Keys.Control))) {

                }
                else {
                    fSkipUp = true;

                    if (i < 0 && e.Button == MouseButtons.Left) {
                        // Start dragging
                        this.fSelDrag = true;
                        this.pt0 = this.pt1 = pt;

                        SSel = Sel2;
                    }
                    else {
                        // Select it
                        fPictDragChance = 10;

                        if (!IsSelected(i)) {
                            if (0 != (ModifierKeys & Keys.Shift)) {
                                Sel2 = i;
                            }
                            else {
                                SSel = i;
                            }
                        }
                        else fSkipUp = false;
                    }
                }
            }
        }

        public event EventHandler PictDrag;

        /// <summary>
        /// 画像を探す
        /// </summary>
        /// <param name="x">絶対 x座標</param>
        /// <param name="y">絶対 y座標</param>
        /// <returns>0以上…アイテム番号、0未満…未発見</returns>
        public int HitTestPic(int x, int y) {
            foreach (Lay lay in lays) {
                if (lay.rcPic.Contains(x, y))
                    return lay.i;
            }
            return -1;
        }

        /// <summary>
        /// アイテムを探す
        /// </summary>
        /// <param name="x">クライアントx座標</param>
        /// <param name="y">クライアントy座標</param>
        /// <returns>0以上…アイテム番号、0未満…未発見</returns>
        public TvHitTestInfo HitTest(int x, int y) {
            return HitTest(new Point(x, y));
        }

        /// <summary>
        /// アイテムを探す
        /// </summary>
        /// <param name="pt">クライアント座標</param>
        /// <returns></returns>
        public TvHitTestInfo HitTest(Point pt) {
            pt = pt - new Size(AutoScrollPosition);
            int cnt = picts.Count;
            bool isEmpty = cnt == 0;
            if (!isEmpty) {
                foreach (Lay lay in lays) {
                    if (lay.rcBound.Contains(pt)) {
                        return new TvHitTestInfo(lay.i, TvHitTestLocation.Box
                            | (lay.rcPic.Contains(pt) ? TvHitTestLocation.Pict : TvHitTestLocation.None)
                            );
                    }
                }
            }
            return new TvHitTestInfo(-1, TvHitTestLocation.None);
        }

        [Flags]
        public enum SuggestFlags {
            None = 0, OnlyUpDown = 1,
        }

        /// <summary>
        /// InsertMark候補を提案する
        /// </summary>
        /// <param name="pt">クライアント座標</param>
        /// <returns></returns>
        public TvInsertMark SuggestInsertMark(Point pt) {
            return SuggestInsertMark(pt, 0
                | (cxTile == 1 ? SuggestFlags.OnlyUpDown : 0)
                );
        }

        public TvInsertMark SuggestInsertMark(Point pt, SuggestFlags flags) {
            pt = pt - new Size(AutoScrollPosition);

            TvInsertMarkLocation before = (0 != (flags & SuggestFlags.OnlyUpDown)) ? TvInsertMarkLocation.Top : TvInsertMarkLocation.Left;
            TvInsertMarkLocation after = (0 != (flags & SuggestFlags.OnlyUpDown)) ? TvInsertMarkLocation.Bottom : TvInsertMarkLocation.Right;

            int cnt = picts.Count;
            bool isEmpty = cnt == 0;
            if (!isEmpty) {
                int cxItem = MaxBox.Width;
                int cyItem = MaxBox.Height;

                if (pt.Y < 0) {
                    // U
                    if (pt.X < 0) {
                        // TL
                        return new TvInsertMark(0, before);
                    }
                    else if (pt.X >= cxTile * cxItem) {
                        // TR
                        return new TvInsertMark(cxTile - 1, after);
                    }
                    else {
                        // Tx
                        return new TvInsertMark(pt.X / cxItem, before);
                    }
                }
                else if (pt.Y >= cyTile * cyItem) {
                    // B
                    if (pt.X < 0) {
                        // BL
                        return new TvInsertMark(cxTile * (cyTile - 1), before);
                    }
                }
                else {
                    // M
                    if (pt.X < 0) {
                        // ML
                        return new TvInsertMark(cxTile * (pt.Y / cyItem), before);
                    }
                    else if (pt.X >= cxTile * cxItem) {
                        // MR
                        return new TvInsertMark(cxTile * (1 + (pt.Y / cyItem)) - 1, after);
                    }
                    else {
                        // Mx
                        foreach (Lay lay in lays) {
                            if (lay.rcBound.Contains(pt)) {
                                bool atAfter = (0 != (flags & SuggestFlags.OnlyUpDown))
                                    ? pt.Y >= lay.rcBound.Top + lay.rcBound.Height / 3
                                    : pt.X >= lay.rcBound.Left + lay.rcBound.Width / 3
                                    ;
                                bool last = 0 == ((lay.i + 1) % cxTile);
                                if (atAfter && last)
                                    return new TvInsertMark(lay.i, after);
                                if (atAfter)
                                    return new TvInsertMark(lay.i + 1, before);
                                return new TvInsertMark(lay.i, before);
                            }
                        }
                    }
                }

                // Last
                return new TvInsertMark(cnt - 1, after);
            }

            return new TvInsertMark(0, before);
        }

        private void ThumbView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.Capture) {
                    if (fSelDrag) {
                        this.fSelDrag = false;
                        this.Refresh();
                    }
                    else if (!fSkipUp) {
                        Point pt = e.Location - new Size(AutoScrollPosition);
                        int i = HitTestPic(pt.X, pt.Y);
                        if (i >= 0)
                            SSel = i;
                    }
                }
            }
        }

        private void ThumbView_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Point pt = e.Location - new Size(AutoScrollPosition);
                if (this.Capture) {
                    if (fSelDrag) {
                        this.pt1 = pt;

                        Rectangle rc = RectDrag;
                        int iFirst = int.MaxValue;
                        int iLast = int.MinValue;
                        foreach (Lay lay in lays) {
                            if (rc.IntersectsWith(lay.rcPic)) {
                                iFirst = Math.Min(iFirst, lay.i);
                                iLast = Math.Max(iLast, lay.i);
                            }
                        }
                        if (iFirst != int.MaxValue && iLast != int.MinValue) {
                            if (iSel != iFirst || iSel2 != iLast) {
                                iSel2 = iFirst;
                                iSel = iLast;

                                if (SelChanged != null)
                                    SelChanged(this, new EventArgs());
                            }
                        }

                        this.Refresh();
                    }
                    else if (fPictDragChance == 0) {
                        if (PictDrag != null)
                            PictDrag(this, new EventArgs());

                        fPictDragChance = -1;
                    }
                    else {
                        fPictDragChance--;
                    }
                }
            }
        }


    }

    public class TvHitTestInfo {
        int i;
        TvHitTestLocation location;

        public TvHitTestInfo(int i, TvHitTestLocation location) {
            this.i = i;
            this.location = location;
        }

        public int Item { get { return i; } }
        public TvHitTestLocation Location { get { return location; } }

        public override string ToString() {
            return string.Format("Item={0}, Location={1}", i, location);
        }
    }

    [Flags]
    public enum TvHitTestLocation {
        None = 0,
        Box = 1,
        Pict = 2,
    }

    [Flags]
    public enum TvInsertMarkLocation {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
    }

    public struct TvInsertMark : IComparable<TvInsertMark> {
        int i;
        TvInsertMarkLocation loc;

        public TvInsertMark(int i) {
            this.i = i;
            this.loc = TvInsertMarkLocation.None;
        }

        public TvInsertMark(int i, TvInsertMarkLocation loc) {
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

        public int CompareTo(TvInsertMark other) {
            int t;
            t = i.CompareTo(other.i); if (t != 0) return t;
            t = loc.CompareTo(other.loc); if (t != 0) return t;
            return 0;
        }

        #endregion

        public override string ToString() {
            return string.Format("Item={0}, Location={1}", i, loc);
        }
    }

    public class TvPict : IDisposable {
        Bitmap pic = null;
        PDFExploder pdfexp;
        int index;

        public TvPict(PDFExploder pdfexp, int index) {
            this.thumbGen = new ThumbGen(this);
            this.pdfexp = pdfexp;
            this.index = index;
        }

        public Bitmap Picture {
            get {
                if (pic == null) {
                    PagePic pp = pdfexp.Extract(1 + index, null, 96, false);
                    pic = pp.LoadPic();
                }
                return pic;
            }
            set {
                if (PictureChanging != null) PictureChanging(this, new EventArgs());

                this.pic = value;

                if (PictureChanged != null) PictureChanged(this, new EventArgs());
            }
        }

        public void Rotate(bool fr) {
            throw new NotSupportedException();
        }

        public event EventHandler PictureChanging, PictureChanged;

        public class ThumbGen {
            TvPict parent;

            SortedDictionary<String, Bitmap> dict = new SortedDictionary<String, Bitmap>();

            internal ThumbGen(TvPict parent) {
                this.parent = parent;
                this.parent.PictureChanged += new EventHandler(parent_PictureChanged);
            }

            ~ThumbGen() {
                this.parent.PictureChanged -= new EventHandler(parent_PictureChanged);
            }

            void parent_PictureChanged(object sender, EventArgs e) {
                foreach (Bitmap pic in dict.Values) pic.Dispose();
                dict.Clear();
            }

            public Bitmap GetThumbnail(Size size) {
                if (size.IsEmpty)
                    size = parent.Picture.Size;
                return GetThumbnail(size.Width, size.Height);
            }

            public Bitmap GetThumbnail(int cx, int cy) {
                String key = cx + "," + cy;
                Bitmap th;
                if (dict.TryGetValue(key, out th)) {
                    return th;
                }
                else {
                    Rectangle rc = FitRect3.Fit(new Rectangle(0, 0, cx, cy), parent.Picture.Size);
                    Bitmap pic = (Bitmap)parent.pic.GetThumbnailImage(rc.Width, rc.Height, (Image.GetThumbnailImageAbort)delegate { return false; }, IntPtr.Zero);
                    return dict[key] = pic;
                }
            }
        }

        ThumbGen thumbGen;

        public ThumbGen DefTumbGen {
            get {
                return thumbGen;
            }
        }

        #region IDisposable メンバ

        public void Dispose() {
            pic.Dispose();
        }

        #endregion

        public TvPict Clone() {
            TvPict o = (TvPict)MemberwiseClone();
            o.pic = (Bitmap)pic.Clone();
            return o;
        }

        public void Relocate(PDFExploder pdfexp, int i) {
            this.pdfexp = pdfexp;
            this.index = i;
        }
    }
}
