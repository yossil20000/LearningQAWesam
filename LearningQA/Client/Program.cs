using LearningQA.Client.Model;
using LearningQA.Client.ViewModel;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			ConfigurationServices(builder.Services);

			
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			await builder.Build().RunAsync();
		}
		private static void ConfigurationServices(IServiceCollection services)
		{
			services.AddScoped<IFetchDataViewModel, FetchDataViewModel>();
			services.AddScoped<IFetchDataModel, FetchData_Model>();
			services.AddScoped<ITestItemModel, TestItemModel>();
			services.AddScoped<ITestItemViewModel, TestItemViewModel>();
			var assemblyAll = AppDomain.CurrentDomain.GetAssemblies();
			var assembly = assemblyAll.Where(a => a.FullName.StartsWith("LearningQA.Client")).FirstOrDefault();

		}
	}
}
