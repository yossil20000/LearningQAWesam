using LearningQA.Client.Model;
using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface IFetchDataViewModel
	{
		IWeatherForecast[] WeatherForecast { get; set; }
		Task RetrieveForecastsAsync();
	}
	public class FetchDataViewModel : IFetchDataViewModel
	{
		private readonly IFetchDataModel fetchDataModel;
		private IWeatherForecast[] weatherForecasts;
		public FetchDataViewModel(IFetchDataModel fetchDataModel)
		{
			this.fetchDataModel = fetchDataModel;
		}
		public IWeatherForecast[] WeatherForecast { get => weatherForecasts; set => weatherForecasts = value; }
		public async Task RetrieveForecastsAsync()
		{
			await fetchDataModel.RetrieveForecastsAsync();
			weatherForecasts = fetchDataModel.WeatherForecasts;
		}
	}
}
