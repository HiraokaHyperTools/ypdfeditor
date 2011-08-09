﻿using System;
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

namespace yPDFEditor {
    public partial class JForm : Form {
        String fp = null;
        String fpCurrent = null;

        public JForm(String fp) {
            InitializeComponent();

            this.fp = fp;
        }

        bool isModified = false;

        private void bNew_Click(object sender, EventArgs e) {
            Newf();
        }

        enum TState {
            Yes, No, Cancel,
        }

        TState TrySave() {
            if (!Modified) return TState.Yes;

            switch (MessageBox.Show(this, "変更されています。保存しますか。", Path.GetFileName(Currentfp), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
                case DialogResult.Yes:
                    Savef(Currentfp);
                    return TState.Yes;
                case DialogResult.No:
                    return TState.No;
                default:
                    return TState.Cancel;
            }
        }

        TState TrySave2() {
            if (!Modified) return TState.Yes;

            switch (MessageBox.Show(this, "先に保存しますか。", Path.GetFileName(Currentfp), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                case DialogResult.Yes:
                    Savef(Currentfp);
                    return TState.Yes;
                case DialogResult.No:
                default:
                    return TState.No;
            }
        }

        private void Newf() {
            switch (TrySave()) {
                case TState.Yes:
                case TState.No:
                    tv.Picts.Clear();
                    Currentfp = null;
                    Modified = false;
                    break;
            }
        }

        String TTLTemp = String.Empty;

        bool Modified {
            get {
                return isModified;
            }
            set {
                isModified = value;
                UpdateTTL();
            }
        }

        String Currentfp {
            get {
                return fpCurrent;
            }
            set {
                fpCurrent = value;
                UpdateTTL();
            }
        }

        private void UpdateTTL() {
            this.Text = (fpCurrent != null)
                ? Path.GetFileName(fpCurrent) + (Modified ? "*" : "") + " -- " + TTLTemp
                : TTLTemp + (Modified ? " *" : "")
                ;
        }

        private void JForm_Load(object sender, EventArgs e) {
            tv.Picts = new BindingList<TvPict>();
            tv.Picts.ListChanged += new ListChangedEventHandler(Picts_ListChanged);

            TTLTemp = this.Text;

            tstop.Location = Point.Empty;
            tsvis.Location = new Point(0, tstop.Height);

            if (fp != null) Openf(fp);

            tscRate.SelectedIndex = 0;
        }

        void Picts_ListChanged(object sender, ListChangedEventArgs e) {
            Modified = true;
        }

        private void bOpenf_Click(object sender, EventArgs e) {
            switch (TrySave()) {
                case TState.Yes:
                case TState.No:
                    if (ofdPict.ShowDialog(this) != DialogResult.OK) return;

                    Openf(ofdPict.FileName);
                    break;
            }
        }

        String fpTmppdf = null;

        static String pdfinfo_exe { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinfo.exe"); } }

        class PDFInfo {
            String rows = String.Empty;
            String fp;

            public PDFInfo(String fp) {
                this.fp = fp;

            }

            public bool Read() {
                ProcessStartInfo psi = new ProcessStartInfo(pdfinfo_exe, " \"" + fp + "\"");
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                Process p = Process.Start(psi);
                rows = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                return p.ExitCode == 0;
            }

            public int PageCount {
                get {
                    Match M = Regex.Match(rows, "^Pages:\\s+(?<a>\\d+)", RegexOptions.Multiline);
                    if (M.Success)
                        return int.Parse(M.Groups["a"].Value);
                    return 0;
                }
            }
        }

        PDFExploder pdfexp = null;

        void Openf(String fp) {
            fpTmppdf = Path.GetTempFileName();
            UtMarkTemp.Add(fpTmppdf);
            File.Copy(fp, fpTmppdf, true);

            PDFInfo pdfi = new PDFInfo(fpTmppdf);
            pdfi.Read();
            pdfexp = new PDFExploder(fpTmppdf);

            tv.Picts.Clear();
            Currentfp = null;
            int cnt = pdfi.PageCount;
            for (int i = 0; i < cnt; i++) {
                tv.Picts.Add(new TvPict(pdfexp, i));
            }
            Currentfp = fp;
            Modified = false;
        }

        private void tv_SelChanged(object sender, EventArgs e) {
            int c = tv.SelCount;
            if (c == 0) {
                tssl.Text = "Ready";
            }
            else if (c == 1) {
                tssl.Text = String.Format("ページ{0:#,##0}を選択。", 1 + tv.SelFirst);
            }
            else {
                tssl.Text = String.Format("ページ{0:#,##0}～{1:#,##0}を選択。(ページ数{2:#,##0})", 1 + tv.SelFirst, 1 + tv.SelLast, c);
            }

            if (c == 0) {
                pvw.Pic = null;
            }
            else {
                try {
                    pvw.Pic = tv.Picts[tv.Sel2].DefTumbGen.GetThumbnail(Size.Empty);
                }
                catch (PageExtractException) {
                    pvw.Pic = new Bitmap(1, 1);
                }
            }
        }

        private void tscRate_SelectedIndexChanged(object sender, EventArgs e) {
            switch (tscRate.SelectedIndex) {
                case 0: //"頁全体"
                    pvw.FitCnf = new FitCnf(SizeSpec.FWH);
                    break;
                case 1: //"頁幅"
                    pvw.FitCnf = new FitCnf(SizeSpec.FW);
                    break;
                default:
                    SetRate(tscRate.Text);
                    break;
            }
        }

        private void SetRate(String text) {
            Match M = Regex.Match(text, "^(\\d+)");
            int percent;
            if (M.Success && int.TryParse(M.Groups[1].Value, out percent) && 1 <= percent && percent <= 1600) {
                pvw.FitCnf = new FitCnf(percent / 100.0f);
            }
            else {
                MessageBox.Show(this, "1 から 1600 までの数値を入れてください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tscRate_Validating(object sender, CancelEventArgs e) {
            if (tscRate.SelectedIndex < 0) {
                SetRate(tscRate.Text);
            }
        }

        private void tscRate_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                SetRate(tscRate.Text);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void bzoomIn_Click(object sender, EventArgs e) {
            if (pvw.Pic != null)
                pvw.FitCnf = new FitCnf(pvw.ActualRate * 1.3f);
        }

        private void bzoomOut_Click(object sender, EventArgs e) {
            if (pvw.Pic != null)
                pvw.FitCnf = new FitCnf(pvw.ActualRate / 1.3f);
        }

        private void preViewer1_FitCnfChanged(object sender, EventArgs e) {
            FitCnf fc = pvw.FitCnf;
            switch (fc.SizeSpec) {
                case SizeSpec.FWH: {
                        int i = 0;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.FW: {
                        int i = 1;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.ResoRate: {
                        tscRate.Text = String.Format("{0:0} %", fc.Rate * 100);
                        break;
                    }
            }
        }

        private void bShowPreView_Click(object sender, EventArgs e) {
            vsc.Panel2Collapsed = !bShowPreView.Checked;
        }

        private void tv_PictDrag(object sender, EventArgs e) {
            DataObject dat = new DataObject();
            dat.SetData("PId", Process.GetCurrentProcess().Id);
            dat.SetData("SelFirst", tv.SelFirst);
            dat.SetData("SelLast", tv.SelLast);

            for (int x = tv.SelFirst; x <= tv.SelLast; x++) {
                using (MemoryStream os = new MemoryStream()) {
                    ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + (1 + x) + " output -");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    Process p = Process.Start(psi);
                    Stream si = p.StandardOutput.BaseStream;
                    byte[] bin = new byte[4000];
                    while (true) {
                        int r = si.Read(bin, 0, bin.Length);
                        if (r < 1) break;
                        os.Write(bin, 0, r);
                    }
                    p.WaitForExit();

                    if (p.ExitCode == 0) {
                        dat.SetData("pdf#" + x, os.ToArray());
                    }
                }
            }

            tv.DoDragDrop(dat, DragDropEffects.Scroll | DragDropEffects.Copy | DragDropEffects.Move);

            if (dat.GetDataPresent("Pasted") && (int)dat.GetData("Pasted") == 1) {
                int iSelLast = tv.SelLast;
                int iSelFirst = tv.SelFirst;

                EditDeletePages(iSelFirst, iSelLast);
            }
        }

        private void EditDeletePages(int iSelFirst, int iSelLast) {
            {
                String fpTmp2 = Path.GetTempFileName();
                UtMarkTemp.Add(fpTmp2);

                String cat = "";
                for (int x = 0; x < tv.Picts.Count; x++) {
                    if (tv.SelFirst <= x && x <= tv.SelLast) {
                        continue;
                    }
                    else {
                        cat += " " + (1 + x);
                    }
                }

                ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process p = Process.Start(psi);
                p.WaitForExit();

                if (p.ExitCode != 0) {
                    MessageBox.Show(this, "削除に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                File.Delete(fpTmppdf);

                pdfexp = new PDFExploder(fpTmppdf = fpTmp2);
            }

            int cx = tv.SelCount;
            for (int x = tv.SelLast; cx > 0; x--, cx--) {
                tv.Picts.RemoveAt(x);
            }
            for (int x = 0; x < tv.Picts.Count; x++) {
                tv.Picts[x].Relocate(pdfexp, x);
            }
        }

        private void tv_QueryContinueDrag(object sender, QueryContinueDragEventArgs e) {
            if (e.EscapePressed)
                e.Action = DragAction.Cancel;
        }

        private void tv_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent("PId") && e.Data.GetDataPresent("SelFirst") && e.Data.GetDataPresent("SelLast")) {
                e.Effect = e.AllowedEffect & ((0 != (e.KeyState & 8)) ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.Move);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = e.AllowedEffect & DragDropEffects.Copy;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tv_DragOver(object sender, DragEventArgs e) {
            tv_DragEnter(sender, e);

            int W = 20, M = 20;

            Point pos = Point.Empty - new Size(tv.AutoScrollPosition);

            Point pt = tv.PointToClient(new Point(e.X, e.Y));

            tv.InsertMark = tv.SuggestInsertMark(pt);

            if (pt.X < W) {
                pos.X -= M;
            }
            else if (tv.Width - W <= pt.X) {
                pos.X += M;
            }
            if (pt.Y < W) {
                pos.Y -= M;
            }
            else if (tv.Height - W <= pt.Y) {
                pos.Y += M;
            }

            tv.AutoScrollPosition = (pos);
        }

        private void tv_DragDrop(object sender, DragEventArgs e) {
            TvInsertMark iMark = tv.InsertMark;
            try {
                int iAt = iMark.Item + ((0 != (iMark.Location & (TvInsertMarkLocation.Right | TvInsertMarkLocation.Bottom))) ? 1 : 0);

                bool isCopy = 0 != (e.Effect & DragDropEffects.Copy);

                String[] alfp = e.Data.GetData(DataFormats.FileDrop) as String[];
                if (alfp != null) {
                    DialogResult res;
                    if (Currentfp == null)
                        res = DialogResult.OK;
                    else
                        using (OpenWayForm form = new OpenWayForm(true))
                            res = form.ShowDialog();
                    if (res == DialogResult.OK) {
                        foreach (String fp in alfp) {
                            Openf(fp);
                            break;
                        }
                    }
                    else if (res != DialogResult.Cancel) {
                        bool fAppend = res == DialogResult.Retry;
                        bool fInsertAfter = res == DialogResult.Yes;

                        if (!fInsertAfter) {
                            iAt = tv.Picts.Count;
                        }

                        foreach (String fp in alfp) {
                            EditAppendPDF(fp, iAt);
                        }
                    }
                    return;
                }
                else {
                    int iPId = (int)e.Data.GetData("PId");
                    int iSelFirst = (int)e.Data.GetData("SelFirst");
                    int iSelLast = (int)e.Data.GetData("SelLast");

                    int cnt = iSelLast - iSelFirst + 1;

                    if (iPId == Process.GetCurrentProcess().Id) {
                        List<TvPict> al = new List<TvPict>();
                        for (int x = iSelFirst; x <= iSelLast; x++) al.Add(tv.Picts[x]);

                        if (isCopy) {
                            EditCopyPages(iAt, iSelFirst, iSelLast);
                        }
                        else {
                            if (iSelFirst <= iAt && iAt <= iSelLast + 1) {

                            }
                            else {
                                EditMovePages(iAt, iSelFirst, iSelLast);
                            }
                        }
                    }
                    else {
                        String fpTmp2 = Path.GetTempFileName();
                        UtMarkTemp.Add(fpTmp2);

                        String inputs = "";
                        String cat = "";
                        int num = 2;
                        for (int x = 0; x <= tv.Picts.Count; x++) {
                            if (iAt == x) {
                                for (int t = 0; t < cnt; t++) {
                                    byte[] bin = CPUt.GetPDF(e.Data, iSelFirst + t);
                                    String fpxTmp = Path.GetTempFileName();
                                    UtMarkTemp.Add(fpxTmp);
                                    File.WriteAllBytes(fpxTmp, bin);
                                    inputs += " " + HIDGen.Generate(num) + "=\"" + fpxTmp + "\"";
                                    cat += " " + HIDGen.Generate(num);
                                    num++;
                                }
                            }
                            if (x < tv.Picts.Count) {
                                cat += " A" + (1 + x);
                            }
                        }

                        ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " A=\"" + fpTmppdf + "\" " + inputs + " cat " + cat + " output \"" + fpTmp2 + "\"");
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        Process p = Process.Start(psi);
                        p.WaitForExit();

                        if (p.ExitCode == 0) {
                            File.Delete(fpTmppdf);

                            pdfexp = new PDFExploder(fpTmppdf = fpTmp2);

                            for (int x = 0; x < cnt; x++) {
                                tv.Picts.Insert(iAt + x, new TvPict(pdfexp, x));
                            }
                            for (int i = 0; i < tv.Picts.Count; i++) {
                                tv.Picts[i].Relocate(pdfexp, i);
                            }
                        }

                        if (!isCopy) {
                            e.Data.SetData("Pasted", (int)1);
                        }
                    }
                }
            }
            finally {
                tv.InsertMark = new TvInsertMark();
            }
        }

        class HIDGen {
            internal static string Generate(int num) {
                String s = "";
                while (num != 0) {
                    int x = num % 26;
                    s += (char)('A' + (char)x);
                    num /= 26;
                }
                return s;
            }
        }

        private void EditAppendPDF(string fp, int iAt) {
            PDFInfo pdfi = new PDFInfo(fp);
            if (pdfi.Read()) {
                int cnt = pdfi.PageCount;

                String fpTmp2 = Path.GetTempFileName();
                UtMarkTemp.Add(fpTmp2);

                String cat = "";
                for (int x = 0; x <= tv.Picts.Count; x++) {
                    if (iAt == x) {
                        for (int y = 0; y < cnt; y++) {
                            cat += " B" + (1 + y);
                        }
                    }
                    if (x < tv.Picts.Count) {
                        cat += " A" + (1 + x);
                    }
                }

                ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " A=\"" + fpTmppdf + "\" B=\"" + fp + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process p = Process.Start(psi);
                p.WaitForExit();

                if (p.ExitCode == 0) {
                    File.Delete(fpTmppdf);

                    pdfexp = new PDFExploder(fpTmppdf = fpTmp2);

                    for (int i = 0; i < cnt; i++) {
                        tv.Picts.Insert(iAt, new TvPict(pdfexp, tv.Picts.Count));
                        iAt++;
                    }
                    for (int i = 0; i < tv.Picts.Count; i++) {
                        tv.Picts[i].Relocate(pdfexp, i);
                    }
                }
            }
        }

        private void EditMovePages(int iAt, int iSelFirst, int iSelLast) {
            String fpTmp2 = Path.GetTempFileName();
            UtMarkTemp.Add(fpTmp2);

            String cat = "";
            for (int x = 0; x <= tv.Picts.Count; x++) {
                if (iAt == x) {
                    for (int y = iSelFirst; y <= iSelLast; y++) {
                        cat += " " + (1 + y);
                    }
                }
                if (x < tv.Picts.Count && (x < iSelFirst || iSelLast < x)) {
                    cat += " " + (1 + x);
                }
            }

            ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();

            if (p.ExitCode != 0) {
                return;
            }

            File.Delete(fpTmppdf);

            pdfexp = new PDFExploder(fpTmppdf = fpTmp2);

            int cnt = iSelLast - iSelFirst + 1;

            List<TvPict> al = new List<TvPict>();
            for (int x = iSelFirst; x <= iSelLast; x++) al.Add(tv.Picts[x]);

            for (int x = iSelLast; x >= iSelFirst; x--) {
                tv.Picts.RemoveAt(x);
            }
            if (iAt > iSelFirst)
                iAt -= cnt;
            for (int x = 0; x < al.Count; x++) {
                tv.Picts.Insert(iAt + x, al[x]);
            }
        }

        private void EditCopyPages(int iAt, int iSelFirst, int iSelLast) {
            String fpTmp2 = Path.GetTempFileName();
            UtMarkTemp.Add(fpTmp2);

            String cat = "";
            for (int x = 0; x <= tv.Picts.Count; x++) {
                if (iAt == x) {
                    for (int y = iSelFirst; y <= iSelLast; y++) {
                        cat += " " + (1 + y);
                    }
                }
                if (x < tv.Picts.Count) {
                    cat += " " + (1 + x);
                }
            }

            ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();

            if (p.ExitCode != 0) {
                return;
            }

            File.Delete(fpTmppdf);

            pdfexp = new PDFExploder(fpTmppdf = fpTmp2);

            List<TvPict> al = new List<TvPict>();
            for (int x = iSelFirst; x <= iSelLast; x++) al.Add(tv.Picts[x]);

            for (int x = 0; x < al.Count; x++) {
                tv.Picts.Insert(iAt + x, al[x].Clone());
            }
            for (int x = 0; x < tv.Picts.Count; x++) {
                tv.Picts[x].Relocate(pdfexp, x);
            }
        }

        class CPUt {
            internal static byte[] GetPDF(IDataObject dat, int iSel) {
                byte[] bin = dat.GetData("pdf#" + iSel) as byte[];
                if (bin == null) throw new NotSupportedException();

                return bin;
            }
        }

        private void tv_DragLeave(object sender, EventArgs e) {
            tv.InsertMark = new TvInsertMark();
        }

        private void tv_MouseMove(object sender, MouseEventArgs e) {
        }

        private void bDelp_Click(object sender, EventArgs e) {
            if (tv.SelCount == 0) {
                MessageBox.Show(this, "削除できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show(this, "選択したページを削除します。", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            EditDeletePages(tv.SelFirst, tv.SelLast);
        }

        static String pdftk_exe { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdftk.exe"); } }

        private void bRotLeft_Click(object sender, EventArgs e) {
            if (tv.SelCount == 0) {
                MessageBox.Show(this, "回転できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            bool fLeft = Object.ReferenceEquals(sender, bRotLeft);

            {
                String fpTmp2 = Path.GetTempFileName();
                UtMarkTemp.Add(fpTmp2);

                String cat = "";
                for (int x = 0; x < tv.Picts.Count; x++) {
                    cat += " " + (1 + x);
                    if (tv.SelFirst <= x && x <= tv.SelLast) {
                        cat += fLeft ? "L" : "R";
                    }
                }

                ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process p = Process.Start(psi);
                p.WaitForExit();

                if (p.ExitCode != 0) {
                    MessageBox.Show(this, "回転に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                File.Delete(fpTmppdf);

                pdfexp = new PDFExploder(fpTmppdf = fpTmp2);
            }

            for (int x = 0; x < tv.Picts.Count; x++) {
                tv.Picts[x].Relocate(pdfexp, x);
            }
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; x++) {
                tv.Picts[x].Picture = null;
                tv.Picts.ResetItem(x);
            }
        }

        private void bSave_Click(object sender, EventArgs e) {
            Savef(Currentfp);
        }

        private void Savef(String fp) {
            if (tv.Picts.Count == 0) {
                MessageBox.Show(this, "空のPDFファイルは保存できません。\n\nページを追加してから保存してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (fp == null) {
                sfdPict.FileName = Currentfp;
                if (sfdPict.ShowDialog(this) != DialogResult.OK)
                    return;
                fp = sfdPict.FileName;
            }

            String fpbak = Path.ChangeExtension(fp, ".bak");
            if (File.Exists(fpbak))
                File.Delete(fpbak);
            if (File.Exists(fp))
                File.Move(fp, fpbak);

            File.Copy(fpTmppdf, fp, true);

            Currentfp = fp;
            Modified = false;

            if (File.Exists(fpbak))
                File.Delete(fpbak);
        }

        private void ss_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void bAbout_Click(object sender, EventArgs e) {
            using (AboutForm form = new AboutForm())
                form.ShowDialog();
        }

        private void bSaveas_Click(object sender, EventArgs e) {
            Savef(null);
        }

        private void bMail_Click(object sender, EventArgs e) {
            if (tv.SelCount == 0) {
                MessageBox.Show(this, "メール送信できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            String fpTmp2 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");
            UtMarkTemp.Add(fpTmp2);

            {
                String cat = "";
                for (int x = 0; x < tv.Picts.Count; x++) {
                    if (tv.SelFirst <= x && x <= tv.SelLast) {
                        cat += " " + (1 + x);
                    }
                }

                ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" cat " + cat + " output \"" + fpTmp2 + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process p = Process.Start(psi);
                p.WaitForExit();

                if (p.ExitCode != 0) {
                    MessageBox.Show(this, "ページの取り出しに失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + fpTmp2 + "\"");
        }

        private void JForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                switch (TrySave()) {
                    case TState.Yes:
                    case TState.No:
                        break;
                    case TState.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void JForm_DragEnter(object sender, DragEventArgs e) {
            e.Effect = e.AllowedEffect & (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void JForm_DragDrop(object sender, DragEventArgs e) {
            String[] alfp = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (alfp != null) {
                DialogResult res = DialogResult.None;
                foreach (String fp in alfp) {
                    if (File.Exists(fp)) {
                        if (Currentfp == null)
                            res = DialogResult.OK;
                        else
                            using (OpenWayForm form = new OpenWayForm(false)) {
                                res = form.ShowDialog();
                            }
                        break;
                    }
                }

                if (false) { }
                else if (res == DialogResult.OK) // Load it
                    SynchronizationContext.Current.Post(delegate {
                        foreach (String fp in alfp) {
                            if (File.Exists(fp)) {
                                switch (TrySave()) {
                                    case TState.Yes:
                                    case TState.No:
                                        Openf(fp);
                                        break;
                                }
                                return;
                            }
                        }
                    }, null);
                else if (res == DialogResult.Retry)// Append
                    SynchronizationContext.Current.Post(delegate {
                        using (WIPPanel wip = new WIPPanel(this)) {
                            foreach (String fp in alfp) {
                                if (File.Exists(fp)) {
                                    PDFInfo pdfi = new PDFInfo(fp);
                                    if (pdfi.Read()) {
                                        int cnt = pdfi.PageCount;

                                        String fpTmp2 = Path.GetTempFileName();
                                        UtMarkTemp.Add(fpTmp2);

                                        ProcessStartInfo psi = new ProcessStartInfo(pdftk_exe, " \"" + fpTmppdf + "\" \"" + fp + "\" cat output \"" + fpTmp2 + "\"");
                                        psi.CreateNoWindow = true;
                                        psi.UseShellExecute = false;
                                        Process p = Process.Start(psi);
                                        p.WaitForExit();

                                        if (p.ExitCode == 0) {
                                            File.Delete(fpTmppdf);

                                            pdfexp = new PDFExploder(fpTmppdf = fpTmp2);

                                            for (int i = 0; i < cnt; i++) {
                                                tv.Picts.Add(new TvPict(pdfexp, tv.Picts.Count));
                                            }
                                            for (int i = 0; i < tv.Picts.Count; i++) {
                                                tv.Picts[i].Relocate(pdfexp, i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }, null);
            }
        }

        private void bMailContents_Click(object sender, EventArgs e) {
            switch (TrySave2()) {
                case TState.Yes: {
                        if (tv.SelCount == 0) {
                            MessageBox.Show(this, "メール送信できるページがありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else {
                            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + Currentfp + "\"");
                        }
                        break;
                    }
                default: {
                        MessageBox.Show(this, "送信するには、先に保存してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
            }
        }

        private void JForm_FormClosed(object sender, FormClosedEventArgs e) {
            UtMarkTemp.Cleanup();
        }
    }

    public class PageExtractException : Exception {
        int erc;

        public PageExtractException(int erc) {
            this.erc = erc;
        }
    }

    public class PagePic {
        MemoryStream os;

        public PagePic(MemoryStream os) {
            this.os = os;
        }

        public Bitmap LoadPic() {
            return new Bitmap(os);
        }
    }

    public class UtMarkTemp {
        static List<string> al = new List<string>();

        public static void Add(String fp) {
            lock (typeof(UtMarkTemp))
                al.Add(fp);
        }

        public static void Cleanup() {
            lock (typeof(UtMarkTemp)) {
                foreach (String fp in al) {
                    try {
                        if (File.Exists(fp))
                            File.Delete(fp);
                    }
                    catch (Exception) { }
                }
                al.Clear();
            }
        }
    }

    public class PDFExploder {
        String fp;

        public PDFExploder(String fp) {
            this.fp = fp;
        }

        internal static String pdftoppm_exe { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdftoppm.exe"); } }

        public PagePic Extract(int page, int? scaleTo, int? dpi, bool png) {
            ProcessStartInfo psi = new ProcessStartInfo(pdftoppm_exe, ""
                + " -f " + page
                + " -l " + page
                + (png ? " -png" : " -jpeg") + " "
                + (scaleTo.HasValue ? " -scale-to " + scaleTo.Value : "")
                + (dpi.HasValue ? " -r " + dpi.Value : "")
                + " \"" + fp + "\""
                );
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process p = Process.Start(psi);
            MemoryStream os = new MemoryStream();
            Stream si = p.StandardOutput.BaseStream;
            byte[] bin = new byte[4000];
            while (true) {
                int r = si.Read(bin, 0, bin.Length);
                if (r < 1) break;
                os.Write(bin, 0, r);
            }
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new PageExtractException(p.ExitCode);
            return new PagePic(os);
        }
    }

}