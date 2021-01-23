using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.Test.Command;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LearningQA.Client.Model
{
	public interface ITestItemModel
	{
		IEnumerable<TestItemInfo> TestItemInfos { get;  }
		Task RetriveTestItemInfos();
		Task<TestItemInfo> RetriveTestItemInfo(int testItemId);
		Task<TestItem<QUestionSql,int>> RetriveTestItem(TestItemInfo testItemInfo);
		Task<ExamModel> RetriveTest(TestItemInfo testItemInfo);
		Task<bool> SaveTest(Test<QUestionSql, int> test);
	}
	public class TestItemModel : ITestItemModel
	{
		private readonly HttpClient httpClient;
		List<TestItemInfo> testItemInfos;
		public IEnumerable<TestItemInfo> TestItemInfos { get => testItemInfos; private set => testItemInfos = value.ToList(); }
		
		public TestItemModel(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}
		public async  Task RetriveTestItemInfos()
		{
			var result = await httpClient.GetFromJsonAsync<IEnumerable<TestItemInfo>>("api/TestItem/TestItemsInfo");
			testItemInfos = result.ToList();
		}

		public async Task<TestItemInfo> RetriveTestItemInfo(int testItemId)
		{
			var result = await httpClient.GetFromJsonAsync<TestItemInfo>($"api/TestItem/TestItemInfo?testItemId={testItemId}");
			return result;
		}
		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			var result = await httpClient.GetFromJsonAsync<TestItem<QUestionSql, int>>($"api/TestItem/TestItem?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");

			return result;
		}
		public async Task<ExamModel> RetriveTest(TestItemInfo testItemInfo)
		{
			ExamModel examModel = new ExamModel();
			try
			{

				//var personinfo = await httpClient.GetFromJsonAsync<PersonInfoModel[]>($"api/Exam/PersonsInfo");
				//examModel = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/CreateExamByTitle?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");
				var result = await httpClient.PutAsJsonAsync($"api/Exam/CreateExamByTitle",testItemInfo);
				examModel = await result.Content.ReadFromJsonAsync<ExamModel>();

			}
			catch(NotSupportedException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			
			return examModel;
		}
		public async Task<bool> SaveTest(Test<QUestionSql,int> test)
		{
			var result = await httpClient.PostAsJsonAsync($"api/Exam/UpdateExam", new UpdateExamCommand(test,1));

			return true;
		}
	}
}
