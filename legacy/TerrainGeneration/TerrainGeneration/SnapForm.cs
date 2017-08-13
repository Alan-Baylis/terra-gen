using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public partial class SnapForm : System.Windows.Forms.Form
	{
		#region Consts

		/// <summary>
		/// Snap distance, in pixels, from the border.
		/// </summary>
		public const int SnapDistance = 10;
		/// <summary>
		/// Duh.
		/// </summary>
		protected static readonly Size DefaultFormSize = new Size(200, 200);
		/// <summary>
		/// Difference is size between the form and pbPreview.
		/// </summary>
		protected static readonly Size SizeModifier = new Size(6, 27);

		/// <summary>
		/// From winuser.h, used to handle snapping.
		/// </summary>
		private const int WM_MOVING = 0x0216;
		private const int WM_WINDOWPOSCHANGING = 0x0046;
		private const int WM_GETMINMAXINFO = 0x0024;
		private const int WM_WINDOWPOSCHANGED = 0x0047;
		private const int WM_MOVE = 0x0003;
		private const int SWP_NOSIZE = 0x0001;
		private const int SWP_NOMOVE = 0x0002;
		private const int SWP_NOZORDER = 0x0004;
		private const int SWP_NOREDRAW = 0x0008;
		private const int SWP_NOACTIVATE = 0x0010;
		private const int SWP_FRAMECHANGED = 0x0020;
		private const int SWP_SHOWWINDOW = 0x0040;
		private const int SWP_HIDEWINDOW = 0x0080;
		private const int SWP_NOCOPYBITS = 0x0100;
		private const int SWP_NOOWNERZORDER = 0x0200;
		private const int SWP_NOSENDCHANGING = 0x0400;
		private const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
		private const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;

		/// <summary>
		/// Number of numerically defined SWP constants. Used to handle snapping.
		/// </summary>
		private const int FlagCount_SWP = 11;

		/// <summary>
		/// Structure to which WM_WINDOWPOSCHANGING.lParam points. Used to handle snapping
		/// </summary>
#pragma warning disable 0649 // Used because I never use most of those values. And I don't intent to explicitly set them somewhere in code just to supress the warnings.
		private struct WINDOWPOS
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public uint flags;
		}
#pragma warning restore 0649

		#endregion
		#region Enums

		/// <summary>
		/// Defines the snapping status of the Form.
		/// None - Not snapped.
		/// Min - Snapped to left or bottom border.
		/// Max - Snapped to right or top border.
		/// </summary>
		private enum SnapModes { None, Min, Max }

		#endregion
		#region Construction / Destruction

		/// <summary>
		/// Initialization.
		/// </summary>
		public SnapForm()
		{
			InitializeComponent();
			_horizontalSnap = SnapModes.None;
			_verticalSnap = SnapModes.None;
			_verticalDistance = 0;
			Location = Point.Empty;
		}
		/// <summary>
		/// Shows the form with the specified owner to the user. 
		/// </summary>
		/// <param name="owner">Any object that implements IWin32Window and represents the top-level window that will own this form.</param>
		/// <param name="image">Image to show.</param>
		new public void Show(IWin32Window owner)
		{
			// There should be a call to base.Show(owner) in here. However, Owner property is used to set the position of the control and it must be used after
			// the base.Show call (in order to have Owner initialized). That results in a form being displayed at (0,0) and then snapped to the correct location.
			// I'm guessing the only two lines of code in base.Show are initialization of Owner and Visible properties.
			Owner = (Form)Control.FromHandle(owner.Handle);
			Owner.Move += new EventHandler(Owner_Move);
			SetPosition();
			HorizontalSnap = SnapModes.Max;
			VerticalSnap = SnapModes.Max;
			VerticalDistance = 0;
			Visible = true;
		}

		#endregion
		#region Properties

		/// <summary>
		/// Gets or sets the value indicating the snap status in horizontal plane (snapped to the right or left).
		/// </summary>
		private SnapModes HorizontalSnap
		{
			get { return _horizontalSnap; }
			set { _horizontalSnap = value; }
		}
		/// <summary>
		/// Gets or sets the value indicating the snap status in vertical plane (snapped to top or bottom).
		/// </summary>
		private SnapModes VerticalSnap
		{
			get { return _verticalSnap; }
			set { _verticalSnap = value; }
		}
		/// <summary>
		/// Gets or sets the value indicating the distance between top of this form and the form it is snapped to.
		/// </summary>
		private int VerticalDistance
		{
			get { return _verticalDistance; }
			set { _verticalDistance = value; }
		}

		#endregion
		#region Methods

		protected virtual void SetPosition()
		{
			if (Location.IsEmpty)
				Location = new Point(Owner.Right, Owner.Top);
			Size = DefaultFormSize + SizeModifier;
		}

		/// <summary>
		/// Intercepts WM_WINDOWPOSCHANGING messages and modifies them to enable snapping.
		/// Usafe because it must deal with pointers.
		/// </summary>
		/// <param name="m"></param>
		unsafe protected override void WndProc(ref Message m)
		{
			WINDOWPOS* wp;

			if ((m.Msg == WM_WINDOWPOSCHANGING) && (Owner != null))
			{
				wp = (WINDOWPOS*)m.LParam.ToPointer();
				if ((wp->x != 0) && (wp->y != 0))
					VerifySnap(ref wp->x, ref wp->y, wp->cx, wp->cy);
			}
			base.WndProc(ref m);
		}
		/// <summary>
		/// Wheen the from is being moved, receives new form coordinates and modifies them to enable snapping
		/// </summary>
		/// <param name="x">Left edge of the form.</param>
		/// <param name="y">Top edge of the form.</param>
		/// <param name="w">Width of the form.</param>
		/// <param name="h">Height of the form.</param>
		private void VerifySnap(ref int x, ref int y, int w, int h)
		{
			int l, r, t, b;

			l = Owner.Bounds.Left;
			r = Owner.Bounds.Right;
			t = Owner.Bounds.Top;
			b = Owner.Bounds.Bottom;

			HorizontalSnap = SnapModes.None;
			VerticalSnap = SnapModes.None;
			if ((y >= (t - h)) && (y <= b))
			{
				if (IsSnap(x, r, SnapDistance))
				{
					x = r;
					HorizontalSnap = SnapModes.Max;
				}
				if (IsSnap(x + w, l, SnapDistance))
				{
					x = l - w;
					HorizontalSnap = SnapModes.Min;
				}
			}

			VerticalSnap = SnapModes.None;
			if ((x == r) || (x == (l - w)))
			{
				if (IsSnap(y, t, SnapDistance))
				{
					y = t;
					VerticalSnap = SnapModes.Max;
				}
				if (IsSnap(y + h, b, SnapDistance))
				{
					y = b - h;
					VerticalSnap = SnapModes.Min;
				}
			}

			VerticalDistance = y - t;
		}
		/// <summary>
		/// If the form is snapped, follows the owner form'm movements.
		/// </summary>
		private void FollowOwner()
		{
			int l, r, t, b;
			int x, y, w, h;

			if ((HorizontalSnap != SnapModes.None) || (VerticalSnap != SnapModes.None))
			{
				l = Owner.Bounds.Left;
				r = Owner.Bounds.Right;
				t = Owner.Bounds.Top;
				b = Owner.Bounds.Bottom;

				x = Location.X;
				y = Location.Y;
				w = Size.Width;
				h = Size.Height;

				if (HorizontalSnap == SnapModes.Min)
				{
					x = l - w;
					y = t + VerticalDistance;
				}
				if (HorizontalSnap == SnapModes.Max)
				{
					x = r;
					y = t + VerticalDistance;
				}
				if (VerticalSnap == SnapModes.Min)
					y = b - h;
				if (VerticalSnap == SnapModes.Max)
					y = t;

				Location = new Point(x, y);
			}
		}
		/// <summary>
		/// Determenes weather the form is in snapping range.
		/// </summary>
		/// <param name="v">Edge coordinate of this from.</param>
		/// <param name="b">Edge coordinate of the owner form.</param>
		/// <param name="s">Snapping distance.</param>
		/// <returns>True if this form is snapping rage; otherwise, false.</returns>
		private bool IsSnap(int v, int b, int s)
		{
			return ((v >= (b - s)) && (v <= (b + s)));
		}
		/// <summary>
		/// Verifies weather the specified flag is in the specified value.
		/// </summary>
		/// <param name="v">Value.</param>
		/// <param name="f">Flag.</param>
		/// <returns>True the value contains the flag; otherwise, false.</returns>
		private bool HasFlag(int v, int f)
		{
			int flagIndex;

			flagIndex = (int)Math.Log(f, 2);
			return (((v >> flagIndex) % 2) == 1);
		}
		/// <summary>
		/// Creates a report cotaining all data about the position of the form.
		/// </summary>
		/// <param name="wp">Pointer to WINDOWPOS structure.</param>
		/// <returns>A report cotaining all data about the position of the form.</returns>
		unsafe private string ReportWP(WINDOWPOS* wp)
		{
			string report, s;

			report = string.Format("L:({0:D4},{1:D4})  |  S:({2:D4},{3:D4})  |  ", wp->x, wp->y, wp->cx, wp->cy);
			s = Convert.ToString(wp->flags, 2);
			for (int n = s.Length - 1; n >= 0; n--)
				if (s[n] == '1')
					switch ((int)Math.Pow(2, s.Length - n - 1))
					{
						case 0x0001:
							report += "SWP_NOSIZE, ";
							break;
						case 0x0002:
							report += "SWP_NOMOVE, ";
							break;
						case 0x0004:
							report += "SWP_NOZORDER, ";
							break;
						case 0x0008:
							report += "SWP_NOREDRAW, ";
							break;
						case 0x0010:
							report += "SWP_NOACTIVATE, ";
							break;
						case 0x0020:
							report += "SWP_FRAMECHANGED, ";
							break;
						case 0x0040:
							report += "SWP_SHOWWINDOW, ";
							break;
						case 0x0080:
							report += "SWP_HIDEWINDOW, ";
							break;
						case 0x0100:
							report += "SWP_NOCOPYBITS, ";
							break;
						case 0x0200:
							report += "SWP_NOOWNERZORDER, ";
							break;
						case 0x0400:
							report += "SWP_NOSENDCHANGING, ";
							break;
					}

			if (report.EndsWith(", "))
				report = report.Remove(report.Length - 2, 2);
			if (report.EndsWith(" | "))
				report = report.Remove(report.Length - 3, 3);

			if (wp->flags > 0)
				report += "  |  " + wp->flags.ToString() + "; " + Convert.ToString(wp->flags, 2);

			return report;
		}
		/// <summary>
		/// Owner.Move event handler.
		/// </summary>
		/// <param name="sender">...</param>
		/// <param name="e">...</param>
		private void Owner_Move(object sender, EventArgs e)
		{
			FollowOwner();
		}

		#endregion
		#region Fields

		/// <summary>
		/// Stores the value indicating the snap status in horizontal plane (snapped to the right or left).
		/// </summary>
		private SnapModes _horizontalSnap;
		/// <summary>
		/// Stores the value indicating the snap status in vertical plane (snapped to top or bottom).
		/// </summary>
		private SnapModes _verticalSnap;
		/// <summary>
		/// Stores the value indicating the distance between top of this form and the form it is snapped to.
		/// </summary>
		private int _verticalDistance;

		#endregion
	}
}