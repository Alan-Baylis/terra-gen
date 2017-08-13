using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace System.Windows.Forms
{
	public class PreviewForm : System.Windows.Forms.SnapForm
	{
		#region Construction / Destruction

		/// <summary>
		/// Initialization.
		/// </summary>
		public PreviewForm()
		{
			InitializeComponent();
			InitializeFields();
		}

		/// <summary>
		/// Form initialization.
		/// </summary>
		private void InitializeComponent()
		{
			SuspendLayout();

			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(497, 357);
			MinimumSize = new System.Drawing.Size(100, 100);
			ControlBox = false;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			Name = "PreviewForm";
			Text = "Preview";

			ResumeLayout(false);
		}
		/// <summary>
		/// Field initialization.
		/// </summary>
		private void InitializeFields()
		{
			pbPreview = new PictureBox();
			pbPreview.BackColor = Color.White;
			pbPreview.BorderStyle = BorderStyle.FixedSingle;
			pbPreview.Dock = DockStyle.Fill;
			pbPreview.Image = null;
			Controls.Add(pbPreview);

			_image = null;
			_mode = PreviewModes.None;
			_items = new PreviewItemCollection();
		}

		/// <summary>
		/// Shows the form with the specified owner to the user. 
		/// </summary>
		/// <param name="owner">Any object that implements IWin32Window and represents the top-level window that will own this form.</param>
		/// <param name="image">Image to show.</param>
		public void Show(IWin32Window owner, Image image)
		{
			base.Show(owner);
			Image = image;
		}
		/// <summary>
		/// Shows the form with the specified owner to the user. 
		/// </summary>
		/// <param name="owner">Any object that implements IWin32Window and represents the top-level window that will own this form.</param>
		new public void Show(IWin32Window owner)
		{
			Show(owner, null);
		}

		#endregion
		#region Properties

		public Image Image
		{
			get { return _image; }
			set
			{
				_image = value;
				SetImage();
			}
		}
		public PreviewModes Mode
		{
			get { return _mode; }
			set
			{
				_mode = value;
				SetImage();
			}
		}
		public PreviewItemCollection Items
		{
			get { return _items; }
		}
		private Image PreviewImage
		{
			get { return pbPreview.Image; }
			set { pbPreview.Image = value; }
		}

		#endregion
		#region Methods

		protected override void SetPosition()
		{
			base.SetPosition();
			if (Image != null)
				Size = Image.Size + SizeModifier;
		}

		private void SetImage()
		{
			switch (Mode)
			{
				case PreviewModes.None:
					SetImage_None();
					break;
				case PreviewModes.Image:
					SetImage_Image();
					break;
				case PreviewModes.Preview:
					SetImage_Preview();
					break;
			}
		}
		private void SetImage_None()
		{
			PreviewImage = null;
		}
		private void SetImage_Image()
		{
			PreviewImage = Image;
		}
		private void SetImage_Preview()
		{
			PreviewImage = pbPreview.ErrorImage;
		}

		#endregion
		#region Fields

		private PictureBox pbPreview;
		private Image _image;
		private PreviewModes _mode;
		private PreviewItemCollection _items;

		#endregion
	}

	public enum PreviewModes { None, Preview, Image }
	public class PreviewItem
	{
		public Rectangle R;
		public string Name;
	}
}

namespace System.Collections.Specialized
{
	/// <summary>
	/// Provides methods for creating and manipulating previewItem lists.
	/// </summary>
	public class PreviewItemCollection : System.Collections.CollectionBase
	{
		#region Construction / Destruction

		/// <summary>
		/// Initializes a new instance of the PreviewItemCollection class that is empty and has the default initial capacity.
		/// </summary>
		public PreviewItemCollection()
			: base()
		{
		}
		/// <summary>
		/// Initializes a new instance of the PreviewItemCollection class that contains elements copied from the specified PreviewItem array and has the same initial capacity as the number of elements copied.
		/// </summary>
		/// <param name="previewItems">The PreviewItem array whose elements are copied to the new list.</param>
		public PreviewItemCollection(PreviewItem[] previewItems)
			: base()
		{
			if (previewItems != null)
				AddRange(previewItems);
			else
				throw new ArgumentException("'previewItems' cannot be null.", "previewItems");
		}

		#endregion
		#region Item Manipualtion

		/// <summary>
		/// Adds the items of the PreviewItem array to the end of the list.
		/// </summary>
		/// <param name="previewItems">The PreviewItem array wohse elements should be added to the end of the list.</param>
		public void AddRange(PreviewItem[] previewItems)
		{
			foreach (PreviewItem previewItem in previewItems)
				List.Add(previewItem);
		}
		/// <summary>
		/// Adds the previewItem to the end of the list.
		/// </summary>
		/// <param name="previewItem">The rule to be added to the end of the list.</param>
		public void Add(PreviewItem previewItem)
		{
			List.Add(previewItem);
		}
		/// <summary>
		/// Removes the previewItem at the specified index of the list.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public new void RemoveAt(int index)
		{
			List.RemoveAt(index);
		}

		#endregion
		#region Properties

		/// <summary>
		/// Gets the list as the PreviewItem array.
		/// </summary>
		public PreviewItem[] GetPreviewItems
		{
			get
			{
				PreviewItem[] result;

				result = new PreviewItem[Count];
				for (int n = 0; n < Count; n++)
					result[n] = this[n];

				return result;
			}
		}
		/// <summary>
		/// Returnes the previewItem at the specified zero-based index.
		/// </summary>
		public PreviewItem this[int index]
		{
			get
			{
				return (PreviewItem)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		#endregion
	}
}
