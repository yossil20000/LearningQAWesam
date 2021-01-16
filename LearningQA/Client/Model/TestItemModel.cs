using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

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
		Task<TestItem<QUestionSql,int>> RetriveTestItem(TestItemInfo testItemInfo);
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
			var result = await httpClient.GetFromJsonAsync<IEnumerable<TestItemInfo>>("api/TestItem/TestItems");
			testItemInfos = result.ToList();
		}

		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			var result = await httpClient.GetFromJsonAsync<TestItem<QUestionSql, int>>($"api/TestItem/TestItem?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");

			return result;
		}
		public async Task<bool> SaveTest(Test<QUestionSql,int> test)
		{
			var result = await httpClient.PostAsJsonAsync($"api/TestItem/UpdateExam",test);

			return true;
		}
	}
}
