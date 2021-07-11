using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	public class LearningQAContext : DbContext
	{
		public LearningQAContext() { }
		public LearningQAContext( DbContextOptions<LearningQAContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person<int>>().HasIndex(person => person.IdNumber).IsUnique();
			modelBuilder.Entity<TestItem<QUestionSql, int>>().HasIndex(testItem => new { testItem.Category, testItem.Chapter, testItem.Subject, testItem.Version }).IsUnique();
			
			//modelBuilder.Entity<QUestionSql>().HasIndex(qUestionSql => qUestionSql.QuestionNumber).IsUnique();
		}
		public DbSet<Person<int>> Person { get; set; }
		public DbSet<AnswareOption<int>> AnswareOptions { get; set; }
		public DbSet<QuestionOption<int>> QuestionOptions { get; set; }
		public DbSet<Answer<int>> Answers { get; set; }

		public DbSet<Supplement<int>> Supplements { get; set; }

		public DbSet<Category<int>> Categories { get; set; }

		public DbSet<TestItem<QUestionSql,int>> TestItems { get; set; }
		public DbSet<QUestionSql> QUestionSqls { get; set; }
		public DbSet<Test<QUestionSql, int>> Tests { get; set; }
		
		
	}
	

	
}
