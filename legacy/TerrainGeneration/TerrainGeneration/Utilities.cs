using System;
using System.Drawing;
using TerrainGeneration;
using System.Collections;

namespace TerrainGeneration
{
	[Serializable]
	public struct ControlData
	{
		public string Type;
		public string Name;
		public string Value;
	}

	public static class Utils
	{
		public static double[] GetCoefficients(string s, double def, int count)
		{
			string d;
			char c;
			double num;
			ArrayList list;
			double[] result;

			d = "";
			s += " ";
			list = new ArrayList();
			for (int n = 0; n < s.Length; n++)
			{
				c = s[n];
				if (char.IsDigit(c) || (c == '-') || (c == '.'))
					d += s[n];
				else
				{
					try
					{
						num = double.Parse(d);
						list.Add(num);
						d = "";
					}
					catch (Exception) { }
				}
			}

			result = new double[count];
			for (int n = 0; n < count; n++)
			{
				if (n < list.Count)
					result[n] = (double)list[n];
				else
					result[n] = def;
			}
			return result;
		}
		public static Point[] GetFeatures(int w, int h, int count)
		{
			Point[] result;
			Random r;

			result = new Point[count];
			r = new Random();
			for (int n = 0; n < count; n++)
				result[n] = new Point(r.Next(w), r.Next(h));

			return result;
		}
	}


}

namespace System.Windows.Forms
{
	public interface IProgressBox
	{
		#region Methods

		void SetTask(string taskName, int taskNum, int taskCount, bool resetProgress);
		void SetTask(string taskName, int taskNum, int taskCount);
		void SetTask(string taskName, int taskNum, bool resetProgress);
		void SetTask(string taskName, int taskNum);
		void SetTask(string taskName, bool resetProgress);
		void SetTask(string taskName);

		void SetProgress(int value);
		void SetProgress(float progress);
		void SetProgress(long numerator, long denominator);
		void ResetProgress();

		#endregion
		#region Properties

		string TaskName { get; set; }
		int TaskNum { get; set; }
		int TaskCount { get; set; }
		int Progress { get; set; }

		#endregion
	}
}

namespace System.Collections.Specialized
{
	/// <summary>
	/// Provides methods for creating and manipulating controlData lists.
	/// </summary>
	[Serializable]
	public class ControlDataCollection : System.Collections.CollectionBase
	{
		#region Construction / Destruction

		/// <summary>
		/// Initializes a new instance of the ControlDataCollection class that is empty and has the default initial capacity.
		/// </summary>
		public ControlDataCollection() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the ControlDataCollection class that contains elements copied from the specified ControlData array and has the same initial capacity as the number of elements copied.
		/// </summary>
		/// <param name="controlsData">The ControlData array whose elements are copied to the new list.</param>
		public ControlDataCollection(ControlData[] controlsData) : base()
		{
			if (controlsData != null)
				AddRange(controlsData);
			else
				throw new ArgumentException("'controlsData' cannot be null.", "controlsData");
		}


		#endregion
		#region Item Manipualtion
		
		/// <summary>
		/// Adds the items of the ControlData array to the end of the list.
		/// </summary>
		/// <param name="controlsData">The ControlData array wohse elements should be added to the end of the list.</param>
		public void AddRange(ControlData[] controlsData)
		{
			foreach (ControlData controlData in controlsData)
				List.Add(controlData);
		}

		/// <summary>
		/// Adds the controlData to the end of the list.
		/// </summary>
		/// <param name="controlData">The rule to be added to the end of the list.</param>
		public void Add(ControlData controlData)
		{
			List.Add(controlData);
		}

		/// <summary>
		/// Removes the controlData at the specified index of the list.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public new void RemoveAt(int index)
		{
			List.RemoveAt(index);
		}


		#endregion
		#region Properties

		/// <summary>
		/// Gets the list as the ControlData array.
		/// </summary>
		public ControlData[] GetControlsData
		{
			get
			{
				ControlData[] result;

				result = new ControlData[Count];
				for (int n = 0; n < Count; n++)
					result[n] = this[n];

				return result;
			}
		}
				
		/// <summary>
		/// Returnes the controlData at the specified zero-based index.
		/// </summary>
		public ControlData this[int index]
		{
			get
			{
				return (ControlData)List[index];
			}
			set
			{
				List[index] = value;
			}
		}


		#endregion
	}
}


