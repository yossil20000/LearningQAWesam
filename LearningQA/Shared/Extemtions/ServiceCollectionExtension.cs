using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using LearningQA.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace LearningQA.Shared.Extensions
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationDbConext(this IServiceCollection services)
		{
			services.AddDbContext<LearningQAContext>(options =>
			options.UseSqlite(@"Data Source=.\LearningQAContext.db")
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine,LogLevel.Information)
			);
			return services;
		}
	}
}
