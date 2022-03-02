namespace BSpline
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTargetPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.listBoxD = new System.Windows.Forms.ListBox();
            this.labelD = new System.Windows.Forms.Label();
            this.listBoxK = new System.Windows.Forms.ListBox();
            this.labelK = new System.Windows.Forms.Label();
            this.labelC = new System.Windows.Forms.Label();
            this.listBoxC = new System.Windows.Forms.ListBox();
            this.buttonC = new System.Windows.Forms.Button();
            this.buttonDrawCurve = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 694);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1249, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1249, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveTargetPointsToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(289, 26);
            this.loadToolStripMenuItem.Text = "Load Target Points";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveTargetPointsToolStripMenuItem
            // 
            this.saveTargetPointsToolStripMenuItem.Name = "saveTargetPointsToolStripMenuItem";
            this.saveTargetPointsToolStripMenuItem.Size = new System.Drawing.Size(289, 26);
            this.saveTargetPointsToolStripMenuItem.Text = "Save Target Points";
            this.saveTargetPointsToolStripMenuItem.Click += new System.EventHandler(this.saveTargetPointsToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(289, 26);
            this.saveToolStripMenuItem.Text = "Save Knots and Control Points";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(289, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(237, 26);
            this.toolStripMenuItem1.Text = "Save Equations to File";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 647);
            this.panel1.TabIndex = 4;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // listBoxD
            // 
            this.listBoxD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxD.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listBoxD.FormattingEnabled = true;
            this.listBoxD.ItemHeight = 18;
            this.listBoxD.Location = new System.Drawing.Point(847, 51);
            this.listBoxD.Name = "listBoxD";
            this.listBoxD.Size = new System.Drawing.Size(130, 346);
            this.listBoxD.TabIndex = 6;
            // 
            // labelD
            // 
            this.labelD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD.AutoSize = true;
            this.labelD.Location = new System.Drawing.Point(848, 28);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(56, 20);
            this.labelD.TabIndex = 7;
            this.labelD.Text = "Targets";
            // 
            // listBoxK
            // 
            this.listBoxK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxK.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listBoxK.FormattingEnabled = true;
            this.listBoxK.ItemHeight = 18;
            this.listBoxK.Location = new System.Drawing.Point(983, 51);
            this.listBoxK.Name = "listBoxK";
            this.listBoxK.Size = new System.Drawing.Size(109, 346);
            this.listBoxK.TabIndex = 8;
            // 
            // labelK
            // 
            this.labelK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelK.AutoSize = true;
            this.labelK.Location = new System.Drawing.Point(977, 28);
            this.labelK.Name = "labelK";
            this.labelK.Size = new System.Drawing.Size(46, 20);
            this.labelK.TabIndex = 9;
            this.labelK.Text = "Knots";
            // 
            // labelC
            // 
            this.labelC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelC.AutoSize = true;
            this.labelC.Location = new System.Drawing.Point(1100, 28);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(64, 20);
            this.labelC.TabIndex = 12;
            this.labelC.Text = "Controls";
            // 
            // listBoxC
            // 
            this.listBoxC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxC.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listBoxC.FormattingEnabled = true;
            this.listBoxC.ItemHeight = 18;
            this.listBoxC.Location = new System.Drawing.Point(1099, 51);
            this.listBoxC.Name = "listBoxC";
            this.listBoxC.Size = new System.Drawing.Size(138, 346);
            this.listBoxC.TabIndex = 11;
            // 
            // buttonC
            // 
            this.buttonC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonC.Location = new System.Drawing.Point(848, 532);
            this.buttonC.Name = "buttonC";
            this.buttonC.Size = new System.Drawing.Size(129, 33);
            this.buttonC.TabIndex = 13;
            this.buttonC.Text = "Control Points";
            this.buttonC.UseVisualStyleBackColor = true;
            this.buttonC.Click += new System.EventHandler(this.buttonC_Click);
            // 
            // buttonDrawCurve
            // 
            this.buttonDrawCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDrawCurve.Location = new System.Drawing.Point(983, 532);
            this.buttonDrawCurve.Name = "buttonDrawCurve";
            this.buttonDrawCurve.Size = new System.Drawing.Size(129, 33);
            this.buttonDrawCurve.TabIndex = 14;
            this.buttonDrawCurve.Text = "Draw Curve";
            this.buttonDrawCurve.UseVisualStyleBackColor = true;
            this.buttonDrawCurve.Click += new System.EventHandler(this.buttonDrawCurve_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 720);
            this.Controls.Add(this.buttonDrawCurve);
            this.Controls.Add(this.buttonC);
            this.Controls.Add(this.labelC);
            this.Controls.Add(this.listBoxC);
            this.Controls.Add(this.labelK);
            this.Controls.Add(this.listBoxK);
            this.Controls.Add(this.labelD);
            this.Controls.Add(this.listBoxD);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "B-Spline for AE7453 A1 (Xu Shifeng G2104007A)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Panel panel1;
        private ToolTip toolTip1;
        private ListBox listBoxD;
        private Label labelD;
        private ListBox listBoxK;
        private Label labelK;
        private Label labelC;
        private ListBox listBoxC;
        private Button buttonC;
        private Button buttonDrawCurve;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem saveTargetPointsToolStripMenuItem;
    }
}