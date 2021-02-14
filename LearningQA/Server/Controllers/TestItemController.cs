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

using Swashbuckle.AspNetCore.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [SwaggerOperation(
            Summary = "CreatEmptyTest",
            Description = "CreatEmptyTest By TestItemInfo(Category, Subject,Chapter)",
            OperationId = "TestItem.Post",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "TestItem<QUestionSql, int>", typeof(TestItem<QUestionSql, int>))]
        public async Task<ActionResult<TestItem<QUestionSql,int>>> CreatEmptyTest([FromQuery] TestItemInfo newTestItem)
		{
			var result = await _mediator.Send(new CreateNewTestItemInfoCommand(newTestItem), cancellationToken);
			return Ok(result);
		}

		[HttpGet]
        [SwaggerOperation(
            Summary = "TestItemsInfo",
            Description = "Get All the testitems titles, Category,Subject,Chapter,Version",
            OperationId = "TestItem.Get",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "IEnumerable<TestItemInfo>", typeof(IEnumerable<TestItemInfo>))]
        public async Task<ActionResult<IEnumerable<TestItemInfo>>> TestItemsInfo(CancellationToken cancellationToken = default)
		{
			var result  = await _mediator.Send(new TestItemsInfoQuery(),cancellationToken);
			var list = result.Data;
			return   Ok(list);
		}

		[HttpGet(Name = "/TestItemInfo")]
        [SwaggerOperation(
            Summary = "TestItemsInfo",
            Description = "Get TestItemsInfo of TestItem by id)",
            OperationId = "TestItem.Get",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "TestItemInfo", typeof(TestItemInfo))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, "TestItemInfo", typeof(TestItemInfo))]
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
        [SwaggerOperation(
            Summary = "TestItems",
            Description = "Get TestItems by Category,Subject,Chapter)",
            OperationId = "TestItem.Get",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "TestItem<QUestionSql,int>", typeof(TestItem<QUestionSql,int>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, "TestItem<QUestionSql,int>", typeof(TestItem<QUestionSql, int>))]
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
        [HttpGet]
        [SwaggerOperation(
            Summary = "TestItems",
            Description = "Get All TestItems)",
            OperationId = "TestItem.Get",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "List<TestItem<QUestionSql, int>>", typeof(List<TestItem<QUestionSql, int>>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, "List<TestItem<QUestionSql, int>>", typeof(List<TestItem<QUestionSql, int>>))]
        public async Task<ActionResult<List<TestItem<QUestionSql,int>>>> Get()
        {
            var result = await _mediator.Send(new GetAllTestItemsQuery());
            return this.FromResult(result);
        }
		/// <summary>
		/// Update TestItem from a TestItem instance
		/// </summary>
		/// <param name="testItem"></param>
		/// <returns>bool</returns>
		[HttpPut]
        [SwaggerOperation(
            Summary = "TestItems",
            Description = "Update TestItems)",
            OperationId = "TestItem.Put",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "bool", typeof(bool))]
        public async Task<IActionResult> UpdateTestItem([FromBody] TestItem<QUestionSql,int> testItem)
		{
			var result = await _mediator.Send(new UpdateTestItemCommand(testItem), cancellationToken);
			return Ok(result);
		}
		
		[HttpPost(Name = "/LoadNewFromFile")]
        [SwaggerOperation(
            Summary = "LoadNewFromFile",
            Description = "Load file in json format, and create new testitems, can be also reset database",
            OperationId = "TestItem.Post",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "bool", typeof(bool))]
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
            ConverSupplement(testitems);
			await _mediator.Send(new CreateRangeTestItemCommand(testitems,person) { CreateNewDatabase = createNewDatabase}, cancellationToken);
			return Ok(true);
		}

		[HttpGet(Name = "/EmptyTestItem")]
        [SwaggerOperation(
            Summary = "EmptyTestItem",
            Description = "Get List of Empty TestItems count for each test create empty questionCount )",
            OperationId = "TestItem.Get",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "List<TestItem<QUestionSql,int>>", typeof(List<TestItem<QUestionSql, int>>))]
        public  async Task<List<TestItem<QUestionSql,int>>> EmptyTestItem(int testCount, int questionCount)
		{
			string[] option = new string[4] { "A", "B", "C", "D" };
			List<TestItem<QUestionSql, int>> testItems = new List<TestItem<QUestionSql, int>>();
			testItems = CreateList<TestItem<QUestionSql, int>>(testCount);
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
                    q.Supplements.Add(new Supplement<int>());
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
        [HttpGet(Name = "/LoadImageFromFile")]
        public async  Task<ActionResult<string>> LoadImageFromFile( string fileName = "")
        {
            var image = DataResourceReader.LoadImageForDisplay(fileName);
            return Ok(image);
        }

        private static List<T> CreateList<T>(int capacity)
		{
			return Enumerable.Repeat(default(T), capacity).ToList();
		}
        private void ConverSupplement(List<TestItem<QUestionSql,int>> items)
        {
            for (int item = 0; item < items.Count; item++)
            {
                for (int qIndex = 0; qIndex < items.ElementAt(item).Questions.Count; qIndex++)
                {
                    var supplement = items.ElementAt(item).Questions.ElementAt(qIndex).Supplements;
                    if (supplement != null && supplement.Count > 0)
                    {
                        if (supplement.ElementAt(0).OriginalcontentType == ContentType.ImageFileName)
                        {
                            var content = supplement.ElementAt(0).OriginalContent.Split(";");
                            if(content.Length > 0 )
                            {
                                var src = content[0].Split(":")[1];
                                supplement.ElementAt(0).Content = DataResourceReader.LoadImageForDisplay(src);
                                supplement.ElementAt(0).ContentType = ContentType.ImageBase64String;
                            }
                          
                        }
                    }
                }
            }
        }
	}

}
