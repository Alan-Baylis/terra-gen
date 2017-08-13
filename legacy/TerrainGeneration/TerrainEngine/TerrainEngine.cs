using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace TerrainEngine
{
	/// <summary>
	/// Represents terrain as a Height Map Array (two-dimensional array of doubles).
	/// </summary>
	public class Terrain
	{
		#region Consts

		/// <summary>
		/// Specifies the default size of newly-created Terrain.
		/// </summary>
		private static readonly Size DefaultSize = new Size(1, 1);
		/// <summary>
		/// Specifies the default height of every point in newly-created Terrain.
		/// </summary>
		private static readonly int DefaultHeight = 0;

		#endregion
		#region Classes

		private class FastBitmap
		{
			public FastBitmap(int width, int height, PixelFormat format)
			{
				_b = new Bitmap(width, height, format);
				_bd = null;
			}

			public void Lock()
			{
				BD = B.LockBits(new Rectangle(0, 0, B.Width, B.Height), ImageLockMode.WriteOnly, B.PixelFormat);
			}
			public void Unlock()
			{
				B.UnlockBits(BD);
			}
			unsafe public void SetPixel(int x, int y, int r, int g, int b)
			{
				int pw;
				byte* bPtr;

				pw = 3;
				bPtr = (byte*)BD.Scan0 + (y * BD.Stride) + (x * pw);
				*bPtr = (byte)b;
				bPtr++;
				*bPtr = (byte)g;
				bPtr++;
				*bPtr = (byte)r;
			}
			public void SetPixel(int x, int y, Color c)
			{
				SetPixel(x, y, c.R, c.B, c.B);
			}

			public Bitmap B
			{
				get { return _b; }
				set { _b = value; }
			}
			private BitmapData BD
			{
				get { return _bd; }
				set { _bd = value; }
			}

			private Bitmap _b;
			private BitmapData _bd;
		}
		private class FeatureDistance : IComparable
		{
			public Point Location;
			public double Distance;

			public int CompareTo(object obj)
			{
				FeatureDistance fd;
				int result;

				result = 0;
				fd = (FeatureDistance)obj;
				if (fd.Distance < Distance)
					result = 1;
				if (fd.Distance > Distance)
					result = -1;

				return result;
			}
		}

		#endregion
		#region Operators

		/// <summary>
		/// Adds two Terrain objects of equal sizes by adding heights of their individual points.
		/// </summary>
		/// <param name="lhs">First Terrain.</param>
		/// <param name="rhs">Second Terrain.</param>
		/// <returns>Terrain of size equal to lhs and rhs whose points' heights are sum of heights of corresponding points in lhs and rhs.</returns>
		public static Terrain operator +(Terrain lhs, Terrain rhs)
		{
			Terrain result;
			int w, h;

			VerifyNotNull(lhs, "lhs");
			VerifyNotNull(rhs, "rhs");
			if (lhs.Size != rhs.Size)
				throw new ArgumentException("Terrain dimensions do not match.", "lhs and rhs");

			w = lhs.Size.Width;
			h = lhs.Size.Height;
			result = new Terrain(w, h);
			for (int nx = 0; nx < w; nx++)
				for (int ny = 0; ny < h; ny++)
					result[nx, ny] = lhs[nx, ny] + rhs[nx, ny];

			return result;
		}
		/// <summary>
		/// Multiplies heights of each point in the Terrain by a given scalar.
		/// </summary>
		/// <param name="lhs">Terrain to transform.</param>
		/// <param name="rhs">Scalar the specifies the multiplication parameter.</param>
		/// <returns>Transformed Terrain.</returns>
		public static Terrain operator *(Terrain lhs, double rhs)
		{
			Terrain t;
			int w, h;

			VerifyNotNull(lhs, "lhs");
			VerifyValidDouble(rhs, "rhs");

			w = lhs.Size.Width;
			h = lhs.Size.Height;
			t = new Terrain(w, h);
			for (int nx = 0; nx < w; nx++)
				for (int ny = 0; ny < h; ny++)
					t[nx, ny] = lhs[nx, ny] * rhs;

			return t;
		}
		/// <summary>
		/// Multiplies heights of each point in the Terrain by a given scalar.
		/// </summary>
		/// <param name="lhs">Scalar the specifies the multiplication parameter.</param>
		/// <param name="rhs">Terrain to transform.</param>
		/// <returns>Transformed Terrain.</returns>
		public static Terrain operator *(double lhs, Terrain rhs)
		{
			VerifyValidDouble(lhs, "lhs");
			VerifyNotNull(rhs, "rhs");

			return rhs * lhs;
		}

		#endregion
		#region Construction / Destruction

		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		/// <param name="size">Terrain size.</param>
		/// <param name="defaultHeight">Height that will be applied to each item.</param>
		public Terrain(Size size, double defaultHeight)
		{
			VerifyPositiveSize(size, "size");
			VerifyValidDouble(defaultHeight, "defaultHeight");

			Initialize(size.Width, size.Height, defaultHeight);
		}
		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		/// <param name="size">Terrain size.</param>
		public Terrain(Size size)
		{
			VerifyPositiveSize(size, "size");

			Initialize(size.Width, size.Height, DefaultHeight);
		}
		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		/// <param name="width">Terrain width.</param>
		/// <param name="height">Terrain height.</param>
		/// <param name="defaultHeight">Height that will be applied to each item.</param>
		public Terrain(int width, int height, double defaultHeight)
		{
			VerifyPositiveInt(width, "width");
			VerifyPositiveInt(height, "height");
			VerifyValidDouble(defaultHeight, "defaultHeight");

			Initialize(width, height, defaultHeight);
		}
		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		/// <param name="width">Terrain width.</param>
		/// <param name="height">Terrain height.</param>
		public Terrain(int width, int height)
		{
			VerifyPositiveInt(width, "width");
			VerifyPositiveInt(height, "height");

			Initialize(width, height, DefaultHeight);
		}
		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		public Terrain()
		{
			Initialize(DefaultSize.Width, DefaultSize.Height, DefaultHeight);
		}

		/// <summary>
		/// Initializes a new instance of Terrain class.
		/// </summary>
		/// <param name="size">Terrain size.</param>
		/// <param name="defaultHeight">Height that will be applied to each item.</param>
		private void Initialize(int width, int height, double defaultHeight)
		{
			VerifyPositiveInt(width, "width");
			VerifyPositiveInt(height, "height");
			VerifyValidDouble(defaultHeight, "defaultHeight");

			_pbw = null;
			InitializeSize(width, height, defaultHeight);
		}
		/// <summary>
		/// Initializes the Height Map Array.
		/// </summary>
		/// <param name="width">Array width.</param>
		/// <param name="height">Array height.</param>
		/// <param name="defaultHeight">Height that will be applied to each item.</param>
		private void InitializeSize(int width, int height, double defaultHeight)
		{
			VerifyPositiveInt(width, "width");
			VerifyPositiveInt(height, "height");
			VerifyValidDouble(defaultHeight, "defaultHeight");

			#region REPORT INIT
			if (PBW != null)
			{
				PBW.Report("Initializing terrain", width * height);
				if (PBW.CancellationPending)
					return;
			}
			#endregion
			_size = new Size(width, height);
			_min = defaultHeight;
			_max = defaultHeight;
			_heightMap = new double[width][];
			for (int nx = 0; nx < width; nx++)
			{
				_heightMap[nx] = new double[height];
				for (int ny = 0; ny < height; ny++)
				{
					#region REPORT
					if (PBW != null)
					{
						PBW.Report();
						if (PBW.CancellationPending)
							return;
					}
					#endregion
					_heightMap[nx][ny] = defaultHeight;
				}
			}
		}

		#endregion
		#region Methods

		/// <summary>
		/// Resizes the Terrain. This action resets all heights to their default values.
		/// </summary>
		/// <param name="newSize">New size.</param>
		public void Resize(Size newSize)
		{
			VerifyPositiveSize(newSize, "newSize");

			Resize(newSize.Width, newSize.Height);
		}
		/// <summary>
		/// Resizes the Terrain. This action resets all heights to their default values.
		/// </summary>
		/// <param name="newWidth">New width.</param>
		/// <param name="newHeight">New height.</param>
		public void Resize(int newWidth, int newHeight)
		{
			VerifyPositiveInt(newWidth, "newWidth");
			VerifyPositiveInt(newHeight, "newHeight");

			InitializeSize(newWidth, newHeight, 0);
		}
		/// <summary>
		/// Adds another Terrain object to this instance.
		/// </summary>
		/// <param name="t">Terrain data to add.</param>
		/// <param name="p">Upper left corner.</param>
		public void Add(Terrain t, Point p)
		{
			int mainX, mainY;

			for (int nx = 0; nx < t.Size.Width; nx++)
				for (int ny = 0; ny < t.Size.Height; ny++)
				{
					mainX = nx + p.X;
					mainY = ny + p.Y;

					this[mainX, mainY] += t[nx, ny];
				}
		}

		#region Terrain Creation

		public static class Creation
		{
			/// <summary>
			/// Terrain creation function template.
			/// </summary>
			/// <param name="t">Terrain to use.</param>
			public static void Empty(Terrain t)
			{
				int w, h;

				VerifyNotNull(t, "t");

				w = t.Size.Width;
				h = t.Size.Height;

				#region REPORT INIT
				if (t.PBW != null)
				{
					t.PBW.Report("Empty Terrain", w * h);
					if (t.PBW.CancellationPending)
						return;
				}
				#endregion
				for (int nx = 0; nx < t.Size.Width; nx++)
					for (int ny = 0; ny < t.Size.Height; ny++)
					{
						#region REPORT
						if (t.PBW != null)
						{
							t.PBW.Report();
							if (t.PBW.CancellationPending)
								return;
						}
						#endregion
						t[nx, ny] = 0;
					}
			}
			/// <summary>
			/// Generates a terrain with a mountain or hole in the middle, defined by the probability function (e^(-x^2*sigma)).
			/// </summary>
			/// <param name="t">Terrain to use.</param>
			/// <param name="s">Center of Terrain has a height of 1. Borders have a height of P / 100.</param>
			public static void ProbabilityFunction(Terrain t, [AttributeDouble("Cutoff Parameter", 10.0, -10.0, 100.0, 0, 1.0)]double P)
			{
				int w, h;
				double sx, sy;
				double cx, cy;
				double fx, fy;

				VerifyNotNull(t, "t");
				VerifyValidDouble(P, "P");
				if (P < 0)
					P = Math.Pow(10, P);

				w = t.Size.Width;
				h = t.Size.Height;
				sx = -Math.Log(P / 100) / Math.Pow(w, 2);
				sy = -Math.Log(P / 100) / Math.Pow(h, 2);
				cx = w / 2;
				cy = h / 2;

				#region REPORT INIT
				if (t.PBW != null)
				{
					t.PBW.Report("Probability Function", w * h);
					if (t.PBW.CancellationPending)
						return;
				}
				#endregion
				for (int nx = 0; nx < w; nx++)
					for (int ny = 0; ny < h; ny++)
					{
						#region REPORT
						if (t.PBW != null)
						{
							t.PBW.Report();
							if (t.PBW.CancellationPending)
								return;
						}
						#endregion
						fx = Math.Exp(-Math.Pow(nx - cx, 2) * sx);
						fy = Math.Exp(-Math.Pow(ny - cy, 2) * sy);
						t[nx, ny] = fx * fy;
					}
			}
			/// <summary>
			/// Strict, iterative implementation of the Diamond-Square terrain generation algorithm.
			/// Terrain must be a square with edges in the form of 2^I + 1, where I is the number of required iteraions.
			/// </summary>
			/// <param name="t">Terrain to use.</param>
			/// <param name="H">Jag parameter.</param>
			public static void DiamondSquare_Strict(Terrain t, [AttributeDouble("Jag Parameter", 1.0, 0.0, 10.0, 2, 0.1)]double H)
			{
				// A  e  B     e:AB f:AC g:BD h:CD
				// f  M  g     M:AD
				// C  h  D

				VerifyNotNull(t, "t");
				VerifyValidDouble(H, "H");
				if (t.Size.Width != t.Size.Height)
					throw new ArgumentException("Terrain height and width must be equal.", "t");
				if (Math.Log(t.Size.Width - 1, 2) != (int)Math.Log(t.Size.Width - 1, 2))
					throw new ArgumentException("Terrain size must be in the form of 2^I + 1.", "t");

				HeightPoint A, B, C, D;
				HeightPoint M, e, f, g, h;
				int size;
				int SP, I, SC, SS, Sx, Sy;
				double R, r;
				Random rg;

				// Get terrain size, initialize RNG and randomize the four terrain corners.
				size = t.Size.Width;
				rg = new Random();
				r = 0; // GetRandom(rg, 1);
				t[0, 0] = r;
				t[size - 1, 0] = r;
				t[0, size - 1] = r;
				t[size - 1, size - 1] = r;

				#region REPORT INIT
				if (t.PBW != null)
				{
					t.PBW.Report("Diamond Square (Strict)", (int)((2.0 / 3.0) * (double)(size * size - 2 * size)));
					if (t.PBW.CancellationPending)
						return;
				}
				#endregion
				SP = (int)Math.Log(size - 1, 2);
				for (I = 0; I < SP; I++)
				{
					SS = (int)Math.Pow(2, SP - I);
					SC = (int)Math.Pow(2, I);
					R = Math.Pow(2, (-H * I));

					// Diamond step
					for (Sx = 0; Sx < SC; Sx++)
						for (Sy = 0; Sy < SC; Sy++)
						{
							#region REPORT
							if (t.PBW != null)
							{
								t.PBW.Report();
								if (t.PBW.CancellationPending)
									return;
							}
							#endregion
							// Find the four corners and thier heights.
							A = new HeightPoint(SS * Sx, SS * Sy);
							B = new HeightPoint(SS * (Sx + 1), SS * Sy);
							C = new HeightPoint(SS * Sx, SS * (Sy + 1));
							D = new HeightPoint(SS * (Sx + 1), SS * (Sy + 1));
							A.Height = t[A.X, A.Y];
							B.Height = t[B.X, B.Y];
							C.Height = t[C.X, C.Y];
							D.Height = t[D.X, D.Y];

							// Find the center and calculate its height.
							M = A % D;
							r = GetRandom(rg, R);
							M.Height = (A.Height + B.Height + C.Height + D.Height) / 4 + r;

							// Store the results.
							t[M.X, M.Y] = M.Height;
						}

					// Square step
					for (Sx = 0; Sx < SC; Sx++)
						for (Sy = 0; Sy < SC; Sy++)
						{
							#region REPORT
							if (t.PBW != null)
							{
								t.PBW.Report();
								if (t.PBW.CancellationPending)
									return;
							}
							#endregion
							// Find the four corners and their heights.
							A = new HeightPoint(SS * Sx, SS * Sy);
							B = new HeightPoint(SS * (Sx + 1), SS * Sy);
							C = new HeightPoint(SS * Sx, SS * (Sy + 1));
							D = new HeightPoint(SS * (Sx + 1), SS * (Sy + 1));

							// For each midpoint, find its coordinates, calculate height and store the result.
							e = A % B;
							e.Height = GetMidpointHeight(t, e, SS);
							t[e.X, e.Y] = e.Height;

							f = A % C;
							f.Height = GetMidpointHeight(t, f, SS);
							t[f.X, f.Y] = f.Height;

							if (Sx == (SC - 1))
							{
								g = B % D;
								g.Height = GetMidpointHeight(t, g, SS);
								t[g.X, g.Y] = g.Height;
							}

							if (Sy == (SC - 1))
							{
								h = C % D;
								h.Height = GetMidpointHeight(t, h, SS);
								t[h.X, h.Y] = h.Height;
							}
						}
				}
				t.Normalize();
			}
			/// <summary>
			/// Iterative implementation of the Diamond-Square terrain generation algorithm.
			/// </summary>
			/// <param name="t">Terrain to use.</param>
			/// <param name="H">Jag parameter.</param>
			public static void DiamondSquare(Terrain t, [AttributeDouble("Jag Parameter", 1.0, 0.0, 10.0, 2, 0.1)]double H)
			{
				int max, SP, size;
				int rx, ry;
				Terrain tmp;

				max = Math.Max(t.Size.Width, t.Size.Height);
				SP = (int)Math.Ceiling(Math.Log(max - 1, 2));
				size = (int)Math.Pow(2, SP) + 1;

				tmp = new Terrain(size, size);
				tmp.PBW = t.PBW;
				DiamondSquare_Strict(tmp, H);

				rx = size / 2 - t.Size.Width / 2;
				ry = size / 2 - t.Size.Height / 2;

				for (int nx = 0; nx < t.Size.Width; nx++)
					for (int ny = 0; ny < t.Size.Height; ny++)
						t[nx, ny] = tmp[nx + rx, ny + ry];
			}
			#region VORONOI
			/*
			/// <summary>
			/// 
			/// </summary>
			/// <param name="t"></param>
			/// <param name="features"></param>
			/// <param name="coefficients"></param>
			public static void VoronoiDiagram(Terrain t, Point[] features, double[] coefficients)
			{
				int w, h;
				double val;
				FeatureDistance[] fd;

				VerifyNotNull(t, "t");
				VerifyNotNull(features, "features");
				VerifyNotNull(coefficients, "coefficients");
				if (coefficients.Length < features.Length)
					throw new ArgumentException("Coefficients length must be less than or equal to features length.", "coefficients");

				w = t.Size.Width;
				h = t.Size.Height;

				#region REPORT INIT
				if (t.PBW != null)
				{
					t.PBW.Report("Voronoi Diagram", w * h);
					if (t.PBW.CancellationPending)
						return;
				}
				#endregion
				for (int nx = 0; nx < t.Size.Width; nx++)
					for (int ny = 0; ny < t.Size.Height; ny++)
					{
						#region REPORT
						if (t.PBW != null)
						{
							t.PBW.Report();
							if (t.PBW.CancellationPending)
								return;
						}
						#endregion

						val = 0;
						fd = SortFeatures(features, nx, ny);
						for (int nf = 0; nf < fd.Length; nf++)
							val += coefficients[nf] * fd[nf].Distance;
						t[nx, ny] = val;
					}
			}
			/**/
			#endregion
			public static Terrain Test(Rectangle r, [AttributeInt("Integer arg", 10, -20, 20, 5)]int i, [AttributeDouble("Double arg", -2.5, -10, 10, 3, 0.01)]double P)
			{
				return null;
			}
		}

		#endregion
		#region Terrain Modifiers

		/// <summary>
		/// Normalizes the Terrain between lowerBound and upperBound (performs a linear interpolation of heights between points (Min, lowerBound) and (Max, upperBound).
		/// </summary>
		/// <param name="lowerBound">New minimal height.</param>
		/// <param name="upperBound">New maximal height.</param>
		public void Normalize(double lowerBound, double upperBound)
		{
			VerifyValidDouble(lowerBound, "lowerBound");
			VerifyValidDouble(upperBound, "upperBound");
			if (lowerBound > upperBound)
				throw new ArgumentException("lowerBound must be less than or equal to upperBound", "lowerBound and upperBound");

			for (int nx = 0; nx < Size.Width; nx++)
				for (int ny = 0; ny < Size.Height; ny++)
					HeightMap[nx][ny] = LinearInterpolation(HeightMap[nx][ny], Min, lowerBound, Max, upperBound);
			_min = lowerBound;
			_max = upperBound;
		}
		/// <summary>
		/// Normalizes the Terrain between 0 and 1 (performs a linear interpolation of heights between points (Min, 0) and (Max, 1).
		/// </summary>
		public void Normalize()
		{
			Normalize(0, 1);
		}

		#endregion
		#region Image Creation

		/// <summary>
		/// Calculates the height map defined by current terrain configuration. Entire height interval is linearly interpolated into [0,255] interval.
		/// </summary>
		/// <param name="zoom">Zoom factor.</param>
		/// <param name="tile">Number of times to tile the height map.</param>
		/// <returns>Grayscale Image (Bitmap) representing the height map of current terrain configuration.</returns>
		public Image CreateImage(int zoom, int tile)
		{
			FastBitmap b;
			int w, h;
			int x, y;

			VerifyPositiveInt(zoom, "zoom");
			VerifyPositiveInt(tile, "tile");

			w = Size.Width;
			h = Size.Height;

			#region REPORT INIT
			if (PBW != null)
			{
				PBW.Report("Creating Image", w * tile * zoom * h * tile * zoom);
				if (PBW.CancellationPending)
					return null;
			}
			#endregion
			// Create and populate the image (each array item corresponds to one image pixel).
			// GetColor performs the interpolation.
			b = new FastBitmap(w * tile * zoom, h * tile * zoom, PixelFormat.Format24bppRgb);
			b.Lock();
			for (int nx = 0; nx < w; nx++)
				for (int ny = 0; ny < h; ny++)
					for (int tx = 0; tx < tile; tx++)
						for (int ty = 0; ty < tile; ty++)
							for (int zx = 0; zx < zoom; zx++)
								for (int zy = 0; zy < zoom; zy++)
								{
									#region REPORT
									if (PBW != null)
									{
										PBW.Report();
										if (PBW.CancellationPending)
											return null;
									}
									#endregion
									x = zx + nx * zoom + tx * zoom * w;
									y = zy + ny * zoom + ty * zoom * h;
									b.SetPixel(x, y, GetColor(HeightMap[nx][ny], Min, Max));
								}
			b.Unlock();
			return (Image)b.B; ;
		}
		/// <summary>
		/// Calculates the height map defined by current terrain configuration. Entire height interval is linearly interpolated into [0,255] interval.
		/// </summary>
		/// <returns>Grayscale Image (Bitmap) representing the height map of current terrain configuration.</returns>
		public Image CreateImage()
		{
			return CreateImage(1, 1);
		}

		#endregion
		#region Utilities

		/// <summary>
		/// Transforms specified Height Value into a Color Value using linear interpolation.
		/// Height Interval is [min,max] (double). Color Interval is [0,255] (integer).
		/// </summary>
		/// <param name="height">Height to transform (must be in Height Interval).</param>
		/// <param name="min">Lower endpoint of Height Interval.</param>
		/// <param name="max">Upper endpoint of Height Interval.</param>
		/// <returns>Obtained Color Value (always in Color Interval).</returns>
		private static Color GetColor(double height, double min, double max)
		{
			int c;

			// No error checking here bacuse it is called from an inner loop.

			c = (int)LinearInterpolation(height, min, 0, max, 255);
			return Color.FromArgb(c, c, c);
		}
		/// <summary>
		/// Sotrs feature points in relation to their distance from the specified coordinates.
		/// </summary>
		/// <param name="features">List of feature points.</param>
		/// <param name="x">Horizontal coordinate of the point against which to perform the sorting.</param>
		/// <param name="y">Vertical coordinate of the point against which to perform the sorting.</param>
		/// <returns>List of FeatureDistance elements sorted according to their distance from x and y.</returns>
		private static FeatureDistance[] SortFeatures(Point[] features, int x, int y)
		{
			FeatureDistance[] result;
			int fx, fy;

			// No error checking here bacuse it is called from an inner loop.

			result = new FeatureDistance[features.Length];
			for (int n = 0; n < features.Length; n++)
			{
				fx = features[n].X;
				fy = features[n].Y;

				result[n] = new FeatureDistance();
				result[n].Location = features[n];
				result[n].Distance = Math.Sqrt(Math.Pow(fx - x, 2) + Math.Pow(fy - y, 2));
			}

			Array.Sort(result);
			return result;
		}
		/// <summary>
		/// Performs a linear interpolation of the specified value.
		/// </summary>
		/// <param name="x">Value to transform.</param>
		/// <param name="x0">Horizontal coordinate of the starting endpoint.</param>
		/// <param name="y0">Vertical coordinate of the starting endpoint.</param>
		/// <param name="x1">Horizontal coordinate of the ending endpoint.</param>
		/// <param name="y1">Vertical coordinate of the ending endpoint.</param>
		/// <returns>Transformed value.</returns>
		private static double LinearInterpolation(double x, double x0, double y0, double x1, double y1)
		{
			double y;

			// No error checking here bacuse it is called from an inner loop.
			// x1 == x0 is an undefined case. So I define it to be the midpoint of y0 and y1.

			if (x1 == x0)
				y = (y0 + y1) / 2;
			else
				y = y0 + (x - x0) * ((y1 - y0) / (x1 - x0));
			return y;
		}
		/// <summary>
		/// Calculates a random number in the interval [-R, R].
		/// </summary>
		/// <param name="rg">Random number generator to use.</param>
		/// <param name="R">Random interval.</param>
		/// <returns>Random value in the interval of [-R, R].</returns>
		private static double GetRandom(Random rg, double R)
		{
			// No error checking here bacuse it is called from an inner loop.
			return (2 * rg.NextDouble() * R - R);
		}
		/// <summary>
		/// Calculates the height of the specified midpoint against its Von Neumann neighborhood.
		/// </summary>
		/// <param name="t">Playground.</param>
		/// <param name="M">Midpoint in question.</param>
		/// <param name="SS">Section step.</param>
		/// <returns>Transformed midpoint.</returns>
		private static double GetMidpointHeight(Terrain t, HeightPoint M, int SS)
		{
			//   T
			// L M R
			//   B

			HeightPoint L, R, T, B;
			int size;

			// No error checking here bacuse it is called from an inner loop.

			size = t.Size.Width;
			L = M + (HeightPoint.LeftVector * SS / 2);
			R = M + (HeightPoint.RightVector * SS / 2);
			T = M + (HeightPoint.TopVector * SS / 2);
			B = M + (HeightPoint.BottomVector * SS / 2);

			if (L.X < 0) L.X = size - 1;
			if (R.X >= size) R.X = 0;
			if (T.Y < 0) T.Y = size - 1;
			if (B.Y >= size) B.Y = 0;

			L.Height = t[L.X, L.Y];
			R.Height = t[R.X, R.Y];
			T.Height = t[T.X, T.Y];
			B.Height = t[B.X, B.Y];

			M = HeightPoint.Midpoint(L, R, T, B);
			return M.Height;
		}

		#endregion
		#region Error checking

		private static void VerifyNotNull(object o, string argName)
		{
			if (o == null)
				throw new ArgumentNullException(argName, "Argument cannot be null.");
		}
		private static void VerifyValidDouble(double d, string argName)
		{
			if (double.IsInfinity(d) || double.IsNaN(d) || double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d))
				throw new ArgumentOutOfRangeException(argName, d, "Argument must be a valid real value.");
		}
		private static void VerifyPositiveInt(int i, string argName)
		{
			if (i < 0)
				throw new ArgumentOutOfRangeException(argName, i, "Argument must not be less than zero.");
		}
		private static void VerifyPositiveSize(Size s, string argName)
		{
			VerifyPositiveInt(s.Width, argName + ".Width");
			VerifyPositiveInt(s.Height, argName + ".Height");
		}

		#endregion

		#endregion
		#region Properties

		/// <summary>
		/// Gets or sets height of the point in Height Array associated with the specified coordinates.
		/// </summary>
		/// <param name="x">Horizontal coordinate of the point to get or set.</param>
		/// <param name="y">Vertical coordinate of the point to get or set.</param>
		/// <returns>Height of the specified point.</returns>
		public double this[int x, int y]
		{
			get { return HeightMap[x][y]; }
			set
			{
				HeightMap[x][y] = value;
				if (value < _min)
					_min = value;
				if (value > Max)
					_max = value;
			}
		}

		/// <summary>
		/// Gets the Height Map Array.
		/// </summary>
		private double[][] HeightMap
		{
			get { return _heightMap; }
		}
		/// <summary>
		/// Gets the terain size.
		/// </summary>
		public Size Size
		{
			get { return _size; }
		}
		/// <summary>
		/// Gets the height of bottommost point.
		/// </summary>
		public double Min
		{
			get { return _min; }
		}
		/// <summary>
		/// Gets the height of uppermost point.
		/// </summary>
		public double Max
		{
			get { return _max; }
		}

		/// <summary>
		/// Gets or sets the reference to ProgressBackgroundWorker that is handling this instance of Terrain.
		/// </summary>
		public ProgressBackgroundWorker PBW
		{
			get { return _pbw; }
			set { _pbw = value; }
		}

		#endregion
		#region Fields

		/// <summary>
		/// Stores the Height Map Array.
		/// </summary>
		private double[][] _heightMap;
		/// <summary>
		/// Stores the terain size.
		/// </summary>
		Size _size;
		/// <summary>
		/// Stores the height of bottommost point.
		/// </summary>
		private double _min;
		/// <summary>
		/// Stores the height of uppermost point.
		/// </summary>
		private double _max;

		/// <summary>
		/// Stores the reference to ProgressBackgroundWorker that is handling this instance of Terrain.
		/// </summary>
		private ProgressBackgroundWorker _pbw;

		#endregion
	}

	[AttributeUsage(AttributeTargets.Parameter)]
	public class AttributeBase : Attribute
	{
		public AttributeBase(string description)
		{
			_description = description;
		}
		public string Description
		{
			get { return _description; }
		}
		private string _description;
	}

	[AttributeUsage(AttributeTargets.Parameter)]
	public class AttributeDouble : AttributeBase
	{
		public AttributeDouble(string description, double defaultValue, double min, double max, int decimals, double increment)
			: base(description)
		{
			_defaultValue = defaultValue;
			_min = min;
			_max = max;
			_decimals = decimals;
			_increment = increment;
		}
		public AttributeDouble(string description, double defaultValue, double min, double max)
			: base(description)
		{
			_defaultValue = defaultValue;
			_min = min;
			_max = max;
			_decimals = 0;
			_increment = 1;
		}
		public AttributeDouble(string description, double min, double max)
			: base(description)
		{
			_defaultValue = 0;
			_min = min;
			_max = max;
			_decimals = 0;
			_increment = 1;
		}
		public AttributeDouble(string description)
			: base(description)
		{
			_defaultValue = 0;
			_min = double.MinValue;
			_max = double.MaxValue;
			_decimals = 0;
			_increment = 1;
		}

		public double DefaultValue
		{
			get { return _defaultValue; }
		}
		public double Min
		{
			get { return _min; }
		}
		public double Max
		{
			get { return _max; }
		}
		public int Decimals
		{
			get { return _decimals; }
		}
		public double Increment
		{
			get { return _increment; }
		}

		private double _defaultValue;
		private double _min;
		private double _max;
		private int _decimals;
		private double _increment;
	}
	[AttributeUsage(AttributeTargets.Parameter)]
	public class AttributeInt : AttributeDouble
	{
		public AttributeInt(string description, int defaultValue, int min, int max, int increment)
			: base(description, defaultValue, min, max, 0, increment)
		{
		}
		public AttributeInt(string description, int defaultValue, int min, int max)
			: base(description, defaultValue, min, max)
		{
		}
		public AttributeInt(string description, int min, int max)
			: base(description, min, max)
		{
		}
		public AttributeInt(string description)
			: base(description)
		{
		}

		public new int DefaultValue
		{
			get { return (int)base.DefaultValue; }
		}
		public new int Min
		{
			get { return (int)base.Min; }
		}
		public new int Max
		{
			get { return (int)base.Max; }
		}
		public new int Increment
		{
			get { return (int)base.Increment; }
		}
	}
}
