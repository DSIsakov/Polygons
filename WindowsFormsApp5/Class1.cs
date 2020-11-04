using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    abstract class Node
    {
        protected int x0;
        protected int y0;
        public static int r { get; set; }
        public static Color color { get; set; }
        public Node(int x, int y)
        {
            x0 = x;
            y0 = y;
        }
        static Node()
        {
            r = 50;
            color = Color.Black;
        }
        public int SetX 
        { 
            get { return x0; } 
            set { x0 = value; } 
        }
        public int SetY
        { 
            get { return y0; } 
            set { y0 = value; } 
        }
        abstract public void DrawNode(Graphics g);
        abstract public bool Check(int x, int y);
    }
    class Triangle : Node
    {
        public Triangle(int x, int y) : base(x, y) { }
        public override void DrawNode(Graphics g)
        {
            Point point1 = new Point(x0 - r/2, y0 + r/2);
            Point point2 = new Point(x0 + r/2, y0 + r/2);
            Point point3 = new Point(x0, y0 - r/2);
            Point[] points = { point1, point2, point3 };
            g.FillPolygon(new SolidBrush(color), points);
        }
        public override bool Check(int x, int y)
        {
            if (x0 - (r / 2) < x && x0 + (r / 2) > x && y0 - r < y && y0 + (r / 2) > y) return true;
            else return false;
        }
    }
    class Circle : Node
    {
        public Circle(int x, int y) : base(x, y) { }
        public override void DrawNode(Graphics g)
        {
            g.FillEllipse(new SolidBrush(color), x0 - (r / 2), y0 - (r / 2), r, r);
        }
        public override bool Check(int x, int y)
        {
            if (Math.Sqrt(Math.Pow(x - x0 - (r / 2), 2) + Math.Pow(y - y0 - (r / 2), 2)) < r) return true;
            else return false;
        }
    }
    class Square : Node
    {
        public Square(int x, int y) : base(x, y) { }
        public override void DrawNode(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), x0 - (r / 2), y0 - (r / 2), r, r);
        }
        public override bool Check(int x, int y)
        {
            if (x0 - (r / 2) < x && x0 + (r / 2) > x && y0 - (r / 2) < y && y0 + (r / 2) > y) return true;
            else return false;
        }
    }
}