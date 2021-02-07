using LearningQA.Shared.DTO;
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

namespace LearningQA.Shared.MediatR.Test.Query
{
	public class GetExamsInfoQuery : IRequestWrapper<IQueryable<ExamInfoModel>>
	{
		public int PersonId { get; set; } = 0;
		public TestItemInfo TestItemInfo { get; set; }
	}

	public class GetExamInfoQueryHandler : BaseDBContextHandler, IHandlerWrapper<GetExamsInfoQuery, IQueryable<ExamInfoModel>>
	{
		public GetExamInfoQueryHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<IQueryable<ExamInfoModel>>> Handle(GetExamsInfoQuery request, CancellationToken cancellationToken)
		{
			try
			{
				//var result = await dbContext.Person.Where(x => x.Tests.Count() > 0).Select(ExamInfoModel.Projection).ToListAsync();
				//var resultTest = dbContext.Tests.Where(x => x.TestItemId > 0).AsQueryable();
				//var query = from test in dbContext.Tests
				//			join testitem in dbContext.TestItems
				//			on test.Id equals testitem.Id


				//			select new { test, Title = $"{ testitem.Category}/{testitem.Subject}/{testitem.Chapter}/{testitem.Version}" };
				//var query2 = from p in dbContext.Person.SelectMany(x => x.Tests)
				//			 from pp in dbContext.Person
				//			 join test in dbContext.Tests
				//			 on p.Id equals test.Id
				//			 join testitem in dbContext.TestItems
				//			on test.Id equals testitem.Id
				//			where test.TestItemId > 0
				//			 select new
				//			 {
				//				 TestId = test.Id,
				//				 DateStart = test.DateStart,
				//				 DateFinish = test.DateFinish,
				//				 Title = $"{ testitem.Category}/{testitem.Subject}/{testitem.Chapter}/{testitem.Version}",
				//				 PersonId = pp.Id, Name = pp.Name
				//			 };

				//var result = from p in dbContext.Person.SelectMany(x => x.Tests)
				//			 from pp in dbContext.Person
				//			 join test in dbContext.Tests
				//			 on p.Id equals test.Id
				//			

				//			 join testitem in dbContext.TestItems
				//			on test.Id equals testitem.Id
				//			 where (test.TestItemId > 0 ) &&
				//					(string.IsNullOrEmpty(request.TestItemInfo.Category) ? true : request.TestItemInfo.Category == testitem.Category) &&
				//					(string.IsNullOrEmpty(request.TestItemInfo.Subject) ? true : request.TestItemInfo.Subject == testitem.Subject) &&
				//					(string.IsNullOrEmpty(request.TestItemInfo.Chapter) ? true : request.TestItemInfo.Chapter == testitem.Chapter)

				//			 select new ExamInfoModel(test,pp,testitem)
				//			 {

				//			 };
				var persons = (from p in dbContext.Person
							  where (request.PersonId == 0 ? true : request.PersonId == p.Id)
							  select p ).ToList();

				var result = new List<ExamInfoModel>();
				foreach(var p in persons)
				{
					var tests = from pp in p.Tests
								 
								 join testItem in dbContext.TestItems
								 on pp.TestItemId equals testItem.Id
								 where (pp.TestItemId > 0) &&
										(string.IsNullOrEmpty(request.TestItemInfo.Category) ? true : request.TestItemInfo.Category == testItem.Category) &&
										(string.IsNullOrEmpty(request.TestItemInfo.Subject) ? true : request.TestItemInfo.Subject == testItem.Subject) &&
										(string.IsNullOrEmpty(request.TestItemInfo.Chapter) ? true : request.TestItemInfo.Chapter == testItem.Chapter)
								 select new ExamInfoModel(pp, p, testItem) { };
					result.AddRange(tests);
				}

				
				

				return new SuccessResult<IQueryable<ExamInfoModel>>(result.AsQueryable<ExamInfoModel>());
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<IQueryable<ExamInfoModel>>(ex.Message);
			}
			
			
		}
	}
}
