﻿using AutoMapper;

using LearningQA.Shared.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Server.Controllers
{

	public class WeatherForecastController : ApiControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		//private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<ApiControllerBase> logger, IMediator mediator, IMapper mapper) :base(logger,mediator,mapper)
		{
			_logger.LogInformation($"WeatherForecastController statr");
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			string s = _mediator == null ? "NULL" : "NotNUll"; 
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Id = index,
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}
