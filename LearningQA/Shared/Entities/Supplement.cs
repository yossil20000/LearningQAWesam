using LearningQA.Shared.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Entities
{
	public class Supplement<Tdb> : ISupplement<Tdb>
	{
		public Tdb Id { get; set; }
		public string TenantId { get; set; }
		public string Content { get; set; }
	}
}
