namespace TerrainGeneration
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStartStop = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.tbPath = new System.Windows.Forms.TextBox();
			this.tbMetaFile = new System.Windows.Forms.TextBox();
			this.lblMetaFile = new System.Windows.Forms.Label();
			this.lblImageFile = new System.Windows.Forms.Label();
			this.lblPath = new System.Windows.Forms.Label();
			this.tbImageFile = new System.Windows.Forms.TextBox();
			this.nudTile = new System.Windows.Forms.NumericUpDown();
			this.lblTile = new System.Windows.Forms.Label();
			this.nudZoom = new System.Windows.Forms.NumericUpDown();
			this.lblZoom = new System.Windows.Forms.Label();
			this.cbEngine = new System.Windows.Forms.ComboBox();
			this.lblEngine = new System.Windows.Forms.Label();
			this.gbRender = new System.Windows.Forms.GroupBox();
			this.gbExport = new System.Windows.Forms.GroupBox();
			this.ssStatus = new System.Windows.Forms.StatusStrip();
			this.tslblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.tbTerrainString = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.nudTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudZoom)).BeginInit();
			this.gbRender.SuspendLayout();
			this.gbExport.SuspendLayout();
			this.ssStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(6, 68);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(320, 26);
			this.btnStartStop.TabIndex = 5;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(6, 94);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(320, 26);
			this.btnExport.TabIndex = 4;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// tbPath
			// 
			this.tbPath.Location = new System.Drawing.Point(68, 16);
			this.tbPath.Name = "tbPath";
			this.tbPath.Size = new System.Drawing.Size(258, 20);
			this.tbPath.TabIndex = 1;
			this.tbPath.Text = "C:\\Programi\\3dsMax8\\images\\";
			// 
			// tbMetaFile
			// 
			this.tbMetaFile.Location = new System.Drawing.Point(68, 68);
			this.tbMetaFile.Name = "tbMetaFile";
			this.tbMetaFile.Size = new System.Drawing.Size(258, 20);
			this.tbMetaFile.TabIndex = 3;
			this.tbMetaFile.Text = "Terrain.cfg";
			// 
			// lblMetaFile
			// 
			this.lblMetaFile.Location = new System.Drawing.Point(6, 68);
			this.lblMetaFile.Name = "lblMetaFile";
			this.lblMetaFile.Size = new System.Drawing.Size(56, 20);
			this.lblMetaFile.TabIndex = 0;
			this.lblMetaFile.Text = "Meta File";
			this.lblMetaFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblImageFile
			// 
			this.lblImageFile.Location = new System.Drawing.Point(6, 42);
			this.lblImageFile.Name = "lblImageFile";
			this.lblImageFile.Size = new System.Drawing.Size(56, 20);
			this.lblImageFile.TabIndex = 0;
			this.lblImageFile.Text = "Image File";
			this.lblImageFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPath
			// 
			this.lblPath.Location = new System.Drawing.Point(6, 16);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(56, 20);
			this.lblPath.TabIndex = 0;
			this.lblPath.Text = "Path";
			this.lblPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbImageFile
			// 
			this.tbImageFile.Location = new System.Drawing.Point(68, 42);
			this.tbImageFile.Name = "tbImageFile";
			this.tbImageFile.Size = new System.Drawing.Size(258, 20);
			this.tbImageFile.TabIndex = 2;
			this.tbImageFile.Text = "HeightMap.bmp";
			// 
			// nudTile
			// 
			this.nudTile.Location = new System.Drawing.Point(68, 42);
			this.nudTile.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudTile.Name = "nudTile";
			this.nudTile.Size = new System.Drawing.Size(50, 20);
			this.nudTile.TabIndex = 2;
			this.nudTile.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblTile
			// 
			this.lblTile.Location = new System.Drawing.Point(6, 42);
			this.lblTile.Name = "lblTile";
			this.lblTile.Size = new System.Drawing.Size(56, 20);
			this.lblTile.TabIndex = 0;
			this.lblTile.Text = "Tile";
			this.lblTile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudZoom
			// 
			this.nudZoom.Location = new System.Drawing.Point(68, 16);
			this.nudZoom.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudZoom.Name = "nudZoom";
			this.nudZoom.Size = new System.Drawing.Size(50, 20);
			this.nudZoom.TabIndex = 1;
			this.nudZoom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblZoom
			// 
			this.lblZoom.Location = new System.Drawing.Point(6, 16);
			this.lblZoom.Name = "lblZoom";
			this.lblZoom.Size = new System.Drawing.Size(56, 20);
			this.lblZoom.TabIndex = 0;
			this.lblZoom.Text = "Zoom";
			this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbEngine
			// 
			this.cbEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEngine.FormattingEnabled = true;
			this.cbEngine.Items.AddRange(new object[] {
            "Terrain",
            "Empty",
            "Probability Function",
            "Diamond-Square (Strict)",
            "Diamond-Square",
            "Voronoi Diagram",
            "Normalization"});
			this.cbEngine.Location = new System.Drawing.Point(170, 16);
			this.cbEngine.Name = "cbEngine";
			this.cbEngine.Size = new System.Drawing.Size(156, 21);
			this.cbEngine.TabIndex = 3;
			this.cbEngine.Tag = "EngineSelector";
			this.cbEngine.SelectedIndexChanged += new System.EventHandler(this.cbEngine_SelectedIndexChanged);
			// 
			// lblEngine
			// 
			this.lblEngine.Location = new System.Drawing.Point(124, 16);
			this.lblEngine.Name = "lblEngine";
			this.lblEngine.Size = new System.Drawing.Size(40, 20);
			this.lblEngine.TabIndex = 0;
			this.lblEngine.Text = "Engine";
			this.lblEngine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gbRender
			// 
			this.gbRender.Controls.Add(this.lblEngine);
			this.gbRender.Controls.Add(this.lblZoom);
			this.gbRender.Controls.Add(this.cbEngine);
			this.gbRender.Controls.Add(this.btnStartStop);
			this.gbRender.Controls.Add(this.nudZoom);
			this.gbRender.Controls.Add(this.nudTile);
			this.gbRender.Controls.Add(this.lblTile);
			this.gbRender.Location = new System.Drawing.Point(12, 12);
			this.gbRender.Name = "gbRender";
			this.gbRender.Size = new System.Drawing.Size(332, 100);
			this.gbRender.TabIndex = 1;
			this.gbRender.TabStop = false;
			this.gbRender.Tag = "";
			this.gbRender.Text = "Render";
			// 
			// gbExport
			// 
			this.gbExport.Controls.Add(this.tbMetaFile);
			this.gbExport.Controls.Add(this.tbPath);
			this.gbExport.Controls.Add(this.tbImageFile);
			this.gbExport.Controls.Add(this.lblPath);
			this.gbExport.Controls.Add(this.lblMetaFile);
			this.gbExport.Controls.Add(this.lblImageFile);
			this.gbExport.Controls.Add(this.btnExport);
			this.gbExport.Location = new System.Drawing.Point(12, 270);
			this.gbExport.Name = "gbExport";
			this.gbExport.Size = new System.Drawing.Size(332, 126);
			this.gbExport.TabIndex = 3;
			this.gbExport.TabStop = false;
			this.gbExport.Tag = "";
			this.gbExport.Text = "Export";
			// 
			// ssStatus
			// 
			this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblProgress,
            this.tspbProgress});
			this.ssStatus.Location = new System.Drawing.Point(0, 484);
			this.ssStatus.Name = "ssStatus";
			this.ssStatus.Size = new System.Drawing.Size(730, 26);
			this.ssStatus.SizingGrip = false;
			this.ssStatus.TabIndex = 8;
			this.ssStatus.Tag = "";
			// 
			// tslblProgress
			// 
			this.tslblProgress.AutoSize = false;
			this.tslblProgress.Name = "tslblProgress";
			this.tslblProgress.Size = new System.Drawing.Size(150, 21);
			this.tslblProgress.Text = "Progress";
			this.tslblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tspbProgress
			// 
			this.tspbProgress.AutoSize = false;
			this.tspbProgress.Name = "tspbProgress";
			this.tspbProgress.Size = new System.Drawing.Size(300, 20);
			this.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// tbTerrainString
			// 
			this.tbTerrainString.Location = new System.Drawing.Point(12, 402);
			this.tbTerrainString.Multiline = true;
			this.tbTerrainString.Name = "tbTerrainString";
			this.tbTerrainString.ReadOnly = true;
			this.tbTerrainString.Size = new System.Drawing.Size(332, 74);
			this.tbTerrainString.TabIndex = 9;
			this.tbTerrainString.Tag = "";
			this.tbTerrainString.DoubleClick += new System.EventHandler(this.tbTerrainString_DoubleClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(730, 510);
			this.Controls.Add(this.tbTerrainString);
			this.Controls.Add(this.ssStatus);
			this.Controls.Add(this.gbExport);
			this.Controls.Add(this.gbRender);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TerrainGen";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.nudTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudZoom)).EndInit();
			this.gbRender.ResumeLayout(false);
			this.gbExport.ResumeLayout(false);
			this.gbExport.PerformLayout();
			this.ssStatus.ResumeLayout(false);
			this.ssStatus.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.Label lblMetaFile;
		private System.Windows.Forms.Label lblImageFile;
		private System.Windows.Forms.Label lblPath;
		private System.Windows.Forms.TextBox tbImageFile;
		private System.Windows.Forms.TextBox tbMetaFile;
		private System.Windows.Forms.NumericUpDown nudZoom;
		private System.Windows.Forms.Label lblZoom;
		private System.Windows.Forms.NumericUpDown nudTile;
		private System.Windows.Forms.Label lblTile;
		private System.Windows.Forms.Label lblEngine;
		private System.Windows.Forms.ComboBox cbEngine;
		private System.Windows.Forms.GroupBox gbRender;
		private System.Windows.Forms.GroupBox gbExport;
		private System.Windows.Forms.StatusStrip ssStatus;
		private System.Windows.Forms.ToolStripStatusLabel tslblProgress;
		private System.Windows.Forms.ToolStripProgressBar tspbProgress;
		private System.Windows.Forms.TextBox tbTerrainString;
	}
}

