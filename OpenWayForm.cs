using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace yPDFEditor
{
    public partial class OpenWayForm : Form
    {
        bool useInsert;

        public OpenWayForm(bool useInsert)
        {
            this.useInsert = useInsert;

            InitializeComponent();
        }

        private void OpenWayForm_Load(object sender, EventArgs e)
        {
            bInsert.Visible = useInsert;
        }
    }
}