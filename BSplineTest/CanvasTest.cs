using Microsoft.VisualStudio.TestTools.UnitTesting;

using BSplineLib;

namespace BSplineTest
{
    [TestClass]
    public class CanvasTest:Canvas
    {
        [TestMethod]
        public void resolveEquationTest()
        {
            double[,] matrix = new double[,] {
                {2, 3, 0},
                {4, 5, 6},
                {0, 7, 8},
            };
            CanvasPoint[] b = new CanvasPoint[]
            {
                new CanvasPoint(0, 13, 8),
                new CanvasPoint(0, 47, 20),
                new CanvasPoint(0, 53, 22),
            };
            int cpCount = matrix.GetLength(0);
            CanvasPoint[] ctrPoints = new CanvasPoint[cpCount];
            Canvas canvas = new Canvas(0, 0);
            canvas.ResolveEquation(matrix, ctrPoints, b);
            Assert.AreEqual(2, ctrPoints[0].X);
            Assert.AreEqual(3, ctrPoints[1].X);
            Assert.AreEqual(4, ctrPoints[2].X);
            Assert.AreEqual(1, ctrPoints[0].Y);
            Assert.AreEqual(2, ctrPoints[1].Y);
            Assert.AreEqual(1, ctrPoints[2].Y);
        }

        [TestMethod]
        public void calcNiByUIdxTest()
        {
            Canvas canvas = new Canvas(0, 0);
            double[,] targetPointArr = new double[,] {
                {0, 0},
                {0, 2},
                {2, 2},
                {2, 0},
                {4, 0},
            };
            for (int i = 0; i < targetPointArr.GetLength(0); i++)
            {
                CanvasPoint p = new CanvasPoint(i, targetPointArr[i, 0], targetPointArr[i, 1]);
                canvas.targetPoints.Add(p);
            }
            canvas.CalcKnotPoints();

            var u = canvas.knotPoints;
            var degree = canvas.degree;
            int u_idx = degree;
            double v1 = canvas.CalcNiByUIdx(0, u_idx);
            double v2 = canvas.CalcNiByUValue(0, u[u_idx]);
            Assert.AreEqual(1, v1, $"u_idx:{u_idx}");
            Assert.AreEqual(1, v2, $"u_idx:{u_idx}");

            v1 = canvas.CalcNiByUIdx(1, u_idx);
            v2 = canvas.CalcNiByUValue(1, u[u_idx]);
            Assert.AreEqual(0, v1, $"u_idx:{u_idx}");
            Assert.AreEqual(0, v2, $"u_idx:{u_idx}");

            v1 = canvas.CalcNiByUIdx(2, u_idx);
            v2 = canvas.CalcNiByUValue(2, u[u_idx]);
            Assert.AreEqual(0, v1, $"u_idx:{u_idx}");
            Assert.AreEqual(0, v2, $"u_idx:{u_idx}");

            int n = targetPointArr.GetLength(0) - 1;
            u_idx = n + degree;
            v1 = canvas.CalcNiByUIdx(n, u_idx);
            v2 = canvas.CalcNiByUValue(n, u[u_idx]);
            Assert.AreEqual(0, v1, $"i:{n}, u_idx:{u_idx}");
            Assert.AreEqual(0, v2, $"i:{n}, u_idx:{u_idx}");

            v1 = canvas.CalcNiByUIdx(n + 1, u_idx);
            v2 = canvas.CalcNiByUValue(n + 1, u[u_idx]);
            Assert.AreEqual(0, v1, $"i:{n + 1}, u_idx:{u_idx}");
            Assert.AreEqual(0, v2, $"i:{n + 1}, u_idx:{u_idx}");

            v1 = canvas.CalcNiByUIdx(n + 2, u_idx);
            v2 = canvas.CalcNiByUValue(n + 2, u[u_idx]);
            Assert.AreEqual(1, v1, $"i:{n + 2}, u_idx:{u_idx}");
            Assert.AreEqual(1, v2, $"i:{n + 2}, u_idx:{u_idx}");
        }
    }
}