using System.Reflection;
using AutoMapper;
using Claims.Application.Behaviours;
using Claims.Application.Features.Claims;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddClaimsApplication(this IServiceCollection services)
        {
          
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            return services;
        }
    }
}