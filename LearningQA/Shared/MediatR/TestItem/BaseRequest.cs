using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.TestItem
{
	public class BaseRequest
	{
		public int UserId { get; set; }
		public string  Name { get; set; }
	}
}
