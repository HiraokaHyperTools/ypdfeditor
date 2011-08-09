namespace yPDFEditor {
    partial class OpenWayForm {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bInsert = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bAppend = new System.Windows.Forms.Button();
            this.bOpenIt = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ファイルが放り込まれました。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "どのように処理しますか。";
            // 
            // bInsert
            // 
            this.bInsert.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bInsert.Image = global::yPDFEditor.Properties.Resources.ExpandSpaceHS;
            this.bInsert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bInsert.Location = new System.Drawing.Point(52, 187);
            this.bInsert.Name = "bInsert";
            this.bInsert.Size = new System.Drawing.Size(253, 44);
            this.bInsert.TabIndex = 4;
            this.bInsert.Text = "読み込んで、カーソルの後に挿入します。";
            this.bInsert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bInsert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInsert.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Image = global::yPDFEditor.Properties.Resources.DeleteHS;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(338, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 44);
            this.button1.TabIndex = 5;
            this.button1.Text = "キャンセル";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // bAppend
            // 
            this.bAppend.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.bAppend.Image = global::yPDFEditor.Properties.Resources.DataContainer_NewRecordHS;
            this.bAppend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bAppend.Location = new System.Drawing.Point(52, 123);
            this.bAppend.Name = "bAppend";
            this.bAppend.Size = new System.Drawing.Size(253, 44);
            this.bAppend.TabIndex = 3;
            this.bAppend.Text = "読み込んで、最後のページに追加します。";
            this.bAppend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bAppend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bAppend.UseVisualStyleBackColor = true;
            // 
            // bOpenIt
            // 
            this.bOpenIt.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOpenIt.Image = global::yPDFEditor.Properties.Resources.openHS;
            this.bOpenIt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bOpenIt.Location = new System.Drawing.Point(52, 73);
            this.bOpenIt.Name = "bOpenIt";
            this.bOpenIt.Size = new System.Drawing.Size(253, 44);
            this.bOpenIt.TabIndex = 2;
            this.bOpenIt.Text = "開く。編集中のファイルは、閉じます。";
            this.bOpenIt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bOpenIt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bOpenIt.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::yPDFEditor.Properties.Resources.question;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // OpenWayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 255);
            this.Controls.Add(this.bInsert);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bAppend);
            this.Controls.Add(this.bOpenIt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenWayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ファイルを受け取りました";
            this.Load += new System.EventHandler(this.OpenWayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bOpenIt;
        private System.Windows.Forms.Button bAppend;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bInsert;

    }
}