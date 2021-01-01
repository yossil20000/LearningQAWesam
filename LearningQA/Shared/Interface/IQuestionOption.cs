using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Interface
{
	interface IQuestionOption<Tdb> : IDb<Tdb>
	{
		string TenantId { get; set; }
		string Content { get; set; }
		 bool IsTrue { get; set; }
	}
}
