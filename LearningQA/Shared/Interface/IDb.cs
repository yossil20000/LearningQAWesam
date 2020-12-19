using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Interface
{
	public interface IDb<Tdb> 
	{
		Tdb Id { get; set; }
	}
}
