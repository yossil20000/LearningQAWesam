using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;

namespace LearningQA.Client.PageBase
{
	public enum RegisterEvent
	{
		SelectedSupplement
	}
	public class PersistanceBase : IDisposable, IViewPersistanceBase
	{
		
		public PersistanceBase()
		{
			events.Add(PageBase.RegisterEvent.SelectedSupplement, new List<Task>());
		}
		protected List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
		public List<TestItemInfo> TestItemInfos
		{
			get => testItemInfos;
			set { testItemInfos = value; Initialize = true; ProcessTestItemInfo(); }
		}
		TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
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
			if (str.Length > 0)
			{
				
				if(int.TryParse(str[0], out var index))
				return index;
			}
			return 0;

		}
		public List<string> Subjectes { get; set; } = new List<string>();
		//Chapter
		private string _selectedChapter = "";
		public string SelectedChapter { get => _selectedChapter; set { _selectedChapter = value; Changed(); } }
		public List<string> Chapteres { get; set; } = new List<string>();
		public bool Initialize { get; set; } = false;
		#region Action Events
		protected readonly List<Action> registration = new List<Action>();
		protected readonly Dictionary<RegisterEvent, List<Task>> events = new Dictionary<RegisterEvent, List<Task>>();
		public void Changed()
		{
			registration.ForEach(a => a());

		}
		public void RegisterEvent(RegisterEvent registerEvent,  Task callBack)
		{
			events[registerEvent].Add(callBack);
		}
		public void URegisterEvent(RegisterEvent registerEvent, Task callBack)
		{
			events[registerEvent].Remove(callBack);
		}

		public void  OnEventChanged(RegisterEvent registerEvent)
		{
			//events[registerEvent].ForEach(a => Task.Run(() => a));
			var tasks = from a in events[PageBase.RegisterEvent.SelectedSupplement] select a;
			Task.WhenAll(tasks.ToList());
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
		protected void ProcessTestItemInfo()
		{
			for(int i=0;i < testItemInfos.Count;i++)
			{
				testItemInfos[i].Category = myTI.ToTitleCase(testItemInfos[i].Category);
				testItemInfos[i].Subject = myTI.ToTitleCase(testItemInfos[i].Subject);
				testItemInfos[i].Chapter = myTI.ToTitleCase(testItemInfos[i].Chapter);
			}

			Categories = testItemInfos.Select(x => x.Category).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			
			Console.WriteLine($"ToTitleCase: {Categories == null}");
			
			Subjectes = testItemInfos.Select(x => x.Subject).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
		
			Chapteres = testItemInfos.Select(x => x.Chapter).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			
			Changed();
		}
	}
}
