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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Command
{
	public class CreateExamByTitlesCommand : BaseRequest, IRequestWrapper<ExamModel>
	{
		public TestItemInfo TestItemInfo { get; private set; }
		public CreateExamByTitlesCommand(TestItemInfo testItemInfo)
		{
			TestItemInfo = testItemInfo;
		}



	}
	public class CreateExamByTitlesCommandHandler : BaseDBContextHandler, IHandlerWrapper<CreateExamByTitlesCommand, ExamModel>
	{
		public CreateExamByTitlesCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<ExamModel>> Handle(CreateExamByTitlesCommand request, CancellationToken cancellationToken)
		{
			try
			{

				//dbContext.ChangeTracker.Clear();
				var testItem = await dbContext.TestItems
					.Where(x => 
					x.Category.ToUpper() == request.TestItemInfo.Category.ToUpper() && 
					x.Subject.ToUpper() == request.TestItemInfo.Subject.ToUpper() && 
					x.Chapter.ToUpper() == request.TestItemInfo.Chapter.ToUpper() &&
					x.Version == request.TestItemInfo.Version).FirstOrDefaultAsync();

				if (testItem == null)
				{


				}
				else
				{

				}


				ExamModel examModel = new ExamModel();
				int result = 0;
				
				if (examModel.FillTest(testItem))
				{
					dbContext.Tests.Add(examModel.Test);
					
					result = await dbContext.SaveChangesAsync();
				}



				if (result > 0)
					return await Task.FromResult(new SuccessResult<ExamModel>(examModel));
				else
					return await Task.FromResult(new ServiceResult.InvalidResult<ExamModel>("") { Message = "CreateExamByTitlesCommand Failed On Save" });


			}
			catch (Exception ex)
			{
				return await Task.FromResult(new UnexpectedResult<ExamModel>(ex.Message) { Message = "CreateExamByTitlesCommand Failed" });
			}


		}
	}
}
