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
        List<Node> nodes = new List<Node>();
        bool draw;
        bool drag;
        int dx;
        int dy;
        int nShape;
        Node thisNode;
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
        private void Form1_Paint(object sender, PaintEventArgs e) { foreach (Node node in nodes) { node.DrawNode(e.Graphics); }; }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Node node in nodes)
            {
                if (node.Check(e.X, e.Y))
                {
                    thisNode = node;
                }
            }
            if (draw && thisNode.Check(e.X, e.Y) && e.Button == MouseButtons.Left)
            {
                dx = e.X - thisNode.SetX;
                dy = e.Y - thisNode.SetY;
                drag = true;
            }
            else if (draw && thisNode.Check(e.X, e.Y) && e.Button == MouseButtons.Right)
            {
                nodes.Remove(thisNode);
                Refresh();
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    draw = true;
                    switch (nShape)
                    {
                        case 0:
                            thisNode = new Triangle(e.X, e.Y);
                            nodes.Add(thisNode);
                            break;
                        case 1:
                            thisNode = new Circle(e.X, e.Y);
                            nodes.Add(thisNode);
                            break;
                        case 2:
                            thisNode = new Square(e.X, e.Y);
                            nodes.Add(thisNode);
                            break;
                    }
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                thisNode.SetX = e.X - dx;
                thisNode.SetY = e.Y - dy;
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