using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Commands.RequestPermission;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Nest;
using Persistence.Repository;

namespace Application.Features.Permissions.Services
{
    public class RequestPermissionService : IRequestPermissionService
    {
        private readonly IRepositoryAsync<Permission> repositoryAsync;
        private readonly IMapper mapper;
        private readonly IPermissionEventPublisher permissionEventPublisher;
        private readonly IElasticClient elasticClient;

        public RequestPermissionService(
            IRepositoryAsync<Permission> repositoryAsync, 
            IMapper mapper, 
            IPermissionEventPublisher permissionEventPublisher,
            IElasticClient elasticClient
        ){
            this.repositoryAsync = repositoryAsync;
            this.mapper = mapper;
            this.permissionEventPublisher = permissionEventPublisher;
            this.elasticClient = elasticClient;
        }

        public async Task<PermissionDTO> Create(RequestPermissionCommand request)
        {
            var newPermission = mapper.Map<Permission>(request);
            newPermission.PermissionDate = DateTime.Now;

            var savedPermission = await repositoryAsync.AddAsync(newPermission);

            await permissionEventPublisher.ProduceAsync(new PermissionMessage(Operation.Request));

            await elasticClient.IndexAsync(savedPermission, x => x.Index("permission"));

            return mapper.Map<PermissionDTO>(savedPermission);
        }
    }
}
