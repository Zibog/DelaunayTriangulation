using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DelaunayTriangulation
{
    public partial class Form1 : Form
    {
		public Form1()
		{
			InitializeComponent();
			_points = new List<Point>();
			_map = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
			pictureBox1.Image = _map;
			_g = Graphics.FromImage(_map);
			_g.Clear(Color.White);
		}
		
		private Bitmap _map;
		private Pen _pen = new Pen(Color.Green, 1);
		private Graphics _g;
		private List<Point> _points; // список точек
		private List<Edge> _livingEdges; // список "живых" граней
		private SolidBrush _brush = new SolidBrush(Color.Red);

		private void buttonClear_Click(object sender, EventArgs e)
		{
			_points = new List<Point>();
			Redraw();
		}
		
		private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
		{
			var p = new Point(e.X, e.Y);
			_points.Add(p);
			DrawPoint(p);
			pictureBox1.Image = _map;
		}

		private Point FindFirstPoint()
		{
			return _points.FindAll(p => p.X == _points.Min(pp => pp.X)).Min();
		}

		private Point FindSecondPoint(Point firstPoint)
		{
			var res = firstPoint;
			double minDegree = 180;
			foreach (var p in _points)
			{
				var deg = FindDegreesBetweenEdges(new Edge(firstPoint, new Point(firstPoint.X, firstPoint.Y - 1)), new Edge(firstPoint, p));
				if (deg < minDegree)
				{
					minDegree = deg;
					res = p;
				}
			}
			return res;
		}

		private void DrawPoint(Point p)
		{
			_g.FillEllipse(_brush, p.X - 4, p.Y - 4, 8, 8);
		}

		private void DrawEdge(Edge e)
		{
			_g.DrawLine(_pen, e.From.X, e.From.Y, e.To.X, e.To.Y);
		}
		
		private void Redraw()
		{
			_g.Clear(Color.White);
			foreach (var p in _points)
				DrawPoint(p);
			pictureBox1.Image = _map;
		}

		// Определяем положение точки относительно направленного ребра
		// 1 - слева, -1 - справа, 0 - на ребре
		private int FindPointCompareToLine(Edge e, Point p)
		{
			var a = e.From.Y - e.To.Y;
			var b = e.To.X - e.From.X;
			var c = e.From.X * e.To.Y - e.To.X * e.From.Y;
			var expr = a * p.X + b * p.Y + c;

			if (expr < 0)
				return 1;
			if (expr > 0)
				return -1;
			return 0;
		}
		
		// Вычисляем угол между двумя ребрами в градусах
		private double FindDegreesBetweenEdges(Edge e1, Edge e2)
		{
			var e1X = e1.To.X - e1.From.X;
			var e1Y = e1.To.Y - e1.From.Y;
			var e2X = e2.To.X - e2.From.X;
			var e2Y = e2.To.Y - e2.From.Y;
			var num = e1X * e2X + e1Y * e2Y;
			var denom = Math.Sqrt(e1X * e1X + e1Y * e1Y) * Math.Sqrt(e2X * e2X + e2Y * e2Y);
			return Math.Acos(num / denom) * 180 / Math.PI;
		}

		private void buttonTriangulate_Click(object sender, EventArgs e)
		{
			Redraw();
			if (_points.Count < 3)
				throw new ArgumentException("Count of points must be at least 3, but was " + _points.Count);
			Triangulate();
		}

		private void Triangulate()
		{
			var firstPoint = FindFirstPoint();
			var secondPoint = FindSecondPoint(firstPoint);
			
			DrawPoint(firstPoint);
			DrawPoint(secondPoint);
			
			_livingEdges = new List<Edge> { new Edge(firstPoint, secondPoint) };

			// Для каждого ребра из списка живых
			while (_livingEdges.Any())
			{
				var currentEdge = _livingEdges.Last();
				_livingEdges.RemoveAt(_livingEdges.Count - 1);
				var currentRadius = double.MaxValue;
				var thirdPoint = new Point(-1, -1);
				
				foreach (var p in _points)
				{
					DrawPoint(p);
					// Ищем правую сопряжённую точку
					if (FindPointCompareToLine(currentEdge, p) != -1)
						continue;
					var t = new Triangle(currentEdge[0], currentEdge[1], p);
					// При этом максим радиус
					if (currentRadius > t.Radius)
					{
						currentRadius = t.Radius;
						thirdPoint = p;
					}
				}

				DrawEdge(currentEdge);

				// Идём дальше, если точки не справа (типа бесконечность)
				if (thirdPoint.X == -1 || thirdPoint.Y == -1) 
					continue;
				
				var nextEdge = new Edge(currentEdge[0], thirdPoint);
				
				int i;
				var edgeToFind = new Edge(nextEdge[1], nextEdge[0]);
				// Если вновь найденные рёбра отсутствуют в списке живых, то заносим
				if ((i = _livingEdges.FindIndex(ee => ee == edgeToFind)) < 0)
				{
					_livingEdges.Add(nextEdge);
				}
				// Если ребро было живым, то рисуем и удаляем из жижзни
				else
				{
					DrawEdge(_livingEdges[i]);
					_livingEdges.RemoveAt(i);
				}

				// Аналогично проверяем для второй точки грани
				nextEdge = new Edge(thirdPoint, currentEdge[1]);
				
				edgeToFind = new Edge(nextEdge[1], nextEdge[0]);
				if ((i = _livingEdges.FindIndex(ee => ee == edgeToFind)) < 0)
				{
					_livingEdges.Add(nextEdge);
				}
				else
				{
					DrawEdge(_livingEdges[i]);
					_livingEdges.RemoveAt(i);
				}
			}
		}
    }
}
