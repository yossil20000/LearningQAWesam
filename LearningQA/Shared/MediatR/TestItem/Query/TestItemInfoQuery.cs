using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.TestItem.Query
{
	public class TestItemInfoQuery : BaseRequest, IRequestWrapper<TestItemInfo>
	{
		public TestItemInfoQuery(int testItemId)
		{
			TestItemId = testItemId;
		}

		public int TestItemId { get; private set; }


	}

	public class TestItemInfoQueryHandler : IHandlerWrapper<TestItemInfoQuery, TestItemInfo>
	{
		private readonly LearningQAContext dbContext;
		public TestItemInfoQueryHandler(LearningQAContext context)
		{
			dbContext = context;
		}

		public async  Task<Result<TestItemInfo>> Handle(TestItemInfoQuery request, CancellationToken cancellationToken)
		{
			//Here we can user the piple data
			//request.UserId .... 
			var testItemInfo = await dbContext.TestItems
				.Where(x => x.Id == request.TestItemId)

				.Select( x => new TestItemInfo()
			{
				Id = x.Id,
				Category = x.Category,
				Subject = x.Subject,
				Chapter = $"{x.Chapter}",
				Version = x.Version,
				NumOfQuestions = x.Questions == null ? 0 : x.Questions.Count
			}).FirstOrDefaultAsync();

			return await Task.FromResult(new SuccessResult<TestItemInfo>(testItemInfo));
		}

		
	}
}
