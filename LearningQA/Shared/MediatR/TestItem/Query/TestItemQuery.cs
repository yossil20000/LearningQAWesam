using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.TestItem.Query
{
	
	public class TestItemQuery : BaseRequest, IRequestWrapper<TestItem<QUestionSql,int>>
	{
		public TestItemInfo TestItemInfo { get; set; }
	}

	public class TestItemQueryHandler : IHandlerWrapper<TestItemQuery, TestItem<QUestionSql, int>>
	{
		private readonly LearningQAContext dbContext;
		public TestItemQueryHandler(LearningQAContext context)
		{
			dbContext = context;
		}

		public async  Task<Result<TestItem<QUestionSql, int>>> Handle(TestItemQuery request, CancellationToken cancellationToken)
		{
			var testItem = await dbContext.TestItems.Where(x =>
		   x.Category == request.TestItemInfo.Category &&
		   x.Subject == request.TestItemInfo.Subject &&
		   x.Chapter == request.TestItemInfo.Chapter).FirstOrDefaultAsync();
			if (testItem == null)
			{
				testItem = new TestItem<QUestionSql, int>();

			}

			return await Task.FromResult(new SuccessResult<TestItem<QUestionSql, int>>(testItem));
		}

		
	}
}
