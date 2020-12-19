using Castle.Core.Logging;

using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.TestItem.Command;
using LearningQA.Shared.MediatR.TestItem.Query;

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
	public class TestItemController : ApiControllerBase
	{
		public TestItemController(ILogger<ApiControllerBase> logger,IMediator mediator) :base(logger,mediator)
		{

		}
		/// <summary>
		/// Create New TestItem
		/// </summary>
		/// <param name="newTestItem"></param>
		/// <returns>Suceed if id > 0 </returns>
		[HttpPost]
		//https://localhost:44335/TestItem?Category=Yossi&Subject=Pop&Chapter=1&Version=12&NumOfQuestions=50
		public async Task<IActionResult> CreatEmptyTest([FromQuery] TestItemInfo newTestItem)
		{
			var result = await _mediator.Send(new CreateNewTestItemCommand(newTestItem), cancellationToken);
			return Ok(newTestItem);
		}

		[HttpGet]
		public async Task<IActionResult> TestItems()
		{
			List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
			CancellationToken cancellationToken = new CancellationToken();
			testItemInfos.Add(new TestItemInfo() { Id = 1,Subject="S1",Category="C1",Chapter="c1",NumOfQuestions=31, Version=1});
			testItemInfos.Add(new TestItemInfo() { Id = 2, Subject = "S2", Category = "C2", Chapter = "c2", NumOfQuestions = 32, Version = 2 });
			var result  = await _mediator.Send(new TestItemsInfoQuery(),cancellationToken);
			return Ok(result);
		}
		[HttpPut]
		public async Task<IActionResult> UpdateTestItem([FromBody] TestItem<QUestionSql,int> testItem)
		{
			var result = _mediator.Send(new UpdateTestItemCommand(testItem), cancellationToken);
			return Ok(true);
		}
	}
}
