using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Extentions;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Command
{
	public class CreateExamCommand : BaseRequest, IRequestWrapper<ExamModel>
	{
		public int TestItemId { get; private set; }
		public int PersonId { get; private set; }
		public CreateExamCommand(int testItemId , int personId)
		{
			TestItemId = testItemId;
			PersonId = personId;
		}



	}
	public class CreateExamCommandHandler : BaseDBContextHandler, IHandlerWrapper<CreateExamCommand, ExamModel>
	{
		public CreateExamCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<ExamModel>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
		{
			try
			{
				
					//dbContext.ChangeTracker.Clear();
					var testItem = dbContext.TestItems.Find(request.TestItemId);
					
					if(request.PersonId == 0)
					{
						
						
					}
					else
					{
						
					}


					ExamModel examModel = new ExamModel();
					int result = 0;
					var person = dbContext.Person.Find(request.PersonId);
					if (examModel.FillTest(testItem))
					{
						person.Tests.Add(examModel.Test);
						dbContext.Update(person);
						result = await dbContext.SaveChangesAsync();
					}
					
					
					
					if (result > 0)
						return await Task.FromResult(new SuccessResult<ExamModel>(examModel));
					else
						return await Task.FromResult(new ServiceResult.InvalidResult<ExamModel>("") { Message = "CreateExamCommand Failed On Save" });
				

			}
			catch (Exception ex)
			{
				return await Task.FromResult(new UnexpectedResult<ExamModel>(ex.Message) { Message = "CreateExamCommand Failed" });
			}


		}
	}
}
