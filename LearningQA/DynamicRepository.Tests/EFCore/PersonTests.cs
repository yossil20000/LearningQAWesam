using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicRepository.EFCore;
using FluentAssertions;
using System.Linq.Expressions;
using LearningQA.Shared.Entities;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{



		
		[Fact]
		public void PersomShouldGetById()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = subject.Get(PersonData.ElementAt(0).Id);
				result.Id.Should().Be(PersonData.ElementAt(0).Id);
				result.Name.Should().Be(PersonData.ElementAt(0).Name);
				//var result1 = context.TestItems.Include(x => x.Questions).ThenInclude(x => x.Options).Include(x => x.Questions).ThenInclude(x => x.Answares).ToList();
			}
		}
		[Fact]
		public async void PersonShouldGetByIdAsync()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = await subject.GetAsync(PersonData.ElementAt(1).Id);
				result.Id.Should().Be(PersonData.ElementAt(1).Id);
				result.Name.Should().Be(PersonData.ElementAt(1).Name);
			}
		}
		[Fact]
		public void PersonShouldInsertEntity()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = subject.Get(PersonData.ElementAt(1).Id);
				subject.Insert(new Person<int>() { Name = "Name inser", IdNumber = "Id" });
				var newIndex = context.SaveChanges();
				context.Person.Count().Should().Be(PersonData.Count() + 1);
				newIndex.Should().BeGreaterThan(0);
			}
		}
		[Fact]
		public void PersonShouldExceptionInsertEntity()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = subject.Get(PersonData.ElementAt(1).Id);
				subject.Insert(new Person<int>() { Name = "Name inser", IdNumber = "059828392" });
				var newIndex = context.SaveChanges();
				context.Person.Count().Should().Be(PersonData.Count() + 1);
				newIndex.Should().BeGreaterThan(0);
			}
		}
		[Fact]
		public void ShouldRemoveEntity()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var entity = context.Person.Where(p => p.Id == 1).FirstOrDefault();
				entity.Should().NotBeNull();
				subject.Delete(entity);
				context.SaveChanges();
				context.Person.Count().Should().Be(PersonData.Count() - 1);
				context.Person.Any(p => p.Id == 1).Should().BeFalse();
			}
		}
		[Fact]
		public void ShouldUpdateEntity()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var entity = context.Person.Where(p => p.Id == 2).FirstOrDefault();
				entity.Should().NotBeNull();
				entity.Name = "My Name is Updated";
				context.SaveChanges();
				entity.Name.Should().Be("My Name is Updated");
				entity = context.Person.Where(p => p.Id == 2).FirstOrDefault();
				entity.Name.Should().Be("My Name is Updated");

			}
		}
		[Fact]
		public void ShouldReturnAll()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = subject.ListAll();
				result.Should().NotBeNull();
				result.Count().Should().Be(PersonData.Count());

			}
		}
		[Fact]
		public void ShouldReturnAllDataOnGetQueryable()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				var subject = new PersonRepository(context);
				var result = subject.GetQueryable();
				result.Should().NotBeNull();
				result.Count().Should().Be(PersonData.Count());

			}
		}
		[Fact]
		public void ShouldReturnFilterDataWhenGlobalFilterIsSet()
		{
			using (var context = new LearningQAContext(_inMemoryDbOptions))
			{
				//context.ChangeTracker.LazyLoadingEnabled = false;
				var subject = new PersonRepository(context);
				subject.SetGlobalFilter(t => t.IdNumber == SecondId.ToString());
				var result = subject.GetQueryable();
				result.Should().NotBeNull();
				result.Count().Should().Be(1);
				Person<int> personData = PersonData.Where(t => t.IdNumber == SecondId.ToString()).FirstOrDefault();
				Person<int> personSql = result.FirstOrDefault();
				personSql.Name.Should().BeEquivalentTo(personData.Name);

			}
		}
	}
}
