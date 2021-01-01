using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LearningQA.Client.Model
{
	public interface IForcastModel
	{
		Task<IWeatherForecast> GetForcast();
	}
	public class ForcastModel : IForcastModel
	{
		private readonly HttpClient httpClient;

		public ForcastModel(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		

		public async Task<IWeatherForecast> GetForcast()
		{
			return await httpClient.GetFromJsonAsync<WeatherForecast>("api/WeatherForecast/Get");
			
		}
	}
}
