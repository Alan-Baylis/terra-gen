using System;
using System.Drawing;
using TerrainGeneration;

namespace System.Windows.Forms
{
	public partial class ProgressForm : Form, IProgressBox
	{
		#region Construction / Destruction

		public ProgressForm()
		{
			InitializeComponent();
		}
		public void Show(IWin32Window owner, Rectangle ownerPosition, int taskCount)
		{
			Point ownerCenter;

			DialogResult = System.Windows.Forms.DialogResult.None;
			_taskName = "";
			_taskNum = 0;
			_taskCount = taskCount;
			_progress = 0;

			ownerCenter = new Point((ownerPosition.Left + ownerPosition.Right) / 2, (ownerPosition.Bottom + ownerPosition.Top) / 2);
			Location = new Point(ownerCenter.X - Width / 2, ownerCenter.Y - Height / 2);

			base.Show(owner);
		}

		#endregion
		#region Methods

		private void SetTask()
		{
			lblTask.Text = "Task " + TaskNum.ToString() + "/" + TaskCount.ToString() + ": " + TaskName + "...";
			Refresh();
		}
		private void SetProgress()
		{
			pbProgress.Value = Progress;
		}

		public void SetTask(string taskName, int taskNum, int taskCount, bool resetProgress)
		{
			_taskName = taskName;
			_taskNum = taskNum;
			_taskCount = taskCount;

			SetTask();
			if (resetProgress)
				ResetProgress();
		}
		public void SetTask(string taskName, int taskNum, int taskCount)
		{
			SetTask(taskName, taskNum, taskCount, true);
		}
		public void SetTask(string taskName, int taskNum, bool resetProgress)
		{
			SetTask(taskName, taskNum, TaskCount, resetProgress);
		}
		public void SetTask(string taskName, int taskNum)
		{
			SetTask(taskName, taskNum, TaskCount, true);
		}
		public void SetTask(string taskName, bool resetProgress)
		{
			SetTask(taskName, TaskNum + 1, TaskCount, resetProgress);
		}
		public void SetTask(string taskName)
		{
			SetTask(taskName, TaskNum + 1, TaskCount, true);
		}

		public void SetProgress(int value)
		{
			_progress = value;
			SetProgress();
		}
		public void SetProgress(float progress)
		{
			SetProgress((int)Math.Round(progress * 100f));
		}
		public void SetProgress(long numerator, long denominator)
		{
			SetProgress((float)numerator / (float)denominator);
		}
		public void ResetProgress()
		{
			SetProgress(0);
		}

		#endregion
		#region Properties

		public string TaskName
		{
			get { return _taskName; }
			set
			{
				_taskName = value;
				SetTask();
			}
		}
		public int TaskNum
		{
			get { return _taskNum; }
			set
			{
				_taskNum = value;
				SetTask();
			}
		}
		public int TaskCount
		{
			get { return _taskCount; }
			set
			{
				_taskCount = value;
				SetTask();
			}
		}
		public int Progress
		{
			get { return _progress; }
			set
			{
				_progress = value;
				SetProgress();
			}
		}

		#endregion
		#region Fields

		private string _taskName;
		private int _taskNum;
		private int _taskCount;
		private int _progress;

		#endregion
	}
}