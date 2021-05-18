using System;
using AutoMapper;
using Orders.Application;
using Orders.Application.Features.Messaging;
using Orders.Infrastructure;
using Orders.Infrastructure.Data;
using Common.ApplicationCore.Interfaces;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Orders.WebApi.Core;
using InjectDependencies = Orders.Application.InjectDependencies;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Orders.WebApi
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
           
            services.AddHealthChecks()
                .AddDbContextCheck<OrdersDbContext>();
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddClaimsApplication();
            
            services.AddClaimsInfrastructure(Configuration);
            
            services.AddAutoMapper(typeof(Startup), typeof(InjectDependencies));

            services.AddTransient<ICurrentUserService, MockCurrentUserService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
                c.CustomSchemaIds(type =>
                {
                    if (!type.FullName.EndsWith("+Command") && !type.FullName.EndsWith("+Query")) return type.Name;
                    var parentTypeName = type.FullName.Substring(type.FullName.LastIndexOf(".", StringComparison.Ordinal) + 1);
                    return parentTypeName.Replace("+Command", "Command").Replace("+Query", "Query");

                });
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderPaidConsumer>();
            
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(context);
            
                    cfg.Host(Configuration["RabbitConnectionString"], hostSettings =>
                        {
                            hostSettings.Username("user");
                            hostSettings.Password("P@ssW0rdTr1ckz");
                        });
                            
                   
                    cfg.ReceiveEndpoint("orders-paid", ep =>
                    {
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                    
                        ep.ConfigureConsumer<OrderPaidConsumer>(context);
                    });
                }));
            });
            
            services.AddScoped<IMessagePublisher, MessagePublisher>();
            services.AddMassTransitHostedService();
            
            
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
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                // endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                // {
                //     Predicate = (check) => check.Tags.Contains("ready"),
                // });
                //
                // endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                // {
                //     // Exclude all checks and return a 200-Ok.
                //     Predicate = (_) => false
                // });
            });
            
           
        }
    }
}