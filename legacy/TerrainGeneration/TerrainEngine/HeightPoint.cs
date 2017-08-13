using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace System.Drawing
{
	/// <summary>
	/// Provides a Point strucutre with associated height.
	/// </summary>
	public struct HeightPoint
	{
		#region Construction / Destruction

		/// <summary>
		/// Creates a new instance of HeightPoint structure.
		/// </summary>
		/// <param name="x">The horizontal position of the point.</param>
		/// <param name="y">The vertical position of the point.</param>
		/// <param name="height">Height associated with the point.</param>
		public HeightPoint(int x, int y, double height)
		{
			_x = x;
			_y = y;
			_height = height;
		}
		/// <summary>
		/// Creates a new instance of HeightPoint structure with height of 0.0.
		/// </summary>
		/// <param name="x">The horizontal position of the point.</param>
		/// <param name="y">The vertical position of the point.</param>
		public HeightPoint(int x, int y)
		{
			_x = x;
			_y = y;
			_height = 0.0;
		}

		#endregion
		#region Operators

		/// <summary>
		/// Compares two HeightPoint objects. The result specifies whether the values of the X, Y and Height properties of the two HeightPoint objects are equal.
		/// </summary>
		/// <param name="lhs">A HeightPoint to compare.</param>
		/// <param name="rhs">A HeightPoint to compare.</param>
		/// <returns>True if the X, Y and Height values of lhs and rhs are equal; otherwise, false.</returns>
		public static bool operator ==(HeightPoint lhs, HeightPoint rhs)
		{
			return
				(
					(lhs.X == rhs.X) &&
					(lhs.Y == rhs.Y) &&
					(lhs.Height == rhs.Height)
				);
		}
		/// <summary>
		/// Compares two HeightPoint objects. The result specifies whether the values of the X, Y and Height properties of the two HeightPoint objects are unequal.
		/// </summary>
		/// <param name="lhs">A HeightPoint to compare.</param>
		/// <param name="rhs">A HeightPoint to compare.</param>
		/// <returns>True if the values of either the X, Y or Height properties of lhs and rhs differ; otherwise, false.</returns>
		public static bool operator !=(HeightPoint lhs, HeightPoint rhs)
		{
			return
				(
					(lhs.X != rhs.X) ||
					(lhs.Y != rhs.Y) ||
					(lhs.Height != rhs.Height)
				);
		}
		/// <summary>
		/// Translates lhs HeightPoint by a specified rhs HeightPoint value.
		/// </summary>
		/// <param name="lhs">HeightPoint to translate.</param>
		/// <param name="rhs">HeightPoint that specifies coordinates to add to lhs.</param>
		/// <returns>The translated HeightPoint.</returns>
		public static HeightPoint operator +(HeightPoint lhs, HeightPoint rhs)
		{
			return new HeightPoint(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Height + rhs.Height);
		}
		/// <summary>
		/// Translates lhs HeightPoint by a specified rhs HeightPoint value.
		/// </summary>
		/// <param name="lhs">HeightPoint to translate.</param>
		/// <param name="rhs">HeightPoint that specifies coordinates to substract from lhs.</param>
		/// <returns>The translated HeightPoint.</returns>
		public static HeightPoint operator -(HeightPoint lhs, HeightPoint rhs)
		{
			return new HeightPoint(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Height - rhs.Height);
		}
		/// <summary>
		/// Finds a geometric center of two given HeightPoints.
		/// Identical to HeightPoint.Midpoint(lhs, rhs).
		/// </summary>
		/// <param name="lhs">First point.</param>
		/// <param name="rhs">Second point.</param>
		/// <returns>HeightPoint exactly in the middle (both positionally and by height) of lhs and rhs.</returns>
		public static HeightPoint operator %(HeightPoint lhs, HeightPoint rhs)
		{
			return Midpoint(lhs, rhs);
		}

		/// <summary>
		/// Multiplies X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">HeightPoint to transform.</param>
		/// <param name="rhs">Scalar the specifies the multiplication parameter.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator *(HeightPoint lhs, int rhs)
		{
			return new HeightPoint(lhs.X * rhs, lhs.Y * rhs, lhs.Height * rhs);
		}
		/// <summary>
		/// Multiplies X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">Scalar the specifies the multiplication parameter.</param>
		/// <param name="rhs">HeightPoint to transform.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator *(int lhs, HeightPoint rhs)
		{
			return rhs * lhs;
		}
		/// <summary>
		/// Multiplies X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">HeightPoint to transform.</param>
		/// <param name="rhs">Scalar the specifies the multiplication parameter.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator *(HeightPoint lhs, double rhs)
		{
			return new HeightPoint((int)((double)lhs.X * rhs), (int)((double)lhs.Y * rhs), lhs.Height * rhs);
		}
		/// <summary>
		/// Multiplies X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">Scalar the specifies the multiplication parameter.</param>
		/// <param name="rhs">HeightPoint to transform.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator *(double lhs, HeightPoint rhs)
		{
			return rhs * lhs;
		}

		/// <summary>
		/// Divides X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">HeightPoint to transform.</param>
		/// <param name="rhs">Scalar the specifies the division parameter.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator /(HeightPoint lhs, int rhs)
		{
			return new HeightPoint(lhs.X / rhs, lhs.Y / rhs, lhs.Height / rhs);
		}
		/// <summary>
		/// Divides X, Y and Height properties of a HeightPoint by a given scalar.
		/// </summary>
		/// <param name="lhs">HeightPoint to transform.</param>
		/// <param name="rhs">Scalar the specifies the division parameter.</param>
		/// <returns>The transformed HeightPoint.</returns>
		public static HeightPoint operator /(HeightPoint lhs, double rhs)
		{
			return new HeightPoint(lhs.X / (int)rhs, lhs.Y / (int)rhs, lhs.Height / rhs);
		}

		#endregion
		#region Methods

		/// <summary>
		/// Specifies whether this HeightPoint contains the same coordinates as the specified Object.
		/// </summary>
		/// <param name="obj">The object to test.</param>
		/// <returns>True if obj is a HeightPoint and has the same coordinates as this HeightPoint.</returns>
		public override bool Equals(object obj)
		{
			HeightPoint hp;

			if (obj.GetType().Name == "HeightPoint")
			{
				hp = (HeightPoint)obj;
				return (this == hp);
			}
			else
				return false;
		}
		/// <summary>
		/// Returns a hash code for this HeightPoint.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this HeightPoint.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/// <summary>
		/// Converts this HeightPoint to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this HeightPoint.</returns>
		public override string ToString()
		{
			System.Globalization.CultureInfo ci;

			ci = new System.Globalization.CultureInfo("en-US");
			return string.Format(ci.NumberFormat, "{0:D}, {1:D}, {2:F2}", X, Y, Height);
		}

		/// <summary>
		/// Finds a geometric center of two given HeightPoints.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns>HeightPoint exactly in the middle (both positionally and by height) of a and b.</returns>
		public static HeightPoint Midpoint(HeightPoint a, HeightPoint b)
		{
			return (a + b) / 2;
		}
		/// <summary>
		/// Finds a geometric center of three given HeightPoints.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <param name="c">Third point.</param>
		/// <returns>HeightPoint exactly in the middle (both positionally and by height) of a, b and c.</returns>
		public static HeightPoint Midpoint(HeightPoint a, HeightPoint b, HeightPoint c)
		{
			return (a + b + c) / 3;
		}
		/// <summary>
		/// Finds a geometric center of four given HeightPoints.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <param name="c">Third point.</param>
		/// <param name="d">Fourth point.</param>
		/// <returns>HeightPoint exactly in the middle (both positionally and by height) of a, b, c and d.</returns>
		public static HeightPoint Midpoint(HeightPoint a, HeightPoint b, HeightPoint c, HeightPoint d)
		{
			return (a + b + c + d) / 4;
		}
		/// <summary>
		/// Finds a geometric center of zero or more given HeightPoints.
		/// </summary>
		/// <param name="heightPoints">A HeightPoint array containing zero or more points.</param>
		/// <returns>HeightPoint exactly in the middle (both positionally and by height) of a, b, c and d.</returns>
		public static HeightPoint Midpoint(params HeightPoint[] heightPoints)
		{
			HeightPoint result;

			result = HeightPoint.Empty;
			foreach (HeightPoint hp in heightPoints)
				result += hp;
			result /= heightPoints.Length;

			return result;
		}

		#endregion
		#region Properties

		/// <summary>
		/// Gets a value indicating whether this HeightPoint is empty.
		/// </summary>
		public bool IsEmpty
		{
			get { return (this == Empty); }
		}

		/// <summary>
		/// Gets or sets the horizontal coordinate of this HeightPoint.
		/// </summary>
		public int X
		{
			get { return _x; }
			set { _x = value; }
		}
		/// <summary>
		/// Gets or sets the vertical coordinate of this HeightPoint.
		/// </summary>
		public int Y
		{
			get { return _y; }
			set { _y = value; }
		}
		/// <summary>
		/// Gets or sets the height of this HeightPoint.
		/// </summary>
		public double Height
		{
			get { return _height; }
			set { _height = value; }
		}

		#endregion
		#region Fields

		/// <summary>
		/// Represents a HeightPoint that is a null reference.
		/// </summary>
		public static readonly HeightPoint Empty = new HeightPoint(0, 0, 0.0);
		/// <summary>
		/// Represents an (-1, 0, 0.0) HeightPoint (vector of unit length to the left).
		/// </summary>
		public static readonly HeightPoint LeftVector = new HeightPoint(-1, 0, 0.0);
		/// <summary>
		/// Represents an (1, 0, 0.0) HeightPoint (vector of unit length to the right).
		/// </summary>
		public static readonly HeightPoint RightVector = new HeightPoint(1, 0, 0.0);
		/// <summary>
		/// Represents a (0, -1, 0.0) HeightPoint (vector of unit length upwards).
		/// </summary>
		public static readonly HeightPoint TopVector = new HeightPoint(0, -1, 0.0);
		/// <summary>
		/// Represents a (0, 1, 0.0) HeightPoint (vector of unit length downwards).
		/// </summary>
		public static readonly HeightPoint BottomVector = new HeightPoint(0, 1, 0.0);

		/// <summary>
		/// Stores the horizontal coordinate of this HeightPoint.
		/// </summary>
		private int _x;
		/// <summary>
		/// Sotres the vertical coordinate of this HeightPoint.
		/// </summary>
		private int _y;
		/// <summary>
		/// Stores the height of this HeightPoint.
		/// </summary>
		private double _height;

		#endregion
	}
}