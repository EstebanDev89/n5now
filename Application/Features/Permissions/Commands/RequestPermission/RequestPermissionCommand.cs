using MediatR;
using Application.DTOs;

namespace Application.Features.Permissions.Commands.RequestPermission
{
    public record RequestPermissionCommand(string EmployeeForename, string EmployeeSurname, int PermissionTypeId) : IRequest<PermissionDTO>;
}
