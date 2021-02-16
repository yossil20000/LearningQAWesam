using LearningQA.Client.Model;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface ITestItemViewModel
	{
		TestItemViewModelPersist TestItemViewModelPersist { get; set; }
		Task RetriveTestItemInfos(int testItemId);
		Task<ExamModel> RetriveTest(TestItemInfo testItemInfo);
		Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo);
		
		Task OnLoadCommand();
		Task OnTestItemId(int testItemId);
		void OnNext();
		void OnPrevious();
		void OnStartTest();
		void OnFinishTest();
		void OnCheckTest();
		public void OnSaveCurrentExam();
		

	}


	public class TestItemViewModel : ITestItemViewModel
	{
		
		public ITestItemModel testItemModel;
		public TestItemViewModelPersist TestItemViewModelPersist { get; set; }
		public TestItemViewModel(ITestItemModel testItemModel, TestItemViewModelPersist testItemViewModelPersist)
		{
			this.testItemModel = testItemModel;
			TestItemViewModelPersist = testItemViewModelPersist;

		}
		public async Task RetriveTestItemInfos(int testItemId)
		{
			//if (TestItemViewModelPersist.Initialize)
			//	return;
			await testItemModel.RetriveTestItemInfos();
			
			TestItemViewModelPersist.TestItemInfos = testItemModel.TestItemInfos.ToList();

		}

		public async Task<ExamModel> RetriveTest(TestItemInfo testItemInfo)
		{
			var result = await testItemModel.RetriveTest(testItemInfo);
			return result;
		}
		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			var result = await testItemModel.RetriveTestItem(testItemInfo);
			return result;
		}
		public async Task OnLoadCommand()
		{
			TestItemInfo testItemInfo = new TestItemInfo()
			{
				Category = TestItemViewModelPersist.SelectedCategory,
				Subject = TestItemViewModelPersist.SelectedSubjecte,
				Chapter = TestItemViewModelPersist.SelectedChapter
			};
			var result = await RetriveTest(testItemInfo);
			if(result != null)
			{
				if(result.Test != null && result.Test.Answers != null)
				{
					
					TestItemViewModelPersist.CurrentTest = result.Test;
					TestItemViewModelPersist.CurrentTest.Answers = result.Test.Answers.OrderBy(x => int.Parse(x.QUestionSql.QuestionNumber)).ToList();
					for (int i = 0; i < TestItemViewModelPersist.CurrentTest.Answers.Count(); i++)
					{
						var item = TestItemViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer;
						if (item == null)
						{
							item = new List<AnswareOption<int>>();
							TestItemViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer = item; 
						}
					}
					TestItemViewModelPersist.SetCurrentQuestion(1);
					TestItemViewModelPersist.CurrentTest.Duration = result.Test.Duration;
					
				}
			}
		}
		public async Task OnLoadTestItemCommand()
		{
			TestItemInfo testItemInfo = new TestItemInfo()
			{
				Category = TestItemViewModelPersist.SelectedCategory,
				Subject = TestItemViewModelPersist.SelectedSubjecte,
				Chapter = TestItemViewModelPersist.SelectedChapter
			};
			var result = await RetriveTestItem(testItemInfo);

			//////TestItem = result;
			//////if (TestItem != null)
			//////{
			//////	if (TestItem.Questions == null) TestItem.Questions = new List<QUestionSql>();

			//////}
			//////TestItem.Questions = TestItem.Questions.OrderBy(x => int.Parse(x.QuestionNumber)).ToList();
			//////TestItemViewModelPersist.TestItem = TestItem;
			//////if (TestItemViewModelPersist?.CurretPerson == null)
			//////{
			//////	TestItemViewModelPersist.CurretPerson = new Person<int>() { Name = "yose", IdNumber = "059828392" };
			//////}
			//////TestItemViewModelPersist.SelectedQuestion = result.Questions.FirstOrDefault();
			//CreateNewTest(TestItemViewModelPersist.CurretPerson);



		}

		public void OnNext()
		{

		}

		public void OnPrevious()
		{

		}
		public void OnStartTest()
		{
			TestItemViewModelPersist.ExamState = ExamState.ExamStart;
			TestItemViewModelPersist.CountDownTimer.Stop();
			TestItemViewModelPersist.CountDownTimer.Duration = TestItemViewModelPersist.CurrentTest.Duration;
			TestItemViewModelPersist.CountDownTimer.Start();
			TestItemViewModelPersist.CurrentTest.DateStart = DateTime.Now;
		}
		public void OnFinishTest()
		{
			TestItemViewModelPersist.ExamState = ExamState.ExamFinished;
			TestItemViewModelPersist.CountDownTimer.Stop();
			TestItemViewModelPersist.CurrentTest.DateFinish = DateTime.Now;

		}
		public void OnCheckTest()
		{
			TestItemViewModelPersist.CurrentTest.Mark = ExamResult(TestItemViewModelPersist.CurrentTest);
			TestItemViewModelPersist.ExamState = ExamState.ExamResultReady;

		}
		private int ExamResult(Test<QUestionSql,int> test)
		{
			var correctdAnswers = test.Answers.Where(x => x.IsAnswered && x.IsCorrect).Count();
			return (correctdAnswers * 100)  / test.Answers.Count ;
		}
		public void OnSaveCurrentExam()
		{
			testItemModel.SaveTest(TestItemViewModelPersist.CurrentTest);
		}
		//public void CreateNewTest(Person<int> person)
		//{
		//	Test<QUestionSql, int> test = new Test<QUestionSql, int>();
		//	FillTestEntity(TestItem, test);
		//	//List<Test<QUestionSql, int>> tests = new List<Test<QUestionSql, int>>();
		//	//if (person.Tests == null)
		//	//	person.Tests = tests;
		//	//person.Tests.Add(test);
		//	//foreach(var q in test.TestItem.Questions)
		//	//{


		//	//	for(int i=0; i<  q.Options.Count();i++)
		//	//	{
		//	//		QuestionOptionView questionOptionView = new QuestionOptionView();
		//	//		questionOptionView = (QuestionOptionView)q.Options.ElementAt(i);
		//	//		q.Options.ElementAt(i) = questionOptionView;
		//	//	}


		//	//}
		//	TestItemViewModelPersist.CurrentTest = test;


		//}
		//private void FillTestEntity(TestItem<QUestionSql, int> testItem, Test<QUestionSql, int> test)
		//{


		//	test.Answers = new List<Answer<int>>();
		//	foreach (var questionAnsware in testItem.Questions)
		//	{
		//		var answare = new Answer<int>();
		//		answare.QUestionSql = questionAnsware;
		//		answare.SelectedAnswer = new List<AnswareOption<int>>();
		//		//var selectedAnsware = new AnswareOption<int>();
		//		//selectedAnsware.TenantId = questionAnsware.Options.ElementAt(0).TenantId;
		//		//selectedAnsware.Content = questionAnsware.Options.ElementAt(0).Content;
		//		//answare.SelectedAnswer.Add(selectedAnsware);
		//		//answare.IsCorrect = questionAnsware.IsCorrect(answare.SelectedAnswer);
		//		//answare.SelectedAnswer.Add(selectedAnsware);
		//		answare.TenantId = questionAnsware.QuestionNumber;
		//		test.Answers.Add(answare);

		//	}

		//}

		public async Task OnTestItemId(int testItemId)
		{
			try
			{
				TestItemInfo testItemInfo = await testItemModel.RetriveTestItemInfo(testItemId);
				TestItemViewModelPersist.SelectedCategory = testItemInfo.Category;
				TestItemViewModelPersist.SelectedSubjecte = testItemInfo.Subject;
				TestItemViewModelPersist.SelectedChapter = testItemInfo.Chapter;


			}
			catch (Exception ex)
			{

				
			}
		}
		
	}

}