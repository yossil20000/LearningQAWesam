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
	public class CreateRangeTestItemCommand : IRequest<int>
	{
		public List<TestItem<QUestionSql, int>> _testItems;
		public bool CreateNewDatabase { get; set; } = false;
		public CreateRangeTestItemCommand(List<TestItem<QUestionSql, int>> testItems)
		{
			_testItems = testItems;
		}
	}

	public class CreateRangeTestItemCommandHandler : IRequestHandler<CreateRangeTestItemCommand, int>
	{
		private readonly LearningQAContext dbContext;
		ILogger<CreateRangeTestItemCommand> logger;

		public CreateRangeTestItemCommandHandler(LearningQAContext context, ILogger<CreateRangeTestItemCommand> logger)
		{
			dbContext = context;
			this.logger = logger;
		}

		public async  Task<int> Handle(CreateRangeTestItemCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request.CreateNewDatabase)
				{
					dbContext.Database.EnsureDeleted();
					dbContext.Database.EnsureCreated();
				}
				dbContext.TestItems.AddRange(request._testItems);
				var result = await dbContext.SaveChangesAsync();
				return result;
				

			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				return 0;
			}
			
		}

		
	}
}
