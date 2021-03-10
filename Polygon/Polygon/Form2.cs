using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polygon
{
    public partial class Form2 : Form
    {
        public int t { get; set; }
        public Form2(int t)
        {
            InitializeComponent();
            this.t = t;
            trackBar1.Value = t;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            t = trackBar1.Value;
        }
    }
}
