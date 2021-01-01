using LearningQA.Shared.DTO;

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
	}
}
