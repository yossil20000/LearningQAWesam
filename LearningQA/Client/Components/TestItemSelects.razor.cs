using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.Components
{
	public partial class TestItemSelects : ComponentBase, IDisposable
	{

		[Parameter]
		public IViewPersistanceBase TestItemViewModelPersist { get; set; }
		[Parameter] public EventCallback OnLoadCommand { get; set; }
		[Parameter] public EventCallback<int> OnVerify { get; set; }
		[Parameter] public string  ButtonTitle {get;set;}
		public TestItemSelects()
		{

		}
		protected override void OnInitialized()
		{
			
			

		}
		bool IsRegistered = false;
		protected override void OnAfterRender(bool firstRender)
		{
			if(!IsRegistered)
			{
				TestItemViewModelPersist.OnChanged(() => base.StateHasChanged());
				IsRegistered = true;
			}
			base.OnAfterRender(firstRender);
		}
		
		public void OnPropertyChanged()
		{
			StateHasChanged();
		}

		public void Dispose()
		{
			if (IsRegistered)
				TestItemViewModelPersist.OnUnChanged(() => base.StateHasChanged());
		}
	}
}
