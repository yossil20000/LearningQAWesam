using LearningQA.Client.PageBase;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public class ExamViewModelPersist : PersistanceBase , IDisposable
	{
		public List<ExamInfoModel> ExamInfoModels { get; set; } = new List<ExamInfoModel>();
		public void Dispose()
		{
			
		}
		#region Entities

		
		public List<TestItemInfo> TestItemInfos
		{
			get => testItemInfos;
			set { testItemInfos = value; Initialize = true; ProcessTestItemInfo(); }
		}


		#endregion
		#region Test Selection
		//Category
		public async Task OnLoadCommand()
		{
			_ = Task.CompletedTask;
		}
		private void ProcessTestItemInfo()
		{
			Categories = testItemInfos.Select(x => x.Category).Distinct().ToList();


			Subjectes = testItemInfos.Select(x => x.Subject).Distinct().ToList();
			Chapteres = testItemInfos.Select(x => x.Chapter).Distinct().ToList();

			Changed();
		}
		
		public int CurrentQuestion { get; set; } = 1;

		private Test<QUestionSql, int> currentTest = new Test<QUestionSql, int>();
		public Test<QUestionSql, int> CurrentTest
		{
			get { return currentTest; }
			set
			{
				currentTest = value; 
			}
		}
		public QUestionSql SelectedQuestion { get; set; } = new QUestionSql();
		public Person<int> CurretPerson { get; set; } = new Person<int>()
		{
			Name = "Yosef",
			IdNumber = "059828392"
		};
		#endregion

		#region Exam Quetion Navigation
		public bool EnablePreviouse { get; set; } = true;
		public bool EnableNext { get; set; } = true;


		private void UpdatePagination()
		{
			EnablePreviouse = CurrentQuestion == 1 ? false : true;
			EnableNext = CurrentQuestion < CurrentTest.Answers.Count ? true : false;

		}
		public void OnNext()
		{
			try
			{
				if (CurrentQuestion < CurrentTest.Answers.Count)
				{
					CurrentQuestion++;
					SelectedQuestion = CurrentTest.Answers.ElementAt(CurrentQuestion - 1).QUestionSql;
				}
				UpdatePagination();
				Changed();
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
					SelectedQuestion = CurrentTest.Answers.ElementAt(CurrentQuestion - 1).QUestionSql;
				}
				UpdatePagination();
				Changed();
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
					SelectedQuestion = CurrentTest.Answers.ElementAt(CurrentQuestion - 1).QUestionSql;
					UpdatePagination();
					Changed();
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
			else if (options != null && !isChecked)
			{
				answer.SelectedAnswer.Remove(options);
			}
			answer.IsAnswered = answer.SelectedAnswer.Count() > 0;
			answer.IsCorrect = answer.QUestionSql.IsCorrect(answer.SelectedAnswer);
		}
		#endregion
	}
}
