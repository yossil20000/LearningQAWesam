using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class TestItemInfo
	{
		public int Id { get; set; }
		public string Category { get; set; } = "";

		public string Subject { get; set; } = "";
		public string Chapter { get; set; } = "";
		public int Version { get; set; } = 0;
		public int NumOfQuestions { get; set; }
	}
}
