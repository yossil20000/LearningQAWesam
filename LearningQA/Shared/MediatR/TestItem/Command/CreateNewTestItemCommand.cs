using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.TestItem.Command
{
	public class CreateNewTestItemInfoCommand : IRequest<TestItem<QUestionSql, int>>
	{
		public TestItemInfo _testItemInfo;
		public CreateNewTestItemInfoCommand(TestItemInfo testItemInfo)
		{
			_testItemInfo = testItemInfo;
		}
	}

	public class CreateNewTestItemInfoCommandHandler : IRequestHandler<CreateNewTestItemInfoCommand, TestItem<QUestionSql, int>>
	{
		private readonly LearningQAContext dbContext;
		ILogger<CreateNewTestItemInfoCommandHandler> logger;

		public CreateNewTestItemInfoCommandHandler(LearningQAContext context, ILogger<CreateNewTestItemInfoCommandHandler> logger)
		{
			dbContext = context;
			this.logger = logger;
		}

		public async  Task<TestItem<QUestionSql, int>> Handle(CreateNewTestItemInfoCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var testItem = new TestItem<QUestionSql, int>();
				testItem.Category = request._testItemInfo.Category;
				testItem.Chapter = request._testItemInfo.Chapter;
				testItem.Subject = request._testItemInfo.Subject;
				testItem.Version = request._testItemInfo.Version;
				dbContext.TestItems.Add(testItem);
				var result = await dbContext.SaveChangesAsync();
				if(result > 0)
				{
					return await dbContext.TestItems.FindAsync(result);
				}
				

			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return await Task.FromResult(new TestItem<QUestionSql, int>());
		}

		
	}
}
