namespace yPDFEditor {
    partial class WIP {
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.flp = new System.Windows.Forms.FlowLayoutPanel();
            this.pb = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tlp.SuspendLayout();
            this.flp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.SuspendLayout();
            // 
            // tlp
            // 
            this.tlp.ColumnCount = 1;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.Controls.Add(this.flp, 0, 0);
            this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp.Location = new System.Drawing.Point(0, 0);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 1;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.Size = new System.Drawing.Size(150, 150);
            this.tlp.TabIndex = 0;
            // 
            // flp
            // 
            this.flp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flp.AutoSize = true;
            this.flp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flp.Controls.Add(this.pb);
            this.flp.Controls.Add(this.label1);
            this.flp.Location = new System.Drawing.Point(3, 64);
            this.flp.Name = "flp";
            this.flp.Size = new System.Drawing.Size(144, 22);
            this.flp.TabIndex = 0;
            // 
            // pb
            // 
            this.pb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pb.Image = global::yPDFEditor.Properties.Resources.ExpirationHS;
            this.pb.Location = new System.Drawing.Point(3, 3);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(16, 16);
            this.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb.TabIndex = 0;
            this.pb.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "しばらく、お待ちください...";
            // 
            // WIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tlp);
            this.Name = "WIP";
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.flp.ResumeLayout(false);
            this.flp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.FlowLayoutPanel flp;
        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.Label label1;
    }
}
