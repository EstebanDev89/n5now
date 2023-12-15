using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Nest;

namespace Application.Features.Permissions.Services
{
    public class GetPermissionsService : IGetPermissionsService
    {
        private readonly IMapper mapper;
        private readonly IPermissionEventPublisher permissionEventPublisher;
        private readonly IElasticClient elasticClient;

        public GetPermissionsService(
            IMapper mapper,
            IPermissionEventPublisher permissionEventPublisher,
            IElasticClient elasticClient
        )
        {
            this.mapper = mapper;
            this.permissionEventPublisher = permissionEventPublisher;
            this.elasticClient = elasticClient;
        }

        public async Task<PermissionDTO> GetById(int id)
        {
            var response = await elasticClient.SearchAsync<Permission>(s => s.Index("permission").Query(q => q.Match(m => m.Field(f => f.Id).Query(id.ToString()))));

            var permission = response?.Documents?.FirstOrDefault();

            await permissionEventPublisher.ProduceAsync(new PermissionMessage(Operation.Get));
            
            return mapper.Map<PermissionDTO>(permission);
        }
    }
}
