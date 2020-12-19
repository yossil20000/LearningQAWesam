using System;
using System.Collections.Generic;
using System.Linq;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{

	public class TestItem<TQuestion, Tdb> : ITestItem<Tdb>
	{
		public Tdb Id { get; set; }
		public string Category { get; set; } = "";
		private string _subject;
		public string Subject { get { return _subject; } set { _subject = value; GetHashCode(); } } 
		public string Chapter { get; set; } = "";
		public int Version { get; set; } = 0;

		public TestQuestionMarking TestQuestionMarking { get; set; } = TestQuestionMarking.TQM_EQUEALS;
		/// <summary>
		/// Test Duration in Seconds
		/// </summary>
		public int Duration { get; set; } = 3600;
		public virtual ICollection<TQuestion> Questions { get; set; }
		

		
		public string GeTestItemTitle()
		{
			return $"{Category}/{Subject}/{Chapter}.{Version}";
		}
		public string GetQuestionNumber(int index)
		{
			return $"{Category}.{Subject}.{Chapter}.{Version}.{index}";
		}
	}
}
