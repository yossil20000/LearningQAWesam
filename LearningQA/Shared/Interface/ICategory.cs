using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Interface
{ 
	public interface ICategory<Tdb> : IDb<Tdb>
	{

		public Tdb Id { get; set; }
		public string Name { get; set; }
	}
}
