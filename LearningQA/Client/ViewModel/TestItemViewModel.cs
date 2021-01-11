using LearningQA.Client.Model;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface ITestItemViewModel
	{
		
		Task RetriveTestItemInfos();

		Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo);
		Task OnLoadCommand();
		TestItem<QUestionSql, int> TestItem { get; set; }
	}	


	public class TestItemViewModel : ITestItemViewModel
	{

		public ITestItemModel testItemModel;
		public TestItemViewModelPersist TestItemViewModelPersist { get; private set; }
		public TestItem<QUestionSql,int> TestItem { get; set; }

			public TestItemViewModel(ITestItemModel testItemModel, TestItemViewModelPersist testItemViewModelPersist )
		{
			this.testItemModel = testItemModel;
			TestItemViewModelPersist = testItemViewModelPersist;
			}
		public async Task RetriveTestItemInfos()
		{
			if (TestItemViewModelPersist.Initialize)
				return;
			await testItemModel.RetriveTestItemInfos();
			TestItemViewModelPersist.TestItemInfos = testItemModel.TestItemInfos.ToList();
			

		}
		
		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			var result = await testItemModel.RetriveTestItem(testItemInfo);
			TestItemViewModelPersist.SelectedQuestion = result.Questions.FirstOrDefault();
			return result;
		}

		public async Task OnLoadCommand()
		{
			TestItemInfo testItemInfo = new TestItemInfo()
			{
				Category = TestItemViewModelPersist.SelectedCategory,
				Subject = TestItemViewModelPersist.SelectedSubjecte,
				Chapter = TestItemViewModelPersist.SelectedChapter
			};
			 TestItem = await RetriveTestItem(testItemInfo);
		}
	}
}
