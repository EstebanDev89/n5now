using Application.DTOs;
using Application.Features.Permissions.Commands.RequestPermission;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public  class AppProfile : Profile
    {
        public AppProfile() 
        {
            CreateMap<Permission, PermissionDTO>();
            CreateMap<RequestPermissionCommand, Permission>();
            CreateMap<RequestPermissionCommand, Permission>();
        }
    }
}
