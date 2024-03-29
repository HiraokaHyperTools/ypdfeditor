using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Threading;
using yPDFEditor.Utils;
using yPDFEditor.Enums;
using PdfiumViewer;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace yPDFEditor
{
    public partial class JForm : Form
    {
        private PdfEdit pdfEdit = new PdfEdit();
        private string openThisFile = null;

        public JForm(string file)
        {
            InitializeComponent();

            tv.Picts = new ObservableCollection<ThumbSet>();
            tv.Picts.CollectionChanged += Picts_CollectionChanged;

            openThisFile = file;

            pdfEdit.UpdateTitle = UpdateTitle;
            pdfEdit.Picts = tv.Picts;

            tv.ThumbSetProvider = (pageIndex) =>
            {
                return LoadPreviewOf(pageIndex);
            };
            preViewer.ThumbSetProvider = () => LoadPict();
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        enum TState
        {
            Yes, No, Cancel,
        }

        TState TrySave()
        {
            if (!pdfEdit.Modified) return TState.Yes;

            switch (MessageBox.Show(this, "変更されています。保存しますか。", Path.GetFileName(pdfEdit.Currentfp), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
            {
                case DialogResult.Yes:
                    SaveFile(pdfEdit.Currentfp);
                    return TState.Yes;
                case DialogResult.No:
                    return TState.No;
                default:
                    return TState.Cancel;
            }
        }

        TState TrySave2()
        {
            if (!pdfEdit.Modified) return TState.Yes;

            switch (MessageBox.Show(this, "先に保存しますか。", Path.GetFileName(pdfEdit.Currentfp), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
            {
                case DialogResult.Yes:
                    SaveFile(pdfEdit.Currentfp);
                    return TState.Yes;
                case DialogResult.No:
                default:
                    return TState.No;
            }
        }

        private void NewFile()
        {
            switch (TrySave())
            {
                case TState.Yes:
                case TState.No:
                    tv.Picts.Clear();
                    pdfEdit.Close();
                    break;
            }
        }

        String TitleTemp = String.Empty;

        private void UpdateTitle()
        {
            this.Text = (pdfEdit.Currentfp != null)
                ? Path.GetFileName(pdfEdit.Currentfp) + (pdfEdit.Modified ? "*" : "") + " -- " + TitleTemp
                : TitleTemp + (pdfEdit.Modified ? " *" : "")
                ;
        }

        private void JForm_Load(object sender, EventArgs e)
        {
            this.Text += " " + Application.ProductVersion;

            TitleTemp = this.Text;

            tstop.Location = Point.Empty;
            tsvis.Location = new Point(0, tstop.Height);

            if (openThisFile != null)
            {
                OpenFile(openThisFile);
            }

            tscRate.SelectedIndex = 0;
        }

        private void Picts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var index = tv.SelectFrom;
            var redraw = false;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    if ((uint)(index - e.NewStartingIndex) < e.NewItems.Count)
                    {
                        redraw = true;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if ((uint)(index - e.OldStartingIndex) < e.OldItems.Count)
                    {
                        redraw = true;
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    redraw = true;
                    break;
            }

            if (redraw)
            {
                tv_SelChanged(this, EventArgs.Empty);
            }
        }

        private void bOpenf_Click(object sender, EventArgs e)
        {
            switch (TrySave())
            {
                case TState.Yes:
                case TState.No:
                    if (ofdPict.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    OpenFile(ofdPict.FileName);
                    break;
            }
        }

        private void OpenFile(String file)
        {
            pdfEdit.OpenFile(file);
        }

        private ThumbSet LoadPreviewOf(int i)
        {
            return new ThumbSet
            {
                Bitmap = GetThumbnailOf(i, 200, 280),
                State = ThumbState.Loaded,
            };
        }

        private ThumbSet LoadPict()
        {
            if (!pdfEdit.IsOpened || tv.SelectionFirst == -1)
            {
                return null;
            }

            return new ThumbSet
            {
                Bitmap = GetThumbnailOf(tv.SelectionFirst, preViewer.Width, preViewer.Height),
                State = ThumbState.Loaded,
            };
        }

        private void tv_SelChanged(object sender, EventArgs e)
        {
            var length = tv.SelectionLength;
            if (length == 0)
            {
                tssl.Text = "Ready";
            }
            else if (length == 1)
            {
                tssl.Text = String.Format("ページ{0:#,##0}を選択。", 1 + tv.SelectionFirst);
            }
            else
            {
                tssl.Text = String.Format("ページ{0:#,##0}～{1:#,##0}を選択。(ページ数{2:#,##0})", 1 + tv.SelectionFirst, 1 + tv.SelectionLast, length);
            }

            if (length == 0)
            {
                preViewer.Pict = null;
            }
            else
            {
                preViewer.Pict = ThumbSet.Delayed;
            }
        }

        private Bitmap GetThumbnailOf(int pageIndex, int maxWidth, int maxHeight)
        {
            return pdfEdit.GetThumbnailOf(pageIndex, maxWidth, maxHeight);
        }

        private void tscRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tscRate.SelectedIndex)
            {
                case 0: //"頁全体"
                    preViewer.FitCnf = new FitCnf(SizeSpec.FWH);
                    break;
                case 1: //"頁幅"
                    preViewer.FitCnf = new FitCnf(SizeSpec.FW);
                    break;
                default:
                    SetRate(tscRate.Text);
                    break;
            }
        }

        private void SetRate(String text)
        {
            var match = Regex.Match(text, "^(\\d+)");
            int percent;
            if (match.Success && int.TryParse(match.Groups[1].Value, out percent) && 1 <= percent && percent <= 1600)
            {
                preViewer.FitCnf = new FitCnf(percent / 100.0f);
            }
            else
            {
                MessageBox.Show(this, "1 から 1600 までの数値を入れてください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tscRate_Validating(object sender, CancelEventArgs e)
        {
            if (tscRate.SelectedIndex < 0)
            {
                SetRate(tscRate.Text);
            }
        }

        private void tscRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetRate(tscRate.Text);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void bzoomIn_Click(object sender, EventArgs e)
        {
            if (preViewer.Pict != null)
            {
                preViewer.FitCnf = new FitCnf(preViewer.ActualRate * 1.3f);
            }
        }

        private void bzoomOut_Click(object sender, EventArgs e)
        {
            if (preViewer.Pict != null)
            {
                preViewer.FitCnf = new FitCnf(preViewer.ActualRate / 1.3f);
            }
        }

        private void preViewer1_FitCnfChanged(object sender, EventArgs e)
        {
            FitCnf fc = preViewer.FitCnf;
            switch (fc.SizeSpec)
            {
                case SizeSpec.FWH:
                    {
                        int i = 0;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.FW:
                    {
                        int i = 1;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.ResoRate:
                    {
                        tscRate.Text = String.Format("{0:0} %", fc.Rate * 100);
                        break;
                    }
            }
        }

        private void bShowPreView_Click(object sender, EventArgs e)
        {
            vsc.Panel2Collapsed = !bShowPreView.Checked;
        }

        public byte[] GetPDF(int pageIdx) => pdfEdit.GetPDF(pageIdx);

        private void tv_PictDrag(object sender, EventArgs e)
        {
            DataObject dat = new DataObject();
            PDFClip clip = new PDFClip();
            clip.ProcessId = Process.GetCurrentProcess().Id;
            clip.SelectionFirst = tv.SelectionFirst;
            clip.SelectionLast = tv.SelectionLast;
            clip.GetPDF = GetPDF;

            dat.SetData("PDFClip", clip);

            tv.DoDragDrop(dat, DragDropEffects.Scroll | DragDropEffects.Copy | DragDropEffects.Move);

            if (dat.GetDataPresent("Pasted") && (int)dat.GetData("Pasted") == 1)
            {
                int iSelLast = tv.SelectionLast;
                int iSelFirst = tv.SelectionFirst;

                EditDeletePages(iSelFirst, iSelLast);
            }
        }

        private void EditDeletePages(int firstIdx, int lastIdx) => pdfEdit.DeletePages(firstIdx, lastIdx);

        private void tv_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;
            }
        }

        private void tv_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("PDFClip"))
            {
                e.Effect = e.AllowedEffect & ((0 != (e.KeyState & 8)) ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.Move);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = e.AllowedEffect & DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tv_DragOver(object sender, DragEventArgs e)
        {
            tv_DragEnter(sender, e);

            int W = 20, M = 20;

            Point pos = Point.Empty - new Size(tv.AutoScrollPosition);

            Point pt = tv.PointToClient(new Point(e.X, e.Y));

            tv.InsertMark = tv.SuggestInsertMark(pt);

            if (pt.X < W)
            {
                pos.X -= M;
            }
            else if (tv.Width - W <= pt.X)
            {
                pos.X += M;
            }
            if (pt.Y < W)
            {
                pos.Y -= M;
            }
            else if (tv.Height - W <= pt.Y)
            {
                pos.Y += M;
            }

            tv.AutoScrollPosition = (pos);
        }

        private void tv_DragDrop(object sender, DragEventArgs e)
        {
            TvInsertMark iMark = tv.InsertMark;
            try
            {
                int iAt = iMark.Index + ((0 != (iMark.Location & (TvInsertMarkLocation.Right | TvInsertMarkLocation.Bottom))) ? 1 : 0);

                DropIt(e, iAt);
            }
            finally
            {
                tv.InsertMark = new TvInsertMark();
            }
        }

        private void DropIt(DragEventArgs e, int destIdx)
        {
            bool isCopy = 0 != (e.Effect & DragDropEffects.Copy);

            var fileList = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileList != null)
            {
                DialogResult res;
                if (pdfEdit.Currentfp == null)
                {
                    res = DialogResult.OK;
                }
                else if (destIdx >= 0)
                {
                    res = DialogResult.Yes;
                }
                else
                {
                    using (OpenWayForm form = new OpenWayForm(true))
                    {
                        res = form.ShowDialog();
                    }
                }
                if (res == DialogResult.OK)
                {
                    foreach (String fp in fileList)
                    {
                        OpenFile(fp);
                        break;
                    }
                }
                else if (res != DialogResult.Cancel)
                {
                    bool ifInsertAfter = res == DialogResult.Yes;

                    if (!ifInsertAfter)
                    {
                        destIdx = tv.Picts.Count;
                    }

                    using (WIPPanel wipp = new WIPPanel(tv))
                    {
                        foreach (String file in fileList)
                        {
                            EditInsertPDF(file, destIdx);
                        }
                    }
                }
                return;
            }
            else
            {
                PDFClip clip = (PDFClip)e.Data.GetData("PDFClip");
                int processId = clip.ProcessId;
                int lastSel = clip.SelectionFirst;
                int firstSel = clip.SelectionLast;

                int cnt = firstSel - lastSel + 1;

                if (processId == Process.GetCurrentProcess().Id)
                {
                    var list = new List<ThumbSet>();
                    for (int x = lastSel; x <= firstSel; x++)
                    {
                        list.Add(tv.Picts[x]);
                    }

                    if (isCopy)
                    {
                        using (WIPPanel wipp = new WIPPanel(tv))
                        {
                            EditCopyPages(destIdx, lastSel, firstSel);
                        }
                    }
                    else
                    {
                        if (lastSel <= destIdx && destIdx <= firstSel + 1)
                        {
                            // out of range
                        }
                        else
                        {
                            using (WIPPanel wipp = new WIPPanel(tv))
                            {
                                EditMovePages(destIdx, lastSel, firstSel);
                            }
                        }
                    }
                }
                else
                {
                    for (int x = 0; x <= tv.Picts.Count; x++)
                    {
                        if (destIdx == x)
                        {
                            for (int t = 0; t < cnt; t++)
                            {
                                byte[] bin = clip.GetPDF(lastSel + t);

                                using (var pdfFile = new MemoryStream(bin, false))
                                {
                                    pdfEdit.InsertPages(pdfFile, x + t);
                                }
                            }
                        }
                    }

                    if (!isCopy)
                    {
                        e.Data.SetData("Pasted", (int)1);
                    }
                }
            }
        }

        private void EditInsertPDF(string file, int destIdx) => pdfEdit.InsertPDF(file, destIdx);

        private void EditMovePages(int destIdx, int firstIdx, int lastIdx) => pdfEdit.MovePages(destIdx, firstIdx, lastIdx);

        private void EditCopyPages(int destIdx, int firstIdx, int lastIdx) => pdfEdit.CopyPages(destIdx, firstIdx, lastIdx);

        private void tv_DragLeave(object sender, EventArgs e)
        {
            tv.InsertMark = new TvInsertMark();
        }

        private void bDelp_Click(object sender, EventArgs e)
        {
            if (tv.SelectionLength == 0)
            {
                MessageBox.Show(this, "削除できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show(this, "選択したページを削除します。", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            {
                return;
            }

            EditDeletePages(tv.SelectionFirst, tv.SelectionLast);
        }

        private void bRotLeft_Click(object sender, EventArgs e)
        {
            if (tv.SelectionLength == 0)
            {
                MessageBox.Show(this, "回転できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var rotateLeft = Object.ReferenceEquals(sender, bRotLeft);

            pdfEdit.RotatePages(tv.SelectionFirst, tv.SelectionLast, rotateLeft);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            SaveFile(pdfEdit.Currentfp);
        }

        private void SaveFile(String saveTo)
        {
            if (tv.Picts.Count == 0)
            {
                MessageBox.Show(this, "空のPDFファイルは保存できません。\n\nページを追加してから保存してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (saveTo == null)
            {
                sfdPict.FileName = pdfEdit.Currentfp;
                if (sfdPict.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                saveTo = sfdPict.FileName;
            }

            pdfEdit.SaveTo(saveTo);
        }

        private void ss_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            using (var form = new AboutForm())
            {
                form.ShowDialog();
            }
        }

        private void bSaveas_Click(object sender, EventArgs e)
        {
            SaveFile(null);
        }

        private void bMail_Click(object sender, EventArgs e)
        {
            if (tv.SelectionLength == 0)
            {
                MessageBox.Show(this, "メール送信できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");
            UtMarkTemp.Add(tempPdf);

            {
                var pageNums = new List<int>();
                for (int x = 0; x < tv.Picts.Count; x++)
                {
                    if (tv.SelectionFirst <= x && x <= tv.SelectionLast)
                    {
                        pageNums.Add(1 + x);
                    }
                }

                pdfEdit.Export(tempPdf, string.Join(",", pageNums));
            }

            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + tempPdf + "\"");
        }

        private void JForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                switch (TrySave())
                {
                    case TState.Yes:
                    case TState.No:
                        break;
                    case TState.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void JForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect & (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void JForm_DragDrop(object sender, DragEventArgs e)
        {
            var fileList = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileList != null)
            {
                DialogResult res = DialogResult.None;
                foreach (var file in fileList)
                {
                    if (File.Exists(file))
                    {
                        if (pdfEdit.Currentfp == null)
                        {
                            res = DialogResult.OK;
                        }
                        else
                        {
                            using (OpenWayForm form = new OpenWayForm(false))
                            {
                                res = form.ShowDialog();
                            }
                        }
                        break;
                    }
                }

                if (false) { }
                else if (res == DialogResult.OK) // Load it
                {
                    SynchronizationContext.Current.Post(_ =>
                    {
                        foreach (String fp in fileList)
                        {
                            if (File.Exists(fp))
                            {
                                switch (TrySave())
                                {
                                    case TState.Yes:
                                    case TState.No:
                                        OpenFile(fp);
                                        break;
                                }
                                return;
                            }
                        }
                    }, null);
                }
                else if (res == DialogResult.Retry)// Append
                {
                    SynchronizationContext.Current.Post(_ =>
                    {
                        using (WIPPanel wip = new WIPPanel(this))
                        {
                            foreach (String file in fileList)
                            {
                                if (File.Exists(file))
                                {
                                    pdfEdit.AppendPages(file);
                                }
                            }
                        }
                    }, null);
                }
            }
        }

        private void bMailContents_Click(object sender, EventArgs e)
        {
            switch (TrySave2())
            {
                case TState.Yes:
                    {
                        if (tv.SelectionLength == 0)
                        {
                            MessageBox.Show(this, "メール送信できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else
                        {
                            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + pdfEdit.Currentfp + "\"");
                        }
                        break;
                    }
                default:
                    {
                        MessageBox.Show(this, "送信するには、先に保存してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
            }
        }

        private void JForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UtMarkTemp.Cleanup();
        }

        private void bAppend_Click(object sender, EventArgs e)
        {
            if (ofdAppend.ShowDialog(this) == DialogResult.OK)
            {
                var alfp = ofdAppend.FileNames;
                using (WIPPanel wip = new WIPPanel(this))
                {
                    AppendFiles(alfp);
                }
            }
        }

        private void bAppend_DragDrop(object sender, DragEventArgs e)
        {
            DropIt(e, tv.Picts.Count);
            return;
        }

        private void AppendFiles(string[] fileList)
        {
            foreach (var file in fileList)
            {
                if (File.Exists(file))
                {
                    pdfEdit.AppendPages(file);
                }
            }
        }

        private void bSort_Click(object sender, EventArgs e)
        {
            using (JSort form = new JSort())
            {
                var cx = tv.Picts.Count;
                form.Set(cx);
                switch (form.ShowDialog())
                {
                    case DialogResult.OK:
                        pdfEdit.EditSetPages(form.Indices);
                        break;
                }
            }
        }

        private void preViewer_Resize(object sender, EventArgs e)
        {
            preViewer.Pict = ThumbSet.Delayed;
        }
    }
}