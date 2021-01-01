using LearningQA.Shared.Entities;

using Microsoft.EntityFrameworkCore.Query.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LearningQA.Client.Model
{
	public interface IFetchDataModel
	{
		IWeatherForecast[] WeatherForecasts { get; }
		Task RetrieveForecastsAsync();

	}

	
	public class FetchData_Model : IFetchDataModel
	{
		
		private IWeatherForecast[] _weatherForecasts;
		private readonly HttpClient httpClient;

		public IWeatherForecast[] WeatherForecasts { get => _weatherForecasts; private set => _weatherForecasts = value; }
		public FetchData_Model(HttpClient httpClient )
		{
			this.httpClient = httpClient;
		}
		public async Task RetrieveForecastsAsync()
		{
			_weatherForecasts = await  httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast/Get");
		}
	}
}
