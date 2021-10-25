using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelaunayTriangulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Clear();
        }

        private Bitmap map = new Bitmap(100, 100);
        private Graphics g;
        private List<Point> points;

        private void buttonTriangulate_Click(object sender, EventArgs e)
        {
            var s = FindStartEdge();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(e.Location);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            points = new List<Point>();
            Rectangle r = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(r.Width, r.Height);
            g = Graphics.FromImage(map);
            g.Clear(Color.White);
            pictureBox1.Image = map;
        }

        private class Edge
        {
            public Point From;
            public Point To;

            public Point this[int i]
            {
                get { if (i == 0) return From; else if (i == 1) return To; else throw new ArgumentException(); }
                set { if (i == 0) From = value; else if (i == 1) To = value; else throw new ArgumentException(); }
            }
        }

        private Edge FindStartEdge()
        {
            // Берём самую левую точку
            Point from = points.Min();

            return new Edge();
        }
    }
}
