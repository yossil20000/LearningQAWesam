using System;
using System.Collections.Generic;
using System.Text;

namespace LearningQA.Shared.Entities
{
	public interface IWeatherForecast
	{
		int Id { get; set; }
		DateTime Date { get; set; }

		int TemperatureC { get; set; }

		string Summary { get; set; }

		int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

	}
	public class WeatherForecast : IWeatherForecast
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		public string Summary { get; set; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
}
