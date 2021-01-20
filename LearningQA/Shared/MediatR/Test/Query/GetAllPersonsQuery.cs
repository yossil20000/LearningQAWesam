using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Query
{
	public class GetAllPersonsQuery : IRequestWrapper<PersonInfoModel[]>
	{

	}

	[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public class GetAllPersonsHandler : BaseDBContextHandler, IHandlerWrapper<GetAllPersonsQuery,PersonInfoModel[]>
	{
		public GetAllPersonsHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<PersonInfoModel[]>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var result = await dbContext.Person.AsNoTracking().ToArrayAsync();
				PersonInfoModel[] personInfoModels = new PersonInfoModel[result.Count()];
				for(var i=0; i < result.Count();i++)
				{
					PersonInfoModel p = new()
					{
						Id = result[i].Id,
						IdNumber = result[i].IdNumber,
						Name = result[i].Name,
						Email = result[i].Email,
						Phone = result[i].Phone,
						Address = result[i].Address

					};
					personInfoModels[i] = p;
				}
				return new SuccessResult<PersonInfoModel[]>(personInfoModels);
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<PersonInfoModel[]>(ex.Message);
			}
		}

		private string GetDebuggerDisplay()
		{
			return ToString();
		}
	}
}
