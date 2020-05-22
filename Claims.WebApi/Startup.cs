using AutoMapper;
using Claims.Application;
using Claims.Application.Core.Interfaces;
using Claims.Infrastructure;
using Claims.WebApi.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Claims.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddClaimsApplication();
            
            services.AddClaimsInfrastructure(Configuration);
            
            services.AddAutoMapper(typeof(Startup), typeof(Application.InjectDependencies));

            services.AddTransient<ICurrentUserService, MockCurrentUserService>();
            
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Sample Appp API";
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}