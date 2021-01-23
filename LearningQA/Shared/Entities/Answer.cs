using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Interface;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LearningQA.Shared.Entities
{
	public class Answer<Tdb> : IAnswer<Tdb>
	{
		
		public Tdb Id { get; set; }
		public virtual  QUestionSql QUestionSql { get; set; }
		public virtual ICollection<AnswareOption<Tdb>> SelectedAnswer { get; set; } 
		public bool IsCorrect { get; set; } = false;
		public bool IsAnswered { get; set; } = false;
		public bool IsMarked { get; set; } = false;
		public bool IsSelected { get; set; } = true;
		public string TenantId { get; set; } = "";
	}
}
