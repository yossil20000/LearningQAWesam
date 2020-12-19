using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Interface
{
	interface ISupplement<Tdb> : IDb<Tdb> 
	{
		string TenantId { get; set; }
		string Content { get; set; }
	}
}
