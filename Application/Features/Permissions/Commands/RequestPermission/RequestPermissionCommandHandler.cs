using Application.DTOs;
using Application.Features.Permissions.Services.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Commands.RequestPermission
{
    public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, PermissionDTO>
    {
        private readonly IRequestPermissionService requestPermissionService;

        public RequestPermissionCommandHandler(IRequestPermissionService requestPermissionService)
        {
            this.requestPermissionService = requestPermissionService;
        }

        public async Task<PermissionDTO> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            return await requestPermissionService.Create(request);
        }
    }
}
