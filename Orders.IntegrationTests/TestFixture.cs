using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Orders.Infrastructure.Data;
using Orders.WebApi;
using Respawn;

namespace Orders.IntegrationTests
{
    [SetUpFixture]
    public class TestFixture
    {
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        private static string _currentUserId;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Test" &&
                w.ApplicationName == "Orders.WebApi"));

            services.AddLogging();

            startup.ConfigureServices(services);

            // Replace service registration for ICurrentUserAccessor
            // Remove existing registration
            var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserAccessor));

            services.Remove(currentUserServiceDescriptor);


            // Register testing version
            services.AddTransient(provider =>
                Mock.Of<ICurrentUserAccessor>(s => s.UserId == _currentUserId));

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] {"__EFMigrationsHistory"}
            };

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<OrdersDbContext>();

            context.Database.Migrate();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public static async Task ResetState()
        {
            try
            {
                _currentUserId = null;
                await _checkpoint.Reset(_configuration["ConnectionString"]);
            }
            catch (Exception)
            {
                //Swallow this error, caused by Respawn not dealing with an empty DB
            }
        }

        public static async Task<TEntity> FindAsync<TEntity>(int id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<OrdersDbContext>();

            return await context.FindAsync<TEntity>(id);
        }

        public static async Task<TEntity> FindAsync<TEntity>(Guid id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<OrdersDbContext>();

            return await context.FindAsync<TEntity>(id);
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<OrdersDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}