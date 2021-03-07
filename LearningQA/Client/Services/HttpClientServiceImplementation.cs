using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearningQA.Client.Services
{
	public class HttpClientServiceImplementation : IHttpClientServiceImplementation
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly JsonSerializerOptions _options;
		private readonly TestItemClient _testItemClient;
		public HttpClientServiceImplementation(IHttpClientFactory httpClientFactory , TestItemClient testItemClient)
		{
			_httpClientFactory = httpClientFactory;
			_testItemClient = testItemClient;
			_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		}
		public async Task Excecute()
		{
			 await RetriveExamInfoModels(new TestItemInfo());
		}

		private async Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo)
		{
			var httpClient = _httpClientFactory.CreateClient("TestItemClient");
			try
			{
				var result = await httpClient.GetFromJsonAsync<List<ExamInfoModel>>($"api/Exam/GetExamList?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}&personId={1}");
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"RetriveExamInfoModels {ex.Message}");
				return new List<ExamInfoModel>();
			}
		}
	}
}
