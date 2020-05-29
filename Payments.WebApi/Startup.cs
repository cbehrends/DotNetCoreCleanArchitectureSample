using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Payments.Application;
using Payments.Application.Features.Messaging;
using Payments.Infrastructure;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Payments.WebApi
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
            services.AddControllers();
            services.AddPaymentsApplication();
            services.AddPaymentsInfrastructure(Configuration);
            services.AddHealthChecks();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payments API", Version = "v1" });
                c.CustomSchemaIds(type =>
                {
                    if (!type.FullName.EndsWith("+Command") && !type.FullName.EndsWith("+Query")) return type.Name;
                    var parentTypeName = type.FullName.Substring(type.FullName.LastIndexOf(".", StringComparison.Ordinal) + 1);
                    return parentTypeName.Replace("+Command", "Command").Replace("+Query", "Query");

                });
            });
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentApprovedConsumer>();
            
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(context);
            
                    cfg.Host(Configuration["RabbitConnectionString"], hostSettings =>
                    {
                        hostSettings.Username("user");
                        hostSettings.Password("P@ssW0rdTr1ckz");
                    });
                    
                    cfg.ReceiveEndpoint("payment-approved", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<PaymentApprovedConsumer>(context);
                    });
                    
                }));
            });
            
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}