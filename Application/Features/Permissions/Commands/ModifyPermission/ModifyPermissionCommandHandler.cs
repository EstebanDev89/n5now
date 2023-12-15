using Application.DTOs;
using Application.Features.Permissions.Services.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Commands.ModifyPermission
{
    public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand, PermissionDTO>
    {
        private IModifyPermissionService modifyPermissionService;

        public ModifyPermissionCommandHandler(IModifyPermissionService modifyPermissionService)
        {
            this.modifyPermissionService = modifyPermissionService;
        }

        public async Task<PermissionDTO> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            return await modifyPermissionService.Update(request);
        }
    }
}
