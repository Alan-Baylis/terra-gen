using System;

namespace System.ComponentModel
{
	public class ProgressBackgroundWorker : BackgroundWorker
	{
		#region Construction / Destruction

		public ProgressBackgroundWorker(int progressReportTimeout)
			: base()
		{
			WorkerReportsProgress = true;
			WorkerSupportsCancellation = true;

			_progressName = "Unnamed";
			_progressTotal = 0;
			_progressValue = 0;
			S = new Stopwatch(progressReportTimeout);
		}

		#endregion
		#region Methods

		public void Report(string progressName, long progressTotal)
		{
			ProgressName = progressName;
			ProgressTotal = progressTotal;
			ProgressValue = 0;
			ReportProgress(0, progressName);
		}
		public void Report(long progressValue)
		{
			int percent;

			ProgressValue = progressValue;
			if (S.Timeout)
			{
				if (ProgressTotal == 0)
					percent = 0;
				else
					percent = (int)((double)progressValue / (double)ProgressTotal * 100.0);

				if (percent > 100)
					percent = 100;

				ReportProgress(percent, null);
				S.Reset();
			}
		}
		public void Report()
		{
			ProgressValue++;
			Report(ProgressValue);
		}

		#endregion
		#region Properties

		public string ProgressName
		{
			get { return _progressName; }
			set { _progressName = value; }
		}
		public long ProgressTotal
		{
			get { return _progressTotal; }
			set { _progressTotal = value; }
		}
		public long ProgressValue
		{
			get { return _progressValue; }
			set { _progressValue = value; }
		}

		#endregion
		#region Fields

		private string _progressName;
		private long _progressTotal;
		private long _progressValue;
		private Stopwatch S;

		#endregion
	}
}

namespace System
{
	public struct Stopwatch
	{
		#region Construction / Destruction

		public Stopwatch(int milliseconds)
		{
			_timeout = TimeSpan.FromMilliseconds(milliseconds);
			_startTime = DateTime.Now;
		}
		public Stopwatch(TimeSpan timeSpan)
		{
			_timeout = timeSpan;
			_startTime = DateTime.Now;
		}

		#endregion
		#region Methods

		public void Reset()
		{
			_startTime = DateTime.Now;
		}

		#endregion
		#region Properties

		public float Elapsed
		{
			get
			{
				TimeSpan elapsed;
				double milliseconds;

				elapsed = DateTime.Now - _startTime;
				milliseconds = elapsed.TotalMilliseconds;

				return (float)milliseconds;
			}
		}
		public bool Timeout
		{
			get
			{
				bool result;
				TimeSpan elapsed;

				elapsed = DateTime.Now - _startTime;
				result = elapsed > _timeout;

				return result;
			}
		}

		#endregion
		#region Fields

		public DateTime _startTime;
		private TimeSpan _timeout;

		#endregion
	}
}
