using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class ExamModel
	{
		public int Duration { get; set; }
		public string Title { get; set; } = "";

		public Test<QUestionSql, int> Test { get; set; } = new Test<QUestionSql, int>();

	}
}
