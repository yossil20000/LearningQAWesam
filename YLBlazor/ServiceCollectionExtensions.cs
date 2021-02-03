using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YLBlazor.CssCustomProperties.Services;

namespace YLBlazor
{
	public static class classServiceCollectionExtensions
	{
		public static IServiceCollection AddYLBlazor(this IServiceCollection services)
		{
			return services.AddScoped<ExampleJsInterop>();
		}
		public static IServiceCollection AddCssCustomProperties(this IServiceCollection services)
		{
			return services.AddScoped<ICustomProperties, CustomProperties>();
		}
	}

	
}
