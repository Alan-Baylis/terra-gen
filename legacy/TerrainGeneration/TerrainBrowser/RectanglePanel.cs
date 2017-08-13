using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public class RectanglePanel : System.Windows.Forms.Panel
	{
		#region Constants

		private const int nudW = 50;
		private const int nudH = 20;

		private const float xp = 0.5F;
		private const float yp = 0.5F;
		private const float wp = 0.5F;
		private const float hp = 0.5F;
		private const float l = 20.0F;
		private const float sw = nudW;
		private const float sh = nudH;
		private const float r = 8.0F;
		private const float aw = 4.0F;
		private const float ah = 8.0F;
		private const float t = 2.0F;
		private const float o = 0.5F;

		private static readonly GraphicsPath ArrowHead = new GraphicsPath(new PointF[4] { new PointF(0, 0), new PointF(-aw, -ah), new PointF(0, 0), new PointF(aw, -ah) }, new byte[4] { (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });
		private static readonly CustomLineCap Cap = new CustomLineCap(null, ArrowHead);

		private const int MIN = int.MinValue;
		private const int MAX = int.MaxValue;

		#endregion
		#region Enums

		private enum StaticParts { WidthMiddleHeight, RightLeft, TopBottom }

		#endregion
		#region Events

		public event EventHandler ValueChanged;
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, e);
		}

		#endregion
		#region Construction / Destruction

		public RectanglePanel()
			: base()
		{
			SetupControls();
		}

		private void SetupControls()
		{
			CreateControls();
			NameControls();
			AddControls();
			InitializeControls();
			PositionNUDs();
		}
		private void CreateControls()
		{
			nudR_Left = new NumericUpDown();
			nudR_Right = new NumericUpDown();
			nudR_Top = new NumericUpDown();
			nudR_Bottom = new NumericUpDown();
			nudR_Width = new NumericUpDown();
			nudR_Height = new NumericUpDown();
			nudR_MiddleX = new NumericUpDown();
			nudR_MiddleY = new NumericUpDown();
			nudSP = new NumericUpDown();
			lblSP = new Label();
		}
		private void NameControls()
		{
			nudR_Left.Name = "nudR_Left";
			nudR_Right.Name = "nudR_Right";
			nudR_Top.Name = "nudR_Top";
			nudR_Bottom.Name = "nudR_Bottom";
			nudR_Width.Name = "nudR_Width";
			nudR_Height.Name = "nudR_Height";
			nudR_MiddleX.Name = "nudR_MiddleX";
			nudR_MiddleY.Name = "nudR_MiddleY";
			nudSP.Name = "nudSP";
			lblSP.Name = "lblSP";
		}
		private void InitializeControls()
		{
			EventHandler wmh, rl, tb;

			wmh = new EventHandler(nudWMH_ValueChanged);
			rl = new EventHandler(nudRL_ValueChanged);
			tb = new EventHandler(nudTB_ValueChanged);

			nudR_Left.Value = 10;
			nudR_Right.Value = 20;
			nudR_Top.Value = 10;
			nudR_Bottom.Value = 20;
			nudR_Width.Value = 10;
			nudR_Height.Value = 10;
			nudR_MiddleX.Value = 15;
			nudR_MiddleY.Value = 15;

			nudSP.Minimum = 1;
			nudSP.Maximum = 20;
			nudSP.Value = 8;
			nudSP.Size = new Size(nudW, nudH);
			nudSP.ReadOnly = true;

			lblSP.AutoSize = false;
			lblSP.Size = new Size(25, 20);
			lblSP.TextAlign = ContentAlignment.MiddleLeft;
			lblSP.Text = "SP";

			nudR_Left.ValueChanged += nudRL_ValueChanged;
			nudR_Right.ValueChanged += nudRL_ValueChanged;
			nudR_Top.ValueChanged += nudTB_ValueChanged;
			nudR_Bottom.ValueChanged += nudTB_ValueChanged;
			nudR_Width.ValueChanged += nudWMH_ValueChanged;
			nudR_Height.ValueChanged += nudWMH_ValueChanged;
			nudR_MiddleX.ValueChanged += nudWMH_ValueChanged;
			nudR_MiddleY.ValueChanged += nudWMH_ValueChanged;
			nudSP.ValueChanged += new EventHandler(nudSP_ValueChanged);

			NumericUpDown nud;
			foreach (Control c in Controls)
				if (c.Name.StartsWith("nudR_"))
				{
					nud = (NumericUpDown)c;
					nud.Maximum = MAX;
					nud.Minimum = MIN;
					nud.Size = new Size(nudW, nudH);
				}

			nudR_Left.TabIndex = 1;
			nudR_Right.TabIndex = 8;
			nudR_Top.TabIndex = 3;
			nudR_Bottom.TabIndex = 7;
			nudR_Width.TabIndex = 6;
			nudR_Height.TabIndex = 4;
			nudR_MiddleX.TabIndex = 2;
			nudR_MiddleY.TabIndex = 5;
			nudSP.TabIndex = 9;
			lblSP.TabIndex = 0;
		}
		private void AddControls()
		{
			Controls.Add(nudR_Left);
			Controls.Add(nudR_Right);
			Controls.Add(nudR_Top);
			Controls.Add(nudR_Bottom);
			Controls.Add(nudR_Width);
			Controls.Add(nudR_Height);
			Controls.Add(nudR_MiddleX);
			Controls.Add(nudR_MiddleY);
			Controls.Add(nudSP);
			Controls.Add(lblSP);
		}

		private void PositionNUDs()
		{
			int x, y, w, h;
			int left, right, top, bottom;

			x = (int)(ClientRectangle.Width * xp);
			y = (int)(ClientRectangle.Height * yp);
			w = (int)(ClientRectangle.Width * wp);
			h = (int)(ClientRectangle.Height * hp);

			left = x - w / 2;
			right = x + w / 2;
			top = y - h / 2;
			bottom = y + h / 2;

			nudR_Left.Location = new Point(left - nudW / 2, top - (int)l - nudH);
			nudR_MiddleX.Location = new Point(x - nudW / 2, top - (int)l - nudH);
			nudR_Top.Location = new Point(left - (int)l - nudW, top - nudH / 2);
			nudR_Height.Location = new Point(left - nudW / 2, y - nudH / 2);
			nudR_MiddleY.Location = new Point(right + (int)l + 1, y - nudH / 2);
			nudR_Width.Location = new Point(x - nudW / 2, bottom - nudH / 2);
			nudR_Bottom.Location = new Point(right + (int)l + 1, bottom - nudH / 2);
			nudR_Right.Location = new Point(right - nudW / 2, bottom + (int)l + 1);

			lblSP.Location = new Point(3, ClientRectangle.Bottom - nudH - 6);
			nudSP.Location = new Point(lblSP.Left + lblSP.Width + 6, ClientRectangle.Bottom - nudH - 6);
		}

		#endregion
		#region Methods

		private void ApplySP()
		{
			int SP, size;

			SP = (int)nudSP.Value;
			size = (int)Math.Pow(2, SP) + 1;

			nudR_Right.Value = nudR_Left.Value + size;
			nudR_Bottom.Value = nudR_Top.Value + size;
		}
		private bool IsSP(int size)
		{
			return (Math.Log(size - 1, 2) == (int)Math.Log(size - 1, 2));
		}
		private void SetSP()
		{
			int w, h;

			w = (int)nudR_Width.Value;
			h = (int)nudR_Height.Value;

			if ((w == h) && IsSP(w) && IsSP(h))
			{
				nudSP.ReadOnly = false;
				nudSP.Value = (int)Math.Log(w - 1, 2);
			}
			else
			{
				nudSP.ReadOnly = true;
			}
		}
		private void SetNUDs(StaticParts staticPart)
		{
			int r, l, t, b, mx, my, w, h;

			if (!Locked)
			{
				Lock();
				r = RR;
				l = RL;
				t = RT;
				b = RB;
				mx = RX;
				my = RY;
				w = RW;
				h = RH;

				switch (staticPart)
				{
					case StaticParts.WidthMiddleHeight:
						RR = mx + w / 2;
						RL = mx - w / 2;
						RT = my - h / 2;
						RB = my + h / 2;
						break;
					case StaticParts.RightLeft:
						RX = (l + r) / 2;
						RW = r - l;
						break;
					case StaticParts.TopBottom:
						RY = (t + b) / 2;
						RH = b - t;
						break;
				}

				SetSP();
				Unlock();
				OnValueChanged(EventArgs.Empty);
			}
		}
		private void Lock()
		{
			Locked = true;
		}
		private void Unlock()
		{
			Locked = false;
		}

		void nudWMH_ValueChanged(object sender, EventArgs e)
		{
			SetNUDs(StaticParts.WidthMiddleHeight);
		}
		void nudRL_ValueChanged(object sender, EventArgs e)
		{
			SetNUDs(StaticParts.RightLeft);
		}
		void nudTB_ValueChanged(object sender, EventArgs e)
		{
			SetNUDs(StaticParts.TopBottom);
		}
		void nudSP_ValueChanged(object sender, EventArgs e)
		{
			ApplySP();
		}
		protected override void OnResize(EventArgs eventargs)
		{
			Refresh();
			base.OnResize(eventargs);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g;
			Brush b;
			Pen p;
			float x, y, w, h;
			float left, right, top, bottom;

			g = e.Graphics;
			b = Brushes.Black;
			p = new Pen(b, t);

			x = g.VisibleClipBounds.Width * xp;
			y = g.VisibleClipBounds.Height * yp;
			w = g.VisibleClipBounds.Width * wp;
			h = g.VisibleClipBounds.Height * hp;

			left = x - w / 2;
			right = x + w / 2;
			top = y - h / 2;
			bottom = y + h / 2;

			g.DrawRectangle(p, left, top, w, h);					// Square
			g.FillEllipse(b, left - r / 2, top - r / 2, r, r);		// Upper left circle
			g.FillEllipse(b, x - r / 2, y - r / 2, r, r);			// Middle circle
			g.FillEllipse(b, right - r / 2, bottom - r / 2, r, r);	// Bottom right circle

			b = new SolidBrush(Color.FromArgb((int)(255.0F * o), Color.Black));
			p = new Pen(b);
			p.CustomEndCap = Cap;
			g.DrawLine(p, left, top, left, top - l);			// Upper left arrow, upwards
			g.DrawLine(p, left, top, left - l, top);			// Upper left arrow, leftwards
			g.DrawLine(p, x, y, x, top - l);					// Middle arrow, upwards
			g.DrawLine(p, x, y, right + l, y);					// Middle arrow, rightwards
			g.DrawLine(p, right, bottom, right, bottom + l);	// Botom right arrow, downwards
			g.DrawLine(p, right, bottom, right + l, bottom);	// Botom right arrow, rightwards
			g.DrawLine(p, left, y - sh / 2, left, y - sh / 2 - l);	// Height arrow, upwards
			g.DrawLine(p, left, y + sh / 2, left, y + sh / 2 + l);	// Height arrow, downwards
			g.DrawLine(p, x - sw / 2, bottom, x - sw / 2 - l, bottom);	// Width arrow, leftwards
			g.DrawLine(p, x + sw / 2, bottom, x + sw / 2 + l, bottom);	// Width arrow, rightwards

			PositionNUDs();
			base.OnPaint(e);
		}

		#endregion
		#region Properties

		public bool Locked
		{
			get { return _locked; }
			set { _locked = value; }
		}
		public Rectangle R
		{
			get { return new Rectangle(RL, RT, RW, RH); }
			set
			{
				Lock();
				RL = value.Left;
				RR = value.Right;
				RT = value.Top;
				RB = value.Bottom;
				RW = value.Width;
				RH = value.Height;
				RX = (value.Left + value.Right) / 2;
				RY = (value.Top + value.Bottom) / 2;
				Unlock();
				OnValueChanged(EventArgs.Empty);
			}
		}
		public int RL
		{
			get { return (int)nudR_Left.Value; }
			set { nudR_Left.Value = value; }
		}
		public int RR
		{
			get { return (int)nudR_Right.Value; }
			set { nudR_Right.Value = value; }
		}
		public int RT
		{
			get { return (int)nudR_Top.Value; }
			set { nudR_Top.Value = value; }
		}
		public int RB
		{
			get { return (int)nudR_Bottom.Value; }
			set { nudR_Bottom.Value = value; }
		}
		public int RW
		{
			get { return (int)nudR_Width.Value; }
			set { nudR_Width.Value = value; }
		}
		public int RH
		{
			get { return (int)nudR_Height.Value; }
			set { nudR_Height.Value = value; }
		}
		public int RX
		{
			get { return (int)nudR_MiddleX.Value; }
			set { nudR_MiddleX.Value = value; }
		}
		public int RY
		{
			get { return (int)nudR_MiddleY.Value; }
			set { nudR_MiddleY.Value = value; }
		}

		#endregion
		#region Fields

		private NumericUpDown nudR_Left;
		private NumericUpDown nudR_Right;
		private NumericUpDown nudR_Top;
		private NumericUpDown nudR_Bottom;
		private NumericUpDown nudR_Width;
		private NumericUpDown nudR_Height;
		private NumericUpDown nudR_MiddleX;
		private NumericUpDown nudR_MiddleY;
		private NumericUpDown nudSP;
		private Label lblSP;
		private bool _locked;

		#endregion
	}
}