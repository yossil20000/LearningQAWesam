using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Extentions;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public enum TestItemState
	{
		Editable,
		View
	}
	public enum ExamState
	{
		ExamEmpty,
		ExamCreate,
		ExamStart,
		ExamFinished,
		ExamResultReady
	}

	//public class QuestionOptionView :  QuestionOption<int>
	//{
	//	public QuestionOptionView()
	//	{
			
	//	}

	//	public bool IsSelected { get; set; }

	//	public static implicit operator QuestionOptionView(QUestionSql v)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
	
	public class TestItemViewModelPersist : IDisposable
	{
		public TestItemViewModelPersist()
		{
			CountDownTimer.Init(60, OnCountFinished, OnCount);
			CountDownTimer.Start();
		}
		public void Dispose()
		{
			CountDownTimer.Stop();
			CountDownTimer.Dispose();
		}
		#region Action Events
		private readonly List<Action> registration = new List<Action>();
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

		#region States
		public bool Initialize { get; set; } = false;
		public TestItemState TestItemState { get; set; } = TestItemState.View;
		public ExamState ExamState { get; set; } = ExamState.ExamEmpty;

		#endregion

		#region Counter
		public TimeSpan CountDownRemain { get; set; } = new TimeSpan();
		public CountDownTimer CountDownTimer = new CountDownTimer();
		public void OnCount(int count)
		{
			Console.WriteLine($"CountDown reamin {count}");
			
			CountDownRemain = TimeSpan.FromSeconds(count);
			Changed();
		}
		public void OnCountFinished()
		{
			Console.WriteLine($"CountDown Finished");
		}
		#endregion

		#region Test Selection
		//Category
		private string _selectedCategory ="";
		public string SelectedCategory { get => _selectedCategory; set { _selectedCategory = value;  OnCategoryChanged(); } }
		public List<string> Categories { get; set; } = new List<string>();
		//Subject
		private string _selectedSubjecte = "";
		public string SelectedSubjecte { get => _selectedSubjecte; set { _selectedSubjecte = value; OnSubjectChanged(); } }
		public List<string> Subjectes { get; set; } = new List<string>();
		//Chapter
		private string _selectedChapter = "";
		public string SelectedChapter { get => _selectedChapter; set{ _selectedChapter = value; Changed(); } }
		public List<string> Chapteres { get; set; } = new List<string>();
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
			SelectedChapter = Chapteres.FirstOrDefault();
			Changed();
		}
		private void OnCategoryChanged()
		{
			Subjectes = TestItemInfos.Where(x => x.Category == SelectedCategory).Select(x => x.Subject).Distinct().ToList();
			SelectedSubjecte = Subjectes.FirstOrDefault();
			Changed();
		}
		public int CurrentQuestion { get; set; } = 1;

		private Test<QUestionSql, int> currentTest = new Test<QUestionSql, int>();
		public Test<QUestionSql, int> CurrentTest
		{
			get { return currentTest; }
			set
			{
				currentTest = value; ExamState = ExamState.ExamCreate;
			}
		}
		public QUestionSql SelectedQuestion { get; set; } = new QUestionSql();
		public Person<int> CurretPerson { get; set; } = new Person<int>()
		{
			Name = "Yosef",
			IdNumber = "059828392"
		};
		#endregion

		#region Entities

		List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
		public List<TestItemInfo> TestItemInfos { get => testItemInfos; set { testItemInfos = value; Initialize = true; ProcessTestItemInfo(); } }

			private TestItem<QUestionSql, int> testItem = new TestItem<QUestionSql, int>();
		public TestItem<QUestionSql, int> TestItem
		{
			get { return testItem; }
			set
			{
				testItem = value;

				CurrentQuestion = 1; UpdatePagination();

				Changed();
			}
		}
		
		#endregion





		public bool EnablePreviouse { get; set; } = true;
		public bool EnableNext { get; set; } = true;

		
		private void UpdatePagination()
		{
			EnablePreviouse = CurrentQuestion == 1 ? false : true;
			EnableNext = CurrentQuestion < TestItem.Questions.Count  ? true : false;
			
		}
		public void OnNext()
		{
			if (CurrentQuestion < TestItem.Questions.Count)
			{
				CurrentQuestion++;
				SelectedQuestion = TestItem.Questions.ElementAt(CurrentQuestion - 1);
			}
			UpdatePagination();
			Changed();
		}

		public void OnPrevious()
		{
			if (CurrentQuestion > 1)
			{
				CurrentQuestion--;
				SelectedQuestion = TestItem.Questions.ElementAt(CurrentQuestion - 1);
			}
			UpdatePagination();
			Changed();
		}
		public void OnChangeQuestionNumber(ChangeEventArgs ea)
		{
			int questionNumber = 0;
			int.TryParse(ea.Value.ToString(), out questionNumber);
			if (questionNumber >= 1 && questionNumber <= testItem.Questions.Count)
			{
				CurrentQuestion = questionNumber;
				SelectedQuestion = testItem.Questions.ElementAt(CurrentQuestion - 1);
				UpdatePagination();
				Changed();
			}
		}
		public void OnQuestionIsSelected(int Id, ChangeEventArgs ea)
		{
			testItem.Questions.Where(x => x.Id == Id).FirstOrDefault().IsActive = (bool)ea.Value;
		}

		public void OnOptionChanged(QuestionOption<int> id, object checkedValue)
		{
			bool isChecked = (bool)checkedValue;
			Console.WriteLine($"Option id{id.Id } status: {(bool)checkedValue} ");
			var answer = CurrentTest.Answers
				.Where(x => x.QUestionSql.Id == SelectedQuestion.Id)
				.FirstOrDefault();
			var options = answer.SelectedAnswer.Where(x => x.TenantId == id.TenantId).FirstOrDefault();
			if (options == null && isChecked)
				answer.SelectedAnswer.Add(new AnswareOption<int>() { TenantId = id.TenantId, IsTrue = id.IsTrue });
			else if(options != null && !isChecked)
			{
				answer.SelectedAnswer.Remove(options);
			}
			answer.IsAnswered = answer.SelectedAnswer.Count() > 0;
			answer.IsCorrect = answer.QUestionSql.IsCorrect(answer.SelectedAnswer);
		}
	}
}
