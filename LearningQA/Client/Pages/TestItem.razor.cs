using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.Pages
{
	public partial class TestItem : ComponentBase
	{
		[Inject]
		ITestItemViewModel testItemViewModel { get; set; }
		private bool IsInitialize { get; set; } = false;
		public TestItem()
		{

		}
		protected override async  Task OnInitializedAsync()
		{
			await testItemViewModel?.RetriveTestItemInfos();
			IsInitialize = true;
			
		}
		protected override void OnAfterRender(bool firstRender)
		{
			//TestItemViewModelPersist.OnChanged(
			//	() => base.StateHasChanged());
			base.OnAfterRender(firstRender);
		}
		
		private async Task OnLoadCommand(TestItemInfo testItemInfo)
		{
			TestItem<QUestionSql, int> testItem = await testItemViewModel.RetriveTestItem(testItemInfo);
		}
	}
}
