using MediatR;
using Application.DTOs;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public record GetPermissionByIdQuery(int id) : IRequest<PermissionDTO>;
}
