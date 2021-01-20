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
using FluentAssertions.Common;
using System.Diagnostics;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{
		
		[Fact]
		public void TestShouHaveQuestionsld()
		{

			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var result = context.Tests.ToList();
					
				result.Count().Should().BeGreaterThan(0);

				
				foreach (var test in result)
				{
					var qIdGreterTheZero = test.Answers.Aggregate(true, (any, item) => any && item.TenantId != "");
					
					qIdGreterTheZero.Should().Be(true);
				}
				Trace.WriteLine($"Trace TestShouldGetAll Pass");
				
			}
		}
		[Fact]
		public void GetTest()
		{
			using(var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var test = context.Tests.Where(x => x.Id == 2).ToList();

			}
		}
		[Fact]
		public void FillTestWithSelectedAnsware()
		{
			using(var context = new LearningQAContext(_inMemoryDbOptions))
			{

				var testlist = context.Tests.Where(x => x.TestItemId >  0).ToList();
				foreach(var tests in testlist)
				{
					TestItem<QUestionSql, int> testItem = context.TestItems.Find(tests.TestItemId);
					tests.Should().NotBeNull();
					tests.Answers.Select(x => x.SelectedAnswer).ToList().ForEach(x => x.Clear());
					Trace.WriteLine(testItem.GeTestItemTitle());
					foreach (var question in testItem.Questions)
					{
						Trace.WriteLine($"Q{question.QuestionNumber} {question.Question}");
						Trace.WriteLine("Options");
						foreach (var options in question.Options)
						{
							Trace.WriteLine($"{options.TenantId} {options.Content}");
						}
						Trace.WriteLine("Right Answare");
						foreach (var rightAnsware in question.Options.Where(x => x.IsTrue))
						{
							Trace.WriteLine($"{rightAnsware.TenantId} {rightAnsware.Content}");
							tests.Answers.Where(x => x.TenantId == question.QuestionNumber).FirstOrDefault().SelectedAnswer.Add(new AnswareOption<int>() { Content = rightAnsware.Content, Id = 0, TenantId = rightAnsware.TenantId });
						}
						Trace.WriteLine("---------");



					}
					context.Update(tests);
					context.SaveChanges();
				}
				
				var result  = context.Tests.Find(1);
			}
		}
	}
}
