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
	public class BaseDBContextHandler
	{
		protected LearningQAContext dbContext;
		protected ILogger<BaseDBContextHandler> logger;
		public BaseDBContextHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger)
		{
			dbContext = context;
			this.logger = logger;
		}
	}
	public class UpdateTestItemCommand : IRequest<bool>
	{
		public TestItem<QUestionSql,int> TestItem { get; set; }
		public UpdateTestItemCommand(TestItem<QUestionSql,int> testItem)
		{
			TestItem = testItem;
		}

	}
	public class UpdateTestItemCommandHandler : BaseDBContextHandler, IRequestHandler<UpdateTestItemCommand, bool>
	{
		public UpdateTestItemCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{

		}
		public async Task<bool> Handle(UpdateTestItemCommand request, CancellationToken cancellationToken)
		{
			try
			{
				dbContext.TestItems.Update(request.TestItem);
				var result = await dbContext.SaveChangesAsync();
				return await Task.FromResult(result > 0 ? true:false);
			}
			catch(Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return await Task.FromResult(false);
		}
	}
}
