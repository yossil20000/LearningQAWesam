using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.PageBase
{
	public class PersistanceBase : IDisposable, IViewPersistanceBase
	{
		protected List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
		private string _selectedCategory = "";
		public string SelectedCategory
		{
			get => _selectedCategory;
			set { _selectedCategory = value; OnCategoryChanged(); }
		}
		public List<string> Categories { get; set; } = new List<string>();
		//Subject
		private string _selectedSubjecte = "";
		public string SelectedSubjecte
		{
			get => _selectedSubjecte;
			set { _selectedSubjecte = value; OnSubjectChanged(); }
		}
		private void OnSubjectChanged()
		{
			Chapteres = testItemInfos.Where(x => x.Subject == SelectedSubjecte).Select(x => x.Chapter).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			SelectedChapter = Chapteres.FirstOrDefault();
			Changed();
		}
		private void OnCategoryChanged()
		{
			Subjectes = testItemInfos.Where(x => x.Category == SelectedCategory).Select(x => x.Subject).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			SelectedSubjecte = Subjectes.FirstOrDefault();
			Changed();
		}
		protected int TestTitleFilter(string title)
		{
			var str = title.Split(".");
			int index = int.Parse(str[0]);
			return index;
		}
		public List<string> Subjectes { get; set; } = new List<string>();
		//Chapter
		private string _selectedChapter = "";
		public string SelectedChapter { get => _selectedChapter; set { _selectedChapter = value; Changed(); } }
		public List<string> Chapteres { get; set; } = new List<string>();
		public bool Initialize { get; set; } = false;
		#region Action Events
		protected readonly List<Action> registration = new List<Action>();
		public void Changed()
		{
			registration.ForEach(a => a());

		}
		public void OnChanged(Action callBack)
		{
			registration.Add(callBack);
		}
		public void OnUnChanged(Action callBack)
		{
			try
			{
				registration.Remove(callBack);
			}
			catch (Exception ex)
			{

			}
		}
		#endregion
		public void Dispose()
		{
			
		}
	}
}
