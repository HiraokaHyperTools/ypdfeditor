namespace yPDFEditor {
    partial class JSort {
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
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.flpW1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bW1 = new System.Windows.Forms.Button();
            this.tlp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp
            // 
            this.tlp.AutoScroll = true;
            this.tlp.AutoSize = true;
            this.tlp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlp.ColumnCount = 1;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.Controls.Add(this.flpW1, 0, 1);
            this.tlp.Controls.Add(this.bW1, 0, 0);
            this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp.Location = new System.Drawing.Point(0, 0);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 3;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp.Size = new System.Drawing.Size(739, 484);
            this.tlp.TabIndex = 1;
            // 
            // flpW1
            // 
            this.flpW1.AutoSize = true;
            this.flpW1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpW1.Location = new System.Drawing.Point(3, 31);
            this.flpW1.Name = "flpW1";
            this.flpW1.Size = new System.Drawing.Size(0, 0);
            this.flpW1.TabIndex = 1;
            // 
            // bW1
            // 
            this.bW1.AutoSize = true;
            this.bW1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bW1.Image = global::yPDFEditor.Properties.Resources.GoLtrHS;
            this.bW1.Location = new System.Drawing.Point(3, 3);
            this.bW1.Name = "bW1";
            this.bW1.Size = new System.Drawing.Size(421, 22);
            this.bW1.TabIndex = 2;
            this.bW1.Text = "表の全ページをスキャンし、裏の全ページをスキャンしました。次のようになっています。";
            this.bW1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bW1.UseVisualStyleBackColor = true;
            this.bW1.Click += new System.EventHandler(this.bW1_Click);
            // 
            // JSort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 484);
            this.Controls.Add(this.tlp);
            this.Name = "JSort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ソート";
            this.Load += new System.EventHandler(this.JSort_Load);
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.FlowLayoutPanel flpW1;
        private System.Windows.Forms.Button bW1;


    }
}