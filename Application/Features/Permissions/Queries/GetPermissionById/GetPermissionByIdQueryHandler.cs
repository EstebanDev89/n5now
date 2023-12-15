using Application.DTOs;
using Application.Features.Permissions.Services.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDTO>
    {
        private readonly IGetPermissionsService getPermissionsSerivice;

        public GetPermissionByIdQueryHandler(IGetPermissionsService getPermissions)
        {
            this.getPermissionsSerivice = getPermissions;
        }
        public async Task<PermissionDTO> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            return await getPermissionsSerivice.GetById(request.id);
        }
    }
}
