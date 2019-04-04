using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PEditor
{
    public partial class BlackAndWhiteSettings : Form
    {
        private int threshold;

        public int Threshold
        {
            get
            {
                return threshold;
            }
        }

        public BlackAndWhiteSettings()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            threshold = (int)numericThreshold.Value * 3;

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
