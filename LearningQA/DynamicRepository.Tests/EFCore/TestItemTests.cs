using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicRepository.EFCore;
using FluentAssertions;
using System.Linq.Expressions;
using LearningQA.Shared.Entities;
using System.Runtime.CompilerServices;
using System.Linq.Dynamic.Core;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{
		private IEnumerable<Person<int>> Persons;
		[Fact]
		public void TestItemsShouldGetAll()
		{
			
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				
				var subject = new PersonRepository(context);
				var result = context.TestItems
					.Include(x => x.Questions).ThenInclude(x => x.Options)
					.ToList();
				result.Count().Should().Be(TestItemsData.Count());
				foreach(var item in result)
				{
					int index = 0;
					item.Questions.Count().Should().Be(TestItemsData.ElementAt(index).Questions.Count());
				}
				
			}
		}
		public TestItem<QUestionSql,int> TestItemGetById(int id)
		{
			TestItem<QUestionSql, int> result;
			//using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var context = new LearningQAContext(_inMemoryDbOptions);
				var testItemSubject = new TestItemRepository(context);
				var testItem = testItemSubject.Get(1);

				result = testItem;

			}
			return result;
		}
		[Fact]
		public void TestItemShouldGetById()
		{ 
			using(var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var testItemSubject = new TestItemRepository(context);
				var person = subject.Get(2);
				var testItem = testItemSubject.Get(1);
				//var testItem = TestItemGetById(1);
				testItem.Should().NotBeNull();


			}
		}
		[Fact]
		public void TestItemShouldTestInsert()
		{
			using(var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				subject.HasGlobalFilter(x => x.IdNumber == SecondId.ToString());
				var result = subject.GetQueryable().FirstOrDefault();
				//var test = context.Tests.Where(x => x.Person.IdNumber == result.FirstOrDefault().IdNumber).ToList();
				var testSubject = new TestItemRepository(context);
				var test = CreateTestItem(4, 5, 1);
				
				testSubject.Insert(test);
				var newIndex = context.SaveChanges();
				newIndex.Should().BeGreaterThan(0);

				



			}
		}

		[Fact]
		public void TestItemShouldExceptionTestInsert()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				subject.HasGlobalFilter(x => x.IdNumber == SecondId.ToString());
				var result = subject.GetQueryable().FirstOrDefault();
				//var test = context.Tests.Where(x => x.Person.IdNumber == result.FirstOrDefault().IdNumber).ToList();
				var testSubject = new TestItemRepository(context);
				var test = CreateTestItem(4, 5, 1);
				
				testSubject.Insert(test);
				var newIndex = context.SaveChanges();
				newIndex.Should().BeGreaterThan(0);
				test = CreateTestItem(4, 5, 1);

				testSubject.Insert(test);
				newIndex = context.SaveChanges();




			}
		}
		[Fact]
		public void TestItemShouldUpdate()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var testItemSubject = new TestItemRepository(context);
				var testItem = TestItemGetById(1);
				//testItem = testItemSubject.Get(1);
				testItem.Duration = 9999;
				testItemSubject.Update(testItem);
				var newIndex = context.SaveChanges();
				testItem = testItemSubject.Get(1);

				testItem.Duration.Should().Be(9999);
				testItem.Questions.Should().NotBeNull();




			}
		}
		[Fact]
		public void TestItemShouldInserQuestion()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var testItemSubject = new TestItemRepository(context);
				//var testItem = TestItemGetById(1);
				var testItem = context.TestItems.Find(1);

				//var testItem = testItemSubject.Get(1);
				QUestionSql question = CreateQuestion(5, testItem.GetQuestionNumber(5), 1);
				testItem.Questions.Add(question);
				testItemSubject.Update(testItem);
				var newIndex = context.SaveChanges();
				testItem = testItemSubject.Get(1);

				testItem.Questions.Should().NotBeNull();
				testItem.Questions.Count().Should().Be(5);



			}
		}

		[Fact]
		public void TestItemDeleteTest()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var testItemSubject = new TestItemRepository(context);
				var testRepository = new TestRepository(context);
				//var testItem = TestItemGetById(1);
				var testItem = testItemSubject.Get(1);
				
				bool deleteTestItem = DeleteTestItem(context, testItem.Id);

				deleteTestItem.Should().BeTrue();

				deleteTestItem = DeleteTestItem(context, testItem.Id);
				deleteTestItem.Should().BeFalse();

			}
			

		}
		[Fact]
		public void TestItemShouldDeleteAllTestsIn()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var testItemSubject = new TestItemRepository(context);
				var testRepository = new TestRepository(context);
				//var testItem = TestItemGetById(1);
				var testItem = testItemSubject.Get(1);
				
				var testInTest = context.Tests.Where(x => x.TestItemId == 1).ToList();
				foreach(var test in testInTest)
				{
					bool result = DeleteTest(context, test.Id);
					result.Should().BeTrue();
				}
				testInTest = context.Tests.Where(x => x.TestItemId == 1).ToList();
				testInTest.Should().BeEmpty();



			}

			
		}
		private bool DeleteTest(LearningQAContext context , int testId)
		{
			try
			{
				var testRepository = new TestRepository(context);
				var testToDelete = testRepository.Get(testId);
				testToDelete.Should().NotBeNull();
				var answareToDelete = testToDelete.Answers;
				var answareOptionsToDelete = answareToDelete.SelectMany(x => x.SelectedAnswer).ToList();
				context.AnswareOptions.RemoveRange(answareOptionsToDelete);
				context.Answers.RemoveRange(answareToDelete);
				context.Tests.RemoveRange(testToDelete);
				var newIndex = context.SaveChanges();
				return newIndex > 0;
			}
			catch(Exception ex)
			{
				return false;
			}
			
		}

		private  bool DeleteTestItem(LearningQAContext context, int testItemId)
		{
			try
			{
				var testItemRepository = new TestItemRepository(context);
				var testItem = testItemRepository.Get(testItemId);
				var testsToRemove = context.Tests.Where(x => x.TestItemId == testItem.Id);
				bool delteteTestResult = true;
				foreach (var test in testsToRemove)
				{
					delteteTestResult &= DeleteTest(context, test.Id);
				}
				context.SaveChanges();
				var questionsToRemove = testItem.Questions;
				
				var questionOptionRemove = questionsToRemove.SelectMany(x => x.Options);
				
				context.QuestionOptions.RemoveRange(questionOptionRemove);
				context.QUestionSqls.RemoveRange(questionsToRemove);
				context.SaveChanges();


				context.TestItems.Remove(testItem);

				context.SaveChanges();

				testItem = testItemRepository.Get(testItemId);
				testItem.Should().BeNull();
				return delteteTestResult;
			}
			catch(Exception ex)
			{
				return false;
			}
			

		}
	}
}
