using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	public record QUestionSql : IQuestion<string, int>
	{
		public int Id { get; set; }

		public string QuestionNumber { get; set; }
		public string Question { get; set; } = string.Empty;
		public int Mark { get; set; }
		public bool IsActive { get; set; }
		//public bool IsAnswered { get; set; } = false;
		//public bool IsMarked { get; set; } = true;
		//public bool IsSelected { get; set; } = true;
		public AnswerType AnswerType { get; set; } = AnswerType.AT_ONE;
		public string AnswerExplain { get; set; } = "";
		public virtual ICollection<Supplement<int>> Supplements { get; set; }
		public virtual ICollection<QuestionOption<int>> Options { get; set; }
		
		//public virtual ICollection<AnswareOption<int>> Answares { get; set; }

		public bool IsCorrect(ICollection<AnswareOption<int>> answare)
		{
			var goodAnsware = Options.Where(x => x.IsTrue).Select(x => x.TenantId).ToList();
			var answared = answare.Select(x => x.TenantId).ToList();
			if (!goodAnsware.Except(answared).Any() && !answared.Except(goodAnsware).Any())
				return true;
			return false;
		}
	}
}

