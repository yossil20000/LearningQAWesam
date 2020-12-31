using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Server.Configuration
{
	public sealed class LeaningConfig
	{
		public static string ConfigSection = "LeaningConfig";
		public Question Question { get; set; }
	}
	public sealed class Question
	{
		public bool IaValid { get; set; }
		public string ExamPrefix { get; set; }
		public int DefaultQuestionNumber { get; set; }
		public int PassingSquer { get; set; }
	}
}
