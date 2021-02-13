using AutoMapper;

using Castle.Core.Logging;

using LearningQA.Server.Configuration;
using LearningQA.Server.Infrasructure;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.Test.Command;
using LearningQA.Shared.MediatR.TestItem.Command;
using LearningQA.Shared.MediatR.TestItem.Query;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ServiceResult;
using ServiceResult.ApiExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Server.Controllers
{
	public class TestItemController : ApiControllerBase
	{
		private readonly IOptions<LeaningConfig> _learningConfig;
		public TestItemController(ILogger<ApiControllerBase> logger,IMediator mediator, IOptions<LeaningConfig> learningConfig , IMapper mapper) :base(logger,mediator,mapper)
		{
			_learningConfig = learningConfig;
		}
        //https://localhost:44335/TestItem?Category=Yossi&Subject=Pop&Chapter=1&Version=12&NumOfQuestions=50
        /// <summary>
        /// Create New TestItem
        /// </summary>
        /// <param name="newTestItem"></param>
        /// <returns>Suceed if id > 0 </returns>
        [HttpPost]
		
		public async Task<ActionResult<TestItem<QUestionSql,int>>> CreatEmptyTest([FromQuery] TestItemInfo newTestItem)
		{
			var result = await _mediator.Send(new CreateNewTestItemInfoCommand(newTestItem), cancellationToken);
			return Ok(result);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TestItemInfo>>> TestItemsInfo(CancellationToken cancellationToken = default)
		{
			var result  = await _mediator.Send(new TestItemsInfoQuery(),cancellationToken);
			var list = result.Data;
			return   Ok(list);
		}

		[HttpGet(Name = "/TestItemInfo")]
		public async Task<IActionResult> TestItemInfo( int testItemId)
		{
			var result = await _mediator.Send(new TestItemInfoQuery(testItemId));
			return this.FromResult(result);
		}
		/// <summary>
		/// Get TestItem 
		/// </summary>
		/// <param name="category"></param>
		/// <param name="subject"></param>
		/// <param name="chapter"></param>
		/// <returns>"<questionsql,int>"</returns>
		[HttpGet]

		public async Task<IActionResult> TestItem([FromQuery] string category, [FromQuery] string subject, [FromQuery] string chapter)
		{
			TestItemQuery testItemQuery = new TestItemQuery()
			{
				TestItemInfo = new TestItemInfo()
				{
					Category = category,
					Subject = subject,
					Chapter = chapter
				} 
			};
			var result = await _mediator.Send(testItemQuery);

			return this.FromResult(result);
		}
		/// <summary>
		/// Update TestItem from a TestItem instance
		/// </summary>
		/// <param name="testItem"></param>
		/// <returns>bool</returns>
		[HttpPut]
		
		public async Task<IActionResult> UpdateTestItem([FromBody] TestItem<QUestionSql,int> testItem)
		{
			var result = await _mediator.Send(new UpdateTestItemCommand(testItem), cancellationToken);
			return Ok(result);
		}
		
		
		[HttpPost(Name = "/CreateTestItem")]
		public async Task<IActionResult> CreateTestItem([FromBody] TestItem<QUestionSql, int> testItem)
		{
			var testitems = DataResourceReader.LoadJson<TestItem<QUestionSql, int>>();
			await _mediator.Send(new CreateRangeTestItemCommand(testitems), cancellationToken);
			return Ok(true);
		}
		[HttpPost(Name = "/LoadNewFromFile")]
		public async Task<IActionResult> LoadNewFromFile(bool createNewDatabase = false, string fileName = "")
		{
            var testitems = DataResourceReader.LoadJson<TestItem<QUestionSql, int>>(fileName);
			Person<int> person = new Person<int>()
			{
				IdNumber = "059828391",
				Name = "Yosef Levy",
				Email ="yos@gmail.com",
				Address = "Gilon, Israel 2010300",
				Phone = "054999888777"
				
			};
			await _mediator.Send(new CreateRangeTestItemCommand(testitems,person) { CreateNewDatabase = createNewDatabase}, cancellationToken);
			return Ok(true);
		}

		[HttpGet(Name = "/EmptyTestItem")]
		public  async Task<List<TestItem<QUestionSql,int>>> EmptyTestItem(int test, int questionCount)
		{
			string[] option = new string[4] { "A", "B", "C", "D" };
			List<TestItem<QUestionSql, int>> testItems = new List<TestItem<QUestionSql, int>>();
			testItems = CreateList<TestItem<QUestionSql, int>>(test);
			for(int i =0; i < testItems.Count();i++)
			{
				testItems[i] = new TestItem<QUestionSql, int>();

				testItems[i].Questions = new List<QUestionSql>();
                
				for(var j=0; j < questionCount;j++)
				{
					QUestionSql q = new QUestionSql();
                    q.IsActive = true;
					q.QuestionNumber = (j + 1).ToString();
					q.Options = new List<QuestionOption<int>>();
					q.Supplements = new List<Supplement<int>>();
					for(var k=0; k < 4; k++)
					{
						QuestionOption<int> qo = new QuestionOption<int>();
						qo.TenantId = option[k];
						q.Options.Add(qo);
					}
					testItems[i].Questions.Add(q);
				}
		
			}


			return await Task.FromResult(testItems);
		}
		private static List<T> CreateList<T>(int capacity)
		{
			return Enumerable.Repeat(default(T), capacity).ToList();
		}
	}

}
