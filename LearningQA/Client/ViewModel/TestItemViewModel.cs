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
		List<TestItemInfo> TestItemInfos { get; }
		Task RetriveTestItemInfos();
	}
	public class TestItemViewModel : ITestItemViewModel
	{
		private readonly ITestItemModel testItemModel;
		List<TestItemInfo> testItemInfos;
		public List<TestItemInfo> TestItemInfos { get => testItemInfos; private set => testItemInfos = value; }
		public TestItemViewModel(ITestItemModel testItemModel)
		{
			this.testItemModel = testItemModel;
		}
		public async Task RetriveTestItemInfos()
		{
			await testItemModel.RetriveTestItemInfos();
		}
	}
}
