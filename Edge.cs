using System;
using System.Drawing;

namespace DelaunayTriangulation
{
    public class Edge
    {
	    // Грани у нас направленные
        public Point From, To;
        		
        public Edge(Point from, Point to) { From = from; To = to; }
        
        // Почему доступ по индексу? Потому что могу
        public Point this[int i]
        {
        	get
        	{
        		switch (i)
        		{
        			case 0:
        				return From;
        			case 1:
        				return To;
        			default:
        				throw new ArgumentException();
        		}
        	}
        	set
        	{
        		switch (i)
        		{
        			case 0:
        				From = value;
        				break;
        			case 1:
        				To = value;
        				break;
        			default:
        				throw new ArgumentException();
        		}
        	}
        }
        
        public static bool operator ==(Edge e1, Edge e2)
        {
        	if (ReferenceEquals(e1, e2))
        		return true;
        	if ((object)e1 == null || (object)e2 == null)
        		return false;
        	return e1.From == e2.From && e1.To == e2.To;
        }
        
        public static bool operator !=(Edge e1, Edge e2)
        {
        	return !(e1 == e2);
        }
    }
}