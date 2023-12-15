using Application.DTOs;
using Application.Features.Permissions.Commands.RequestPermission;

namespace Application.Features.Permissions.Services.Interfaces
{
    public interface IRequestPermissionService
    {
        Task<PermissionDTO> Create(RequestPermissionCommand request);
    }
}
