using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Reflection;
using TerrainEngine;

namespace TerrainGeneration
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		#region Consts

		private static readonly Point CGBLocation = new Point(12, 118);
		private static readonly Size FormSize = new Size(736, 541);
		private const int ProgressReportTimeout = 100;
		private const string CGBTag = "CGB";
		private const string DataFile = "data.dat";

		#endregion
		#region Enums, Structs

		public struct RenderData
		{
			#region Construction / Destruction

			public RenderData(string xml, int zoom, int tile)
			{
				_xml = xml;
				_zoom = zoom;
				_tile = tile;
			}
			public RenderData(string xml)
			{
				_xml = xml;
				_zoom = 1;
				_tile = 1;
			}

			#endregion
			#region Properties

			public string XML
			{
				get { return _xml; }
			}
			public int Zoom
			{
				get { return _zoom; }
			}
			public int Tile
			{
				get { return _tile; }
			}

			#endregion
			#region Fields

			private string _xml;
			private int _zoom;
			private int _tile;

			#endregion
		}
		[Serializable]
		private struct Settings
		{
			public Settings(int zoom, int tile, int engineIndex, string exportPath, string exportImageFile, string exportMetaFile, string terrainString)
			{
				Zoom = zoom;
				Tile = tile;
				EngineIndex = engineIndex;
				ExportPath = exportPath;
				ExportImageFile = exportImageFile;
				ExportMetaFile = exportMetaFile;
				TerrainString = terrainString;
			}

			public static readonly Settings Default = new Settings(1, 1, 0, @"C:\", @"HeightMap.bmp", @"Terrain.cfg", "T(S(257,257))");
			public int Zoom;
			public int Tile;
			public int EngineIndex;
			public string ExportPath;
			public string ExportImageFile;
			public string ExportMetaFile;
			public string TerrainString;
		}

		#endregion
		#region Construction / Destruction

		public MainForm()
		{
			InitializeComponent();
			InitUI();
		}

		private void InitUI()
		{
			Size = FormSize;

			frmPreview = new PreviewForm();
			frmPreview.Show(this);

			PBW = new ProgressBackgroundWorker(ProgressReportTimeout);
			PBW.DoWork += new DoWorkEventHandler(PBW_DoWork);
			PBW.ProgressChanged += new ProgressChangedEventHandler(PBW_ProgressChanged);
			PBW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PBW_RunWorkerCompleted);

			AddNUDHandlers(this);
			LoadSettings();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveSettings(GetSettings(), DataFile);
		}

		private void LoadSettings()
		{
			Settings settings;

			settings = Settings.Default;
			if (File.Exists(DataFile))
			{
				try { settings = LoadSettings(DataFile); }
				catch (Exception ex) { MessageBox.Show(this, "An error occured while loading settigs!\r\nUsing default settings.\r\n\r\nError details:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
			}
			else
				MessageBox.Show(this, "Data file not found!\r\nUsing default settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			ApplySettings(settings);
		}
		private static Settings LoadSettings(string path)
		{
			Settings result;
			Stream stream;
			IFormatter formatter;

			formatter = new BinaryFormatter();
			stream = File.OpenRead(path);
			result = (Settings)formatter.Deserialize(stream);
			stream.Close();

			return result;
		}
		private static void SaveSettings(Settings data, string path)
		{
			Stream stream;
			IFormatter formatter;

			formatter = new BinaryFormatter();
			stream = File.Create(path);
			formatter.Serialize(stream, data);
			stream.Close();
		}
		private void ApplySettings(Settings settings)
		{
			nudZoom.Value = settings.Zoom;
			nudTile.Value = settings.Tile;
			cbEngine.SelectedIndex = settings.EngineIndex;
			tbPath.Text = settings.ExportPath;
			tbImageFile.Text = settings.ExportImageFile;
			tbMetaFile.Text = settings.ExportMetaFile;
			tbTerrainString.Text = settings.TerrainString;
		}
		private Settings GetSettings()
		{
			Settings settings;

			settings = new Settings();
			settings.Zoom = (int)nudZoom.Value;
			settings.Tile = (int)nudTile.Value;
			settings.EngineIndex = cbEngine.SelectedIndex;
			settings.ExportPath = tbPath.Text;
			settings.ExportImageFile = tbImageFile.Text;
			settings.ExportMetaFile = tbMetaFile.Text;
			settings.TerrainString = tbTerrainString.Text;

			return settings;
		}

		private void AddNUDHandlers(Control parent)
		{
			NumericUpDown nud;
			if (parent.GetType().Name == "NumericUpDown")
			{
				nud = (NumericUpDown)parent;
				nud.Enter += new EventHandler(NUDEnterHandler);
				nud.Click += new EventHandler(NUDEnterHandler);
			}

			foreach (Control child in parent.Controls)
				AddNUDHandlers(child);
		}

		#endregion
		#region Methods

		#region Render Control

		private void StartStopRender()
		{
			if (btnStartStop.Text == "Start")
				StartRender();
			else
				StopRender();
		}
		private void StartRender()
		{
			//ITerrainDescriptor[] tds; // !!!
			//RenderData rd;

			//tds = new ITerrainDescriptor[tlvCommands.Items.Count];
			//for (int n = 0; n < tlvCommands.Items.Count; n++)
			//    tds[n] = tlvCommands.Items[n].TD;

			//rd = new RenderData(tds, (int)nudZoom.Value, (int)nudTile.Value);
			//RenderStarted(rd);
		}
		private void StopRender()
		{
			PBW.CancelAsync();
		}

		private void RenderStarted(RenderData rd)
		{
			btnStartStop.Text = "Stop";
			btnExport.Enabled = false;
			PBW.RunWorkerAsync(rd);
		}
		private void RenderEnded(Image i)
		{
			btnStartStop.Text = "Start";
			btnExport.Enabled = true;
			tslblProgress.Text = "Progress";
			tspbProgress.Value = 0;

			if (i != null)
				frmPreview.Show(this, i);
		}

		#endregion
		#region Rendering

		private Image RenderMethod(RenderData rd, ProgressBackgroundWorker bw)
		{
			// ITerrainDescriptor td;
			Terrain main, t;
			Image i;

			i = null;
			//main = new Terrain(rd.TDs[0].Rectangle.Size); // !!!
			//for (int n = 1; n < rd.TDs.Length; n++)
			//{
			//    td = rd.TDs[n];
			//    t = GenerateTerrain(td) * td.Weight;
			//    main.Add(t, td.Rectangle.Location);
			//}
			//CreateImage(ref i, main, rd.Zoom, rd.Tile);

			return i;
		}

		private Terrain GenerateTerrain(string xml)
		{
			Terrain t;

			t = null;
			//switch (td.ShortNameString) // !!!
			//{
			//    case TerrainEmpty.ShortName:
			//        t = GenerateTerrain_Empty((TerrainEmpty)td);
			//        break;
			//    case TerrainProbabilityFunction.ShortName:
			//        t = GenerateTerrain_ProbabilityFunction((TerrainProbabilityFunction)td);
			//        break;
			//    case TerrainDiamondSquare_Strict.ShortName:
			//        t = GenerateTerrain_DiamondSquare_Strict((TerrainDiamondSquare_Strict)td);
			//        break;
			//    case TerrainDiamondSquare.ShortName:
			//        t = GenerateTerrain_DiamondSquare((TerrainDiamondSquare)td);
			//        break;
			//}

			return t;
		}
		//private Terrain GenerateTerrain_Empty(TerrainEmpty td)
		//{
		//    Terrain t;
		//    t = new Terrain(td.Rectangle.Size);
		//    Terrain.Creation.Empty(t);

		//    return t;
		//}
		//private Terrain GenerateTerrain_ProbabilityFunction(TerrainProbabilityFunction td)
		//{
		//    Terrain t;
		//    t = new Terrain(td.Rectangle.Size);
		//    Terrain.Creation.ProbabilityFunction(t, td.P);

		//    return t;
		//}
		//private Terrain GenerateTerrain_DiamondSquare_Strict(TerrainDiamondSquare_Strict td)
		//{
		//    Terrain t;
		//    t = new Terrain(td.Rectangle.Size);
		//    Terrain.Creation.DiamondSquare_Strict(t, td.H);

		//    return t;
		//}
		//private Terrain GenerateTerrain_DiamondSquare(TerrainDiamondSquare td)
		//{
		//    Terrain t;
		//    t = new Terrain(td.Rectangle.Size);
		//    Terrain.Creation.DiamondSquare(t, td.H);

		//    return t;

		//}
		//private void GenerateTerrain_VoronoiDiagram(Terrain t)
		//{
		//    //int w, h, fc;
		//    //double[] coefficients;
		//    //double def;
		//    //Point[] features;

		//    //w = (int)nudVD_SizeX.Value;
		//    //h = (int)nudVD_SizeY.Value;
		//    //fc = (int)nudVD_FeaturesCount.Value;
		//    //def = (double)nudVD_DefaultCoefficients.Value;
		//    //coefficients = Utils.GetCoefficients(tbVD_Coefficients.Text, def, fc);
		//    //features = Utils.GetFeatures(w, h, fc);

		//    //t.Resize(w, h);
		//    //Terrain.VoronoiDiagram(t, features, coefficients);
		//}

		private void CreateImage(ref Image i, Terrain t, int zoom, int tile)
		{
			i = t.CreateImage(zoom, tile);
		}

		#endregion
		#region Render Event Handlers

		private void PBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			RenderData rd;
			Image i;

			rd = (RenderData)e.Argument;
			e.Result = null;
			i = RenderMethod(rd, PBW);
			e.Result = i;
		}
		private void PBW_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			if (e.UserState == null)
			{
				tspbProgress.Value = e.ProgressPercentage;
			}
			else
			{
				tslblProgress.Text = (string)e.UserState;
				tspbProgress.Value = 0;
			}
		}
		private void PBW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			RenderEnded((Image)e.Result);
		}

		#endregion
		#region Form Event Handlers

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			// StartStopRender();
			// SaveCDC(GetCDC(), DataFile);
			Test();
		}
		private void btnExport_Click(object sender, EventArgs e)
		{
			string imagePath, metaPath;
			StreamWriter sw;

			imagePath = tbPath.Text + tbImageFile.Text;
			metaPath = tbPath.Text + tbMetaFile.Text;

			if (frmPreview.Image != null)
			{
				frmPreview.Image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Bmp);
				sw = File.CreateText(metaPath);
				sw.WriteLine(frmPreview.Image.Size.Width.ToString());
				sw.WriteLine(frmPreview.Image.Size.Height.ToString());
				sw.Close();
			}
		}
		private void cbEngine_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void tbTerrainString_DoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show(this, "NOT YET IMPLEMENTED");
		}
		void NUDEnterHandler(object sender, EventArgs e)
		{
			NumericUpDown nud;

			nud = (NumericUpDown)sender;
			nud.Select(0, nud.Text.Length);
		}

		#endregion

		#endregion
		#region Properties

		private ProgressBackgroundWorker PBW
		{
			get { return _pbw; }
			set { _pbw = value; }
		}


		#endregion
		#region Fields

		private PreviewForm frmPreview;
		private ProgressBackgroundWorker _pbw;

		#endregion

		#region Dynamic UI

		private void Test()
		{
			//Terrain t;
			//Type type;
			//double P;
			//System.Reflection.MethodInfo mi, mig;

			//P = -2.0;
			//t = new Terrain(200, 200);
			//type = typeof(Terrain);
			//mi = type.GetMethod("ProbabilityFunction");
			//// mig = mi.MakeGenericMethod(new Type[2] { typeof(Terrain), typeof(double) });
			//mi.Invoke(null, new object[2] { t, P });

			//frmPreview.Show(this, t.CreateImage());

			CreateUI();
		}

		private MethodInfo[] GetMethodsInfo(Type t)
		{
			MethodInfo[] msi;
			MethodInfo[] result;
			int n;

			msi = t.GetMethods();
			result = new MethodInfo[msi.Length - 4];
			n = 0;
			foreach (MethodInfo mi in msi)
				if ((mi.Name != "GetType") && (mi.Name != "ToString") && (mi.Name != "Equals") && (mi.Name != "GetHashCode"))
				{
					result[n] = mi;
					n++;
				}

			
			return result;
		}

		private static readonly Size CGBSize = new Size(332, 146);
		private const int NUDWidth = 50;
		private const int LBLWidth = 150;
		private const int ItemHeight = 20;
		private const int VPadding = 16;
		private const int VSpacing = 26;
		private const int HPadding = 6;
		private const int HSpacing = 6;
		private void CreateUI()
		{
			MethodInfo[] msi;

			msi = GetMethodsInfo(typeof(Terrain.Creation));
			foreach (MethodInfo mi in msi)
			    CreateUIElement(mi);
		}
		private void CreateUIElement(MethodInfo mi)
		{
			int slot;
			GroupBox gb;

			slot = 1;
			gb = CreateUIElement_GB(mi.Name);
			foreach (ParameterInfo pi in mi.GetParameters())
			{
				switch (pi.ParameterType.Name)
				{
					case ("Int32"):
						CreateUIElement_SlotInt(gb, slot, pi.Name, pi.GetCustomAttributes(true)[0]);
						slot++;
						break;
					case ("Double"):
						CreateUIElement_SlotDouble(gb, slot, pi.Name, pi.GetCustomAttributes(true)[0]);
						slot++;
						break;
				}
			}

			CreateUIElement_SlotDouble(gb, 0, "W", new AttributeDouble("Weight", 1.0, 0.0, 1.0, 2, 1));
			CreateUIElement_Active(gb);
			Controls.Add(gb);
		}

		private void CreateUIElement_SlotInt(GroupBox parent, int slot, string name, object o)
		{
			AttributeInt a;
			Label lbl;
			NumericUpDown nud;

			a = (AttributeInt)o;
			lbl = CreateUIElement_LBL(name, slot, a);
			nud = CreateUIElement_NUD(name, slot, a);
			parent.Controls.Add(lbl);
			parent.Controls.Add(nud);
		}
		private void CreateUIElement_SlotDouble(GroupBox parent, int slot, string name, object o)
		{
			AttributeDouble a;
			Label lbl;
			NumericUpDown nud;

			a = (AttributeDouble)o;
			lbl = CreateUIElement_LBL(name, slot, a);
			nud = CreateUIElement_NUD(name, slot, a);
			parent.Controls.Add(lbl);
			parent.Controls.Add(nud);
		}
		private void CreateUIElement_Active(GroupBox parent)
		{

			CheckBox cb;

			cb = CreateUIElement_CB("Active", 4);
			parent.Controls.Add(cb);

		}		

		private GroupBox CreateUIElement_GB(string name)
		{
			GroupBox gb;

			gb = new GroupBox();
			gb.Name = "gb" + name;
			gb.Text = name;
			gb.Tag = "CGB";
			gb.Location = CGBLocation;
			gb.Size = CGBSize;

			return gb;
		}
		private Label CreateUIElement_LBL(string name, int slot, AttributeBase a)
		{
			Label lbl;

			lbl = new Label();
			lbl.AutoSize = false;
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			lbl.Name = "lbl" + name;
			lbl.Text = a.Description;
			lbl.Location = new Point(HPadding, VPadding + slot * VSpacing);
			lbl.Size = new Size(LBLWidth, ItemHeight);

			return lbl;
		}
		private NumericUpDown CreateUIElement_NUD(string name, int slot, AttributeDouble a)
		{
			NumericUpDown nud;

			nud = new NumericUpDown();
			nud.Name = "lbl" + name;
			nud.Minimum = (decimal)a.Min;
			nud.Maximum = (decimal)a.Max;
			nud.Value = (decimal)a.DefaultValue;
			nud.DecimalPlaces = a.Decimals;
			nud.Increment = (decimal)a.Increment;
			nud.Location = new Point(HPadding + LBLWidth + HSpacing, VPadding + slot * VSpacing);
			nud.Size = new Size(NUDWidth, ItemHeight);

			return nud;
		}
		private CheckBox CreateUIElement_CB(string name, int slot)
		{
			CheckBox cb;

			cb = new CheckBox();
			cb.Checked = true;
			cb.AutoSize = false;
			cb.Name = "lbl" + name;
			cb.Text = "Active";
			cb.Location = new Point(HPadding, VPadding + slot * VSpacing);
			cb.Size = new Size(CGBSize.Width - 2 * HPadding, ItemHeight);

			return cb;
		}

		#endregion
	}
}

/*
 * 

<?xml version="1.0" encoding="utf-8" ?>
<terrain width="10" height="10">
</terrain>

 * BASIC TYPES
 *   double[]
 *     double[d1, d2, d3, ...]
 *   Point
 *     P(int x, int y)
 *   Size
 *     S(int width, int height)
 *   Point[]
 *     P[P1, P2, P3, ...]
 *   Rectangle
 *     R(int l, int t, int w, int h)
 * 
 * TERRAIN
 *   Terrain
 *     T(Size size)
 *     R(Rectangle R)
 * 
 * TERRAIN GENERATION
 *   Empty
 *     E(bool active, Rectangle R, double weight)
 *   Probability Function
 *     PF(bool active, Rectangle R, double weight, double P)
 *   Diamond-Square (Strict)
 *     DSS(bool active, Rectangle R, double weight, double H)
 *   Diamond-Square
 *     DS(bool active, Rectangle R, double weight, double H)
 *   Voronoi Diagrams
 *     VD(bool active, Rectangle R, double weight, int RPC, double DC, Point[] P, double[] C)
 * 
 * TERRAIN MODIFIERS
 *   Normalization
 *     N(bool active, double lowerBound, double upperBound)
 *     N(bool active)
 * 
 * RPC - random point count
 * DC - default coefficient
*/
