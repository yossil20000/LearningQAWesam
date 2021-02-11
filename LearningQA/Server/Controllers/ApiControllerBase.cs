using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public  class ApiControllerBase : ControllerBase
	{
		public readonly ILogger<ApiControllerBase> _logger;
		public readonly IMediator _mediator;
		public readonly IMapper _mapper;
		public CancellationToken cancellationToken = new CancellationToken();

		public ApiControllerBase(ILogger<ApiControllerBase> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;
		}
	}
}
