using System;
using AutoMapper;
using Claims.Application.Features.Claims;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Services;
using Claims.Application.Features.Services.Commands;
using Claims.Domain.Entities;
using NUnit.Framework;


namespace Claims.UnitTests.Mapping
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClaimsMapper>();
                cfg.AddProfile<ServiceMapper>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void Configuration_Should_Be_Valid()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        [TestCase(typeof(Claim), typeof(NewClaim.Command))]
        [TestCase(typeof(NewClaim.Command), typeof(Claim))]
        [TestCase(typeof(Service), typeof(AddService.Command))]
        [TestCase(typeof(AddService.Command), typeof(Service))]
        public void Should_Map_Commands_To_Entity(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}