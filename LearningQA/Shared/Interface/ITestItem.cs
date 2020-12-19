using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.Interface
{
	public enum TestQuestionMarking
	{
		TQM_EQUEALS,
		TQM_PER_QUESTION
	}
	public interface ITestItem<Tdb> : IDb<Tdb>
	{

	}
}
