using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        bool draw;
        bool drag;
        int dx;
        int dy;
        int nShape;
        Node node;
        public Form1()
        {
            InitializeComponent();
            draw = false;
            drag = false;
            dx = 0;
            dy = 0;
            nShape = 0;
        }
        private void Form1_Load(object sender, EventArgs e) { DoubleBuffered = true; }
        private void triangleToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 0; }
        private void circleToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 1; }
        private void squareToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 2; }
        private void Form1_Paint(object sender, PaintEventArgs e) { if (draw) node.DrawNode(e.Graphics); }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (draw && node.Check(e.X, e.Y) && e.Button == MouseButtons.Left)
            {
                dx = e.X - node.SetX;
                dy = e.Y - node.SetY;
                drag = true;
            } 
            else if (draw && node.Check(e.X, e.Y) && e.Button == MouseButtons.Right)
            {
                draw = false;
                node = null;
                Refresh();
            }
            else
            {
                draw = true;
                switch (nShape)
                {
                    case 0:
                        node = new Triangle(e.X, e.Y);
                        break;
                    case 1:
                        node = new Circle(e.X, e.Y);
                        break;
                    case 2:
                        node = new Square(e.X, e.Y);
                        break;
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                node.SetX = e.X - dx;
                node.SetY = e.Y - dy;
                Refresh();
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            Refresh();
        }
    }
}