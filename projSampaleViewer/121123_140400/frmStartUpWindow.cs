using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projSampaleViewer
{
    public partial class frmStartUpWindow : Form
    {
        private int startupTime = 0;
        public int numCards     = 0;

        public frmStartUpWindow()
        {
            InitializeComponent();
        }

        private void tmrStartup_Tick(object sender, EventArgs e)
        {
            startupTime++;
            lblTime.Text = startupTime.ToString() + "/" + numCards.ToString();            
        }

        private void frmStartUpWindow_Load(object sender, EventArgs e)
        {
            tmrStartup.Interval = 1000;
            tmrStartup.Enabled = true;
        }

        private void frmStartUpWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrStartup.Enabled = false;
        }
    }
}
