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
    public delegate void RChEventHandler(object sender, RadiusEventArgs e);
    public partial class Form3 : Form
    {
        public event RChEventHandler RCh;
        public Form3(int r)
        {
            InitializeComponent();
            numericUpDown1.Value = r;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int v = Convert.ToInt32(numericUpDown1.Value);
            RCh?.Invoke(this, new RadiusEventArgs(v));
        }
    }
    public class RadiusEventArgs : EventArgs
    {
        public int R { get; set; }
        public RadiusEventArgs(int r) { R = r; }
    }
}
