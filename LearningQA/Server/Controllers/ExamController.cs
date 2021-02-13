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
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;
using System.Threading;

namespace LearningQA.Server.Controllers
{
    public class ExamController : ApiControllerBase
    {
        public ExamController(ILogger<ApiControllerBase> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
        //https://localhost:44335/api/Exam/Get?testId=2
        [HttpGet(Name = "/GetExamById/{testId}")]
        [SwaggerOperation(
            Summary = "Get Exam By Id",
            Description = "Get An Already Done Exam by it's Id",
            OperationId = "Exam.Get",
            Tags = new[] { "ExamEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "ExamModel", typeof(ExamModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, "ExamModel", typeof(ExamModel))]
        public async Task<ActionResult<ExamModel>> Get(int testId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetExamByIdCommand(testId), cancellationToken);
            return this.FromResult(result);
        }
        [HttpGet(Name = "/GetExamList")]
        [SwaggerOperation(
            Summary = "Get All Done Exam List",
            Description = "Get An Already Done Exams by Category,Subject and Chapter. Person is option",
            OperationId = "Exam.Get",
            Tags = new[] { "ExamEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "ExamModel", typeof(ExamModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, "ExamModel", typeof(ExamModel))]
        public async Task<ActionResult<IQueryable<ExamInfoModel>>> GetExamList([FromQuery] string category, [FromQuery] string subject, [FromQuery] string chapter, [FromQuery] int personId)
        {
            var result = await _mediator.Send(
                new GetExamListQuery(
                new ExamListRequest()
                {
                    PersonId = personId,
                    TestItemInfo = new TestItemInfo()
                    {
                        Category = category,
                        Subject = subject,
                        Chapter = chapter
                    }
                }
            ));
            return this.FromResult(result);
        }
        [HttpGet(Name = "/PersonsInfo")]
        public async Task<ActionResult<PersonInfoModel[]>> PersonsInfo()
        {
            var result = await _mediator.Send(new GetAllPersonsQuery());
            return this.FromResult(result);
        }

        [HttpPut(Name = "/UpdatePerson")]
        public async Task<ActionResult<PersonInfoModel>> UpdatePerson([FromBody] PersonInfoModel person)
        {
            var result = await _mediator.Send(new UpdatePersonCommand(person));
            return this.FromResult(result);
        }
        [HttpPut(Name = "/UpdateExam")]
        public async Task<ActionResult<bool>> UpdateExam([FromBody] UpdateExamCommand updateExamCommand)
        {
            var result = await _mediator.Send(updateExamCommand);

            return this.FromResult(result);

        }

        [HttpPost(Name = "/CreateExam")]
        public async Task<ActionResult<ExamModel>> CreateExam(int personId, int testItemId)
        {
            var result = await _mediator.Send(new CreateExamCommand(testItemId, personId));
            return this.FromResult(result);
        }

        [HttpPost(Name = "/CreateExamByTitle")]
        public async Task<ActionResult<ExamModel>> CreateExamByTitle(TestItemInfo testItemInfo)
        {
            int version = 0;
            var result = await _mediator.Send(new CreateExamByTitlesCommand(new TestItemInfo() { Category = testItemInfo.Category.ToString(), Subject = testItemInfo.Subject.ToString(), Chapter = testItemInfo.Chapter.ToString(), Version = version }));
            return result.Data;
        }

        [HttpDelete(Name ="DeleteExamById/{id}")]

        public async Task<ActionResult<int>> DeleteExamById(int id)
        {
            var result = await _mediator.Send(new DeleteExamByIdCommand(id));
            return this.FromResult(result);
        }
       
    }
}
