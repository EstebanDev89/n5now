using Application.DTOs;

namespace Application.Features.Permissions.Services.Interfaces
{

    public interface IGetPermissionsService
    {
        Task<PermissionDTO> GetById(int id);
    }
}
