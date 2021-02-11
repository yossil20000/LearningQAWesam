using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Query
{
	public class GetExamByIdCommand : IRequestWrapper<ExamModel>
	{
		public int TestId { get; private set; }

		public GetExamByIdCommand(int testId)
		{
			TestId = testId;
		}
	}
	public class GetExamByIdCommandHandler : BaseDBContextHandler, IHandlerWrapper<GetExamByIdCommand, ExamModel>
	{
		public GetExamByIdCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<ExamModel>> Handle(GetExamByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
				//var result = await dbContext.Tests.Where(x => x.Id == request.TestId).FirstOrDefaultAsync();
				var result = await (from test in dbContext.Tests
							 where test.Id == request.TestId
							 join testItem in dbContext.TestItems
							 on test.TestItemId equals testItem.Id
							 select new ExamModel() { Test = test, Duration = testItem.Duration, Title=testItem.GeTestItemTitle()}).FirstOrDefaultAsync();
				if(result != null)
				{
					//ExamModel examModel = new ExamModel() { Test = result };
					return new SuccessResult<ExamModel>(result);
				}
				return new ServiceResult.NotFoundResult<ExamModel>($"{request.GetType().Name} Command Entity  testid:{request.TestId} not found");
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<ExamModel>(ex.Message);
			}
		}
	}
}
