using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LearningQA.Shared.Extentions
{
	public static class TimerExtension
	{
		public static int Duration {get;set;}

	}

	public class CountDownTimer : Timer
	{
		private int duration;
		public int Duration 
		{ 
			get { return duration; }
			set { duration = value;	} 
		}
		private DateTime baseTime;

		public Action OnCountFinish;
		public Action<int> OnCount;
		public Action OnCount25Percent;
		
		public CountDownTimer() :base()
		{

			Interval = 1000;
			base.Elapsed += CountDownTimer_Elapsed;
		}

		private void CountDownTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			int remain = duration - (int)(DateTime.Now - baseTime).TotalSeconds;
			OnCount(remain);
			if (remain <= 0)
				OnCountFinish();
		}

		 
		public void Init(int duration,Action onCountFinish = null,Action<int> onCount = null, Action onCount25Percent = null)
		{
			this.duration = duration;
			OnCount = onCount;
			OnCountFinish = onCountFinish;
			OnCount25Percent = onCount25Percent;
		}
		public new void Start(int duration)
		{
			this.duration = duration;
			baseTime = DateTime.Now;
			base.Start();
		}
		public new void Start()
		{
			baseTime = DateTime.Now;
			base.Start();
		}
		public new void Stop()
		{
			base.Stop();
		}
	}
}
