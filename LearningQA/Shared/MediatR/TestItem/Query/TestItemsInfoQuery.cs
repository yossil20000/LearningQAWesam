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
	public class TestItemsInfoQuery : BaseRequest, IRequestWrapper<IEnumerable<TestItemInfo>>
	{
	}

	public class TestItemsInfoQueryHandler : IHandlerWrapper<TestItemsInfoQuery, IEnumerable<TestItemInfo>>
	{
		private readonly LearningQAContext dbContext;
		public TestItemsInfoQueryHandler(LearningQAContext context)
		{
			dbContext = context;
		}

		public async  Task<Result<IEnumerable<TestItemInfo>>> Handle(TestItemsInfoQuery request, CancellationToken cancellationToken)
		{
			//Here we can user the piple data
			//request.UserId .... 
			var testItemsInfo = await dbContext.TestItems.Select(x => new TestItemInfo()
			{
				Id = x.Id,
				Category = x.Category,
				Subject = x.Subject,
				Chapter = $"{x.Chapter}",
				Version = x.Version,
				NumOfQuestions = x.Questions == null ? 0 : x.Questions.Count
			}).ToListAsync();

			return await Task.FromResult(new SuccessResult<IEnumerable<TestItemInfo>>(testItemsInfo));
		}

		
	}
}
