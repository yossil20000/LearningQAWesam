using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	
	public class Test<TQuestion ,Tdb> : ITest<TQuestion,Tdb>
	{
	
		public Tdb Id { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateFinish { get; set; }
		public int Mark { get; set; }
		public int Duration { get; set; }
		public int TestItemId { get; set; }
		//public virtual TestItem<TQuestion,Tdb> TestItem { get; set; }
		public virtual ICollection<Answer<Tdb>> Answers { get; set; }
		
	}
}
