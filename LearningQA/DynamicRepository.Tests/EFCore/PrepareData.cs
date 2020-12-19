using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicRepository.EFCore;
using FluentAssertions;
using System.Linq.Expressions;
using LearningQA.Shared.Entities;
using MongoDB.Driver;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{
		
		private List<Person<int>> CreatePersons()
		{
			int index = 1;
			Persons = new[]
			{

				new Person<int>()
				{
					Id = 0,
					Name = $"Name {index}",
					Address = $"Address {index}",
					Email = $"Email {index}",
					Phone = $"Phone {index}",
					IdNumber = FirstId.ToString(),
					Tests = new List<Test<QUestionSql,int>>()
				},
				new Person<int>()
				{
					Id = 0,
					Name = $"Name {++index}",
					Address = $"Address {index}",
					Email = $"Email {index}",
					Phone = $"Phone {index}",
					IdNumber = SecondId.ToString(),
					Tests = new List<Test<QUestionSql,int>>()
				},
				new Person<int>()
				{
					Id = 0,
					Name = $"Name {++index}",
					Address = $"Address {index}",
					Email = $"Email {index}",
					Phone = $"Phone {index}",
					IdNumber = "059828392",
					Tests = new List<Test<QUestionSql,int>>()
				}

			};
			return Persons.ToList();
		}

		private Person<int> CreatePerson(char index)
		{
			Person<int> person = new Person<int>()
			{
				Id = 0,
				Name = $"Name {index}",
				Address = $"Address {index}",
				Email = $"Email {index}",
				Phone = $"Phone {index}",
				IdNumber = new String(index, 8),
				Tests = new List<Test<QUestionSql, int>>()
			};
			return person;
		}
		private Test<QUestionSql, int> StartTest()
		{
			
			var test = new Test<QUestionSql, int>();
			test.DateStart = DateTime.Now;
			return test;
			
		}

		private  void  CreateTestItemEntity(int startIndex = 0 ,int numTestItem = 4, int numTest = 1)
		{

			var TestItems = new List<TestItem<QUestionSql, int>>();

			
			for (int i = startIndex; i < numTestItem + startIndex; i++)
			{
				//TestItem<QUestionSql, int> q = new TestItem<QUestionSql, int>();
				//q.Category = $"C{i}";
				//q.Chapter = $"Ch{1}";
				//q.Subject = $"S{i}";
				//q.Duration = 3600;
				//q.Questions = new List<QUestionSql>();
				//for (int j = 0; j < 4; j++)
				//{
				//	QUestionSql qq = new QUestionSql();
				//	qq.Options = new List<QuestionOption<int>>();
				//	qq.Answares = new List<AnswareOption<int>>();
				//	qq.QuestionNumber = $"Itemm:{i} {j}";
				//	qq.Question = $"Select {j}";

				//	for (int k = 0; k < 4; k++)
				//	{
				//		QuestionOption<int> questionOption = new QuestionOption<int>();
				//		questionOption.TenantId = $"Item:{qq.QuestionNumber} {k}";
				//		questionOption.Content = $"I am {k}";
				//		qq.Options.Add(questionOption);
				//	}
				//	qq.Answares.Add(qq.Options.ElementAt(j).ToAnswareOption<int>());
				//	q.Questions.Add(qq);
				//}

				TestItems.Add(CreateTestItem(i,4,1));
			}
			TestItemsData = TestItems;
			
		}


		private Test<QUestionSql, int> FillTestEntity(int startIndex = 0, int numTestItem = 4, int numTest = 1)
		{

			
			var test = StartTest();
			test.TestItem = TestItemsData.ElementAt(0);
			test.DateFinish = test.DateStart.AddSeconds(60);
			test.Answers = new List<Answer<int>>();
			foreach (var questionAnsware in TestItemsData.ElementAt(0).Questions)
			{
				var answare = new Answer<int>();
				answare.SelectedAnswer = new List<AnswareOption<int>>();
				var selectedAnsware = new AnswareOption<int>();
				selectedAnsware.TenantId = questionAnsware.Answares.ElementAt(0).TenantId;
				selectedAnsware.Content = questionAnsware.Answares.ElementAt(0).Content;
				answare.SelectedAnswer.Add(selectedAnsware);
				answare.IsCorrect = questionAnsware.IsCorrect(answare.SelectedAnswer);
				answare.SelectedAnswer.Add(selectedAnsware);
				answare.TenantId = questionAnsware.QuestionNumber;
				test.Answers.Add(answare);

			}
			var tests = new List<Test<QUestionSql, int>>();
			tests.Add(test);
			TestsData = tests;
			return tests.FirstOrDefault();
		}
		private TestItem<QUestionSql, int> CreateTestItem(int modifier = 0, int numOfQuestion = 4, int numOfRightQuestion=1)
		{
			
				TestItem<QUestionSql, int> q = new TestItem<QUestionSql, int>();
				q.Category = $"Catagory:{modifier}";
				q.Chapter = $"Chapter:{modifier}";
				q.Subject = $"Subject:{modifier}";
				q.Duration = 3600;
			q.Questions = new List<QUestionSql>();
			var mark = 100 / numOfQuestion;
			var lastMark = 100 % numOfQuestion;

			for (int j = 0; j <  numOfQuestion; j++)
			{
				QUestionSql qq = new QUestionSql();
				qq.Options = new List<QuestionOption<int>>();
				qq.Answares = new List<AnswareOption<int>>();
				qq.QuestionNumber = $"{q.GetQuestionNumber(j)}";
				qq.Question = $"Question Select {j}";
				qq.AnswerType = numOfQuestion > 1 ? LearningQA.Shared.Interface.AnswerType.AT_MULTI : LearningQA.Shared.Interface.AnswerType.AT_ONE;
				qq.Mark = mark;
				for (int k = 0; k < 4; k++)
				{
					QuestionOption<int> questionOption = new QuestionOption<int>();
					questionOption.TenantId = $"{qq.QuestionNumber}";
					questionOption.Content = $"I am {k}";
					
					qq.Options.Add(questionOption);
				}
				qq.Answares.Add(qq.Options.Last().ToAnswareOption<int>());
				q.Questions.Add(qq);
			}
			//if (lastMark > 0) q.Questions.Last().Mark = lastMark; 
			return q;
			
			
		}
		private QUestionSql CreateQuestion(int modifier, string questionNumber , int numOfAnswares)
		{
			QUestionSql qq = new QUestionSql();
			qq.Options = new List<QuestionOption<int>>();
			qq.Answares = new List<AnswareOption<int>>();
			qq.QuestionNumber = $"{questionNumber}";
			qq.Question = $"Question Select {questionNumber}";
			qq.AnswerType = numOfAnswares > 1 ? LearningQA.Shared.Interface.AnswerType.AT_MULTI : LearningQA.Shared.Interface.AnswerType.AT_ONE;
			for (int k = 0; k < 4; k++)
			{
				QuestionOption<int> questionOption = new QuestionOption<int>();
				questionOption.TenantId = $"{qq.QuestionNumber}";
				questionOption.Content = $"I am {k}";

				qq.Options.Add(questionOption);
			}
			qq.Answares.Add(qq.Options.ElementAt(0).ToAnswareOption<int>());
			
			qq.Supplements = new List<Supplement<int>>()
			{
				new Supplement<int>{Content = "see table 2", TenantId = $"{qq.QuestionNumber}" }
			};
			return qq;
		}
		private Test<QUestionSql,int> CreateTest(TestItem<QUestionSql,int> testItem, bool makeWrongAnsware)
		{
			var test = StartTest();
			test.TestItem = testItem;
			test.DateFinish = test.DateStart.AddSeconds(60);
			test.Answers = new List<Answer<int>>();
			foreach (var questionAnsware in test.TestItem.Questions)
			{
				var answare = new Answer<int>();
				answare.SelectedAnswer = new List<AnswareOption<int>>();
				//foreach(var goodAnsware in questionAnsware.Answares)
				//{
				//	var selectedAnsware = new AnswareOption<int>();
				//	selectedAnsware.TenantId = makeWrongAnsware ?  goodAnsware.TenantId + "W" : goodAnsware.TenantId;
				//	selectedAnsware.Content = goodAnsware.Content;
				//	answare.SelectedAnswer.Add(selectedAnsware);
					
				//}
				answare.IsCorrect = questionAnsware.IsCorrect(answare.SelectedAnswer);
				answare.IsCorrect = false;
				answare.TenantId = questionAnsware.QuestionNumber;
				test.Answers.Add(answare);

			}
			
			return test;
		}

		private Test<QUestionSql, int> CreateNewTest(TestItem<QUestionSql, int> testItem)
		{
			var test = StartTest();
			test.TestItem = testItem;
			test.DateFinish = test.DateStart.AddSeconds(60);
			test.Answers = new List<Answer<int>>();
			foreach (var questionAnsware in test.TestItem.Questions)
			{
				var answare = new Answer<int>();
				answare.SelectedAnswer = new List<AnswareOption<int>>();

				answare.TenantId = questionAnsware.QuestionNumber;
				answare.IsCorrect = false;
				answare.InAnswered = false;
				test.Answers.Add(answare);
				
			}

			return test;
		}
	}
}
