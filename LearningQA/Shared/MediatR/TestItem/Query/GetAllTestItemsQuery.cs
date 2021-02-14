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

namespace LearningQA.Shared.MediatR.TestItem.Query
{
	/// <summary>
	/// Get All TestItems 
	/// return List<TestItem<QUestionSql, int>>
	/// </summary>
	public class GetAllTestItemsQuery : BaseRequest, IRequestWrapper<List<TestItem<QUestionSql, int>>>
	{
		public GetAllTestItemsQuery()
		{
		}
	}
	public class GetAllTestItemsQueryHandler : BaseDBContextHandler, IHandlerWrapper<GetAllTestItemsQuery, List<TestItem<QUestionSql, int>>>
	{
		public GetAllTestItemsQueryHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async  Task<Result<List<TestItem<QUestionSql, int>>>> Handle(GetAllTestItemsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var result = await dbContext.TestItems.ToListAsync();
				return new SuccessResult<List<TestItem<QUestionSql, int>>>(result);
			}
			catch (Exception ex)
			{
				return new UnexpectedResult<List<TestItem<QUestionSql, int>>>(ex.Message);
			}
		}
	}
}
