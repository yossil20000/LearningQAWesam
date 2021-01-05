using LearningQA.Client.Model;
using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface ITestItemViewModel
	{
		
		Task RetriveTestItemInfos();
		

		
	}	


	public class TestItemViewModel : ITestItemViewModel
	{

		public ITestItemModel testItemModel;
		public TestItemViewModelPersist TestItemViewModelPersist { get; private set; }

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
		

		}
}
