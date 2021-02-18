using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Polygon
{
    public partial class Form1 : Form
    {
        List<Node> nodes = new List<Node>();
        int nShape;
        bool ifDragNode;
        Random random;
        Node node0;
        public Form1()
        {
            InitializeComponent();
            nShape = 0;
            random = new Random();
            ifDragNode = false;
            node0 = new Triangle(0, 0);

        }
        private void Form1_Load(object sender, EventArgs e) { DoubleBuffered = true; }
        private void triangleToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 0; }
        private void circleToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 1; }
        private void squareToolStripMenuItem_Click(object sender, EventArgs e) { nShape = 2; }
        private void startToolStripMenuItem_Click(object sender, EventArgs e){ timer1.Start(); }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e){ timer1.Stop(); }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Node node in nodes) node.DrawNode(e.Graphics);
            if (nodes.Count > 2)
            {
                foreach (Node node1 in nodes)
                {
                    node1.anyLine = false;
                    foreach (Node node2 in nodes)
                    {
                        node2.anyLine = false;
                        if (node1 == node2) continue;
                        int up = 0;
                        int down = 0;
                        foreach (Node node3 in nodes)
                        {
                            if (node3 == node1 || node3 == node2) continue;
                            if ((node3.SetY - node1.SetY) * (node2.SetX - node1.SetX) > (node3.SetX - node1.SetX) * (node2.SetY - node1.SetY)) up++;
                            else down++;
                        }
                        if (up == 0 || down == 0)
                        {
                            e.Graphics.DrawLine(new Pen(Color.Red), node1.SetX, node1.SetY, node2.SetX, node2.SetY);
                            node1.anyLine = true;
                            node2.anyLine = true;
                        }
                    }
                }
            }
        }
        private bool IsInsidePolygon(int x, int y)
        {
            node0.SetX = x;
            node0.SetY = y;
            int up;
            int down;
            node0.anyLine = false;
            foreach (Node node1 in nodes)
            {
                if (node0.SetX == node1.SetX && node0.SetY == node1.SetY) continue;
                up = 0;
                down = 0;
                foreach (Node node2 in nodes)
                {
                    if (node1 == node2) continue;
                    else if (node0.SetX == node2.SetX && node0.SetY == node2.SetY) continue;
                    if ((node2.SetY - node0.SetY) * (node1.SetX - node0.SetX) > (node2.SetX - node0.SetX) * (node1.SetY - node0.SetY)) up++;
                    else down++;
                }
                if (up == 0 || down == 0)
                {
                    node0.anyLine = true;
                    break;
                }
            }
            if (!node0.anyLine)
            {
                return true;
            }
            
            return false;
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (nodes.Any())
            {
                ifDragNode = false;
                foreach (Node node in nodes) 
                {
                    if (node.Check(e.X, e.Y))
                    {
                        ifDragNode = true;
                        if (e.Button == MouseButtons.Left)
                        {
                            node.drag = true;
                            node.dx = e.X - node.SetX;
                            node.dy = e.Y - node.SetY;
                        }
                        else
                        {
                            nodes.Remove(node);
                            Refresh();
                            break;
                        }
                    }
                }
                if (IsInsidePolygon(e.X, e.Y) && !ifDragNode && nodes.Count > 2) 
                {
                    foreach (Node node in nodes)
                    {
                        node.drag = true;
                        node.dx = e.X - node.SetX;
                        node.dy = e.Y - node.SetY;
                    }
                }
                else if (!IsInsidePolygon(e.X, e.Y) && !ifDragNode){ 
                    switch (nShape)
                    {
                        case 0:
                            nodes.Add(new Triangle(e.X, e.Y));
                            break;
                        case 1:
                            nodes.Add(new Circle(e.X, e.Y));
                            break;
                        case 2:
                            nodes.Add(new Square(e.X, e.Y));
                            break;
                    }
                    Refresh();
                }
            }
            else
            {
                switch (nShape)
                {
                    case 0:
                        nodes.Add(new Triangle(e.X, e.Y));
                        break;
                    case 1:
                        nodes.Add(new Circle(e.X, e.Y));
                        break;
                    case 2:
                        nodes.Add(new Square(e.X, e.Y));
                        break;
                }
                Refresh();
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Node node in nodes)
            {
                if (node.drag)
                {
                    node.SetX = e.X - node.dx;
                    node.SetY = e.Y - node.dy;
                    Refresh();
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Node node in nodes)
            {
                node.drag = false;
            }
            if (nodes.Count > 2)
            {
                int i = 0;
                int k = nodes.Count;
                for (; i < k ; i++)
                {
                    if (IsInsidePolygon(nodes[i].SetX, nodes[i].SetY))
                    {
                        nodes.Remove(nodes[i]);
                        i--;
                        k--;
                    }
                }
                //for (int i = 0; i < nodes.Count; i++)
                //{
                //    if (IsInsidePolygon(nodes[i].SetX, nodes[i].SetY))
                //    {
                //        nodes.Remove(nodes[i]);
                //    }
                //}
            }
            Refresh();
        }

        private void nodeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = false;
            colorDialog.Color = Node.color;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Node.color = colorDialog.Color;
                Refresh();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int k;
            foreach (Node node in nodes)
            {
                k = random.Next(-5, 5);
                node.SetX += k;
                k = random.Next(-5, 5);
                node.SetY += k;
            }
            if (nodes.Count > 2)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (IsInsidePolygon(nodes[i].SetX, nodes[i].SetY))
                    {
                        nodes.Remove(nodes[i]);
                    }
                }
            }
            Refresh();
        }
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    int k;
        //    k = random.Next(-5, 5);
        //    foreach (Node node in nodes)
        //    {
        //        node.SetX += k;
        //        node.SetY += k;
        //    }
        //    Refresh();
        //}

        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(timer1.Interval);
            form.ShowDialog();
            timer1.Interval = form.t;
        }
    }
}