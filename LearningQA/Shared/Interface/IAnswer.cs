using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LearningQA.Shared.Interface
{
	public interface IAnswer<Tdb> : IDb<Tdb>
	{
		ICollection<AnswareOption<Tdb>> SelectedAnswer { get; set; }
		bool IsCorrect { get; set; }
		bool IsAnswered { get; set; }
		bool IsMarked { get; set; }
		bool IsSelected { get; set; } 
		string TenantId { get; set; }
	}
}
