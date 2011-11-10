namespace yPDFEditor {
    partial class JForm {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JForm));
            this.tsc = new System.Windows.Forms.ToolStripContainer();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.tssl = new System.Windows.Forms.ToolStripStatusLabel();
            this.vsc = new System.Windows.Forms.SplitContainer();
            this.tv = new yPDFEditor.ThumbView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pvw = new yPDFEditor.PreViewer();
            this.tstop = new System.Windows.Forms.ToolStrip();
            this.bNew = new System.Windows.Forms.ToolStripButton();
            this.bOpenf = new System.Windows.Forms.ToolStripButton();
            this.bSave = new System.Windows.Forms.ToolStripButton();
            this.bSaveas = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bRotLeft = new System.Windows.Forms.ToolStripButton();
            this.bRotRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bMail = new System.Windows.Forms.ToolStripSplitButton();
            this.bMailContents = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.bDelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.bAbout = new System.Windows.Forms.ToolStripButton();
            this.tsvis = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscRate = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.bzoomIn = new System.Windows.Forms.ToolStripButton();
            this.bzoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.bShowPreView = new System.Windows.Forms.ToolStripButton();
            this.ofdPict = new System.Windows.Forms.OpenFileDialog();
            this.mThumb = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sfdPict = new System.Windows.Forms.SaveFileDialog();
            this.tsc.BottomToolStripPanel.SuspendLayout();
            this.tsc.ContentPanel.SuspendLayout();
            this.tsc.TopToolStripPanel.SuspendLayout();
            this.tsc.SuspendLayout();
            this.ss.SuspendLayout();
            this.vsc.Panel1.SuspendLayout();
            this.vsc.Panel2.SuspendLayout();
            this.vsc.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tstop.SuspendLayout();
            this.tsvis.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsc
            // 
            // 
            // tsc.BottomToolStripPanel
            // 
            this.tsc.BottomToolStripPanel.Controls.Add(this.ss);
            // 
            // tsc.ContentPanel
            // 
            this.tsc.ContentPanel.Controls.Add(this.vsc);
            this.tsc.ContentPanel.Size = new System.Drawing.Size(789, 440);
            this.tsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsc.Location = new System.Drawing.Point(0, 0);
            this.tsc.Name = "tsc";
            this.tsc.Size = new System.Drawing.Size(789, 522);
            this.tsc.TabIndex = 0;
            this.tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            this.tsc.TopToolStripPanel.Controls.Add(this.tstop);
            this.tsc.TopToolStripPanel.Controls.Add(this.tsvis);
            // 
            // ss
            // 
            this.ss.Dock = System.Windows.Forms.DockStyle.None;
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl});
            this.ss.Location = new System.Drawing.Point(0, 0);
            this.ss.Name = "ss";
            this.ss.Size = new System.Drawing.Size(789, 22);
            this.ss.TabIndex = 0;
            this.ss.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ss_ItemClicked);
            // 
            // tssl
            // 
            this.tssl.Name = "tssl";
            this.tssl.Size = new System.Drawing.Size(37, 17);
            this.tssl.Text = "Ready";
            // 
            // vsc
            // 
            this.vsc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vsc.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.vsc.Location = new System.Drawing.Point(0, 0);
            this.vsc.Name = "vsc";
            // 
            // vsc.Panel1
            // 
            this.vsc.Panel1.Controls.Add(this.tv);
            // 
            // vsc.Panel2
            // 
            this.vsc.Panel2.Controls.Add(this.panel1);
            this.vsc.Size = new System.Drawing.Size(789, 440);
            this.vsc.SplitterDistance = 262;
            this.vsc.SplitterWidth = 6;
            this.vsc.TabIndex = 0;
            // 
            // tv
            // 
            this.tv.AllowDrop = true;
            this.tv.AutoScroll = true;
            this.tv.BackColor = System.Drawing.Color.Transparent;
            this.tv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv.Location = new System.Drawing.Point(0, 0);
            this.tv.Name = "tv";
            this.tv.Picts = null;
            this.tv.Sel2 = -1;
            this.tv.Size = new System.Drawing.Size(260, 438);
            this.tv.SSel = -1;
            this.tv.TabIndex = 2;
            this.tv.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.tv_QueryContinueDrag);
            this.tv.DragOver += new System.Windows.Forms.DragEventHandler(this.tv_DragOver);
            this.tv.SelChanged += new System.EventHandler(this.tv_SelChanged);
            this.tv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tv_MouseMove);
            this.tv.PictDrag += new System.EventHandler(this.tv_PictDrag);
            this.tv.DragDrop += new System.Windows.Forms.DragEventHandler(this.tv_DragDrop);
            this.tv.DragLeave += new System.EventHandler(this.tv_DragLeave);
            this.tv.DragEnter += new System.Windows.Forms.DragEventHandler(this.tv_DragEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pvw);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 438);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // pvw
            // 
            this.pvw.AutoScroll = true;
            this.pvw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pvw.Location = new System.Drawing.Point(0, 0);
            this.pvw.Name = "pvw";
            this.pvw.Pic = null;
            this.pvw.Size = new System.Drawing.Size(519, 438);
            this.pvw.TabIndex = 0;
            this.pvw.FitCnfChanged += new System.EventHandler(this.preViewer1_FitCnfChanged);
            // 
            // tstop
            // 
            this.tstop.Dock = System.Windows.Forms.DockStyle.None;
            this.tstop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bNew,
            this.bOpenf,
            this.bSave,
            this.bSaveas,
            this.toolStripSeparator1,
            this.bRotLeft,
            this.bRotRight,
            this.toolStripSeparator2,
            this.bMail,
            this.toolStripSeparator6,
            this.bDelp,
            this.toolStripSeparator3,
            this.bAbout});
            this.tstop.Location = new System.Drawing.Point(3, 0);
            this.tstop.Name = "tstop";
            this.tstop.Size = new System.Drawing.Size(657, 35);
            this.tstop.TabIndex = 0;
            // 
            // bNew
            // 
            this.bNew.AutoSize = false;
            this.bNew.Image = global::yPDFEditor.Properties.Resources.NewDocumentHS;
            this.bNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNew.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(33, 32);
            this.bNew.Text = "新規";
            this.bNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bOpenf
            // 
            this.bOpenf.Image = global::yPDFEditor.Properties.Resources.openHS;
            this.bOpenf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bOpenf.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.bOpenf.Name = "bOpenf";
            this.bOpenf.Size = new System.Drawing.Size(27, 32);
            this.bOpenf.Text = "開く";
            this.bOpenf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bOpenf.Click += new System.EventHandler(this.bOpenf_Click);
            // 
            // bSave
            // 
            this.bSave.Image = global::yPDFEditor.Properties.Resources.saveHS;
            this.bSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(66, 32);
            this.bSave.Text = "上書き保存";
            this.bSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bSaveas
            // 
            this.bSaveas.Image = global::yPDFEditor.Properties.Resources.saveHS;
            this.bSaveas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveas.Name = "bSaveas";
            this.bSaveas.Size = new System.Drawing.Size(97, 32);
            this.bSaveas.Text = "名前を付けて保存";
            this.bSaveas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bSaveas.Click += new System.EventHandler(this.bSaveas_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // bRotLeft
            // 
            this.bRotLeft.Image = global::yPDFEditor.Properties.Resources.TLeft;
            this.bRotLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRotLeft.Name = "bRotLeft";
            this.bRotLeft.Size = new System.Drawing.Size(54, 32);
            this.bRotLeft.Text = "左に回転";
            this.bRotLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bRotLeft.Click += new System.EventHandler(this.bRotLeft_Click);
            // 
            // bRotRight
            // 
            this.bRotRight.Image = global::yPDFEditor.Properties.Resources.TRight;
            this.bRotRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRotRight.Name = "bRotRight";
            this.bRotRight.Size = new System.Drawing.Size(54, 32);
            this.bRotRight.Text = "右に回転";
            this.bRotRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bRotRight.Click += new System.EventHandler(this.bRotLeft_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // bMail
            // 
            this.bMail.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bMailContents});
            this.bMail.Image = global::yPDFEditor.Properties.Resources.NewMessageHS;
            this.bMail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bMail.Name = "bMail";
            this.bMail.Size = new System.Drawing.Size(136, 32);
            this.bMail.Text = "選択ページをメール送信";
            this.bMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bMail.ButtonClick += new System.EventHandler(this.bMail_Click);
            // 
            // bMailContents
            // 
            this.bMailContents.Name = "bMailContents";
            this.bMailContents.Size = new System.Drawing.Size(173, 22);
            this.bMailContents.Text = "この文書をメール送信";
            this.bMailContents.Click += new System.EventHandler(this.bMailContents_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 35);
            // 
            // bDelp
            // 
            this.bDelp.Image = global::yPDFEditor.Properties.Resources.DeleteHS;
            this.bDelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDelp.Name = "bDelp";
            this.bDelp.Size = new System.Drawing.Size(63, 32);
            this.bDelp.Text = "ページ削除";
            this.bDelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bDelp.Click += new System.EventHandler(this.bDelp_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // bAbout
            // 
            this.bAbout.Image = global::yPDFEditor.Properties.Resources.Information;
            this.bAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAbout.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(33, 32);
            this.bAbout.Text = "情報";
            this.bAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // tsvis
            // 
            this.tsvis.Dock = System.Windows.Forms.DockStyle.None;
            this.tsvis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscRate,
            this.toolStripSeparator4,
            this.bzoomIn,
            this.bzoomOut,
            this.toolStripSeparator5,
            this.bShowPreView});
            this.tsvis.Location = new System.Drawing.Point(3, 35);
            this.tsvis.Name = "tsvis";
            this.tsvis.Size = new System.Drawing.Size(365, 25);
            this.tsvis.TabIndex = 1;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(74, 22);
            this.toolStripLabel1.Text = "画像の大きさ：";
            // 
            // tscRate
            // 
            this.tscRate.DropDownHeight = 199;
            this.tscRate.IntegralHeight = false;
            this.tscRate.Items.AddRange(new object[] {
            "頁全体",
            "頁幅",
            "25 %",
            "37 %",
            "50 %",
            "75 %",
            "100 %",
            "150 %",
            "200 %",
            "300 %",
            "400 %"});
            this.tscRate.Name = "tscRate";
            this.tscRate.Size = new System.Drawing.Size(87, 25);
            this.tscRate.SelectedIndexChanged += new System.EventHandler(this.tscRate_SelectedIndexChanged);
            this.tscRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tscRate_KeyDown);
            this.tscRate.Validating += new System.ComponentModel.CancelEventHandler(this.tscRate_Validating);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // bzoomIn
            // 
            this.bzoomIn.Image = ((System.Drawing.Image)(resources.GetObject("bzoomIn.Image")));
            this.bzoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bzoomIn.Name = "bzoomIn";
            this.bzoomIn.Size = new System.Drawing.Size(49, 22);
            this.bzoomIn.Text = "拡大";
            this.bzoomIn.Click += new System.EventHandler(this.bzoomIn_Click);
            // 
            // bzoomOut
            // 
            this.bzoomOut.Image = ((System.Drawing.Image)(resources.GetObject("bzoomOut.Image")));
            this.bzoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bzoomOut.Name = "bzoomOut";
            this.bzoomOut.Size = new System.Drawing.Size(49, 22);
            this.bzoomOut.Text = "縮小";
            this.bzoomOut.Click += new System.EventHandler(this.bzoomOut_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // bShowPreView
            // 
            this.bShowPreView.Checked = true;
            this.bShowPreView.CheckOnClick = true;
            this.bShowPreView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bShowPreView.Image = ((System.Drawing.Image)(resources.GetObject("bShowPreView.Image")));
            this.bShowPreView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bShowPreView.Name = "bShowPreView";
            this.bShowPreView.Size = new System.Drawing.Size(82, 22);
            this.bShowPreView.Text = "画像を表示";
            this.bShowPreView.Click += new System.EventHandler(this.bShowPreView_Click);
            // 
            // ofdPict
            // 
            this.ofdPict.DefaultExt = "pdf";
            this.ofdPict.Filter = "*.pdf|*.pdf";
            // 
            // mThumb
            // 
            this.mThumb.Name = "contextMenuStrip1";
            this.mThumb.Size = new System.Drawing.Size(61, 4);
            // 
            // sfdPict
            // 
            this.sfdPict.DefaultExt = "pdf";
            this.sfdPict.Filter = "*.pdf|*.pdf";
            // 
            // JForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 522);
            this.Controls.Add(this.tsc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JForm";
            this.Text = "your PDF Editor";
            this.Load += new System.EventHandler(this.JForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.JForm_DragDrop);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.JForm_FormClosed);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.JForm_DragEnter);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JForm_FormClosing);
            this.tsc.BottomToolStripPanel.ResumeLayout(false);
            this.tsc.BottomToolStripPanel.PerformLayout();
            this.tsc.ContentPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.PerformLayout();
            this.tsc.ResumeLayout(false);
            this.tsc.PerformLayout();
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            this.vsc.Panel1.ResumeLayout(false);
            this.vsc.Panel2.ResumeLayout(false);
            this.vsc.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tstop.ResumeLayout(false);
            this.tstop.PerformLayout();
            this.tsvis.ResumeLayout(false);
            this.tsvis.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tsc;
        private System.Windows.Forms.ToolStrip tstop;
        private System.Windows.Forms.SplitContainer vsc;
        private System.Windows.Forms.ToolStripButton bNew;
        private System.Windows.Forms.ToolStripButton bOpenf;
        private System.Windows.Forms.ToolStripButton bSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bRotLeft;
        private System.Windows.Forms.ToolStripButton bRotRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bDelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton bAbout;
        private System.Windows.Forms.OpenFileDialog ofdPict;
        private ThumbView tv;
        private System.Windows.Forms.StatusStrip ss;
        private System.Windows.Forms.ToolStripStatusLabel tssl;
        private System.Windows.Forms.Panel panel1;
        private PreViewer pvw;
        private System.Windows.Forms.ToolStrip tsvis;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscRate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton bzoomIn;
        private System.Windows.Forms.ToolStripButton bzoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton bShowPreView;
        private System.Windows.Forms.ContextMenuStrip mThumb;
        private System.Windows.Forms.SaveFileDialog sfdPict;
        private System.Windows.Forms.ToolStripButton bSaveas;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSplitButton bMail;
        private System.Windows.Forms.ToolStripMenuItem bMailContents;
    }
}

