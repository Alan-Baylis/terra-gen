using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace System.Windows.Forms
{
	public class TerrainBrowser : System.Windows.Forms.UserControl
	{
		#region Consts

		private const int VPadding = 6;
		private const int VSpacing = 6;
		private const int HPadding = 6;
		private const int HSpacing = 6;
		private const int UpDownWidth = 45;
		private const int BTNHeight = 23;
		private const int RPNLHeight = 200;

		#endregion
		#region Construction / Destruction

		public TerrainBrowser()
		{
			Init();
		}

		private void Init()
		{
			this.SuspendLayout();

			CreateControls();
			SetControlNames();
			SetControlPositions();
			InitializeControls();
			InitializeEventHandlers();
			AddControls();

			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Name = "RectangleControl";
			Size = new System.Drawing.Size(300, 300);

			this.ResumeLayout(false);

		}

		private void CreateControls()
		{
			lvItems = new ListView();
			rpnlRectangle = new RectanglePanel();
			btnAdd = new Button();
			btnRemove = new Button();
			btnUp = new Button();
			btnDown = new Button();
		}
		private void SetControlNames()
		{
			lvItems.Name = "lvItems";
			rpnlRectangle.Name = "rpnlRectangle";
			btnAdd.Name = "btnAdd";
			btnAdd.Text = "Add";
			btnRemove.Name = "btnRemove";
			btnRemove.Text = "Remove";
			btnUp.Name = "btnUp";
			btnUp.Text = "Up";
			btnDown.Name = "btnDown";
			btnDown.Text = "Down";
		}
		private void SetControlPositions()
		{
			int btnTop, rpnlTop, w;

			btnTop = H - VPadding - BTNHeight;
			rpnlTop = btnTop - VSpacing - RPNLHeight;
			w = (W - 2 * HPadding - 2 * UpDownWidth - 3 * HSpacing) / 2;

			lvItems.Location = new Point(HPadding, VPadding);
			lvItems.Size = new Size(W - 2 * HPadding, rpnlTop - VSpacing - VPadding);
			rpnlRectangle.Location = new Point(HPadding, rpnlTop);
			rpnlRectangle.Size = new Size(W - 2 * HPadding, RPNLHeight);
			btnAdd.Location = new Point(HPadding, btnTop);
			btnAdd.Size = new Size(w, BTNHeight);
			btnRemove.Location = new Point(HPadding + w + HSpacing, btnTop);
			btnRemove.Size = new Size(w, BTNHeight);
			btnUp.Location = new Point(W - HPadding - 2 * UpDownWidth - HSpacing, btnTop);
			btnUp.Size = new Size(UpDownWidth, BTNHeight);
			btnDown.Location = new Point(W - HPadding - UpDownWidth, btnTop);
			btnDown.Size = new Size(UpDownWidth, BTNHeight);
		}
		private void InitializeControls()
		{
			lvItems.TabIndex = 1;
			rpnlRectangle.TabIndex = 2;
			btnAdd.TabIndex = 3;
			btnAdd.UseVisualStyleBackColor = true;
			btnRemove.TabIndex = 4;
			btnRemove.UseVisualStyleBackColor = true;
			btnUp.TabIndex = 5;
			btnUp.UseVisualStyleBackColor = true;
			btnDown.TabIndex = 6;
			btnDown.UseVisualStyleBackColor = true;
		}
		private void InitializeEventHandlers()
		{
			btnAdd.Click += new EventHandler(btnAdd_Click);
			btnRemove.Click += new EventHandler(btnRemove_Click);
			btnUp.Click += new EventHandler(btnUp_Click);
			btnDown.Click += new EventHandler(btnDown_Click);
		}
		private void AddControls()
		{
			Controls.Add(lvItems);
			Controls.Add(rpnlRectangle);
			Controls.Add(btnDown);
			Controls.Add(btnUp);
			Controls.Add(btnRemove);
			Controls.Add(btnAdd);
		}

		#endregion
		#region Properties

		private int W
		{
			get { return ClientRectangle.Width; }
		}
		private int H
		{
			get { return ClientRectangle.Height; }
		}
		public XmlDocument XML
		{
			get { return _xml; }
			set { LoadXML(value); }
		}
		public string XMLString
		{
			get { return XML.OuterXml; }
			set { LoadXML(value); }
		}

		#endregion
		#region Methods

		public void LoadXML(string xmlString)
		{
			_xml.LoadXml(xmlString);
		}
		public void LoadXML(XmlDocument xml)
		{
			_xml = xml;
		}

		private void RefreshItems()
		{
		}

		private void AddItem()
		{
			//tlvCommands.Items.Add(""); // !!! new TerrainEmpty(true, rpnlRectangle.R, 1));
			//UpdateTerrainString();
		}
		private void RemoveItem()
		{
			//int index;

			//index = SelectedIndex;
			//if (index > 0)
			//{
			//    tlvCommands.Items.RemoveAt(index);
			//    if (index < tlvCommands.Items.Count)
			//        SelectedIndex = index;
			//    else
			//        SelectedIndex = tlvCommands.Items.Count - 1;

			//    UpdateTerrainString();
			//}
		}
		private void MoveItemUp()
		{
			//int index;
			//TerrainListViewItem tlviSelected, tlviUpper;

			//index = SelectedIndex;
			//if (index > 1)
			//{
			//    tlviSelected = tlvCommands.Items[index];
			//    tlviUpper = tlvCommands.Items[index - 1];

			//    tlvCommands.Items.RemoveAt(index);
			//    tlvCommands.Items.RemoveAt(index - 1);

			//    tlvCommands.Items.Insert(index - 1, tlviSelected);
			//    tlvCommands.Items.Insert(index, tlviUpper);

			//    UpdateTerrainString();
			//}
		}
		private void MoveItemDown()
		{
			//int index;
			//TerrainListViewItem tlviSelected, tlviLower;

			//index = SelectedIndex;
			//if ((index > 0) && (index < (tlvCommands.Items.Count - 1)))
			//{
			//    tlviSelected = tlvCommands.Items[index];
			//    tlviLower = tlvCommands.Items[index + 1];

			//    tlvCommands.Items.RemoveAt(index + 1);
			//    tlvCommands.Items.RemoveAt(index);

			//    tlvCommands.Items.Insert(index, tlviLower);
			//    tlvCommands.Items.Insert(index + 1, tlviSelected);

			//    UpdateTerrainString();
			//}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			SetControlPositions();
		}

		#region Event Handlers

		void btnAdd_Click(object sender, EventArgs e)
		{
		}
		void btnRemove_Click(object sender, EventArgs e)
		{
		}
		void btnUp_Click(object sender, EventArgs e)
		{
		}
		void btnDown_Click(object sender, EventArgs e)
		{
		}

		#endregion

		#endregion
		#region Fields

		private XmlDocument _xml;
		private ListView lvItems;
		private RectanglePanel rpnlRectangle;
		private Button btnAdd;
		private Button btnRemove;
		private Button btnUp;
		private Button btnDown;

		#endregion

		//private void tlvCommands_ItemChecked(object sender, ItemCheckedEventArgs e)
		//{
		//}
		//private void tlvCommands_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//}
	}
}