namespace yPDFEditor {
    partial class ThumbView {
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
            this.SuspendLayout();
            // 
            // ThumbView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Name = "ThumbView";
            this.Load += new System.EventHandler(this.ThumbView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ThumbView_Paint);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ThumbView_PreviewKeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ThumbView_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ThumbView_MouseDown);
            this.Resize += new System.EventHandler(this.ThumbView_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ThumbView_MouseUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThumbView_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
