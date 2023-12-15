using Application.DTOs;
using MediatR;

namespace Application.Features.Permissions.Commands.ModifyPermission
{
    public record ModifyPermissionCommand(int id, string EmployeeForename, string EmployeeSurname, int PermissionTypeId) : IRequest<PermissionDTO>;
}
