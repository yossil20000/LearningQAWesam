﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class ExamListRequest
	{
		public TestItemInfo TestItemInfo { get; set; }
		public int PersonId { get; set; }
	}
}
