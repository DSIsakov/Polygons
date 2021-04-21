using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Polygon
{
    abstract class XOperator
    {
        abstract public void Undo();
        abstract public void Redo();
    }
    class XColor : XOperator
    {
        Color color;
        Color newColor;
        public XColor(Color color)
        {
            this.color = color;
        }
        public override void Undo()
        {
            newColor = Node.color;
            Node.color = color;
        }
        public override void Redo()
        {
            color = Node.color;
            Node.color = newColor;
        }
    }
    class XRadius : XOperator
    {
        int radius;
        int newRadius;
        public XRadius(int r)
        {
            radius = r;
        }
        public override void Undo()
        {
            newRadius = Node.r;
            Node.r = radius;
        }
        public override void Redo()
        {
            radius = Node.r;
            Node.r = newRadius;
        }
    }
}
