using System;
using System.Drawing;
using System.Windows.Forms;

namespace TerrainGeneration
{
	class RectangleControl : System.Windows.Forms.UserControl
	{
		#region Construction / Destruction

		public RectangleControl()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();

			CreateControls();
			SetControlNames();
			SetControlPositions();
			InitializeControls();
			AddControls();

			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Name = "RectangleControl";
			Size = new System.Drawing.Size(302, 243);

			this.ResumeLayout(false);

		}
		private void CreateControls()
		{
			btnAdd = new Button();
			btnRemove = new Button();
			btnUp = new Button();
			btnDown = new Button();
		}
		private void SetControlNames()
		{
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
			btnAdd.Location = new Point(3, 217);
			btnAdd.Size = new Size(75, 23);
			btnRemove.Location = new Point(62, 217);
			btnRemove.Size = new Size(75, 23);
			btnUp.Location = new Point(143, 217);
			btnUp.Size = new Size(75, 23);
			btnDown.Location = new Point(224, 217);
			btnDown.Size = new Size(75, 23);
		}
		private void InitializeControls()
		{
			btnAdd.TabIndex = 2;
			btnAdd.UseVisualStyleBackColor = true;
			btnRemove.TabIndex = 3;
			btnRemove.UseVisualStyleBackColor = true;
			btnUp.TabIndex = 4;
			btnUp.UseVisualStyleBackColor = true;
			btnDown.TabIndex = 5;
			btnDown.UseVisualStyleBackColor = true;
		}
		private void AddControls()
		{
			Controls.Add(btnDown);
			Controls.Add(btnUp);
			Controls.Add(btnRemove);
			Controls.Add(btnAdd);
		}

		#endregion
		#region Methods

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		}

		#endregion
		#region Properties

		private Button btnAdd;
		private Button btnRemove;
		private Button btnUp;
		private Button btnDown;

		#endregion
	}
}
