using LearningQA.Client.ViewModel;

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
	}
}
