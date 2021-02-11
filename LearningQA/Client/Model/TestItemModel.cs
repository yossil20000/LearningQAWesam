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
		Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo);
		Task<ExamModel> RetriveTest(TestItemInfo testItemInfo);
		Task<bool> SaveTest(Test<QUestionSql, int> test);
		Task<ExamModel> LoadTest(int testId);
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
		public async Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<List<ExamInfoModel>>($"api/Exam/GetExamList?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}&personId={1}");
				return result;
			}
			catch(Exception ex)
			{
				Console.WriteLine($"RetriveExamInfoModels {ex.Message}");
				return new List<ExamInfoModel>();
			}
		}
		public async  Task RetriveTestItemInfos()
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<IEnumerable<TestItemInfo>>("api/TestItem/TestItemsInfo");
				testItemInfos = result.ToList();
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItemInfos {ex.Message}");
			}
		}

		public async Task<TestItemInfo> RetriveTestItemInfo(int testItemId)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<TestItemInfo>($"api/TestItem/TestItemInfo?testItemId={testItemId}");
				return result;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItemInfo({testItemId}) {ex.Message}");
			}
			return null;
		}
		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<TestItem<QUestionSql, int>>($"api/TestItem/TestItem?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");

				return result;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItem({testItemInfo?.Category}) {ex.Message}");
			}
			return null;
		}
		public async Task<ExamModel> RetriveTest(TestItemInfo testItemInfo)
		{
			ExamModel examModel = new ExamModel();
			try
			{

				//var personinfo = await httpClient.GetFromJsonAsync<PersonInfoModel[]>($"api/Exam/PersonsInfo");
				//examModel = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/CreateExamByTitle?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");
				var result = await httpClient.PostAsJsonAsync($"api/Exam/CreateExamByTitle",testItemInfo);
				examModel = await result.Content.ReadFromJsonAsync<ExamModel>();

			}
			catch(NotSupportedException ex)
			{
				Console.WriteLine($"RetriveTest({testItemInfo?.Category}) {ex.Message}");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"RetriveTest({testItemInfo?.Category}) {ex.Message}");
			}
			
			return examModel;
		}
		public async Task<bool> SaveTest(Test<QUestionSql,int> test)
		{
			var result = await httpClient.PutAsJsonAsync($"api/Exam/UpdateExam", new UpdateExamCommand(test,1));

			return true;
		}
		public async Task<ExamModel> LoadTest(int testId)
		{
			
			try
			{
				var result = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/Get?testId={testId}");
				return result;
			}
			catch(Exception ex)
			{
				Console.WriteLine($"LoadTest({testId}) {ex.Message}");
				return null;
			}
		}
	}
}
