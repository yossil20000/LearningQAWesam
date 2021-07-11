using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Castle.Core.Internal;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{

	public class TestItem<TQuestion, Tdb> : ITestItem<Tdb>, IValidatableObject
	{
		public Tdb Id { get; set; }
		public string Category { get; set; } = "";
		private string _subject;
		public string Subject { get { return _subject; } set { _subject = value; GetHashCode(); } } 
		public string Chapter { get; set; } = "";
		public int Version { get; set; } = 0;

		public string ExamRemarks { get; set; } = "";
		public TestQuestionMarking TestQuestionMarking { get; set; } = TestQuestionMarking.TQM_EQUEALS;
		/// <summary>
		/// Test Duration in Seconds
		/// </summary>
		public int Duration { get; set; } = 3600;
		public virtual ICollection<TQuestion> Questions { get; set; }
		
		
		public string GeTestItemTitle()
		{
			return $"{Category ?? ""}/{Subject ?? ""}/{Chapter ?? "" }.{Version}";
		}
		public string GetQuestionNumber(int index)
		{
			return $"{Category}.{Subject}.{Chapter}.{Version}.{index}";
		}

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> validationResult = new List<ValidationResult>();
			if (string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(Chapter) || string.IsNullOrEmpty(Subject))
				validationResult.Add(new ValidationResult($"{GeTestItemTitle()} One of the foloowint is null or empty : {nameof(Category)} , {nameof(Subject)} , {nameof(Chapter)}"));
			var q = Questions as List<QUestionSql>;
			if(q is not null)
			{
				int result;
				var query = q.Where(x => !int.TryParse(x.QuestionNumber, out result)).Select(x => x.QuestionNumber);
				
				if (query.Count() > 0)
					validationResult.Add(new ValidationResult($"{GeTestItemTitle()} Question Numbers Problem: {query.Aggregate((s1, s2) => s1 + " , " + s2)}"));
			}
			return validationResult;
		}
		
	}
}
