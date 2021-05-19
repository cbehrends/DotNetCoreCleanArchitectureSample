using System;
using AutoMapper;
using NUnit.Framework;
using Orders.Application.Features.Orders;
using Orders.Application.Features.Orders.Commands;
using Orders.Application.Features.Services;
using Orders.Application.Features.Services.Commands;
using Orders.Domain.Entities;
using Orders.WebApi.ViewModels;

namespace Orders.UnitTests.Mapping
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrdersMapper>();
                cfg.AddProfile<ServicesMapper>();
                cfg.AddProfile<OrderViewModelMapper>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void Configuration_Should_Be_Valid()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Order), typeof(NewOrder.Command))]
        [TestCase(typeof(NewOrder.Command), typeof(Order))]
        [TestCase(typeof(Service), typeof(NewService.Command))]
        [TestCase(typeof(NewService.Command), typeof(Service))]
        public void Should_Map_Commands_To_Entity(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }

        [Test]
        [TestCase(typeof(Order), typeof(OrderViewModel))]
        [TestCase(typeof(RenderedServiceViewModel), typeof(RenderedService))]
        public void Should_Map_Entity_To_ViewModels(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}