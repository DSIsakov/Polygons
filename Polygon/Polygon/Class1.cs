using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Polygon
{
    abstract class Node
    {
        protected int x0;
        protected int y0;
        public bool flag { get; set; }
        public bool anyLine { get; set; }
        public static int r { get; set; }
        public static Color color { get; set; }
        public bool drag { get; set; }
        public int dx { get; set; }
        public int dy { get; set; }
        public Node(int x, int y)
        {
            x0 = x;
            y0 = y;
            flag = false;
            anyLine = false;
        }
        static Node()
        {
            r = 50;
            color = Color.Red;
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
            Point point1 = new Point(x0 - r / 2, y0 + r / 2);
            Point point2 = new Point(x0 + r / 2, y0 + r / 2);
            Point point3 = new Point(x0, y0 - r / 2);
            Point[] points = { point1, point2, point3 };
            g.FillPolygon(new SolidBrush(color), points);
        }
        public override bool Check(int x, int y)
        {
            int xd = x;
            int xa = x0;
            int xb = x0 - r / 2;
            int xc = x0 + r / 2;
            int yd = y;
            int ya = y0 - r / 2;
            int yb = y0 + r / 2;
            int yc = y0 + r / 2;
            if ((((xd - xa) * (yb - ya) - (yd - ya) * (xb - xa)) * ((xc - xa) * (yb - ya) - (yc - ya) * (xb - xa)) >= 0) && (((xd - xb) * (yc - yb) - (yd - yb) * (xc - xb)) * ((xa - xb) * (yc - yb) - (ya - yb) * (xc - xb)) >= 0) && (((xd - xc) * (ya - yc) - (yd - yc) * (xa - xc)) * ((xb - xc) * (ya - yc) - (yb - yc) * (xa - xc)) >= 0)) return true;
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
            if (Math.Pow(x - x0, 2) + Math.Pow(y - y0, 2) <= Math.Pow(r / 2, 2)) return true;
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