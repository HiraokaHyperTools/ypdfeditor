using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace yPDFEditor
{
    public partial class WIP2 : UserControl
    {
        public WIP2()
        {
            InitializeComponent();
        }

        private void WIP2_Load(object sender, EventArgs e)
        {

        }
    }

    public class WIPPanel : IDisposable
    {
        WIP2 wip;

        public WIPPanel(Control parent)
        {
            wip = new WIP2();
            wip.Parent = parent;
            wip.Left = (parent.Width - wip.Width) / 2;
            wip.Top = (parent.Height - wip.Height) / 2;
            wip.Show();
            wip.BringToFront();
            parent.Update();
        }

        #region IDisposable ÉÅÉìÉo

        public void Dispose()
        {
            if (wip != null)
                wip.Dispose();
        }

        #endregion
    }
}
