using LearningQA.Server.Configuration;
using LearningQA.Server.Infrasructure;
using LearningQA.Shared;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Extensions;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LearningQA.Server
{
	
	public class Startup
	{
		//By using IOption
		
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var config = Configuration.GetSection("LeaningConfig").GetSection("Question").GetValue<int>("PassingSquer");
			//in line
			config = Configuration.GetValue<int>("LeaningConfig:Question:PassingSquer");
			//Binding
			var configBinding = new LeaningConfig();
			Configuration.GetSection("LeaningConfig").Bind(configBinding);
			//ByService, can injected to the control
			services.Configure<LeaningConfig>(Configuration.GetSection(LeaningConfig.ConfigSection));
			//Add swagger
			services.AddSwaggerGen();
			//
			//Mediator

			//MediatR Pipeline support
			services.AddHttpContextAccessor();
			//The order is the pipe order
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleMediatRPipe<,>));
			//
			//AddMediatR(Assembly.GetExecutingAssembly()
			//provide the assambly where the handler exist
			services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

			//

			//
			//AutoMapper
			services.AddAutoMapper(typeof(Startup));
			//

			//Applicatiob DBContext
			services.AddApplicationDbConext();
			//
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
			using(var serviceScope = serviceScopeFactory.CreateScope())
			{
				var dbContext = serviceScope.ServiceProvider.GetService<LearningQAContext>();
				dbContext.Database.EnsureCreated();
			}
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();
			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			//
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
