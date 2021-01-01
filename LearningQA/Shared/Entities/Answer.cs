using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	public class Answer<Tdb> : IAnswer<Tdb>
	{
		public Tdb Id { get; set; }
		public virtual  QUestionSql QUestionSql { get; set; }
		public virtual ICollection<AnswareOption<Tdb>> SelectedAnswer { get; set; }
		public bool IsCorrect { get; set; }
		public bool IsAnswered { get; set; }
		public string TenantId { get; set; }
	}
}
