using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LearningQA.Shared.Interface
{
	public enum AnswerType
	{
		AT_ONE,
		AT_MULTI,
		AT_OPEN

	}
	public enum SupplementType
	{
		ST_TEXT,
		ST_IMAGE,
		ST_IMAGE_LINK,
		ST_VIDEO
	}
	public interface IQuestion<TQuestion,Tdb> : IDb<Tdb> where TQuestion : class 
	{
		
		TQuestion Question { get; set; }
		string QuestionNumber { get; set; }
		int Mark { get; set; }
		bool IsActive { get; set; }
		//ICategory<Tdb> Category { get; set; }
		AnswerType AnswerType { get; set; }
		string AnswerExplain { get; set; } 
		ICollection<Supplement<Tdb>> Supplements  { get; set; }
		ICollection<QuestionOption<int>> Options { get; set; }
		//ICollection<AnswareOption<int>> Answares { get; set; }
		
	}
}
