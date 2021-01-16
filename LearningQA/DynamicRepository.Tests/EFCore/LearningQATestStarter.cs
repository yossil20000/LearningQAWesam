using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicRepository.EFCore;
using FluentAssertions;
using System.Linq.Expressions;
using LearningQA.Shared.Entities;
using DnsClient.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{
		protected bool _useSqlight = true;
		protected readonly int FirstId = 1;
		protected readonly int SecondId = 2;

		private readonly DbContextOptions<LearningQAContext> _inMemoryDbOptions;

		protected readonly IEnumerable<Person<int>> PersonData;
		protected IEnumerable< TestItem<QUestionSql, int>> TestItemsData;
		protected ICollection<Test<QUestionSql, int>> TestsData = new List<Test<QUestionSql,int>>();
	

		
		public InMemoryLearningQATest()
		{
			
			if(_useSqlight)
			{
				_inMemoryDbOptions = new DbContextOptionsBuilder<LearningQAContext>()
				.UseLazyLoadingProxies()
				.UseSqlite<LearningQAContext>(@"Data Source=.\LearningQAContext.db")
				.LogTo(Console.WriteLine , new[] { DbLoggerCategory.Database.Command.Name },(Microsoft.Extensions.Logging.LogLevel)LogLevel.Information,DbContextLoggerOptions.SingleLine | DbContextLoggerOptions.LocalTime)
				.Options;
			}
			else
			{
				_inMemoryDbOptions = new DbContextOptionsBuilder<LearningQAContext>()
				.UseLazyLoadingProxies()
				.UseInMemoryDatabase<LearningQAContext>("LearningQAContext")
				.Options;
			}
			
			using(var context = new LearningQAContext(_inMemoryDbOptions))
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
				CreateTestItemEntity();
				context.TestItems.AddRange(TestItemsData);
				context.SaveChanges();
				PersonData =  CreatePersons();
				FillTestEntity();
				FillTestEntity();


				context.AddRange(TestsData);
				Persons.ElementAt(1).Tests.Add(TestsData.ElementAt(0));
				context.Person.AddRange(Persons);

				context.SaveChanges();
			}
		}
		
		
		
	}
}
