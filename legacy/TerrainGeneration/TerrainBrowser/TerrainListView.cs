using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Xml;

namespace System.Windows.Forms
{
	using XmlType = XmlNode;

	public class TerrainListView : System.Windows.Forms.ListView
	{
		#region Constants

		private const string NotSupportedMessage = "Method or property is disabled in TerrainListView control.";

		#endregion
		#region Construction / Destruction

		public TerrainListView()
			: base()
		{
			Initialize();
		}

		private void Initialize()
		{
			base.Activation = ItemActivation.Standard;
			base.Alignment = ListViewAlignment.Top;
			base.AllowColumnReorder = false;
			base.AllowDrop = false;
			base.AutoArrange = false;
			base.CheckBoxes = true;
			base.FullRowSelect = true;
			base.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			base.HideSelection = false;
			base.LabelEdit = false;
			base.MultiSelect = false;
			base.View = View.Details;

			base.Columns.Add("+", 25);
			base.Columns.Add("Command", 150);
			base.Columns.Add("Rectangle", 110);
			base.Columns.Add("Weight", 50);
			base.Columns.Add("Source", 700);

			_items = new TerrainListViewItemCollection(base.Items);
		}

		#endregion
		#region Enums / Struct / Classes

		public class TerrainListViewItemCollection : IList, ICollection, IEnumerable
		{
			#region Construction / Destruction

			public TerrainListViewItemCollection(ListViewItemCollection baseCollection)
			{
				_bc = baseCollection;
			}

			#endregion
			#region Enums / Struct / Classes

			private class LLVICEnum : IEnumerator
			{
				public LLVICEnum(TerrainListViewItem[] items)
				{
					_items = items;
					_position = -1;
				}

				public bool MoveNext()
				{
					_position++;
					return (_position < _items.Length);
				}
				public void Reset()
				{
					_position = -1;
				}
				public object Current
				{
					get
					{
						try
						{
							return _items[_position];
						}
						catch (IndexOutOfRangeException)
						{
							throw new InvalidOperationException();
						}
					}
				}

				private TerrainListViewItem[] _items;
				private int _position;
			}

			#endregion
			#region Methods

			public TerrainListViewItem Add(TerrainListViewItem value)
			{
				return (TerrainListViewItem)BC.Add(value);
			}
			public TerrainListViewItem Add(XmlType node)
			{
				return Add(new TerrainListViewItem(node));
			}
			public TerrainListViewItem Add(string text, int imageIndex)
			{
				return (TerrainListViewItem)BC.Add(text, imageIndex);
			}
			public TerrainListViewItem Add(string text, string imageKey)
			{
				return (TerrainListViewItem)BC.Add(text, imageKey);
			}
			public TerrainListViewItem Add(string key, string text, int imageIndex)
			{
				return (TerrainListViewItem)BC.Add(key, text, imageIndex);
			}
			public TerrainListViewItem Add(string key, string text, string imageKey)
			{
				return (TerrainListViewItem)BC.Add(key, text, imageKey);
			}
			public void AddRange(TerrainListViewItemCollection items)
			{
				foreach (TerrainListViewItem item in items)
					BC.Add(item);
			}
			public void AddRange(TerrainListViewItem[] items)
			{
				BC.AddRange(items);
			}
			public void Clear()
			{
				BC.Clear();
			}
			public bool Contains(TerrainListViewItem item)
			{
				return BC.Contains(item);
			}
			public bool ContainsKey(string key)
			{
				return BC.ContainsKey(key);
			}
			public void CopyTo(Array dest, int index)
			{
				BC.CopyTo(dest, index);
			}
			public TerrainListViewItem[] Find(string key, bool searchAllSubItems)
			{
				ListViewItem[] items;
				TerrainListViewItem[] linkItems;

				items = BC.Find(key, searchAllSubItems);
				linkItems = new TerrainListViewItem[items.Length];
				for (int n = 0; n < items.Length; n++)
					linkItems[n] = (TerrainListViewItem)items[n];

				return linkItems;
			}
			public IEnumerator GetEnumerator()
			{
				return BC.GetEnumerator();
			}
			public int IndexOf(TerrainListViewItem item)
			{
				return BC.IndexOf(item);
			}
			public virtual int IndexOfKey(string key)
			{
				return BC.IndexOfKey(key);
			}
			public TerrainListViewItem Insert(int index, TerrainListViewItem item)
			{
				return (TerrainListViewItem)BC.Insert(index, item);
			}
			public TerrainListViewItem Insert(int index, XmlType node)
			{
				return Insert(index, new TerrainListViewItem(node));
			}
			public TerrainListViewItem Insert(int index, string text, int imageIndex)
			{
				return (TerrainListViewItem)BC.Insert(index, text, imageIndex);
			}
			public TerrainListViewItem Insert(int index, string text, string imageKey)
			{
				return (TerrainListViewItem)BC.Insert(index, text, imageKey);
			}
			public TerrainListViewItem Insert(int index, string key, string text, int imageIndex)
			{
				return (TerrainListViewItem)BC.Insert(index, key, text, imageIndex);
			}
			public TerrainListViewItem Insert(int index, string key, string text, string imageKey)
			{
				return (TerrainListViewItem)BC.Insert(index, key, text, imageKey);
			}
			public void Remove(TerrainListViewItem item)
			{
				BC.Remove(item);
			}
			public void RemoveAt(int index)
			{
				BC.RemoveAt(index);
			}
			public void RemoveByKey(string key)
			{
				BC.RemoveByKey(key);
			}

			#region Explicit Interface Implementations

			int IList.Add(Object item)
			{
				return BCList.Add(item);
			}
			void IList.Clear()
			{
				BCList.Clear();
			}
			bool IList.Contains(Object item)
			{
				return BCList.Contains(item);
			}
			void ICollection.CopyTo(Array array, int index)
			{
				BCList.CopyTo(array, index);
			}
			int IList.IndexOf(Object item)
			{
				return BCList.IndexOf(item);
			}
			void IList.Insert(int index, Object item)
			{
				BCList.Insert(index, item);
			}
			void IList.Remove(Object item)
			{
				BCList.Remove(item);
			}
			void IList.RemoveAt(int index)
			{
				BCList.RemoveAt(index);
			}

			#endregion

			#endregion
			#region Properties

			private ListViewItemCollection BC
			{
				get { return _bc; }
			}
			private IList BCList
			{
				get { return (IList)BC; }
			}

			public int Count
			{
				get { return BC.Count; }
			}
			public bool IsReadOnly
			{
				get { return BC.IsReadOnly; }
			}
			public virtual TerrainListViewItem this[int index]
			{
				get { return (TerrainListViewItem)BC[index]; }
				set { BC[index] = value; }
			}
			public virtual ListViewItem this[string key]
			{
				get { return (TerrainListViewItem)BC[key]; }
			}

			#region Explicit Interface Implementations

			int ICollection.Count
			{
				get { return BCList.Count; }
			}
			bool ICollection.IsSynchronized
			{
				get { return BCList.IsSynchronized; }
			}
			Object ICollection.SyncRoot
			{
				get { return BCList.SyncRoot; }
			}
			bool IList.IsFixedSize
			{
				get { return BCList.IsFixedSize; }
			}
			Object IList.this[int index]
			{
				get { return BCList[index]; }
				set { BCList[index] = value; }
			}
			bool IList.IsReadOnly
			{
				get { return BCList.IsReadOnly; }
			}

			#endregion

			#endregion
			#region Fields

			private ListViewItemCollection _bc;

			#endregion
		}

		#endregion
		#region Methods

		public new void ArrangeIcons()
		{
			throw new NotSupportedException(NotSupportedMessage);
		}
		public new void ArrangeIcons(ListViewAlignment value)
		{
			throw new NotSupportedException(NotSupportedMessage);
		}
		public void LoadXML(XmlDocument d)
		{
			XmlNodeChangedEventHandler nodeChangedHandler;

			foreach (XmlType node in d.DocumentElement.ChildNodes)
				Items.Add(new TerrainListViewItem(node));
			
			nodeChangedHandler = new XmlNodeChangedEventHandler(NodeChanged);
			d.NodeChanged += nodeChangedHandler;
			d.NodeRemoved += nodeChangedHandler;
		}

		private void NodeChanged(object sender, XmlNodeChangedEventArgs e)
		{
			switch (e.Action)
			{
				case XmlNodeChangedAction.Change:
					FindParentItem((XmlAttribute)e.NewParent).Refresh();
					break;
				case XmlNodeChangedAction.Insert:
					throw new NotSupportedException("Node inserting not supported.");
					break;
				case XmlNodeChangedAction.Remove:
					FindParentItem(e.Node).Remove();
					break;
			}
		}
		private TerrainListViewItem FindParentItem(XmlAttribute attribute)
		{
			TerrainListViewItem result;

			result = null;
			foreach (TerrainListViewItem item in Items)
				foreach (XmlAttribute child in item.Node.Attributes)
					if (child == attribute)
						result = item;
			return result;
		}
		private TerrainListViewItem FindParentItem(XmlType node)
		{
			TerrainListViewItem result;

			result = null;
			foreach (TerrainListViewItem item in Items)
				if (item.Node == node)
					result = item;
			return result;
		}

		#endregion
		#region Properties

		public new ItemActivation Activation
		{
			get { return base.Activation; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new ListViewAlignment Alignment
		{
			get { return base.Alignment; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool AllowColumnReorder
		{
			get { return base.AllowColumnReorder; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool AllowDrop
		{
			get { return base.AllowDrop; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool AutoArrange
		{
			get { return base.AutoArrange; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool CheckBoxes
		{
			get { return base.CheckBoxes; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new CheckedIndexCollection CheckedIndices
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new CheckedListViewItemCollection CheckedItems
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new TerrainListViewItem FocusedItem
		{
			get { return (TerrainListViewItem)base.FocusedItem; }
		}
		public new bool FullRowSelect
		{
			get { return base.FullRowSelect; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new ListViewGroupCollection Groups
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new ColumnHeaderStyle HeaderStyle
		{
			get { return base.HeaderStyle; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new TerrainListViewItemCollection Items
		{
			get { return _items; }
		}
		public new bool LabelEdit
		{
			get { return base.LabelEdit; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool LabelWrap
		{
			get { return base.LabelWrap; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new ImageList LargeImageList
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool MultiSelect
		{
			get { return base.MultiSelect; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new SelectedIndexCollection SelectedIndices
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new SelectedListViewItemCollection SelectedItems
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new bool ShowGroups
		{
			get { return base.ShowGroups; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new ImageList SmallImageList
		{
			get { throw new NotSupportedException(NotSupportedMessage); }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new Size TileSize
		{
			get { return base.TileSize; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}
		public new TerrainListViewItem TopItem
		{
			get { return (TerrainListViewItem)base.TopItem; }
			set { base.TopItem = value; }
		}
		public new View View
		{
			get { return base.View; }
			set { throw new NotSupportedException(NotSupportedMessage); }
		}

		public int SelectedIndex
		{
			get
			{
				if (base.SelectedIndices.Count > 0)
					return base.SelectedIndices[0];
				else
					return -1;
			}
			set
			{
				base.Items[value].Selected = true;
				base.Items[value].Focused = true;
				base.EnsureVisible(value);
			}
		}
		public TerrainListViewItem SelectedItem
		{
			get
			{
				if (base.SelectedItems.Count > 0)
					return (TerrainListViewItem)base.SelectedItems[0];
				else
					return null;
			}
			set
			{
				int index = SelectedIndex;
				Items[index] = value;
				SelectedIndex = index;
			}
		}
		public XmlType SelectedNode
		{
			get
			{
				if (SelectedItem != null)
					return SelectedItem.Node;
				else
					return null;
			}
			set
			{
				int index = SelectedIndex;
				Items[index] = new TerrainListViewItem(value);
				SelectedIndex = index;
			}
		}

		#endregion
		#region Fields

		private TerrainListViewItemCollection _items;

		#endregion
	}

	public class TerrainListViewItem : System.Windows.Forms.ListViewItem
	{
		#region Consts

		private static readonly IFormatProvider Culture = new System.Globalization.CultureInfo("en-US", true);
		public const string XML_Algorithm = "algorithm";
		public const string XML_Active = "active";
		public const string XML_Rectangle = "rectangle";
		public const string XML_Weight = "weight";

		public const int INDEX_Active = 0;
		public const int INDEX_Algorithm = 1;
		public const int INDEX_Rectangle = 2;
		public const int INDEX_Weight = 3;
		public const int INDEX_Source = 4;

		#endregion
		#region Construction / Destruction

		public TerrainListViewItem(XmlType node)
			: base(GetSubItemsArray(node))
		{
			_node = node;
			Checked = ParseBool(NodeValue(node, XML_Active));
			if (ParseType(node.Name) == TerrainItemTypes.Modifier)
				ForeColor = Color.Blue;
		}

		private static string[] GetSubItemsArray(XmlType node)
		{
			string[] subItems;
			string algorithm, source;
			TerrainItemTypes type;

			type = ParseType(node.Name);
			algorithm = NodeValue(node, XML_Algorithm);
			source = node.OuterXml;

			subItems = new string[5] { "", "", "", "", "" };
			subItems[INDEX_Algorithm] = algorithm;
			subItems[INDEX_Source] = source;
			switch (type)
			{
				case TerrainItemTypes.Generator:
					GetGeneratorSubItemsArray(subItems, node);
					break;
				case TerrainItemTypes.Modifier:
					GetModifierSubItemsArray(subItems, node);
					break;
			}

			return subItems;
		}
		private static void GetGeneratorSubItemsArray(string[] subItems, XmlType node)
		{
			Rectangle r;
			double w;

			r = ParseRectangle(NodeValue(node, XML_Rectangle));
			w = ParseDouble(NodeValue(node, XML_Weight));

			subItems[INDEX_Rectangle] = RectangleToString(r);
			subItems[INDEX_Weight] = DoubleToString(w);
		}
		private static void GetModifierSubItemsArray(string[] subItems, XmlType node)
		{
			subItems[INDEX_Rectangle] = "";
			subItems[INDEX_Weight] = "";
		}

		#endregion
		#region Methods

		public void Refresh()
		{
			Refresh(true, true, true, true);
		}
		public void Refresh(bool active, bool algorithm, bool rectangle, bool weight)
		{
			SubItems[INDEX_Source].Text = Node.OuterXml;
			if (active)
				Checked = ParseBool(NodeValue(XML_Active));
			if (algorithm)
				SubItems[INDEX_Algorithm].Text = NodeValue(XML_Algorithm);
			if (rectangle)
				SubItems[INDEX_Rectangle].Text = RectangleToString(ParseRectangle(NodeValue(XML_Rectangle)));
			if (weight)
				SubItems[INDEX_Weight].Text = DoubleToString(ParseDouble(NodeValue(XML_Weight)));
		}

		private string NodeValue(string attribute)
		{
			return Node.Attributes[attribute].Value;
		}
		private static string NodeValue(XmlNode node, string attribute)
		{
			return node.Attributes[attribute].Value;
		}

		#region Custom Attributes

		public string GetCustomAttribute(string attribute)
		{
			VerifyAttribute(Node, attribute);
			return Node.Attributes[attribute].Value;
		}
		public int GetIntAttribute(string attribute)
		{
			return ParseInt(GetCustomAttribute(attribute));
		}
		public double GetDoubleAttribute(string attribute)
		{
			return ParseDouble(GetCustomAttribute(attribute));
		}
		public bool GetBoolAttribute(string attribute)
		{
			return ParseBool(GetCustomAttribute(attribute));
		}
		public Rectangle GetRectangleAttribute(string attribute)
		{
			return ParseRectangle(GetCustomAttribute(attribute));
		}
		public TerrainItemTypes GetTypeAttribute(string attribute)
		{
			return ParseType(GetCustomAttribute(attribute));
		}

		#endregion
		#region Verification

		private static void VerifyString(string s)
		{
			if (s == null)
				throw new ArgumentNullException("Parsing error. Value cannot be null.");
			if (s == "")
				throw new ArgumentNullException("Parsing error. Value cannot be empty.");
		}
		private static void VerifyAttribute(XmlType node, string attribute)
		{
			//if (!node.HasAttribute(attribute)) // !!!
			//    throw new ArgumentException("Specified attribute does not exist.");
		}
		private static void PrepareString(ref string s)
		{
			s = s.Trim().ToLower();
		}
		private static void VerifyPrepareString(ref string s)
		{
			VerifyString(s);
			PrepareString(ref s);
		}

		#endregion
		#region Parsing

		private static int ParseInt(string s)
		{
			VerifyPrepareString(ref s);
			return int.Parse(s, Culture);
		}
		private static double ParseDouble(string s)
		{
			VerifyPrepareString(ref s);
			return double.Parse(s, Culture);
		}
		private static bool ParseBool(string s)
		{
			bool b;

			VerifyPrepareString(ref s);
			switch (s)
			{
				case "1":
					b = true;
					break;
				case "0":
					b = false;
					break;
				case "t":
					b = true;
					break;
				case "f":
					b = false;
					break;
				case "true":
					b = true;
					break;
				case "false":
					b = false;
					break;
				default:
					throw new ArgumentException("Incorrect boolean string!");
			}

			return b;
		}
		private static Rectangle ParseRectangle(string s)
		{
			string[] a;
			int rx, ry, rw, rh;

			VerifyPrepareString(ref s);
			a = s.Split(',');
			if (a.Length != 4)
				throw new ArgumentException("Incorrect number of arguments for a rectangle!");
			rx = ParseInt(a[0]);
			ry = ParseInt(a[1]);
			rw = ParseInt(a[2]);
			rh = ParseInt(a[3]);

			return new Rectangle(rx, ry, rw, rh);
		}
		private static TerrainItemTypes ParseType(string s)
		{
			TerrainItemTypes type;

			VerifyPrepareString(ref s);
			switch (s)
			{
				case "generator":
					type = TerrainItemTypes.Generator;
					break;
				case "modifier":
					type = TerrainItemTypes.Modifier;
					break;
				default:
					throw new ArgumentException("Incorrect type string!");
			}

			return type;
		}

		#endregion
		#region ToString

		private static string IntToString(int i)
		{
			return i.ToString(Culture);
		}
		private static string DoubleToString(double d)
		{
			return d.ToString(Culture);
		}
		private static string RectangleToString(Rectangle r)
		{
			return string.Format(Culture, "{0},{1},{2},{3}", r.X, r.Y, r.Width, r.Height);
		}

		#endregion

		#endregion
		#region Properties

		public XmlType Node
		{
			get { return _node; }
		}
		public TerrainItemTypes Type
		{
			get { return ParseType(Node.Name); }
		}
		public string Algorithm
		{
			get { return NodeValue(XML_Algorithm); }
		}
		public bool Active
		{
			get { return ParseBool(NodeValue(XML_Active)); }
		}
		public Rectangle R
		{
			get { return ParseRectangle(NodeValue(XML_Rectangle)); }
		}
		public double Weight
		{
			get { return ParseDouble(NodeValue(XML_Weight)); }
		}
		public string this[string attribute]
		{
			get { return GetCustomAttribute(attribute); }
		}

		#endregion
		#region Fields

		private XmlType _node;

		#endregion
	}

	public enum TerrainItemTypes { Generator, Modifier };
}
