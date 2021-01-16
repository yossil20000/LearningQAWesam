﻿using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem;
using LearningQA.Shared.MediatR.TestItem.Command;

using MediatR;

using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Command
{
	public class UpdateExamCommand : BaseRequest, IRequestWrapper<bool>
	{
		public Test<QUestionSql, int> Test { get; private set; }
		public UpdateExamCommand(Test<QUestionSql,int> test)
		{
			Test = test;
		}

		
		
	}
	public class UpdateExamCommandHandler : BaseDBContextHandler, IHandlerWrapper<UpdateExamCommand, bool>
	{
		public UpdateExamCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<bool>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
		{
			try
			{
				dbContext.Tests.Update(request.Test);
				var result = await dbContext.SaveChangesAsync();
				if (result > 0)
					return await Task.FromResult(new SuccessResult<bool>(true));
				else
					return await Task.FromResult(new ServiceResult.InvalidResult<bool>("") { Message = "UpdateExamCommand Failed On Save" });
			}
			catch(Exception ex)
			{
				return await Task.FromResult(new UnexpectedResult<bool>(ex.Message) { Message = "UpdateExamCommand Failed" });
			}
			

		}
	}
}
