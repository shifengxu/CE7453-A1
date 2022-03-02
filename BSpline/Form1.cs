using System.Globalization;
using BSplineLib;
using Timer = System.Windows.Forms.Timer;

namespace BSpline
{
    public partial class Form1 : Form
    {
        private string loadTargetPointsFilePath = "C:\\";
        private string saveTargetPointsFilePath = "C:\\";
        private string saveCtrPointsFilePath = "C:\\";

        // configs, like loadTargetPointsFilePath value
        private string initCfgFilePath = ".\\config.cfg";

        private string saveEquationsFilePath = ".\\equations.txt";

        private Canvas canvas;

        private Graphics _g;

        private Timer panel1ResizeTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            _g = this.panel1.CreateGraphics();
            canvas = new Canvas(this.panel1.Width, this.panel1.Height);
            _g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            panel1ResizeTimer.Interval = 1000;
            panel1ResizeTimer.Tick += new EventHandler(panel1ResizeTimerTick);
            panel1ResizeTimer.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dock the PictureBox to the form and set its background to white.

            this.initConfig();
        }

        private void drawTargetPoints(List<CanvasPoint> points)
        {
            if (points.Count == 0)
                return;
            // draw lines first, then dots will cover the line at intersection
            Pen pen4Line = new Pen(Color.LightBlue, 1);
            _g.Clear(Color.White);
            int pHeight = canvas.panelHeight;
            int pWidth = canvas.panelWidth;

            // draw coordinate lines
            Pen pen4Coordinate = new Pen(Color.LightGray, 2);
            pen4Coordinate.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen4Coordinate.DashPattern = new float[] { 1, 3 };
            int panelOriginX = canvas.RealX2CanvasX(0.0);
            int panelOriginY = pHeight - canvas.RealY2CanvasY(0.0);
            _g.DrawLine(pen4Coordinate, panelOriginX, 0, panelOriginX, pHeight);
            _g.DrawLine(pen4Coordinate, 0, panelOriginY, pWidth, panelOriginY);

            for (int i = 1; i < points.Count; i++)
            {
                int x1 = points[i - 1].canvasX;
                int y1 = pHeight - points[i - 1].canvasY;
                int x2 = points[i].canvasX;
                int y2 = pHeight - points[i].canvasY;
                _g.DrawLine(pen4Line, x1, y1, x2, y2);
            }

            Pen pen4Point = new Pen(Color.Blue, 2);
            Brush brush = new SolidBrush(pen4Point.Color);
            foreach (CanvasPoint point in points)
            {
                int x = point.canvasX;
                int y = pHeight - point.canvasY;
                _g.DrawEllipse(pen4Point, x - 3, y - 3, 6, 6);
                _g.FillEllipse(brush, x - 3, y - 3, 6, 6);
            }
        }

        private void drawControlPoints(List<CanvasPoint> points)
        {
            if (points.Count == 0)
                return;

            Pen pen = new Pen(Color.DeepPink);
            Brush brush = new SolidBrush(pen.Color);
            int r = 3; // radius
            int r2 = r * 2;
            foreach (CanvasPoint point in points)
            {
                int x = point.canvasX;
                int y = this.canvas.panelHeight - point.canvasY;
                _g.DrawRectangle(pen, x - r, y - r, r2, r2);
                _g.FillEllipse(brush, x - r, y - r, r2, r2);
            }
        }

        /**
         * Draw the B-Spline curve.
         * The range should be [0, 1], aka [t0, tn]
         */
        private void drawCurve(int segCount = 1000)
        {
            Pen pen4Curve = new Pen(Color.Red, 2);
            int pht = canvas.panelHeight;
            var targetPoints = canvas.targetPoints;
            int lastCanvasX = targetPoints[0].canvasX;
            int lastCanvasY = targetPoints[0].canvasY;
            int currCanvasX;
            int currCanvasY;
            for (int i = 1; i < segCount; i++)
            {
                var tuple = canvas.GetCanvasXYByUValue((double)i / segCount);
                currCanvasX = tuple.Item1;
                currCanvasY = tuple.Item2;
                _g.DrawLine(pen4Curve, lastCanvasX, pht - lastCanvasY, currCanvasX, pht - currCanvasY);
                lastCanvasX = currCanvasX;
                lastCanvasY = currCanvasY;
            }
            var lt = targetPoints.Last(); // last target point
            _g.DrawLine(pen4Curve, lastCanvasX, pht - lastCanvasY, lt.canvasX, pht - lt.canvasY);
        }

        private bool initConfig()
        {
            if (File.Exists(this.initCfgFilePath) == false)
                return true;
            string[] lines = File.ReadAllLines(this.initCfgFilePath);
            int configCount = 0;
            foreach(string line in lines)
            {
                string l = line.Trim();
                if (l.Length == 0 || l.StartsWith("#"))
                    continue;
                string[] parts = l.Split(new char[] { '=' }, StringSplitOptions.TrimEntries);
                string key = parts[0];
                string val = parts[1];
                switch (key)
                {
                    case "loadTargetPointsFilePath":
                        this.loadTargetPointsFilePath = val;
                        break;
                    case "saveEquationsFilePath":
                        this.saveEquationsFilePath = val;
                        break;
                    case "saveCtrPointsFilePath":
                        this.saveCtrPointsFilePath = val;
                        break;
                    case "saveTargetPointsFilePath":
                        this.saveTargetPointsFilePath = val;
                        break;
                    default:
                        this.toolStripStatusLabel1.Text = $"Unknown key {key} from config file {this.initCfgFilePath}";
                        return false;
                }
                configCount++;
            }
            this.toolStripStatusLabel1.Text = $"Initilized {configCount} item(s) from config file {this.initCfgFilePath}";
            return true;
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _g.Dispose();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0} = {1}", "loadTargetPointsFilePath", this.loadTargetPointsFilePath);
            sb.AppendLine();
            sb.AppendFormat("{0} = {1}", "saveTargetPointsFilePath", this.saveTargetPointsFilePath);
            sb.AppendLine();
            sb.AppendFormat("{0} = {1}", "saveEquationsFilePath", this.saveEquationsFilePath);
            sb.AppendLine();
            sb.AppendFormat("{0} = {1}", "saveCtrPointsFilePath", this.saveCtrPointsFilePath);
            sb.AppendLine();
            
            File.WriteAllText(this.initCfgFilePath, sb.ToString());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ******************************************************************** load target points
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = this.loadTargetPointsFilePath;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                //Get the path of specified file
                this.loadTargetPointsFilePath = openFileDialog.FileName;
            }

            // load target points and calculate knot points
            List<CanvasPoint> tps = this.canvas.LoadTargetPoints(this.loadTargetPointsFilePath);
            List<double> knotPoints = canvas.CalcKnotPoints();
            this.canvas.RefreshPointCoordinate();

            // update status bar message.
            var msg = $"Loaded {tps.Count} point(s) from file {this.loadTargetPointsFilePath}";
            this.toolStripStatusLabel1.Text = msg;

            this.drawTargetPoints(tps);

            // listbox of target point
            this.listBoxD.Items.Clear();
            foreach (CanvasPoint pt in tps)
                this.listBoxD.Items.Add(pt.ToString());

            // listbox if knots
            this.listBoxK.Items.Clear();
            foreach (var kp in knotPoints)
                this.listBoxK.Items.Add($"{kp,4:N2}".Replace(".00", "   "));

            this.Text = this.loadTargetPointsFilePath;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            double x2 = this.canvas.CanvasX2RealX(e.X);
            double y2 = this.canvas.CanvasY2RealY(this.canvas.panelHeight - e.Y);
            string x2Str = x2.ToString("N", CultureInfo.InvariantCulture);
            string y2Str = y2.ToString("N", CultureInfo.InvariantCulture);
            this.toolStripStatusLabel1.Text = $"{x2Str} {y2Str}";

            int dr = 6; // dot radius
            int x = e.X;
            int y = this.canvas.panelHeight - e.Y;
            bool found = false;
            foreach(var p in canvas.targetPoints)
            {
                if (found) break;
                if (x - dr <= p.canvasX && p.canvasX <= x + dr &&
                    y - dr <= p.canvasY && p.canvasY <= y + dr)
                {
                    this.toolTip1.SetToolTip(this.panel1, p.ToString());
                    found = true;
                }
            }
            foreach (var p in canvas.controlPoints)
            {
                if (found) break;
                if (x - dr <= p.canvasX && p.canvasX <= x + dr &&
                    y - dr <= p.canvasY && p.canvasY <= y + dr)
                {
                    this.toolTip1.SetToolTip(this.panel1, p.ToString());
                    found = true;
                }
            }
            if (!found)
                this.toolTip1.SetToolTip(this.panel1, null);
        }

        // ******************************************************************** calculate control points
        private void buttonC_Click(object sender, EventArgs e)
        {
            // Now, let's calculate the control points (de Boor points)
            if (canvas.targetPoints.Count == 0)
            {
                MessageBox.Show("Please load target points first.");
                return;
            }
            List<CanvasPoint> ctrPoints;
            try
            {
                ctrPoints = canvas.CalcControlPoints(this.saveEquationsFilePath);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.listBoxC.Items.Clear();
            foreach(var cp in ctrPoints)
            {
                this.listBoxC.Items.Add(cp.ToString());
            }
            canvas.RefreshPointCoordinate();

            // coordiates changed. so need to clear and repaint.
            _g.Clear(Color.White);
            this.drawTargetPoints(canvas.targetPoints);
            this.drawControlPoints(canvas.controlPoints);

        }

        private void buttonDrawCurve_Click(object sender, EventArgs e)
        {
            if (canvas.controlPoints.Count == 0)
            {
                MessageBox.Show("Please calculate control points first.");
                return;
            }
            this.drawCurve(1000);
        }

        // ******************************************************************** panel1 resize
        private void panel1_Resize(object sender, EventArgs e)
        {
            _g.Clear(Color.White);
            _g.Dispose();
            _g = panel1.CreateGraphics();
            canvas.panelWidth = panel1.Width;
            canvas.panelHeight = panel1.Height;

            /* can not run the following direct. Because it may not render well.
             * Especially when make panel bigger.
             */
            //canvas.RefreshPointCoordinate();
            //this.drawTargetPoints(canvas.targetPoints);
            //this.drawControlPoints(canvas.controlPoints);

            /* Put the rendering task into a timer, and render points after 1 second.
             * The reason is, when panel resizing, the graphics may not render well.
             * And after 1 second, the resize is done. Then we render the points.
             */
            if (panel1ResizeTimer.Enabled)
            {
                panel1ResizeTimer.Enabled = false;
                panel1ResizeTimer.Stop();
            }
            panel1ResizeTimer.Enabled = true;
            panel1ResizeTimer.Start();
        }

        private void panel1ResizeTimerTick(object sender, EventArgs e)
        {
            canvas.RefreshPointCoordinate();
            this.drawTargetPoints(canvas.targetPoints);
            this.drawControlPoints(canvas.controlPoints);
            panel1ResizeTimer.Enabled = false;
            panel1ResizeTimer.Stop();
        }

        // ******************************************************************** Save Equations to File
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string msg = "To debug the procedure, we can save equations to file.";

            if (string.IsNullOrEmpty(this.saveEquationsFilePath))
            {
                msg += "\r\n\r\nCurrently, we do not save to file.";
            }
            else
            {
                msg += $"\r\n\r\nCurrently, we save to {saveEquationsFilePath}";
            }
            msg += "\r\n\r\nClick Yes to save equations to file.";
            msg += "\r\n\r\nClick No to not to save.";
            var res = MessageBox.Show(this, msg, "Save Equations to File . . .",
                MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Yes)
            {
                this.saveEquationsFilePath = ".\\equations.txt";
            }else if (res == DialogResult.No)
            {
                this.saveEquationsFilePath = String.Empty;
            }
        }

        // ******************************************************************** save knots and control points
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = this.loadTargetPointsFilePath;
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                this.saveCtrPointsFilePath = sfd.FileName;
            }
            var cps = canvas.controlPoints;
            var knots = canvas.knotPoints;
            string str1, str2;
            using(StreamWriter sw = new StreamWriter(this.saveCtrPointsFilePath)){
                sw.WriteLine(3);
                sw.WriteLine(cps.Count);
                for (int i = 0; i < knots.Count; i++)
                {
                    str1 = $"{knots[i],4:N2}";
                    if (str1.EndsWith(".00")) str1 = str1.Replace(".00", "   ");
                    sw.Write($"{str1}  ");
                }
                sw.WriteLine();
                for (int i = 0; i < cps.Count; i++)
                {
                    str1 = $"{cps[i].X,5:N2}";
                    str2 = $"{cps[i].Y,5:N2}";
                    if (str1.EndsWith(".00")) str1 = str1.Replace(".00", "   ");
                    if (str2.EndsWith(".00")) str2 = str2.Replace(".00", "   ");
                    sw.WriteLine($"{str1}  {str2}");
                }
            }
        }

        // ******************************************************************** Save Target Points
        private void saveTargetPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = this.loadTargetPointsFilePath;
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                this.saveTargetPointsFilePath = sfd.FileName;
            }
            string str1, str2;
            using (StreamWriter sw = new StreamWriter(this.saveTargetPointsFilePath))
            {
                foreach (var p in canvas.targetPoints)
                {
                    str1 = $"{p.X,5:N2}";
                    str2 = $"{p.Y,5:N2}";
                    if (str1.EndsWith(".00")) str1 = str1.Replace(".00", "   ");
                    if (str2.EndsWith(".00")) str2 = str2.Replace(".00", "   ");
                    sw.WriteLine($"{str1}  {str2}");
                }
            }
        }
    }
}