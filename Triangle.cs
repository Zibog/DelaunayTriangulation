using System;
using System.Drawing;

namespace DelaunayTriangulation
{
    public class Triangle
    {
        // Точки, по которым собираемся строить предполагаемый треугольник
        private Point _a, _b, _c;
        // Центр описанной окружности
        private PointF _circleCenter;
        // Радиус описанной окружности
        public double Radius;

        public Triangle(Point a, Point b, Point c)
        {
            _a = a; _b = b; _c = c;
            FindCircleCenter();
            FindRadius();
        }
		
        // Находим координаты центра описанной окружности
        // https://matematiku.ru/zadachi-po-visshei-matematike-500/439.php
        private void FindCircleCenter()
        {
            var dxAB = _b.X - _a.X;
            var dyAB = _b.Y - _a.Y;
            var dxAC = _c.X - _a.X;
            var dyAC = _c.Y - _a.Y;
			
            var meanABbyX = (_a.X + _b.X) / 2;
            var meanABbyY = (_a.Y + _b.Y) / 2;
            var meanACbyX = (_a.X + _c.X) / 2;
            var meanACbyY = (_a.Y + _c.Y) / 2;
			
            var c1 = meanABbyX * dxAB + meanABbyY * dyAB;
            var c2 = meanACbyX * dxAC + meanACbyY * dyAC;

            var centerX = (float)Math.Round((c1 * dyAC - c2 * dyAB) / (double)(dxAB * dyAC - dxAC * dyAB));
            var centerY = (float)Math.Round((dxAB * c2 - dxAC * c1) / (double)(dxAB * dyAC - dxAC * dyAB));
            _circleCenter = new PointF(centerX, centerY);
        }
		
        // Находим квадрат радиуса описанной окружности
        private void FindRadius()
        {
            var dyAB = _b.Y - _a.Y;
            var dxAB = _b.X - _a.X;
            var num = -1 * dyAB * _circleCenter.X + dxAB * _circleCenter.Y + (_a.X * _b.Y + _b.X * _a.Y);
            Radius = num / Math.Sqrt(dxAB * dxAB + dyAB * dyAB);
        }
    }
}