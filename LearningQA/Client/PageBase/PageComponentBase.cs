using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.PageBase
{
	public class PageComponentBase : ComponentBase
	{
		private bool IsInitialize { get; set; } = false;
		
		protected override async Task OnInitializedAsync()
		{
			IsInitialize = true;
			await base.OnInitializedAsync();
		}
		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();
		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
		}
	}
}
