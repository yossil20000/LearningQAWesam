using LearningQA.Client.PageBase;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ObjectTExtensions;
using System.Threading;

namespace LearningQA.Client.ViewModel
{
	[Flags]
	public enum QuestionListFilter 
	{
		All =					0b0111,
		Marked =				0b0001,
		Wrong =					0b0010,
		NotAnswered =			0b0100,
		[Display(Name = "Marked & Wrong")]
		MarkedAndWrong =		0b0011,
		[Display(Name ="Marked & NotAnswered")]
		MarkedAndNotAnswered =	0b0101

	}
	public class ExamViewModelPersist : PersistanceBase , IDisposable
	{
		public List<ExamInfoModel> ExamInfoModels { get; set; } = new List<ExamInfoModel>();
		public void Dispose()
		{
			
		}
		#region Entities

		public ICollection<Answer<int>> FilteredAnsware { get; set; }
		


		#endregion
		#region Test Selection
		//Category
		public async Task OnLoadCommand()
		{
			_ = Task.CompletedTask;
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
			EnableNext = CurrentQuestion < FilteredAnsware.Count ? true : false;

		}
		public void OnNext()
		{
			try
			{
				if (CurrentQuestion < FilteredAnsware.Count)
				{
					CurrentQuestion++;
					SelectedQuestion = FilteredAnsware.ElementAt(CurrentQuestion - 1).QUestionSql;
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
					SelectedQuestion = FilteredAnsware.ElementAt(CurrentQuestion - 1).QUestionSql;
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
				if (questionNumber >= 1 && questionNumber <= FilteredAnsware.Count)
				{
					CurrentQuestion = questionNumber;
					SelectedQuestion = FilteredAnsware.ElementAt(CurrentQuestion - 1).QUestionSql;
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
			FilteredAnsware.Where(x => x.Id == Id).FirstOrDefault().IsSelected = (bool)ea.Value;
		}

		public void OnOptionChanged(QuestionOption<int> id, object checkedValue)
		{
			bool isChecked = (bool)checkedValue;
			Console.WriteLine($"Option id{id.Id } status: {(bool)checkedValue} ");
			var answer = FilteredAnsware
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
		#region Filters
		private QuestionListFilter questionListFilter;
		public QuestionListFilter QuestionListFilter
		{
			get
			{
				return questionListFilter;
			}
			set
			{
				questionListFilter = value;
				OnQuestionListFilter();
			}
		}
		private void OnQuestionListFilter()
		{
			FilteredAnsware = CurrentTest.Answers;
			bool isMarked = questionListFilter.IsFlagSet(QuestionListFilter.Marked);

			bool isNotAnswered = questionListFilter.IsFlagSet(QuestionListFilter.NotAnswered);

			bool isWrong = questionListFilter.IsFlagSet(QuestionListFilter.Wrong);
			Console.WriteLine($"OnQuestionListFilter IsMarked:{isMarked} IsWrong:{isWrong} IsNotAnswered:{isNotAnswered}");
			FilteredAnsware = CurrentTest.Answers.Where(x => ((isWrong ? !x.IsCorrect : false) || (isMarked ? x.IsMarked : false) || (isNotAnswered ? !x.IsAnswered : false ))).ToList();
			
			if(FilteredAnsware.Count > 0)
			{
				CurrentQuestion = 1;
				SelectedQuestion = FilteredAnsware.ElementAt(0).QUestionSql;
				EnablePreviouse = false;
				EnableNext = true;
			}
			else
			{
				CurrentQuestion = 0;
				SelectedQuestion = null;
				EnablePreviouse = false;
				EnableNext = false;
				FilteredAnsware = null;
			}
			
			Console.WriteLine($"SelectedQuestionList: {questionListFilter}");
		}
		
		#endregion
	}
}
