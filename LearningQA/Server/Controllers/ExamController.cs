using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.Test.Command;
using LearningQA.Shared.MediatR.Test.Query;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ServiceResult.ApiExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace LearningQA.Server.Controllers
{
	public class ExamController : ApiControllerBase
	{
		public ExamController(ILogger<ApiControllerBase> logger, IMediator mediator) : base(logger, mediator)
		{
		}

		[HttpGet(Name ="/GetExamById")]
		//https://localhost:44335/api/Exam/Get?testId=2
		public async Task<IActionResult> Get(int testId)
		{
			var result = await _mediator.Send(new GetExamByIdCommand(testId));
			return this.FromResult(result);
		}
		[HttpGet(Name ="/GetExamInfo")]
		public async Task<IActionResult> GetExamInfo(int personId)
		{
			var result = await _mediator.Send(new GetExamsInfoQuery() { PersonId = personId});
			return this.FromResult(result);
		}
		[HttpGet(Name = "/PersonsInfo")]
		public async Task<IActionResult> PersonsInfo()
		{
			var result =  await _mediator.Send(new GetAllPersonsQuery());
			return this.FromResult(result);
		}

		[HttpPut(Name ="/UpdatePerson")]
		public async Task<IActionResult> UpdatePerson([FromBody] PersonInfoModel person)
		{
			var result = await _mediator.Send(new UpdatePersonCommand(person));
			return this.FromResult(result);
		}
		[HttpPost(Name = "/CreateExam")]
		public async Task<IActionResult> CreateExam(int  personId , int testItemId)
		{
			var result = await _mediator.Send(new CreateExamCommand(testItemId,personId));
			return this.FromResult(result);
		}

		[HttpPut(Name = "/CreateExamByTitle")]
		public async Task<ActionResult<ExamModel>> CreateExamByTitle(TestItemInfo testItemInfo)
		{
			//string category = "", subject = "", chapter = "";
			int version = 0;
			//var result = await _mediator.Send(new CreateExamByTitlesCommand(new TestItemInfo() { Category = category, Subject = subject, Chapter = chapter, Version = version }));
			//var result = await _mediator.Send(new CreateExamByTitlesCommand(new TestItemInfo() { Category = category.ToString(), Subject = subject.ToString(), Chapter = chapter.ToString(), Version = version }));
			var result = await _mediator.Send(new CreateExamByTitlesCommand(new TestItemInfo() { Category = testItemInfo.Category.ToString(), Subject = testItemInfo.Subject.ToString(), Chapter = testItemInfo.Chapter.ToString(), Version = version }));
			//return this.FromResult(result);
			return result.Data;
		}

		[HttpPost(Name = "/UpdateExam")]
		public async Task<IActionResult> UpdateExam([FromBody] UpdateExamCommand updateExamCommand)
		{
			var result = await _mediator.Send(updateExamCommand);

			return this.FromResult(result);

		}
	}
}
