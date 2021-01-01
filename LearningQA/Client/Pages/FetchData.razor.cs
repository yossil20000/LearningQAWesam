using LearningQA.Client.ViewModel;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.Pages
{
	public partial class FetchData : ComponentBase
	{
		[Inject]
		private  IFetchDataViewModel fetchDataViewModel { get; set; }
		

		public FetchData()
		{
			
		}

		protected override async Task OnInitializedAsync()
		{
			//forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
			await fetchDataViewModel.RetrieveForecastsAsync();
		}
	}
}
