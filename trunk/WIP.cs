using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace yPDFEditor {
    public partial class WIP : UserControl {
        public WIP() {
            InitializeComponent();
        }
    }

    public class WIPPanel : IDisposable {
        WIP wip;

        public WIPPanel(Control parent) {
            wip = new WIP();
            wip.Parent = parent;
            wip.Location = Point.Empty;
            wip.Size = parent.ClientSize;
            wip.Show();
            wip.BringToFront();
            wip.Refresh();
        }

        #region IDisposable ÉÅÉìÉo

        public void Dispose() {
            wip.Dispose();
        }

        #endregion
    }
}
