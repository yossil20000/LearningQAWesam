using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface IViewPersistanceBase
	{

		string SelectedCategory { get; set; }
		List<string> Categories { get; set; }
		string SelectedSubjecte { get; set; }

		List<string> Subjectes { get; set; }

		string SelectedChapter { get; set; }
		List<string> Chapteres { get; set; }
		bool Initialize { get; set; }
		#region Action Events

		void Changed();

		void OnChanged(Action callBack);
		void OnUnChanged(Action callBack);

		#endregion
		void Dispose();
		
	}
}
