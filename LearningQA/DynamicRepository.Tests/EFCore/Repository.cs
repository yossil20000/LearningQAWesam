using DynamicRepository.EFCore;

using LearningQA.Shared.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRepository.Tests.EFCore
{
	public partial class InMemoryLearningQATest
	{
		private class PersonRepository : Repository<int, Person<int>>, IRepository<int, Person<int>>
		{
			public PersonRepository(DbContext context) : base(context)
			{

			}

			internal void SetGlobalFilter(Expression<Func<Person<int>, bool>> filter)
			{
				this.HasGlobalFilter(filter);
			}
		}

		public class TestItemRepository : Repository<int, TestItem<QUestionSql, int>>, IRepository<int, TestItem<QUestionSql, int>>
		{
			public TestItemRepository(DbContext context) : base(context)
			{
			}
		}

		public class TestRepository : Repository<int, Test<QUestionSql, int>>, IRepository<int, Test<QUestionSql, int>>
		{
			public TestRepository(DbContext context) : base(context)
			{
			}
		}
	}
}
