using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public class TestItemViewModelPersist
	{
		private readonly List<Action> registration = new List<Action>();
		public bool Initialize { get; set; }
		private void Changed()
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
			catch(Exception ex)
			{

			}
		}
		public TestItemViewModelPersist()
		{

		}
		private string _selectedCategory;
		public string SelectedCategory { get => _selectedCategory; set { _selectedCategory = value;  OnCategoryChanged(); } }
		public List<string> Categories { get; set; } = new List<string>();
		
		private string _selectedSubjecte = "";
		public string SelectedSubjecte { get => _selectedSubjecte; set { _selectedSubjecte = value; OnSubjectChanged(); } }
		public List<string> Subjectes { get; set; } = new List<string>();
		private string _selectedChapter;
		public string SelectedChapter { get => _selectedChapter; set{ _selectedChapter = value; Changed(); } }
		public List<string> Chapteres { get; set; } = new List<string>();
		List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
		public List<TestItemInfo> TestItemInfos { get => testItemInfos; set { testItemInfos = value; Initialize = true; ProcessTestItemInfo(); } }
		private void ProcessTestItemInfo()
		{
			Categories = testItemInfos.Select(x => x.Category).Distinct().ToList();


			Subjectes = testItemInfos.Select(x => x.Subject).Distinct().ToList();
			Chapteres = testItemInfos.Select(x => x.Chapter).Distinct().ToList();

			Changed();
		}

		private void OnSubjectChanged()
		{
			Chapteres = TestItemInfos.Where(x => x.Subject == SelectedSubjecte).Select(x => x.Chapter).Distinct().ToList();
			Changed();
		}
		private void OnCategoryChanged()
		{
			Subjectes = TestItemInfos.Where(x => x.Category == SelectedCategory).Select(x => x.Subject).Distinct().ToList();
			Changed();
		}
		public QUestionSql SelectedQuestion { get; set; }
	}
}
