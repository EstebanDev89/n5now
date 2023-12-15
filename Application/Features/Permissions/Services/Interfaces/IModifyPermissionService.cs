using Application.DTOs;
using Application.Features.Permissions.Commands.ModifyPermission;

namespace Application.Features.Permissions.Services.Interfaces
{
    public interface IModifyPermissionService
    {
        Task<PermissionDTO> Update(ModifyPermissionCommand request);
    }
}
