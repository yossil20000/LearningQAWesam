using LearningQA.Client.Model;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

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
		Task RetriveTestItemInfos();

		Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo);
		Task OnLoadCommand();
		TestItem<QUestionSql, int> TestItem { get; set; }
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
		public TestItem<QUestionSql, int> TestItem { get; set; }

		public TestItemViewModel(ITestItemModel testItemModel, TestItemViewModelPersist testItemViewModelPersist)
		{
			this.testItemModel = testItemModel;
			TestItemViewModelPersist = testItemViewModelPersist;

		}
		public async Task RetriveTestItemInfos()
		{
			if (TestItemViewModelPersist.Initialize)
				return;
			await testItemModel.RetriveTestItemInfos();
			TestItemViewModelPersist.TestItemInfos = testItemModel.TestItemInfos.ToList();

			TestItemViewModelPersist.SelectedQuestion = TestItemViewModelPersist.TestItem.Questions.ElementAt(0);
			TestItemViewModelPersist.CurrentQuestion = 1;
			TestItemViewModelPersist.EnablePreviouse = false;
			if (TestItemViewModelPersist.TestItem.Questions.Count > 1)
				TestItemViewModelPersist.EnableNext = true;
		}


		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			var result = await testItemModel.RetriveTestItem(testItemInfo);
			//TestItemViewModelPersist.SelectedQuestion = result.Questions.FirstOrDefault();
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
			var result = await RetriveTestItem(testItemInfo);

			TestItem = result;
			if (TestItem != null)
			{
				if (TestItem.Questions == null) TestItem.Questions = new List<QUestionSql>();

			}
			TestItem.Questions = TestItem.Questions.OrderBy(x => int.Parse(x.QuestionNumber)).ToList();
			TestItemViewModelPersist.TestItem = TestItem;
			if (TestItemViewModelPersist?.CurretPerson == null)
			{
				TestItemViewModelPersist.CurretPerson = new Person<int>() { Name = "yose", IdNumber = "059828392" };
			}
			TestItemViewModelPersist.SelectedQuestion = result.Questions.FirstOrDefault();
			CreateNewTest(TestItemViewModelPersist.CurretPerson);



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
			TestItemViewModelPersist.CountDownTimer.Duration = TestItemViewModelPersist.TestItem.Duration;
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
			TestItemViewModelPersist.ExamState = ExamState.ExamResultReady;

		}
		public void OnSaveCurrentExam()
		{
			testItemModel.SaveTest(TestItemViewModelPersist.CurrentTest);
		}
		public void CreateNewTest(Person<int> person)
		{
			Test<QUestionSql, int> test = new Test<QUestionSql, int>();
			FillTestEntity(TestItem, test);
			//List<Test<QUestionSql, int>> tests = new List<Test<QUestionSql, int>>();
			//if (person.Tests == null)
			//	person.Tests = tests;
			//person.Tests.Add(test);
			//foreach(var q in test.TestItem.Questions)
			//{


			//	for(int i=0; i<  q.Options.Count();i++)
			//	{
			//		QuestionOptionView questionOptionView = new QuestionOptionView();
			//		questionOptionView = (QuestionOptionView)q.Options.ElementAt(i);
			//		q.Options.ElementAt(i) = questionOptionView;
			//	}


			//}
			TestItemViewModelPersist.CurrentTest = test;


		}
		private void FillTestEntity(TestItem<QUestionSql, int> testItem, Test<QUestionSql, int> test)
		{
			test.TestItem = testItem;

			test.Answers = new List<Answer<int>>();
			foreach (var questionAnsware in testItem.Questions)
			{
				var answare = new Answer<int>();
				answare.QUestionSql = questionAnsware;
				answare.SelectedAnswer = new List<AnswareOption<int>>();
				//var selectedAnsware = new AnswareOption<int>();
				//selectedAnsware.TenantId = questionAnsware.Options.ElementAt(0).TenantId;
				//selectedAnsware.Content = questionAnsware.Options.ElementAt(0).Content;
				//answare.SelectedAnswer.Add(selectedAnsware);
				//answare.IsCorrect = questionAnsware.IsCorrect(answare.SelectedAnswer);
				//answare.SelectedAnswer.Add(selectedAnsware);
				answare.TenantId = questionAnsware.QuestionNumber;
				test.Answers.Add(answare);

			}

		}
	}

}