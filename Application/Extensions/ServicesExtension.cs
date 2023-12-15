using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services;
using Application.Features.Permissions.Services.Interfaces;
using Confluent.Kafka;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System.Reflection;

namespace Application.Extensions
{
    public static class ServicesExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.AddTransient<IGetPermissionsService, GetPermissionsService>();
            services.AddTransient<IModifyPermissionService, ModifyPermissionService>();
            services.AddTransient<IRequestPermissionService, RequestPermissionService>();

            services.AddSingleton<IProducer<Null, string>>(x => new ProducerBuilder<Null, string>(
                new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] }
            ).Build());
            services.AddSingleton<IPermissionEventPublisher, PermissionEventPublisher>();

            var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"))
                            .DefaultMappingFor<Application.Features.Permissions.ElasticSearchModel.Permission>(m => m
                                .IndexName("permission"));

            services.AddSingleton<IElasticClient>(new ElasticClient(settings));
        }
    }
}
    