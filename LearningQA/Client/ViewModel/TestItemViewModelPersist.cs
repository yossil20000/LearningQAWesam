using LearningQA.Client.PageBase;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Extentions;

using Microsoft.AspNetCore.Components;

using System;
using System.Linq;

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
	
	public class TestItemViewModelPersist : PersistanceBase, IDisposable
	{
		#region Ctor / Dtor
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
			//Console.WriteLine($"CountDown reamin {count}");
			
			CountDownRemain = TimeSpan.FromSeconds(count);
			Changed();
		}
		public void OnCountFinished()
		{
			Console.WriteLine($"CountDown Finished");
			ExamState = ExamState.ExamFinished;
			CountDownTimer.Stop();
			Changed();
		}
		#endregion

		#region Test Selection
				
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
		private Supplement<int> selectedSupplement = null;
		public Supplement<int> SelectedSupplement { 
			get {
				if (selectedSupplement == null)
					selectedSupplement = SelectedQuestion?.Supplements?.ElementAt(0);
				return selectedSupplement; 
			} 
			set { selectedSupplement = value; } } 
		public Person<int> CurretPerson { get; set; } = new Person<int>()
		{
			Name = "Yosef",
			IdNumber = "059828392"
		};
		#endregion

		#region Entities

		

		#endregion

		#region Question Navigation
		public bool EnablePreviouse { get; set; } = true;
		public bool EnableNext { get; set; } = true;

		
		private void UpdatePagination()
		{
			EnablePreviouse = CurrentQuestion == 1 ? false : true;
			EnableNext = CurrentQuestion < CurrentTest.Answers.Count  ? true : false;
			
		}
		public void SetCurrentQuestion(int index)
		{
			CurrentQuestion = index;
			if (CurrentQuestion <= CurrentTest.Answers.Count)
			{
				SelectedQuestion = CurrentTest.Answers.ElementAt(CurrentQuestion - 1).QUestionSql;
			}
			UpdatePagination();
			Changed();
		}
		public void OnNext()
		{
			try
			{
				if (CurrentQuestion < CurrentTest.Answers.Count)
				{
					CurrentQuestion++;
				}
				SetCurrentQuestion(CurrentQuestion);
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
		}

		public void OnPrevious()
		{
			try
			{
				if (CurrentQuestion > 1)
				{
					CurrentQuestion--;
					
				}
				SetCurrentQuestion(CurrentQuestion);
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
		}
		public void OnChangeQuestionNumber(ChangeEventArgs ea)
		{
			try
			{
				int questionNumber = 0;
				int.TryParse(ea.Value.ToString(), out questionNumber);
				if (questionNumber >= 1 && questionNumber <= CurrentTest.Answers.Count)
				{
					CurrentQuestion = questionNumber;
					SetCurrentQuestion(CurrentQuestion);
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
		}
		public void OnQuestionIsSelected(int Id, ChangeEventArgs ea)
		{
			CurrentTest.Answers.Where(x => x.Id == Id).FirstOrDefault().IsSelected = (bool)ea.Value;
		}
		public void MarkCurrentQuestion()
		{

			var answer = CurrentTest.Answers.Where(x => x.QUestionSql.Id == SelectedQuestion.Id).FirstOrDefault();//.Select(x => { return  x.IsMarked = !x.IsMarked; });

			answer.IsMarked = !answer.IsMarked;
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
		#endregion
	}
}
