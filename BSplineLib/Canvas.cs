using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BSplineLib
{
    public class Canvas
    {
        public List<CanvasPoint> targetPoints = new List<CanvasPoint>();
        public List<double> knotPoints = new List<double>();
        public List<CanvasPoint> controlPoints = new List<CanvasPoint>();

        public readonly int degree = 3;

        public int panelWidth;
        public int panelHeight;

        // X * targetRatio = canvasX; Y * targetRatio = canvasY
        private double _targetRatio = 1;
        public double targetRatio { get => _targetRatio; }

        private int _marginX = 0;
        public int marginX { get => _marginX; }

        private int _marginY = 0;
        public int marginY { get => _marginY; }

        private double _minX = 0;
        public double minX { get => _minX; }

        private double _minY = 0;
        public double minY { get => _minY; }

        public Canvas(int panelWidth, int panelHeight)
        {
            this.panelWidth = panelWidth;
            this.panelHeight = panelHeight;
        }

        public Canvas(): this(0, 0) { }

        public List<CanvasPoint> LoadTargetPoints(string filepath)
        {
            this.targetPoints.Clear();
            this.controlPoints.Clear();
            this.knotPoints.Clear();
            string[] lines = File.ReadAllLines(filepath);
            int index = 0;
            foreach (string line in lines)
            {
                string l = line.Trim();
                if (string.IsNullOrWhiteSpace(l) || l.StartsWith('#'))
                    continue;
                string[] arr = Regex.Split(l, @"\s+");
                int x = Int32.Parse(arr[0]);
                int y = Int32.Parse(arr[1]);
                CanvasPoint p = new CanvasPoint(index++, x, y);
                targetPoints.Add(p);

            }
            return targetPoints;
        }

        public void RefreshPointCoordinate()
        {
            var tps = this.targetPoints;
            var cps = this.controlPoints;
            if (tps.Count == 0 && cps.Count == 0)
                return;
            double min_x = 0;
            double min_y = 0;
            double max_x = 0;
            double max_y = 0;
            for (int i = 0; i < tps.Count; i++)
            {
                min_x = Math.Min(min_x, tps[i].X);
                min_y = Math.Min(min_y, tps[i].Y);
                max_x = Math.Max(max_x, tps[i].X);
                max_y = Math.Max(max_y, tps[i].Y);
            }
            for (int i = 0; i < cps.Count; i++)
            {
                min_x = Math.Min(min_x, cps[i].X);
                min_y = Math.Min(min_y, cps[i].Y);
                max_x = Math.Max(max_x, cps[i].X);
                max_y = Math.Max(max_y, cps[i].Y);
            }
            double delta_x = max_x - min_x;
            double delta_y = max_y - min_y;
            int width = this.panelWidth;
            int height = this.panelHeight;
            _marginX = width / 10;
            _marginY = height / 10;
            _minX = min_x;
            _minY = min_y;
            // as delta_x or delta_y could be 0, so not divide by them.
            double ratio_x = (double)delta_x / (width - 2 * _marginX);
            double ratio_y = (double)delta_y / (height - 2 * _marginY);
            double ratio = Math.Max(ratio_x, ratio_y);
            this._targetRatio = 1.0 / ratio;

            foreach (CanvasPoint p in tps)
            {
                p.canvasX = RealX2CanvasX(p.X);
                p.canvasY = RealY2CanvasY(p.Y);
            }
            foreach (CanvasPoint p in cps)
            {
                p.canvasX = RealX2CanvasX(p.X);
                p.canvasY = RealY2CanvasY(p.Y);
            }
        }

        public int RealX2CanvasX(double x)
        {
            return (int)((x - _minX) * targetRatio) + _marginX;
        }

        public int RealY2CanvasY(double y)
        {
            return (int)((y - _minY) * targetRatio) + _marginY;
        }

        public double CanvasX2RealX(int x)
        {
            return (double)(x - _marginX) / targetRatio + _minX;
        }

        public double CanvasY2RealY(int y)
        {
            return (double)(y - _marginY) / targetRatio + _minY;
        }

        public List<double> CalcKnotPoints()
        {
            this.knotPoints.Clear();
            for (int i = 0; i <= degree; i++)    // the first degree points, and t0
            {
                this.knotPoints.Add(0.0);
            }
            // So far, t0 = 0 is already added.
            // Now, calcuate t1, t2, , , , t(n-1)
            var tps = this.targetPoints;
            double[] distArr = new double[tps.Count];
            distArr[0] = 0;
            for (int i = 1; i < tps.Count; i++)
            {
                var p0 = tps[i - 1];
                var p1 = tps[i];
                var dist = Math.Sqrt(Math.Pow(p0.X - p1.X, 2) + Math.Pow(p0.Y - p1.Y, 2));
                distArr[i] = distArr[i - 1] + dist;
            }

            var totalDist = distArr[distArr.Length - 1];
            for (int i = 1; i < tps.Count - 1; i++)
            {
                this.knotPoints.Add(distArr[i] / totalDist);
            }

            for (int i = 0; i <= degree; i++)    // tn, and the last degree points
            {
                this.knotPoints.Add(1.0);
            }
            return this.knotPoints;
        }

        public List<CanvasPoint> CalcControlPoints(string outputFilepath="")
        {
            /* If degree is 3, target points count is 5: D0, D1, D2, D3, D4.
             * Then the parameters will be t0, t1, t2, t3, t4; 
             * And knot will be: t0 t0 t0 t0 t1 t2 t3 t4 t4 t4 t4.
             * So knot count will be 5 + 3 + 3 = 11
             * So control point count will be 11 - 1 - 3 = 7
             */
            int cpCount = knotPoints.Count - 1 - degree; // control point count
            double[,] matrix = new double[cpCount, cpCount];
            CanvasPoint[] b = new CanvasPoint[cpCount];
            /* matrix operation: M x P = B
             * M: matrix
             * P: ctrPoints
             * B: b
             */
            int n = targetPoints.Count - 1; // also cpCount - 3
            // ------------------------------------------------------ row 0
            matrix[0, 0] = 1;
            b[0] = new CanvasPoint(0, targetPoints[0]);
            // ------------------------------------------------------ row n+2
            matrix[n + 2, n + 2] = 1;
            var tp = targetPoints[n];
            b[n + 2] = new CanvasPoint(n + 2, tp);
            // ------------------------------------------------------ row 1
            matrix[1, 0] = CalcNiDerivative2(0, 3);
            matrix[1, 1] = CalcNiDerivative2(1, 3);
            matrix[1, 2] = CalcNiDerivative2(2, 3);
            b[1] = new CanvasPoint(1, 0.0, 0.0);
            // ------------------------------------------------------ row n+1
            matrix[n + 1, n] = CalcNiDerivative2(n, n + 3);
            matrix[n + 1, n + 1] = CalcNiDerivative2(n + 1, n + 3);
            matrix[n + 1, n + 2] = CalcNiDerivative2(n + 2, n + 3);
            b[n + 1] = new CanvasPoint(n + 1, 0.0, 0.0);
            // ------------------------------------------------------ row 2 ~ n
            for (int rIdx = 2; rIdx <= n; rIdx++)
            {
                for (int cIdx = rIdx - 1; cIdx <= rIdx + 1; cIdx++)
                {
                    int n_idx = rIdx - 1;
                    matrix[rIdx, cIdx] = CalcNiByUIdx(cIdx, n_idx + 3);
                }
                b[rIdx] = new CanvasPoint(rIdx, targetPoints[rIdx - 1]);
            }

            CanvasPoint[] ctrPoints = new CanvasPoint[cpCount];
            ResolveEquation(matrix, ctrPoints, b, outputFilepath);
            this.controlPoints.Clear();
            this.controlPoints.AddRange(ctrPoints);
            return this.controlPoints;
        }

        public void ResolveEquation(double[,] matrix, CanvasPoint[] ctrPoints,
            CanvasPoint[] b, string outputFilepath = "")
        {
            if (outputFilepath != null && outputFilepath != string.Empty)
                PrintEquation(matrix, b, outputFilepath, false);

            double k = matrix[0, 0];
            if (k != 1)
            {
                matrix[0, 0] = 1;
                matrix[0, 1] /= k;
                b[0].X /= k;
                b[0].Y /= k;
            }

            int len = matrix.GetLength(0);
            for (int r = 1; r < len; r++)
            {
                if (matrix[r, r - 1] != 0)
                {
                    k = matrix[r, r - 1];
                    matrix[r, r - 1] = 0;
                    matrix[r, r] -= k * matrix[r - 1, r];
                    if (r + 1 < len) matrix[r, r + 1] -= k * matrix[r - 1, r + 1];
                    b[r].X -= k * b[r - 1].X;
                    b[r].Y -= k * b[r - 1].Y;
                }
                if (matrix[r, r] != 1)
                {
                    double q = matrix[r, r];
                    if (q == 0)
                    {
                        if (outputFilepath != null && outputFilepath != string.Empty)
                            PrintEquation(matrix, b, outputFilepath, true);
                        string msg = $"Matrix[{r},{r}] is 0 when resolving equations. ";
                        msg += "Possible reason is target points have overlapping.";
                        throw new DivideByZeroException(msg);
                    }
                    matrix[r, r] = 1;
                    if (r + 1 < len) matrix[r, r + 1] /= q;
                    b[r].X /= q;
                    b[r].Y /= q;
                }
            }
            if (outputFilepath != null && outputFilepath != string.Empty)
                PrintEquation(matrix, b, outputFilepath, true);

            for (int i = len - 1; i > 0; i--)
            {
                ctrPoints[i] = new CanvasPoint(i, b[i]);
                k = matrix[i - 1, i];
                matrix[i - 1, i] = 0;
                b[i - 1].X -= k * b[i].X;
                b[i - 1].Y -= k * b[i].Y;
            }
            ctrPoints[0] = new CanvasPoint(0, b[0]);

            if (outputFilepath != null && outputFilepath != string.Empty)
                PrintEquation(matrix, b, outputFilepath, true);
        }

        private void PrintEquation(double[,] matrix, CanvasPoint[] b, string fpath, bool append)
        {
            using (StreamWriter sw = new StreamWriter(fpath, append))
            {
                if (append)
                {
                    sw.WriteLine();
                    sw.WriteLine("------------------------------------");
                    sw.WriteLine();
                }
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        string str = $"{matrix[i, j],7:N2}".Replace(".00", "   ");
                        sw.Write($"{str} ");
                    }
                    sw.WriteLine(b[i].ToString());
                }
            }
        }

        public Tuple<int, int> GetCanvasXYByUValue(double u_value)
        {
            double n1, n2, n3, n4, x, y;
            var P = controlPoints;
            int u_idx = this.GetUIndexByValue(u_value);
            n1 = this.CalcNiByUValueIdx(u_idx - 3, u_value, u_idx);
            n2 = this.CalcNiByUValueIdx(u_idx - 2, u_value, u_idx);
            n3 = this.CalcNiByUValueIdx(u_idx - 1, u_value, u_idx);
            n4 = this.CalcNiByUValueIdx(u_idx, u_value, u_idx);
            x = n1 * P[u_idx - 3].canvasX;
            x += n2 * P[u_idx - 2].canvasX;
            x += n3 * P[u_idx - 1].canvasX;
            x += n4 * P[u_idx].canvasX;
            y = n1 * P[u_idx - 3].canvasY;
            y += n2 * P[u_idx - 2].canvasY;
            y += n3 * P[u_idx - 1].canvasY;
            y += n4 * P[u_idx].canvasY;
            return new Tuple<int, int>((int)x, (int)y);
        }

        public int GetUIndexByValue(double u_value)
        {
            /* Assume t0 ~ tn corresponds to target points D0 ~ Dn.
             * So knot vector is: t0, t0, t0, t0, t1, , tn, tn, tn, tn
             * for knot vector u: t0 is u3, and tn is u(n+3)
             * The domain range of u_value is: [t0, tn], namely:
             *      [u3, u(n+3)]
             * degree is 3.
             * So for u_idx, 
             *      start index is degree, 
             *      end index is u_len - 1 - degree.
             * 
             */
            var u = knotPoints;
            int u_len = this.knotPoints.Count;
            int endIdx = u_len - 1 - degree;
            for (int idx = degree; idx < endIdx; idx++)
            {
                if (u_value >= u[idx] && u_value < u[idx + 1])
                    return idx;
            }
            /* special case for u_value is tn.
             */
            if (u_value == u[endIdx])
                return endIdx;

            throw new ArgumentException($"Not found u_idx for input: u_value:{u_value}");
        }

        /**
         * Calculate Ni by u value
         */
        public double CalcNiByUValue(int i_idx, double u_value)
        {
            var u_idx = GetUIndexByValue(u_value);
            return CalcNiByUValueIdx(i_idx, u_value, u_idx);
        }

        /**
         * Calculate Ni by u value and index. The value and index must match.
         */
        public double CalcNiByUValueIdx(int i_idx, double u_value, int u_idx)
        {
            var u = knotPoints;
            int endIdx = u.Count - 1 - degree;
            if (u_idx == endIdx)
            {
                // handle special case when u_value is tn (usually is 1).
                return i_idx + 1 == u_idx ? 1 : 0;
            }
            int i = i_idx;
            double numerator, denominator;
            if (u_idx == i) // -------------------------------------- case 1 [Ui, Ui+1)
            {
                numerator = Math.Pow(u_value - u[i], 3);
                if (numerator == 0) return 0;
                denominator = (u[i + 1] - u[i]) * (u[i + 2] - u[i]) * (u[i + 3] - u[i]);
                if (denominator == 0) return 0;
                return numerator / denominator;
            }
            else if (u_idx == i + 1) // ----------------------------- case 2 [Ui+1, Ui+2)
            {
                double res = 0.0;
                numerator = (u_value - u[i]) * (u_value - u[i]) * (u[i + 2] - u_value);
                if (numerator != 0)
                {
                    denominator = (u[i + 2] - u[i + 1]) * (u[i + 3] - u[i]) * (u[i + 2] - u[i]);
                    if (denominator != 0) res += numerator / denominator;
                }
                numerator = (u[i + 3] - u_value) * (u_value - u[i]) * (u_value - u[i + 1]);
                if (numerator != 0)
                {
                    denominator = (u[i + 2] - u[i + 1]) * (u[i + 3] - u[i + 1]) * (u[i + 3] - u[i]);
                    if (denominator != 0) res += numerator / denominator;
                }
                numerator = (u[i + 4] - u_value) * (u_value - u[i + 1]) * (u_value - u[i + 1]);
                if (numerator != 0)
                {
                    denominator = (u[i + 2] - u[i + 1]) * (u[i + 4] - u[i + 1]) * (u[i + 3] - u[i + 1]);
                    if (denominator != 0) res += numerator / denominator;
                }
                return res;
            }
            else if (u_idx == i + 2) // ----------------------------- case 3 [Ui+2, Ui+3)
            {
                double res = 0.0;
                numerator = (u_value - u[i]) * (u[i + 3] - u_value) * (u[i + 3] - u_value);
                if (numerator != 0)
                {
                    denominator = (u[i + 3] - u[i + 2]) * (u[i + 3] - u[i + 1]) * (u[i + 3] - u[i]);
                    if (denominator != 0) res += numerator / denominator;
                }
                numerator = (u[i + 4] - u_value) * (u[i + 3] - u_value) * (u_value - u[i + 1]);
                if (numerator != 0)
                {
                    denominator = (u[i + 3] - u[i + 2]) * (u[i + 4] - u[i + 1]) * (u[i + 3] - u[i + 1]);
                    if (denominator != 0) res += numerator / denominator;
                }
                numerator = (u[i + 4] - u_value) * (u[i + 4] - u_value) * (u_value - u[i + 2]);
                if (numerator != 0)
                {
                    denominator = (u[i + 3] - u[i + 2]) * (u[i + 4] - u[i + 2]) * (u[i + 4] - u[i + 1]);
                    if (denominator != 0) res += numerator / denominator;
                }
                return res;
            }
            else if (u_idx == i + 3) // ----------------------------- case 4 [Ui+3, Ui+4)
            {
                numerator = Math.Pow(u[i + 4] - u_value, 3);
                if (numerator == 0) return 0;
                denominator = (u[i + 4] - u[i + 3]) * (u[i + 4] - u[i + 2]) * (u[i + 4] - u[i + 1]);
                if (denominator == 0) return 0;
                return numerator / denominator;
            }
            else
            {
                return 0;
            }
        }

        /**
         * Calculate Ni by u index
         */
        public double CalcNiByUIdx(int i_idx, int u_idx)
        {
            var u = knotPoints;
            int i = i_idx;
            if (u_idx == i)
            {
                return 0;
            }
            else if (u_idx == i + 1)
            {
                double numerator = Math.Pow(u[i + 1] - u[i], 2);
                if (numerator == 0) return 0;
                double denominator = (u[i + 3] - u[i]) * (u[i + 2] - u[i]);
                if (denominator == 0) return 0;
                return numerator / denominator;
            }
            else if (u_idx == i + 2)
            {
                double numerator = (u[i + 2] - u[i]) * (u[i + 3] - u[i + 2]);
                double res = 0;
                if (numerator != 0)
                {
                    double denominator = (u[i + 3] - u[i + 1]) * (u[i + 3] - u[i]);
                    if (denominator != 0) res += numerator / denominator;
                }
                numerator = (u[i + 4] - u[i + 2]) * (u[i + 2] - u[i + 1]);
                if (numerator != 0)
                {
                    double denominator = (u[i + 4] - u[i + 1]) * (u[i + 3] - u[i + 1]);
                    if (denominator != 0) res += numerator / denominator;
                }
                return res;
            }
            else if (u_idx == i + 3)
            {
                double numerator = Math.Pow(u[i + 4] - u[i + 3], 2);
                if (numerator == 0) return 0;
                double denominator = (u[i + 4] - u[i + 2]) * (u[i + 4] - u[i + 1]);
                if (denominator == 0) return 0;
                return numerator / denominator;
            }
            else
            {
                throw new ArgumentException($"Unsupported argument. i_idx:{i_idx}, u_idx:{u_idx}");
            }
        }

        private double CalcNiDerivative2(int i_idx, int u_idx)
        {
            var u = knotPoints;
            int n = targetPoints.Count - 1;
            double res;
            if (i_idx == 0 && u_idx == 3)
            {
                double numerator = 6;
                double denominator = Math.Pow(u[4] - u[3], 2);
                res = numerator / denominator;
            }
            else if (i_idx == 1 && u_idx == 3)
            {
                res = -4.0 / Math.Pow(u[4] - u[3], 2);
                res += (4 * u[3] - 2 * u[5] - 2 * u[4]) / (Math.Pow(u[4] - u[3], 2) * (u[5] - u[3]));
                res += -4.0 / ((u[4] - u[3]) * (u[5] - u[3]));
            }
            else if (i_idx == 2 && u_idx == 3)
            {
                res = 6.0 / ((u[4] - u[3]) * (u[5] - u[3]));
            }
            else if (i_idx == n && u_idx == n + 3)
            {
                res = 6.0 / ((u[n + 3] - u[n + 2]) * (u[n + 3] - u[n + 1]));
            }
            else if (i_idx == n + 1 && u_idx == n + 3)
            {
                res = -4.0 / ((u[n + 3] - u[n + 2]) * (u[n + 4] - u[n + 1]));
                res += (-4 * u[n + 3] + 2 * u[n + 2] + 2 * u[n + 1]) / (Math.Pow(u[n + 3] - u[n + 2], 2) * (u[n + 3] - u[n + 1]));
                res += -4 / Math.Pow(u[n + 3] - u[n + 2], 2);
            }
            else if (i_idx == n + 2 && u_idx == n + 3)
            {
                res = 6.0 / Math.Pow(u[n + 3] - u[n + 2], 2);
            }
            else
            {
                throw new ArgumentException($"Unsupported argument. i_idx:{i_idx}; u_idx: {u_idx}");
            }
            return res;
        }
    } // class
}
