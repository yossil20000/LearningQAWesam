using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LearningQA.Shared.Interface
{
	public interface ITest<TQuestion,Tdb> : IDb<Tdb>
	{
		Tdb Id { get; set; }
		DateTime DateStart { get; set; }
		DateTime DateFinish { get; set; }
		int Mark { get; set; }
		int Duration { get; set; }
		int TestItemId { get; set; }

		//TestItem<TQuestion, Tdb> TestItem { get; set; }
		ICollection<Answer<Tdb>> Answers { get; set; }
		
	}
}
