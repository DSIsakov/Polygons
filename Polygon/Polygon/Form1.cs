using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Polygon
{
    public partial class Form1 : Form
    {
        List<Node> nodes = new();
        int nShape;
        bool ifDragNode;
        static Random random;
        int Radius;
        Form3 boo;
        bool isOpened;
        bool isChanged;
        string path;
        public Form1()
        {
            InitializeComponent();
            nShape = 0;
            random = new Random();
            ifDragNode = false;
            Radius = Node.r;
            isOpened = false;
            isChanged = false;
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
            Triangle node0 = new(x, y);
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
            if (!node0.anyLine) return true;
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
                            isChanged = true;
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
                    isChanged = true;
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
                isChanged = true;
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
                    isChanged = true;
                    Refresh();
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Node node in nodes) node.drag = false;
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
                        isChanged = true;
                    }
                }
            }
            Refresh();
        }
        private void nodeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new();
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
        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new(timer1.Interval);
            form.ShowDialog();
            timer1.Interval = form.t;
        }
        private void RadiusDelegate(object sender, RadiusEventArgs e)
        {
            Radius = e.R;
            Node.r = Radius;
            Refresh();
        }
        private void radiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (boo == null || boo.IsDisposed)
            {
                boo = new Form3(Radius);
                boo.Show();
                boo.RCh += RadiusDelegate;
            }
            else
            {
                boo.Show();
                boo.RCh += RadiusDelegate;
            }
            boo.Focus();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Save?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (isOpened) Save(path);
                    else
                    {
                        SaveFileDialog save = new SaveFileDialog() { InitialDirectory = @"..\..\Images", Filter = "Binary files(*.dat) | *.dat" };
                        if (save.ShowDialog() == DialogResult.Yes) Save(save.FileName);
                    }
                }
            }
            OpenFileDialog open = new OpenFileDialog() { InitialDirectory = @"..\..\Images", Filter = "Binary files(*.dat) | *.dat" };
            if (open.ShowDialog() == DialogResult.OK)
            {
                Open(open.FileName);
                isOpened = true;
                path = open.FileName;
            }
            isChanged = false;
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog() { InitialDirectory = @"..\..\Images", Filter = "Binary files(*.dat) | *.dat" };
            if (save.ShowDialog() == DialogResult.OK) Save(save.FileName);
            isChanged = false;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Save?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (isOpened) Save(path);
                    else
                    {
                        SaveFileDialog save = new SaveFileDialog() { InitialDirectory = @"..\..\Images", Filter = "Binary files(*.dat) | *.dat" };
                        if (save.ShowDialog() == DialogResult.Yes) Save(save.FileName);
                    }
                }
            }
            nodes.Clear();
            Node.color = Color.Turquoise;
            Node.r = 50;
            timer1.Stop();
            timer1.Interval = 100;
            nShape = 0;
            isChanged = false;
            Refresh();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Save?", "", MessageBoxButtons.YesNoCancel);
                if (DialogResult.Yes == result)
                {
                    if (isOpened) Save(path);
                    else
                    {
                        SaveFileDialog save = new SaveFileDialog() { InitialDirectory = @"..\..\Images", Filter = "Binary files(*.dat) | *.dat" };
                        if (save.ShowDialog() == DialogResult.Yes) Save(save.FileName);
                    }
                }
                else if (DialogResult.Cancel == result) e.Cancel = true;
            }
        }
        private void Save(string path)
        {
            Stream stream = File.Open(path, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            List<object> info = new List<object>();
            foreach (Node node in nodes) info.Add(node);
            info.Add(Node.color);
            info.Add(Node.r);
            bf.Serialize(stream, info);
            stream.Close();
        }
        private void Open(string path)
        {
            Stream stream = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<object> info = (List<object>)bf.Deserialize(stream);
            nodes.Clear();
            if (info.Count > 2) for (int i = 0; i < info.Count - 2; i++) nodes.Add((Node)info[i]);
            Node.color = (Color)info[info.Count - 2];
            Node.r = (int)info[info.Count - 1];
            stream.Close();
            Refresh();
        }
    }
}