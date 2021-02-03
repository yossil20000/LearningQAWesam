using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.Test.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class PersonTest
	{
		public int Title { get; set; }

		public DateTime DateStart { get; set; }
		public DateTime DateFinish { get; set; }
	}

	public class ExamInfoModel
	{
		public int PersonId { get; set; }
		public int TestId { get; set; }
		public int TestItemId { get; set; }
		public string IdNumber { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateFinish { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public int Mark { get; set; }
		public ExamInfoModel()
		{

		}
		public ExamInfoModel(Test<QUestionSql,int> test)
		{

		}
		public ExamInfoModel(Person<int> person)
		{
			PersonId = person.Id;
			Name = person.Name;
			IdNumber = person.IdNumber;
			foreach (var p in person.Tests)
			{
				//PersonTests.Add(new PersonTest()
				//{
				//	DateFinish = p.DateFinish,
				//	DateStart = p.DateStart,
				//	Title = p.TestItemId
				//});
			};
		}
		public ExamInfoModel(Test<QUestionSql, int> test, Person<int> person, TestItem<QUestionSql, int> testItem)
		{
			TestId = test.Id;
			DateStart = test.DateStart;
			DateFinish = test.DateFinish;
			Title = $"{ testItem.Category}/{testItem.Subject}/{testItem.Chapter}/{testItem.Version}";
			TestItemId = testItem.Id;
			PersonId = person.Id;
			Name = person.Name;
			IdNumber = person.IdNumber;
			Mark = test.Mark ;
		}
		
		
		
		public static Expression<Func<Person<int>, ExamInfoModel>> Projection
		{
			get
			{

				return projection => new ExamInfoModel(projection);
				
				//return projection => new ExamInfoModel()
				//{
				//	PersonId = projection.Id,
				//	Name = projection.Name,
				//	IdNumber = projection.IdNumber,
					
				//	DateFinish = projection.Tests == null ? DateTime.Now.AddYears(-2000) : projection.Tests.FirstOrDefault().DateFinish,
				//	DateStart = projection.Tests == null ? DateTime.Now.AddYears(-2000) : projection.Tests.FirstOrDefault().DateStart,
				//	TestItemId = projection.Tests == null ? 0 : projection.Tests.FirstOrDefault().TestItemId
				//};
			}
		}
		public static Expression<Func<Test<QUestionSql,int>, ExamInfoModel>> ProjectFromTest
		{
			get
			{
				return Projection => new ExamInfoModel(Projection);
			}
		}
		
	}
}
