using LearningQA.Shared.DTO;
using LearningQA.Shared.MediatR.TestItem;

using MediatR;

using Microsoft.AspNetCore.Http;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Server.Infrasructure
{
	public class ExampleMediatRPipe<Tin, Tout> : IPipelineBehavior<Tin, Tout>
	{
		public ExampleMediatRPipe(IHttpContextAccessor httpContextAccessor)
		{
			HttpContextAccessor = httpContextAccessor;
			
		}

		public IHttpContextAccessor HttpContextAccessor { get; }

		public async Task<Tout> Handle(Tin request, CancellationToken cancellationToken, RequestHandlerDelegate<Tout> next)
		{
			var claims = HttpContextAccessor.HttpContext.User.Claims.ToList();
			if(request is BaseRequest br)
			{
				br.Name = "Yossi";
				br.UserId = 12345;
			}
			
			var result = await next();
			if(result is Result<IEnumerable<TestItemInfo>> testItemResponse )
			{
				
				testItemResponse.Message = "Good Job";
			}
			return result;
		}
	}
}
