using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Extentions
{
	public static class TestExtensions
	{
		public static bool FillTest(this ExamModel test , TestItem<QUestionSql,int> testItem)
		{
			bool succeed = false;

			try
			{
				if(testItem != null)
				{
					test.Duration = testItem.Duration;
					test.Title = $"{testItem.Category}/{testItem.Subject}/{testItem.Chapter}/{testItem.Version}";
					test.Test = new Test<QUestionSql, int>();
					List<Answer<int>> answers = new List<Answer<int>>();
					foreach(var question in testItem.Questions)
					{
						Answer<int> answer = new();
						answer.QUestionSql = question;
						answers.Add(answer);
					}
					test.Test.Answers = answers;
					test.Test.TestItemId = testItem.Id;
					succeed = true;
				}
			}
			catch(Exception ex)
			{
				succeed = false;
			}
			return succeed;
		}
	}
}
