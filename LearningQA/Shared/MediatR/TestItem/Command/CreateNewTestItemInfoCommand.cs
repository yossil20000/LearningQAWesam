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
	public class CreateNewTestItemCommand : IRequest<TestItem<QUestionSql,int>>
	{
		public TestItem<QUestionSql,int> _testItem;
		public CreateNewTestItemCommand(TestItem<QUestionSql, int> testItem)
		{
			_testItem = testItem;
		}
	}

	public class CreateNewTestItemCommandHandler : IRequestHandler<CreateNewTestItemCommand,TestItem<QUestionSql,int>>
	{
		private readonly LearningQAContext dbContext;
		ILogger<CreateNewTestItemCommandHandler> logger;

		public CreateNewTestItemCommandHandler(LearningQAContext context, ILogger<CreateNewTestItemCommandHandler> logger)
		{
			dbContext = context;
			this.logger = logger;
		}

		public async  Task<TestItem<QUestionSql, int>> Handle(CreateNewTestItemCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request._testItem.Id == 0)
				{
					dbContext.TestItems.Add(request._testItem);
					var result = await dbContext.SaveChangesAsync();
					if (result > 0)
					{
						return await dbContext.TestItems.FindAsync(result);
					}
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
