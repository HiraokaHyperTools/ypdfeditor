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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using yPDFEditor.Properties;

namespace yPDFEditor
{
    public partial class ThumbView : UserControl
    {
        private int selectFrom = -1;
        private int selectTo = -1;
        private Bitmap loadingImage = Resources.ExpirationHS;
        private List<Lay> lays = new List<Lay>();
        private int cxTile = 0;
        private int cyTile = 0;
        private Size maxBox => new Size(200, 280);
        private Pen penSelFocus = new Pen(Color.FromArgb(50, 50, 250));
        private Pen penSel = new Pen(Color.FromArgb(150, 150, 200));
        private bool draggingNow = false;
        private TvInsertMark insertMark = new TvInsertMark(-1);
        private int numDragChance = -1;
        private bool skipMouseUp = true;

        /// <summary>
        /// AutoScrollPosition 加算済み
        /// </summary>
        private Point pt0 = Point.Empty;

        /// <summary>
        /// AutoScrollPosition 加算済み
        /// </summary>
        private Point pt1 = Point.Empty;

        public event EventHandler PictDrag;

        public ThumbView()
        {
            InitializeComponent();
        }

        public event EventHandler SelectionChanged;

        private void ThumbView_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        }

        [NonSerialized()]
        ObservableCollection<ThumbSet> picts = null;

        public Func<int, ThumbSet> ThumbSetProvider { get; set; }

        public ObservableCollection<ThumbSet> Picts
        {
            get
            {
                return picts;
            }
            set
            {
                if (picts != null)
                {
                    picts.CollectionChanged -= Picts_CollectionChanged;
                }

                picts = value;

                if (picts != null)
                {
                    picts.CollectionChanged += Picts_CollectionChanged;
                }
            }
        }

        private void Picts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewStartingIndex <= SelectionLast)
                        {
                            selectTo++;
                            selectFrom++;

                            LayoutClient();

                            SelectionChanged?.Invoke(this, e);
                        }
                        else
                        {
                            LayoutClient();
                        }
                        break;
                    }

                case NotifyCollectionChangedAction.Remove:
                    {
                        bool isUp = false;

                        if (e.NewStartingIndex < SelectionFirst || SelectionFirst == picts.Count)
                        {
                            if (selectTo < selectFrom)
                            {
                                selectTo--;
                            }
                            else
                            {
                                selectFrom--;
                            }
                            isUp = true;
                        }

                        if (e.NewStartingIndex < SelectionLast || SelectionLast == picts.Count)
                        {
                            if (selectTo < selectFrom)
                            {
                                selectFrom--;
                            }
                            else
                            {
                                selectTo--;
                            }
                            isUp = true;
                        }

                        bool itsMe = e.NewStartingIndex == SelectionFirst || e.NewStartingIndex == SelectionLast;

                        LayoutClient();

                        if (isUp || itsMe)
                        {
                            SelectionChanged?.Invoke(this, e);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                    {
                        LayoutClient();
                        SelectionChanged?.Invoke(this, e);
                        break;
                    }

                case NotifyCollectionChangedAction.Replace:
                    {
                        Invalidate();
                        break;
                    }
            }
        }

        private Rectangle RectPic
        {
            get
            {
                Size size = maxBox;
                return Rectangle.FromLTRB(15, 15, size.Width - 15, size.Height - 20);
            }
        }

        public bool IsSelected(int i)
        {
            if (0 <= i && SelectionFirst <= i && i <= SelectionLast)
            {
                return true;
            }
            return false;
        }

        private class Lay
        {
            public int pageIndex;
            public Rectangle boundRect;
            public Rectangle picRect;

            public Lay(int index, Rectangle rcBound, Rectangle rcPic)
            {
                this.pageIndex = index;
                this.boundRect = rcBound;
                this.picRect = rcPic;
            }
        }

        [Browsable(false)]
        public int RowCount { get { return cyTile; } }

        [Browsable(false)]
        public int ColumnCount { get { return cxTile; } }

        void LayoutClient()
        {
            int cnt = picts?.Count ?? 0;
            cxTile = Math.Max(1, ClientSize.Width / maxBox.Width);
            cyTile = Math.Max(1, (cnt + cxTile - 1) / (cxTile));

            lays.Clear();

            if (cnt != 0)
            {
                int index = 0;
                for (int y = 0; y < cyTile || index < cnt; y++)
                {
                    for (int x = 0; x < cxTile && index < cnt; x++, index++)
                    {
                        Rectangle rc1 = new Rectangle(new Point(maxBox.Width * x, maxBox.Height * y), maxBox);
                        Rectangle rc2 = RectPic;
                        rc2.Offset(rc1.Location);
                        lays.Add(new Lay(index, rc1, rc2));
                    }
                }

                if (selectTo < 0 && lays.Count != 0)
                {
                    SelectTo = 0;
                }

                AutoScrollMinSize = new Size(maxBox.Width, maxBox.Height * cyTile);
            }
            else
            {
                if (selectTo >= 0)
                {
                    SelectTo = -1;
                }

                AutoScrollMinSize = Size.Empty;
            }

            Invalidate();
        }

        [Browsable(false)]
        public TvInsertMark InsertMark
        {
            get => insertMark;
            set
            {
                if (insertMark != value)
                {
                    insertMark = value;

                    Invalidate();
                }
            }
        }

        private void ThumbView_Paint(object sender, PaintEventArgs e)
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
                using (HatchBrush br = new HatchBrush(HatchStyle.Divot, this.ForeColor, Color.Transparent))
                {
                    cv.FillRectangle(br, rc);
                }

                return;
            }

            foreach (var lay in lays)
            {
                Rectangle rc1 = lay.boundRect;
                rc1.Offset(AutoScrollPosition);
                if (e.ClipRectangle.IntersectsWith(rc1))
                {
                    int i = lay.pageIndex;
                    if (picts[i].State == ThumbState.Delayed)
                    {
                        picts[i] = ThumbSetProvider(i);
                    }
                    Bitmap pic = picts[i].Bitmap ?? loadingImage;
                    Rectangle rc2 = lay.picRect;
                    rc2.Offset(AutoScrollPosition);
                    cv.DrawImage(pic, FitRect3.Fit(rc2, pic.Size));
                    cv.DrawRectangle(Pens.Black, rc2);

                    {
                        Rectangle rcBorder = rc2;
                        Pen p = Pens.Transparent;
                        switch (GetSelectionLevelOf(i))
                        {
                            case SelectionLevel.Selected: p = penSel; break;
                            case SelectionLevel.Focused: p = penSelFocus; break;
                        }
                        rcBorder.Inflate(1, 1);
                        if (p != Pens.Transparent)
                        {
                            for (int w = 0; w < 3; w++)
                            {
                                rcBorder.Inflate(1, 1);
                                cv.DrawRectangle(p, rcBorder);
                            }
                        }
                    }

                    if (insertMark.Index == lay.pageIndex)
                    {
                        int w = 8, hw = w / 2;
                        Rectangle rc3 = rc1;
                        rc3.Inflate(-1, -1);
                        if (0 != (insertMark.Location & TvInsertMarkLocation.Right))
                        {
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
                        if (0 != (insertMark.Location & TvInsertMarkLocation.Left))
                        {
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
                        if (0 != (insertMark.Location & TvInsertMarkLocation.Top))
                        {
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
                        if (0 != (insertMark.Location & TvInsertMarkLocation.Bottom))
                        {
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

            if (draggingNow)
            {
                cv.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(Color.Blue, 1.5f))
                {
                    pen.DashStyle = DashStyle.Dash;

                    Rectangle rc = RectDrag;
                    rc.Offset(AutoScrollPosition);
                    cv.DrawRectangle(pen, rc);
                }

                cv.SmoothingMode = SmoothingMode.Default;
            }
        }

        /// <summary>
        /// AutoScrollPosition 加算済み
        /// </summary>
        private Rectangle RectDrag
        {
            get
            {
                return Rectangle.FromLTRB(
                    Math.Min(pt0.X, pt1.X),
                    Math.Min(pt0.Y, pt1.Y),
                    Math.Max(pt0.X, pt1.X),
                    Math.Max(pt0.Y, pt1.Y)
                    );
            }
        }

        private void ThumbView_Resize(object sender, EventArgs e)
        {
            LayoutClient();
        }

        /// <summary>
        /// zero based
        /// </summary>
        [Browsable(false)]
        public int SelectTo
        {
            get
            {
                return selectTo;
            }
            set
            {
                int newSel = Math.Min(lays.Count - 1, Math.Max(0, value));
                if (newSel == selectFrom && newSel == selectTo)
                {
                    return;
                }

                selectTo = selectFrom = newSel;
                MakeVisible(selectFrom);
                Invalidate();

                SelectionChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// zero based
        /// </summary>
        [Browsable(false)]
        public int SelectionFirst => Math.Min(selectTo, selectFrom);

        /// <summary>
        /// zero based
        /// </summary>
        [Browsable(false)]
        public int SelectionLast => Math.Max(selectTo, selectFrom);

        [Browsable(false)]
        public int SelectionLength
        {
            get
            {
                if (SelectionFirst < 0)
                {
                    return 0;
                }
                return SelectionLast - SelectionFirst + 1;
            }
        }

        [Browsable(false)]
        public int SelectFrom
        {
            get
            {
                return selectFrom;
            }
            set
            {
                int newSel = Math.Min(lays.Count - 1, Math.Max(0, value));
                if (newSel == selectFrom)
                {
                    return;
                }

                selectFrom = newSel;
                MakeVisible(selectFrom);
                Invalidate();

                SelectionChanged?.Invoke(this, new EventArgs());
            }
        }

        private enum SelectionLevel
        {
            None,
            Selected,
            Focused,
        }

        private SelectionLevel GetSelectionLevelOf(int pageIndex)
        {
            if (pageIndex == selectFrom)
            {
                return SelectionLevel.Focused;
            }
            if (selectTo <= selectFrom && selectTo <= pageIndex && pageIndex <= selectFrom)
            {
                return SelectionLevel.Selected;
            }
            if (selectFrom <= selectTo && selectFrom <= pageIndex && pageIndex <= selectTo)
            {
                return SelectionLevel.Selected;
            }

            return SelectionLevel.None;
        }

        private void MakeVisible(int pageIndex)
        {
            foreach (Lay lay in lays)
            {
                if (lay.pageIndex == pageIndex)
                {
                    Point pos = Point.Empty - new Size(AutoScrollPosition);
                    Rectangle rc = new Rectangle(pos, ClientSize);

                    if (lay.boundRect.X < rc.X)
                    {
                        // ←へ
                        pos.X = lay.boundRect.X;
                    }
                    else if (rc.Right < lay.boundRect.Right)
                    {
                        // →へ
                        pos.X = lay.boundRect.Right - rc.Width;
                    }

                    if (lay.boundRect.Y < rc.Y)
                    {
                        // ↑へ
                        pos.Y = lay.boundRect.Y;
                    }
                    else if (rc.Bottom < lay.boundRect.Bottom)
                    {
                        // ↓へ
                        pos.Y = lay.boundRect.Bottom - rc.Height;
                    }

                    AutoScrollPosition = (pos);
                    break;
                }
            }
        }

        private void ThumbView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt || e.Control)
            {

            }
            else if (e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Home:
                        SelectFrom = 0;
                        break;
                    case Keys.End:
                        SelectFrom = int.MaxValue;
                        break;
                    case Keys.Left:
                        SelectFrom--;
                        break;
                    case Keys.Right:
                        SelectFrom++;
                        break;
                    case Keys.Up:
                        SelectFrom -= cxTile;
                        break;
                    case Keys.Down:
                        SelectFrom += cxTile;
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Home:
                        SelectTo = 0;
                        break;
                    case Keys.End:
                        SelectTo = int.MaxValue;
                        break;
                    case Keys.Left:
                        SelectTo = SelectFrom - 1;
                        break;
                    case Keys.Right:
                        SelectTo = SelectFrom + 1;
                        break;
                    case Keys.Up:
                        SelectTo = SelectFrom - cxTile;
                        break;
                    case Keys.Down:
                        SelectTo = SelectFrom + cxTile;
                        break;
                }
            }
        }

        private void ThumbView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Alt || e.Control)
            {

            }
            else
            {
                switch (e.KeyCode)
                {
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

        private void ThumbView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                Point pt = e.Location - new Size(AutoScrollPosition);
                int i = HitTestPic(pt.X, pt.Y);
                if (0 != (ModifierKeys & (Keys.Alt | Keys.Control)))
                {

                }
                else
                {
                    skipMouseUp = true;

                    if (i < 0 && e.Button == MouseButtons.Left)
                    {
                        // Start dragging
                        this.draggingNow = true;
                        this.pt0 = this.pt1 = pt;

                        SelectTo = SelectFrom;
                    }
                    else
                    {
                        // Select it
                        numDragChance = 10;

                        if (!IsSelected(i))
                        {
                            if (0 != (ModifierKeys & Keys.Shift))
                            {
                                SelectFrom = i;
                            }
                            else
                            {
                                SelectTo = i;
                            }
                        }
                        else
                        {
                            skipMouseUp = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画像を探す
        /// </summary>
        /// <param name="x">絶対 x座標</param>
        /// <param name="y">絶対 y座標</param>
        /// <returns>0以上…アイテム番号、0未満…未発見</returns>
        public int HitTestPic(int x, int y)
        {
            foreach (Lay lay in lays)
            {
                if (lay.picRect.Contains(x, y))
                {
                    return lay.pageIndex;
                }
            }
            return -1;
        }

        /// <summary>
        /// アイテムを探す
        /// </summary>
        /// <param name="x">クライアントx座標</param>
        /// <param name="y">クライアントy座標</param>
        /// <returns>0以上…アイテム番号、0未満…未発見</returns>
        public TvHitTestInfo HitTest(int x, int y)
        {
            return HitTest(new Point(x, y));
        }

        /// <summary>
        /// アイテムを探す
        /// </summary>
        /// <param name="pt">クライアント座標</param>
        /// <returns></returns>
        public TvHitTestInfo HitTest(Point pt)
        {
            pt = pt - new Size(AutoScrollPosition);
            int cnt = picts.Count;
            bool isEmpty = cnt == 0;
            if (!isEmpty)
            {
                foreach (Lay lay in lays)
                {
                    if (lay.boundRect.Contains(pt))
                    {
                        return new TvHitTestInfo(
                            lay.pageIndex,
                            TvHitTestLocation.Box
                                | (lay.picRect.Contains(pt) ? TvHitTestLocation.Pict : TvHitTestLocation.None)
                        );
                    }
                }
            }
            return new TvHitTestInfo(-1, TvHitTestLocation.None);
        }

        [Flags]
        public enum SuggestFlags
        {
            None = 0, OnlyUpDown = 1,
        }

        /// <summary>
        /// InsertMark候補を提案する
        /// </summary>
        /// <param name="pt">クライアント座標</param>
        /// <returns></returns>
        public TvInsertMark SuggestInsertMark(Point pt)
        {
            return SuggestInsertMark(pt, 0
                | (cxTile == 1 ? SuggestFlags.OnlyUpDown : 0)
                );
        }

        public TvInsertMark SuggestInsertMark(Point pt, SuggestFlags flags)
        {
            pt = pt - new Size(AutoScrollPosition);

            TvInsertMarkLocation before = (0 != (flags & SuggestFlags.OnlyUpDown)) ? TvInsertMarkLocation.Top : TvInsertMarkLocation.Left;
            TvInsertMarkLocation after = (0 != (flags & SuggestFlags.OnlyUpDown)) ? TvInsertMarkLocation.Bottom : TvInsertMarkLocation.Right;

            int cnt = picts.Count;
            bool isEmpty = cnt == 0;
            if (!isEmpty)
            {
                int cxItem = maxBox.Width;
                int cyItem = maxBox.Height;

                if (pt.Y < 0)
                {
                    // U
                    if (pt.X < 0)
                    {
                        // TL
                        return new TvInsertMark(0, before);
                    }
                    else if (pt.X >= cxTile * cxItem)
                    {
                        // TR
                        return new TvInsertMark(cxTile - 1, after);
                    }
                    else
                    {
                        // Tx
                        return new TvInsertMark(pt.X / cxItem, before);
                    }
                }
                else if (pt.Y >= cyTile * cyItem)
                {
                    // B
                    if (pt.X < 0)
                    {
                        // BL
                        return new TvInsertMark(cxTile * (cyTile - 1), before);
                    }
                }
                else
                {
                    // M
                    if (pt.X < 0)
                    {
                        // ML
                        return new TvInsertMark(cxTile * (pt.Y / cyItem), before);
                    }
                    else if (pt.X >= cxTile * cxItem)
                    {
                        // MR
                        return new TvInsertMark(cxTile * (1 + (pt.Y / cyItem)) - 1, after);
                    }
                    else
                    {
                        // Mx
                        foreach (Lay lay in lays)
                        {
                            if (lay.boundRect.Contains(pt))
                            {
                                bool atAfter = (0 != (flags & SuggestFlags.OnlyUpDown))
                                    ? pt.Y >= lay.boundRect.Top + lay.boundRect.Height / 3
                                    : pt.X >= lay.boundRect.Left + lay.boundRect.Width / 3
                                    ;
                                bool last = 0 == ((lay.pageIndex + 1) % cxTile);
                                if (atAfter && last)
                                    return new TvInsertMark(lay.pageIndex, after);
                                if (atAfter)
                                    return new TvInsertMark(lay.pageIndex + 1, before);
                                return new TvInsertMark(lay.pageIndex, before);
                            }
                        }
                    }
                }

                // Last
                return new TvInsertMark(cnt - 1, after);
            }

            return new TvInsertMark(0, before);
        }

        private void ThumbView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Capture)
                {
                    if (draggingNow)
                    {
                        this.draggingNow = false;
                        this.Refresh();
                    }
                    else if (!skipMouseUp)
                    {
                        Point pt = e.Location - new Size(AutoScrollPosition);
                        int i = HitTestPic(pt.X, pt.Y);
                        if (i >= 0)
                        {
                            SelectTo = i;
                        }
                    }
                }
            }
        }

        private void ThumbView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point pt = e.Location - new Size(AutoScrollPosition);
                if (this.Capture)
                {
                    if (draggingNow)
                    {
                        this.pt1 = pt;

                        Rectangle rc = RectDrag;
                        int iFirst = int.MaxValue;
                        int iLast = int.MinValue;
                        foreach (Lay lay in lays)
                        {
                            if (rc.IntersectsWith(lay.picRect))
                            {
                                iFirst = Math.Min(iFirst, lay.pageIndex);
                                iLast = Math.Max(iLast, lay.pageIndex);
                            }
                        }
                        if (iFirst != int.MaxValue && iLast != int.MinValue)
                        {
                            if (selectTo != iFirst || selectFrom != iLast)
                            {
                                selectFrom = iFirst;
                                selectTo = iLast;

                                if (SelectionChanged != null)
                                    SelectionChanged(this, new EventArgs());
                            }
                        }

                        this.Refresh();
                    }
                    else if (numDragChance == 0)
                    {
                        PictDrag?.Invoke(this, new EventArgs());

                        numDragChance = -1;
                    }
                    else
                    {
                        numDragChance--;
                    }
                }
            }
        }
    }
}
