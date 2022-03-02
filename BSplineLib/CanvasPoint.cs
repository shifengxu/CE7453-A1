using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSplineLib
{
    public class CanvasPoint
    {
        public int index;
        public double X;    // dobule. logical location in canvas.
        public double Y;

        /**
         * int. the pixel location in canvas.
         * But this is not the Panel pixel location (coordinate).
         * Because the origin point (0, 0) is different:
         *      Canvas origin point is at bottom left corner;
         *      Panel origin point is at top left corner.
         */
        public int canvasX;
        public int canvasY;

        public CanvasPoint(int index, CanvasPoint canvasPoint)
        {
            this.index = index;
            this.X = canvasPoint.X;
            this.Y = canvasPoint.Y;
        }

        public CanvasPoint(int index, double x, double y)
        {
            this.index = index;
            this.X = x;
            this.Y = y;
        }

        override
        public string ToString()
        {
            string xStr = $"{X,5:N2}".Replace(".00", "   ");
            string yStr = $"{Y,5:N2}".Replace(".00", "   ");
            return $"({xStr}, {yStr})";
        }
    }
}
