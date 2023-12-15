using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Commands.ModifyPermission;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Nest;
using Persistence.Repository;

namespace Application.Features.Permissions.Services
{
    public class ModifyPermissionService : IModifyPermissionService
    {
        private readonly IRepositoryAsync<Permission> repositoryAsync;
        private readonly IReadRepositoryAsync<Permission> readRepositoryAsync;
        private readonly IMapper mapper;
        private readonly IPermissionEventPublisher permissionEventPublisher;
        private readonly IElasticClient elasticClient;

        public ModifyPermissionService(
            IRepositoryAsync<Permission> repositoryAsync, 
            IReadRepositoryAsync<Permission> readRepositoryAsync, 
            IMapper mapper,
            IPermissionEventPublisher permissionEventPublisher,
            IElasticClient elasticClient
        )
        {
            this.repositoryAsync = repositoryAsync;
            this.readRepositoryAsync = readRepositoryAsync;
            this.mapper = mapper;
            this.permissionEventPublisher = permissionEventPublisher;
            this.elasticClient = elasticClient;
        }

        public async Task<PermissionDTO> Update(ModifyPermissionCommand request)
        {
            var permission = await readRepositoryAsync.GetByIdAsync(request.id);

            if (permission == null)
            {
                throw new Exception($"Permission {request.id} not found");
            }

            permission.EmployeeForename = request.EmployeeForename;
            permission.EmployeeSurname = request.EmployeeSurname;
            permission.PermissionTypeId = request.PermissionTypeId;

            var permissionModify = await repositoryAsync.UpdateAsync(permission);

            elasticClient.Update<Permission, object>(request.id, u => u
                .Index("permission") 
                .Doc(new
                {
                    EmployeeForename = permissionModify.EmployeeForename,
                    EmployeeSurname = permissionModify.EmployeeSurname,
                    PermissionTypeId = permissionModify.PermissionTypeId
                })
            );

            await permissionEventPublisher.ProduceAsync(new PermissionMessage(Operation.Modify));

            return mapper.Map<PermissionDTO>(permissionModify);
        }
    }
}
